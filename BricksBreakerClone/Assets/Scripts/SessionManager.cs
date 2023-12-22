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

        private bool isSessonEnded = false;

        private async void Awake()
        {
            this.targetController.AllTargetsDestroyed += OnAllTargetsDestroyed;
            this.gameLostTrigger.TargetReachedGameLostTrigger += OnGameLost;
            await WaitForSessionEndAndShowLeaderboard();
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
            isSessonEnded = true;
        }
        
        [ContextMenu("Show leaderboard popup")]
        private async Task WaitForSessionEndAndShowLeaderboard()
        {
            while (!isSessonEnded) {
                await Task.Yield();
            }
            DisableInput();

            var mult = await GetMultiplicatorAsync();
            var score = GameSessionPointsDisplay.Points * mult;
            this.leaderboardPopup.Warmup();
            this.leaderboardPopup.SetLocalPlayerScore(score);
            this.leaderboardPopup.Show();
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