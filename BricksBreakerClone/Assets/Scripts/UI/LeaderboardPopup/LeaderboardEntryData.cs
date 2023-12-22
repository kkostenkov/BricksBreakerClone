public class LeaderboardEntryData
{
    public int Score { get; set; }
    public int PlayerId { get; }

    public LeaderboardEntryData(int score, int playerId)
    {
        Score = score;
        PlayerId = playerId;
    }
}