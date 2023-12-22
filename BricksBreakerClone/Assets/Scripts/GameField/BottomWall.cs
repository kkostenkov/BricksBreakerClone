using UnityEngine;
using TMPro;

namespace BrickBreaker
{
    public class BottomWall : MonoBehaviour
    {
        public static int count;
        public static bool Shooting;
        public static bool firstHit;
        public static Vector3 NextPosition;
        public GameObject Text;
        private TextMeshPro tmp;
        public static int currentShotPoints;
        public GameObject FirstBall;
        private bool setText;

        private ITargetDownMover downStepper;
        private BallSpawner bs;

        public void Inject(ITargetDownMover downStepper, BallSpawner ballSpawner)
        {
            this.downStepper = downStepper;
            this.bs = ballSpawner;
        }

        private void Start()
        {
            count = 0;
            Shooting = false;
            firstHit = false;
            currentShotPoints = 0;

            this.tmp = this.Text.GetComponent<TextMeshPro>();
            NextPosition = this.bs.transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag(Constants.Tags.Ball)) {
                if (firstHit == false) {
                    this.setText = true;
                    NextPosition = new Vector3(collision.transform.position.x, gameObject.transform.position.y + 0.2f, 0);

                    this.Text.transform.parent.position = NextPosition;
                    if (BallDetector.isTriggered == true) {
                        this.Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.116f, 0.295f);
                    }
                    else {
                        this.Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.116f, -0.295f);
                    }

                    this.FirstBall.transform.gameObject.SetActive(true);
                    Destroy(collision.gameObject);
                    firstHit = true;
                }

                else {
                    collision.transform.GetComponent<CircleCollider2D>().enabled = false;
                    collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    collision.transform.GetComponent<BallMover>().Move = true;
                }
            }
        }

        private void Update()
        {
            var areTargetsStill = downStepper.IsMoving == false;
            var noBallsOnTheField = this.bs.transform.childCount == 0;
            var isRoundOver = firstHit == true &&
                              noBallsOnTheField && 
                              areTargetsStill;
                              
            if (isRoundOver) {
                this.bs.SpeedupOff();
                currentShotPoints = 0;
                this.bs.transform.position = NextPosition;
                
                downStepper.MakeStep();
                
                this.setText = false;
                count = 0;
                firstHit = false;
            }

            if (this.setText == true) {
                if (count < BallSpawner.BallCount) {
                    this.tmp.text = count + 1 + "x";
                }
            }
            else {
                if (Shooting == false) {
                    this.tmp.text = BallSpawner.BallCount + "x";
                    this.bs.Reset.SetActive(false);
                    this.bs.slider.transform.parent.gameObject.SetActive(true);
                }
            }
        }
    }
}