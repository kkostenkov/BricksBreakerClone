
namespace BrickBreaker
{
    [System.Serializable]
    public class LeaderboardEntryData
    {
        public int Score;
        public int PlayerId;

        public LeaderboardEntryData(int score, int playerId)
        {
            this.Score = score;
            this.PlayerId = playerId;
        }
    }
}