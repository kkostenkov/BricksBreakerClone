using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BrickBreaker
{
    public class BallSpawner : MonoBehaviour, IInputController
    {
        private const float shotDelaySeconds = 0.08f;
        private float angle;
        public GameObject Ball;
        public TextMeshPro tmp;
        public GameObject SpeedUpParent;
        public float Force;
        public int layerMask;

        //public bool moving;
        public GameObject BallSprite;
        public List<GameObject> list = new List<GameObject>();
        private bool stop;
        public GameObject FirstBall;
        public GameObject Reset;
        public Slider slider;
        private bool IsPayerAimingWithSlider;
        public float angleMin;
        public float angleMax;

        private ITargetDownMover downStepper;

        public void Inject(ITargetDownMover downStepper)
        {
            this.downStepper = downStepper;
        }
        
        private void Start()
        {
            this.tmp.text = Constants.BallCount + "x";
            this.layerMask = (LayerMask.GetMask("Wall"));
        }

        private void FixedUpdate()
        {
            if (BottomWall.IsShooting) {
                return;
            }
            
            if (this.IsPayerAimingWithSlider) {
                if (this.BallSprite.activeSelf == false) {
                    this.BallSprite.SetActive(true);
                }

                this.angle = GetSliderAngle();
                AimAndPredictTrajectory(this.angle);
            }
            else {
                if (IsPlayerAimingWithinGameField()) {
                    this.angle = GetManualAimAngle();
                    AimAndPredictTrajectory(this.angle);
                }
            }
        }

        private float GetManualAimAngle()
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        private float GetSliderAngle()
        {
            return 180f - this.slider.value;
        }

        private void AimAndPredictTrajectory(float angle)
        {
            var ray = Raycast();
                    
            UpdateSliderBallPosition(ray);
                    
            if (angle >= this.angleMin && angle <= this.angleMax) {
                DrawDottedLines(ray);
                        
                if (this.BallSprite.activeSelf == false) {
                    this.BallSprite.SetActive(true);
                }
            }
            else {
                if (this.BallSprite.activeSelf) {
                    this.BallSprite.SetActive(false);
                }
            }

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void UpdateSliderBallPosition(RaycastHit2D ray)
        {
            this.BallSprite.transform.position = ray.point;
        }

        private void DrawDottedLines(RaycastHit2D ray)
        {
            DottedLine.DottedLine.Instance.DrawDottedLine(gameObject.transform.position, ray.point);
            Vector2 poss = Vector2.Reflect(new Vector3(ray.point.x, ray.point.y) - this.transform.position,
                ray.normal);
            DottedLine.DottedLine.Instance.DrawDottedLine(ray.point, ray.point + poss.normalized * 2);
        }

        private RaycastHit2D Raycast()
        {
            var ray = Physics2D.Raycast(gameObject.transform.position, transform.right, 
                12f, this.layerMask);
            Debug.DrawRay(gameObject.transform.position, transform.right * ray.distance, Color.red);
            return ray;
        }

        private static bool IsPlayerAimingWithinGameField()
        {
            return Input.GetMouseButton(0);
        }

        public void PointerD()
        {
            this.IsPayerAimingWithSlider = true;
        }

        public void PointerUp()
        {
            this.IsPayerAimingWithSlider = false;
        }

        private void Update()
        {
            if (BallMover.firstHit) {
                if (this.FirstBall.activeSelf == false) {
                    this.FirstBall.SetActive(true);
                }
            }

            if (BottomWall.IsShooting) {
                return;
            }

            if (Input.GetMouseButtonUp(0)) {
                TryStartShooting();
            }
        }

        private void TryStartShooting()
        {
            this.BallSprite.SetActive(false);
            if (IsAngleWithinLimits(this.angle)) {
                BottomWall.wasHitThisRound = false;
                    
                StartCoroutine(ShootBall());
            }
            else {
                transform.rotation = Quaternion.identity;
            }
        }

        private bool IsAngleWithinLimits(float angle)
        {
            return angle >= this.angleMin && angle <= this.angleMax;
        }

        private IEnumerator ShootBall()
        {
            this.slider.value = 90;
            this.slider.transform.parent.gameObject.SetActive(false);
            this.Reset.SetActive(true);
            
            downStepper.PrepareNextPosition();
            
            StartCoroutine(SpeedUp());
            BottomWall.IsShooting = true;
            this.tmp.text = "";
            BallDetector.isTriggered = false;
            this.stop = false;
            this.list.Clear();
            this.FirstBall.SetActive(false);
            for (int i = 0; i < Constants.BallCount; i++) {
                yield return new WaitForSeconds(shotDelaySeconds);
                if (this.stop) {
                    break;
                }

                GameObject myinst = Instantiate(this.Ball, gameObject.transform.position, Quaternion.identity,
                    gameObject.transform);
                this.list.Add(myinst);
                myinst.GetComponent<Rigidbody2D>().AddForce(transform.right * this.Force);
            }
        }

        private IEnumerator SpeedUp()
        {
            yield return new WaitForSeconds(4.6f);

            if (BottomWall.IsShooting) {
                this.SpeedUpParent.SetActive(true);
                Time.timeScale = 1.6f;
            }
        }

        public void SpeedupOff()
        {
            this.SpeedUpParent.SetActive(false);
            Time.timeScale = 1;
        }
    
        public void ResetBalls()
        {
            BottomWall.wasHitThisRound = true;
            this.stop = true;
            foreach (GameObject go in this.list) {
                if (go != null) {
                    go.GetComponent<CircleCollider2D>().enabled = false;
                    go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    go.GetComponent<BallMover>().Move = true;
                }
            }
        }

        public void TurnOff()
        {
            this.gameObject.SetActive(false);
        }
    }
}