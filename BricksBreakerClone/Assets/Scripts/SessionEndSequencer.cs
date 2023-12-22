using System.Threading.Tasks;

namespace BrickBreaker
{
    public class SessionEndSequencer
    {
        private IInputController inputController;
        private LeaderboardController leaderboardController;
        private ScoreMultiplierPopup multiplierPopup;
        private LeaderboardPopup leaderboardPopup;

        public SessionEndSequencer(IInputController inputController, LeaderboardController leaderboardController, 
            ScoreMultiplierPopup multiplierPopup, LeaderboardPopup leaderboardPopup)
        {
            this.inputController = inputController;
            this.leaderboardController = leaderboardController;
            this.multiplierPopup = multiplierPopup;
            this.leaderboardPopup = leaderboardPopup;
        }
        
        public async Task GetMultiplierAndShowLeaderboard()
        {
            DisablePlayerInput();

            var mult = await GetMultiplierAsync();
            var score = GameSessionPointsDisplay.Points * mult;
            this.leaderboardController.RegisterLocalPlayerSessionScore(score);

            this.leaderboardPopup.Warmup();
            this.leaderboardPopup.Show();
        }

        private void DisablePlayerInput()
        {
            this.inputController.TurnOff();
        }

        private async Task<int> GetMultiplierAsync()
        {
            this.multiplierPopup.Show();
            var mult = await this.multiplierPopup.GetMultiplierAsync();
            this.multiplierPopup.Hide();
            return mult;
        }
    }
}