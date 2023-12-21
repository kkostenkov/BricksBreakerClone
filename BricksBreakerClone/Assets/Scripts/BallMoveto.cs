using UnityEngine;

namespace BrickBreaker
{
    public class BallMoveto : MonoBehaviour
    {
        public bool Move;
        private float speed = 8f;
        private float step;
        public static bool firstHit;

        private void Start()
        {
            this.Move = false;
        }

        private void FixedUpdate()
        {
            if (this.Move == true) {
                this.step = this.speed * Time.deltaTime;

                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, Wall.NextPosition, this.step);
                if (Vector2.Distance(gameObject.transform.position, Wall.NextPosition) < 0.0001f) {
                    Destroy(this.gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            if (firstHit == false) {
                firstHit = true;
            }

            Wall.count++;
        }
    }
}