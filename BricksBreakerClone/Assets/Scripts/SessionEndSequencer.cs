using System.Threading.Tasks;

namespace BrickBreaker
{
    public class SessionEndSequencer
    {
        private IInputController inputController;
        private ScoreMultiplicatorPopup multiplicatorPopup;
        private LeaderboardPopup leaderboardPopup;

        public SessionEndSequencer(IInputController inputController, ScoreMultiplicatorPopup multiplicatorPopup,
            LeaderboardPopup leaderboardPopup)
        {
            this.inputController = inputController;
            this.multiplicatorPopup = multiplicatorPopup;
            this.leaderboardPopup = leaderboardPopup;
        }
        
        public async Task GetMultiplicatorAndShowLeaderboard()
        {
            DisablePlayerInput();

            var mult = await GetMultiplicatorAsync();
            var score = GameSessionPointsDisplay.Points * mult;
            
            this.leaderboardPopup.Warmup();
            this.leaderboardPopup.RegisterLocalPlayerSessionScore(score);
            this.leaderboardPopup.Show();
        }

        private void DisablePlayerInput()
        {
            this.inputController.TurnOff();
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