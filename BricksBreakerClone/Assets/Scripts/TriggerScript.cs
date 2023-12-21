using UnityEngine;

namespace BrickBreaker
{
    public class TriggerScript : MonoBehaviour
    {
        public static bool onRight;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.transform.CompareTag(Constants.Tags.Ball)) {
                return;
            }

            if (onRight == false) {
                onRight = true;
            }
        }
    }
}