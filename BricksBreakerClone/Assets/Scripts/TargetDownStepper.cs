using UnityEngine;

namespace BrickBreaker
{
    public class TargetDownStepper : MonoBehaviour
    {
        public static bool ShouldMove;
        private float speed = 8f;
        private float step;
        public Vector2 newPos;

        private void Start()
        {
            ShouldMove = false;
        }

        private void FixedUpdate()
        {
            if (ShouldMove != true) {
                return;
            }

            MoveDown();
        }

        private void MoveDown()
        {
            this.step = this.speed * Time.deltaTime;

            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, this.newPos, this.step);
            if (Vector2.Distance(gameObject.transform.position, this.newPos) < 0.0001f) {
                ShouldMove = false;
                BottomWall.Shooting = false;
            }
        }
    }
}