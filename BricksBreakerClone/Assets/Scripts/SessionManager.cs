using System.Threading.Tasks;
using UnityEngine;

namespace BrickBreaker
{
    public class SessionManager : MonoBehaviour
    {
        private ITargetsDestroyedNotifier targetsNotifier;
        private IGameLostNotifier gameLostNotifier;

        private bool isSessonEnded = false;
        private SessionEndSequencer sessionEndSequencer;

        public void Inject(ITargetsDestroyedNotifier targetsNotifier, IGameLostNotifier gameLostNotifier, 
            SessionEndSequencer sessionEndSequencer)
        {
            this.targetsNotifier = targetsNotifier;
            this.gameLostNotifier = gameLostNotifier;
            this.sessionEndSequencer = sessionEndSequencer;
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

            await this.sessionEndSequencer.GetMultiplicatorAndShowLeaderboard();
        }
    }
}