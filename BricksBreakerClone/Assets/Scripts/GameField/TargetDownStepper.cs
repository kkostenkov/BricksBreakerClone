using UnityEngine;

namespace BrickBreaker
{
    public class TargetDownStepper : MonoBehaviour, ITargetDownMover
    {
        public bool IsMoving { get; private set; }
        private float speed = 8f;
        private float step;
        private Vector2 newPos;

        private void Start()
        {
            IsMoving = false;
        }

        private void Update()
        {
            if (IsMoving != true) {
                return;
            }

            MoveDownFrame();
        }

        public void PrepareNextPosition()
        {
            var currentPosition = this.transform.position;
            newPos = new Vector2(currentPosition.x, currentPosition.y - 0.75f);
        }

        public void MakeStep()
        {
            IsMoving = true;
        }

        private void MoveDownFrame()
        {
            this.step = this.speed * Time.deltaTime;

            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, this.newPos, this.step);
            if (gameObject.transform.position.IsCloseEnoughTo(this.newPos)) {
                IsMoving = false;
                BottomWall.IsShooting = false;
            }
        }
    }
}