
[System.Serializable]
public class LeaderboardEntryData
{
    public int Score;
    public int PlayerId;

    public LeaderboardEntryData(int score, int playerId)
    {
        Score = score;
        PlayerId = playerId;
    }
}