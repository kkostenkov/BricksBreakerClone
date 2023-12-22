using System.Threading.Tasks;
using UnityEngine;

namespace BrickBreaker
{
    public class SessionManager : MonoBehaviour
    {
        private ITargetsDestroyedNotifier targetsNotifier;
        private IInputController inputController;
        private IGameLostNotifier gameLostNotifier;
        private ScoreMultiplicatorPopup multiplicatorPopup;
        private LeaderboardPopup leaderboardPopup;

        private bool isSessonEnded = false;

        public void Inject(IInputController inputController, ITargetsDestroyedNotifier targetsNotifier,
            IGameLostNotifier gameLostNotifier, ScoreMultiplicatorPopup multiplicatorPopup,
            LeaderboardPopup leaderboardPopup)
        {
            this.inputController = inputController;
            this.targetsNotifier = targetsNotifier;
            this.gameLostNotifier = gameLostNotifier;
            this.multiplicatorPopup = multiplicatorPopup;
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

            DisablePlayerInput();

            var mult = await GetMultiplicatorAsync();
            var score = GameSessionPointsDisplay.Points * mult;
            
            leaderboardPopup.Warmup();
            leaderboardPopup.RegisterLocalPlayerSessionScore(score);
            leaderboardPopup.Show();
        }

        private void DisablePlayerInput()
        {
            inputController.TurnOff();
        }

        private async Task<int> GetMultiplicatorAsync()
        {
            this.multiplicatorPopup.Show();
            var mult = await this.multiplicatorPopup.GetMultiplicatorAsync();
            this.multiplicatorPopup.Hide();
            return mult;
        }
    }
}