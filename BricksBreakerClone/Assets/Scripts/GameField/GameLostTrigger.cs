using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrickBreaker
{
    public class GameLostTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag(Constants.Tags.Target)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}