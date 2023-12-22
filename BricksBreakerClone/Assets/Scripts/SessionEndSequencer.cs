using System.Threading.Tasks;

namespace BrickBreaker
{
    public class SessionEndSequencer
    {
        private IInputController inputController;
        private ILeaderboardController leaderboardController;
        private ScoreMultiplierPopup multiplierPopup;
        private LeaderboardPopup leaderboardPopup;
        private SessionPoints pointsHolder;

        public SessionEndSequencer(IInputController inputController, ILeaderboardController leaderboardController, 
            ScoreMultiplierPopup multiplierPopup, LeaderboardPopup leaderboardPopup, SessionPoints pointsHolder)
        {
            this.inputController = inputController;
            this.leaderboardController = leaderboardController;
            this.multiplierPopup = multiplierPopup;
            this.leaderboardPopup = leaderboardPopup;
            this.pointsHolder = pointsHolder;
        }
        
        public async Task GetMultiplierAndShowLeaderboard()
        {
            DisablePlayerInput();

            var mult = await GetMultiplierAsync();
            var score = pointsHolder.Points * mult;
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