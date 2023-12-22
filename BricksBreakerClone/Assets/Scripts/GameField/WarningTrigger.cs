using UnityEngine;

namespace BrickBreaker
{
    public class WarningTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject warningBackground;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.transform.CompareTag(Constants.Tags.Target)) {
                return;
            }

            if (this.warningBackground.activeSelf == false) {
                this.warningBackground.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.transform.CompareTag(Constants.Tags.Target)) {
                return;
            }

            if (this.warningBackground.activeSelf == true) {
                this.warningBackground.SetActive(false);
            }
        }
    }
}