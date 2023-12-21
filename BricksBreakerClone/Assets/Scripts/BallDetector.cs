using UnityEngine;

namespace BrickBreaker
{
    public class BallDetector : MonoBehaviour
    {
        public static bool isTriggered;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.transform.CompareTag(Constants.Tags.Ball)) {
                return;
            }

            if (isTriggered == false) {
                isTriggered = true;
            }
        }
    }
}