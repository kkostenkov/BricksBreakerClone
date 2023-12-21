using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BrickBreaker
{
    public class BallSpawner : MonoBehaviour
    {
        private RaycastHit2D ray;
        private float angle;
        public GameObject Ball;
        public static int BallCount = 52;
        public TextMeshPro tmp;
        public GameObject SpeedUpParent;
        public float Force;
        public int layerMask;

        public GameObject TargetParent;

        //public bool moving;
        public GameObject BallSprite;
        public List<GameObject> list = new List<GameObject>();
        private bool stop;
        public GameObject FirstBall;
        public GameObject Reset;
        public Slider slider;
        private bool PointerDown;
        public float angleMin;
        public float angleMax;

        private void Start()
        {
            this.tmp.text = BallCount + "x";
            this.layerMask = (LayerMask.GetMask("Wall"));
        }

        private void FixedUpdate()
        {
            if (BottomWall.Shooting != false) {
                return;
            }

            if (this.PointerDown == true) {
                if (this.BallSprite.activeSelf == false) {
                    this.BallSprite.SetActive(true);
                }

                this.ray = Physics2D.Raycast(gameObject.transform.position, transform.right, 12f, this.layerMask);
                Debug.DrawRay(gameObject.transform.position, transform.right * this.ray.distance, Color.red);
                this.BallSprite.transform.position = this.ray.point;
                Vector2 poss = Vector2.Reflect(new Vector3(this.ray.point.x, this.ray.point.y) - this.transform.position,
                    this.ray.normal);
                DottedLine.DottedLine.Instance.DrawDottedLine(gameObject.transform.position, this.ray.point);
                DottedLine.DottedLine.Instance.DrawDottedLine(this.ray.point, this.ray.point + poss.normalized * 2);

                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 dir = Input.mousePosition - pos;
                this.angle = 180f - this.slider.value;
                transform.rotation = Quaternion.AngleAxis(this.angle, Vector3.forward);
            }
            else {
                if (Input.GetMouseButton(0)) {
                    this.ray = Physics2D.Raycast(gameObject.transform.position, transform.right, 12f, this.layerMask);
                    Debug.DrawRay(gameObject.transform.position, transform.right * this.ray.distance, Color.red);

                    Vector2 poss = Vector2.Reflect(new Vector3(this.ray.point.x, this.ray.point.y) - this.transform.position,
                        this.ray.normal);

                    this.BallSprite.transform.position = this.ray.point;
                    Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                    Vector3 dir = Input.mousePosition - pos;
                    this.angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    if (this.angle >= this.angleMin && this.angle <= this.angleMax) {
                        DottedLine.DottedLine.Instance.DrawDottedLine(gameObject.transform.position, this.ray.point);
                        DottedLine.DottedLine.Instance.DrawDottedLine(this.ray.point, this.ray.point + poss.normalized * 2);
                        if (this.BallSprite.activeSelf == false) {
                            this.BallSprite.SetActive(true);
                        }
                    }
                    else {
                        if (this.BallSprite.activeSelf == true) {
                            this.BallSprite.SetActive(false);
                        }
                    }

                    transform.rotation = Quaternion.AngleAxis(this.angle, Vector3.forward);
                }
            }
        }

        public void PointerD()
        {
            this.PointerDown = true;
        }

        public void PointerUp()
        {
            this.PointerDown = false;
        }

        private void Update()
        {
            if (BallMover.firstHit == true) {
                if (this.FirstBall.activeSelf == false) {
                    this.FirstBall.SetActive(true);
                }
            }

            if (BottomWall.Shooting != false) {
                return;
            }

            if (Input.GetMouseButtonUp(0)) {
                this.BallSprite.SetActive(false);
                if (this.angle >= this.angleMin && this.angle <= this.angleMax) {
                    if (BottomWall.firstHit == true) {
                        BottomWall.firstHit = false;
                    }

                    StartCoroutine(ShootBall());
                }
                else {
                    transform.rotation = Quaternion.identity;
                }
            }
        }

        private IEnumerator ShootBall()
        {
            this.slider.value = 90;
            this.slider.transform.parent.gameObject.SetActive(false);
            this.Reset.SetActive(true);
            this.TargetParent.GetComponent<GetDown>().newPos = new Vector2(this.TargetParent.transform.position.x,
                this.TargetParent.transform.position.y - 0.75f);
            StartCoroutine(SpeedUp());
            BottomWall.Shooting = true;
            this.tmp.text = "";
            BallDetector.isTriggered = false;
            this.stop = false;
            this.list.Clear();
            this.FirstBall.SetActive(false);
            for (int i = 0; i < BallCount; i++) {
                yield return new WaitForSeconds(0.08f);
                if (this.stop == true) {
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

            if (BottomWall.Shooting == true) {
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
            BottomWall.firstHit = true;
            this.stop = true;
            foreach (GameObject go in this.list) {
                if (go != null) {
                    go.GetComponent<CircleCollider2D>().enabled = false;
                    go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    go.GetComponent<BallMover>().Move = true;
                }
            }
        }
    }
}