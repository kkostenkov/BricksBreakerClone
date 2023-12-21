using UnityEngine;

namespace BrickBreaker
{
    public class BallMover : MonoBehaviour
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
            if (this.Move != true) {
                return;
            }

            this.step = this.speed * Time.deltaTime;

            gameObject.transform.position = Vector2.MoveTowards(
                gameObject.transform.position, 
                BottomWall.NextPosition, 
                this.step);
            if (Vector2.Distance(gameObject.transform.position, BottomWall.NextPosition) < 0.0001f) {
                Destroy(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            if (firstHit == false) {
                firstHit = true;
            }

            BottomWall.count++;
        }
    }
}