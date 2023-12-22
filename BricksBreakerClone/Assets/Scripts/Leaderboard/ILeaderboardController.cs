using System.Collections.Generic;

namespace BrickBreaker
{
    public interface ILeaderboardController
    {
        void RegisterLocalPlayerSessionScore(int score);
        List<LeaderboardEntryData> Get();
    }
}