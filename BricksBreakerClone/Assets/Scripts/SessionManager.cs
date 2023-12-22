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
        [SerializeField]
        private LeaderboardPopup leaderboardPopup;

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
        private async Task EndSession()
        {
            DisableInput();

            var mult = await GetMultiplicatorAsync();
            var score = GameSessionPointsDisplay.Points * mult;
            this.leaderboardPopup.gameObject.SetActive(true);
        }

        private void DisableInput()
        {
            this.ballSpawner.gameObject.SetActive(false);
        }

        private async Task<int> GetMultiplicatorAsync()
        {
            multipicatorPopup.gameObject.SetActive(true);
            var mult = await multipicatorPopup.GetMultiplicatorAsync();
            multipicatorPopup.gameObject.SetActive(false);
            Debug.Log(mult);
            return mult;
        }
    }
}