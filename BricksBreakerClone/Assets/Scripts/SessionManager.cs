using System.Threading.Tasks;
using UnityEngine;

namespace BrickBreaker
{
    public class SessionManager : MonoBehaviour
    {
        [SerializeField]
        private TargetController targetController;

        [SerializeField]
        private BallSpawner ballSpawner;
        [SerializeField]
        private GameLostTrigger gameLostTrigger;

        [SerializeField]
        private ScoreMultiplicatorPopup multipicatorPopup;

        private void Awake()
        {
            this.targetController.AllTargetsDestroyed += OnAllTargetsDestroyed;
            this.gameLostTrigger.TargetReachedGameLostTrigger += OnGameLost;
        }

        private void OnDestroy()
        {
            this.targetController.AllTargetsDestroyed -= OnAllTargetsDestroyed;
            this.gameLostTrigger.TargetReachedGameLostTrigger -= OnGameLost;
        }

        private void OnAllTargetsDestroyed()
        {
            EndSession();
        }

        private void OnGameLost()
        {
            EndSession();
        }

        [ContextMenu("EndSession")]
        private void EndSession()
        {
            targetController.gameObject.SetActive(false);
            
            GetMultiplicatorAsync();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private async Task<int> GetMultiplicatorAsync()
        {
            ballSpawner.gameObject.SetActive(false);
            multipicatorPopup.gameObject.SetActive(true);
            var mult = await multipicatorPopup.GetMultiplicatorAsync();
            multipicatorPopup.gameObject.SetActive(false);
            Debug.Log(mult);
            return mult;
        }
    }
}