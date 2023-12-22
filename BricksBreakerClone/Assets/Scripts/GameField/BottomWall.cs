using UnityEngine;
using TMPro;

namespace BrickBreaker
{
    public class BottomWall : MonoBehaviour
    {
        public static int count;
        public static bool IsShooting;
        public static bool wasHitThisRound;
        public static Vector3 NextPosition;
        public GameObject Text;
        private TextMeshPro tmp;
        public static int currentShotPoints;
        public GameObject FirstBall;
        private bool setText;

        private ITargetDownMover downStepper;
        private BallSpawner ballSpawner;

        public void Inject(ITargetDownMover downStepper, BallSpawner ballSpawner)
        {
            this.downStepper = downStepper;
            this.ballSpawner = ballSpawner;
        }

        private void Start()
        {
            count = 0;
            IsShooting = false;
            wasHitThisRound = false;
            currentShotPoints = 0;

            this.tmp = this.Text.GetComponent<TextMeshPro>();
            NextPosition = this.ballSpawner.transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag(Constants.Tags.Ball)) {
                return;
            }

            if (wasHitThisRound) {
                ProcessFollowingHits(collision);
            }
            else {
                ProcessFirstHit(collision);
            }
        }

        private void ProcessFirstHit(Collision2D collision)
        {
            this.setText = true;
            NextPosition = new Vector3(collision.transform.position.x, gameObject.transform.position.y + 0.2f, 0);

            this.Text.transform.parent.position = NextPosition;
            if (BallDetector.isTriggered) {
                this.Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.116f, 0.295f);
            }
            else {
                this.Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.116f, -0.295f);
            }

            this.FirstBall.transform.gameObject.SetActive(true);
            Destroy(collision.gameObject);
            wasHitThisRound = true;
        }

        private void ProcessFollowingHits(Collision2D collision)
        {
            collision.transform.GetComponent<CircleCollider2D>().enabled = false;
            collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            collision.transform.GetComponent<BallMover>().Move = true;
        }

        private void Update()
        {
            var areTargetsStill = downStepper.IsMoving == false;
            var noBallsOnTheField = this.ballSpawner.transform.childCount == 0;
            var isRoundOver = wasHitThisRound &&
                              noBallsOnTheField && 
                              areTargetsStill;
                              
            if (isRoundOver) {
                this.ballSpawner.SpeedupOff();
                currentShotPoints = 0;
                this.ballSpawner.transform.position = NextPosition;
                
                downStepper.MakeStep();
                
                this.setText = false;
                count = 0;
                wasHitThisRound = false;
            }

            if (this.setText) {
                if (count < Constants.BallCount) {
                    this.tmp.text = count + 1 + "x";
                }
            }
            else {
                if (IsShooting == false) {
                    this.tmp.text = Constants.BallCount + "x";
                    this.ballSpawner.Reset.SetActive(false);
                    this.ballSpawner.slider.transform.parent.gameObject.SetActive(true);
                }
            }
        }
    }
}