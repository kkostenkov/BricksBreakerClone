using UnityEngine;

public class WarningTrigger : MonoBehaviour
{
    public GameObject WarningBg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Target") {
            if (WarningBg.activeSelf == false) {
                this.WarningBg.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Target") {
            if (WarningBg.activeSelf == true) {
                this.WarningBg.SetActive(false);
            }
        }
    }
}