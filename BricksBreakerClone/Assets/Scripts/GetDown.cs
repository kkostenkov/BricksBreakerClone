using UnityEngine;

namespace BrickBreaker
{
    public class GetDown : MonoBehaviour
    {
        public static bool Move;
        private float speed = 8f;
        private float step;
        public Vector2 newPos;

        private void Start()
        {
            Move = false;
        }

        private void FixedUpdate()
        {
            if (Move == true) {
                this.step = this.speed * Time.deltaTime;

                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, this.newPos, this.step);
                if (Vector2.Distance(gameObject.transform.position, this.newPos) < 0.0001f) {
                    Move = false;
                    Wall.Shooting = false;
                }
            }
        }
    }
}