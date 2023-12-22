using System.Threading.Tasks;
using UnityEngine;

namespace BrickBreaker
{
    public class SessionManager : MonoBehaviour
    {
        private TargetController targetController;
        private BallSpawner ballSpawner;
        private GameLostTrigger gameLostTrigger;
        private ScoreMultiplicatorPopup multipicatorPopup;
        private LeaderboardPopup leaderboardPopup;

        private bool isSessonEnded = false;

        public void Inject(BallSpawner ballSpawner, TargetController targetController, 
            GameLostTrigger gameLostTrigger, ScoreMultiplicatorPopup multipicatorPopup, 
            LeaderboardPopup leaderboardPopup)
        {
            this.ballSpawner = ballSpawner;
            this.targetController = targetController;
            this.gameLostTrigger = gameLostTrigger;
            this.multipicatorPopup = multipicatorPopup;
            this.leaderboardPopup = leaderboardPopup;
        }
        
        private async void Start()
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
            this.leaderboardPopup.RegisterLocalPlayerSessionScore(score);
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
            return mult;
        }
    }
}