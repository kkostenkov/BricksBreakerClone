using System.Threading.Tasks;
using UnityEngine;

namespace BrickBreaker
{
    public class SessionManager : MonoBehaviour
    {
        private ITargetsDestroyedNotifier targetsNotifier;
        private IInputController inputController;
        private IGameLostNotifier gameLostNotifier;
        private ScoreMultiplicatorPopup multipicatorPopup;
        private LeaderboardPopup leaderboardPopup;

        private bool isSessonEnded = false;

        public void Inject(IInputController inputController, ITargetsDestroyedNotifier targetsNotifier, 
            IGameLostNotifier gameLostNotifier, ScoreMultiplicatorPopup multipicatorPopup, 
            LeaderboardPopup leaderboardPopup)
        {
            this.inputController = inputController;
            this.targetsNotifier = targetsNotifier;
            this.gameLostNotifier = gameLostNotifier;
            this.multipicatorPopup = multipicatorPopup;
            this.leaderboardPopup = leaderboardPopup;
        }
        
        private async void Start()
        {
            this.targetsNotifier.AllTargetsDestroyed += OnAllTargetsDestroyed;
            this.gameLostNotifier.TargetReachedGameLostTrigger += OnGameLost;
            await WaitForSessionEndAndShowLeaderboard();
        }

        private void OnDestroy()
        {
            this.targetsNotifier.AllTargetsDestroyed -= OnAllTargetsDestroyed;
            this.gameLostNotifier.TargetReachedGameLostTrigger -= OnGameLost;
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
            this.inputController.TurnOff();
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