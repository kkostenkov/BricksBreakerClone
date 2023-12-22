using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace BrickBreaker
{
    public class LeaderboardStorage : ILeaderboardStorage
    {
        private const string leaderboardPrefsKey = "leaderboard";

        public List<LeaderboardEntryData> Load()
        {
            var jsonData = PlayerPrefs.GetString(leaderboardPrefsKey);
            var entries = JsonConvert.DeserializeObject<List<LeaderboardEntryData>>(jsonData);
            if (entries == null || entries.Count == 0) {
                entries = CreateFakeData();
            }
            return entries;
        }

        public void Save(List<LeaderboardEntryData> data)
        {
            var savedEntries = data.Take(Constants.LeaderboardEntriesLimit).ToList();
            var jsonData = JsonConvert.SerializeObject(savedEntries);
            PlayerPrefs.SetString(leaderboardPrefsKey, jsonData);
        }

        private List<LeaderboardEntryData> CreateFakeData()
        {
            var entries = new List<LeaderboardEntryData>();
            for (int i = 0; i < Constants.LeaderboardEntriesLimit; i++) {
                var entry = new LeaderboardEntryData(Random.Range(10, 17800), i);
                entries.Add(entry);
            }

            return entries;
        }
    }
}