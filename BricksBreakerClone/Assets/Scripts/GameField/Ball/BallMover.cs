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

        private void Update()
        {
            if (this.Move != true) {
                return;
            }

            this.step = this.speed * Time.deltaTime;

            gameObject.transform.position = Vector2.MoveTowards(
                gameObject.transform.position, 
                BottomWall.NextPosition, 
                this.step);
            if (gameObject.transform.position.IsCloseEnoughTo(BottomWall.NextPosition)) {
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