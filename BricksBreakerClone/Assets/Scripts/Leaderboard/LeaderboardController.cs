using System.Collections.Generic;
using System.Linq;

namespace BrickBreaker
{
    public class LeaderboardController : ILeaderboardController
    {
        private LeaderboardEntryData localPlayerEntry;
        private List<LeaderboardEntryData> dataEntries;
        private ILeaderboardStorage leaderboardStorage;
        private IPlayerInfoProvider player;

        public LeaderboardController(ILeaderboardStorage leaderboardStorage, IPlayerInfoProvider player)
        {
            this.leaderboardStorage = leaderboardStorage;
            this.player = player;
            this.dataEntries = this.leaderboardStorage.Load();
            SortEntries();
        }

        public List<LeaderboardEntryData> Get()
        {
            return this.dataEntries;
        }

        public void RegisterLocalPlayerSessionScore(int score)
        {
            CachePlayerEntry();

            this.localPlayerEntry.Score += score;
            SortEntries();
            this.leaderboardStorage.Save(this.dataEntries);
        }

        private void CachePlayerEntry()
        {
            if (this.localPlayerEntry != null) {
                return;
            }

            this.localPlayerEntry = this.dataEntries.FirstOrDefault(data => data.PlayerId == player.Id);

            if (this.localPlayerEntry == null) {
                CreateLocalPlayerEntry();
            }
        }

        private void CreateLocalPlayerEntry()
        {
            this.localPlayerEntry = new LeaderboardEntryData(score: 0, player.Id);
            this.dataEntries.Add(this.localPlayerEntry);
        }

        private void SortEntries()
        {
            this.dataEntries = this.dataEntries.OrderByDescending(d => d.Score).ToList();
        }
    }
}