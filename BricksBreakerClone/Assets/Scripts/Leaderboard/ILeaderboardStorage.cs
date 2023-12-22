using System.Collections.Generic;

namespace BrickBreaker
{
    public interface ILeaderboardStorage
    {
        List<LeaderboardEntryData> Load();
        void Save(List<LeaderboardEntryData> data);
    }
}