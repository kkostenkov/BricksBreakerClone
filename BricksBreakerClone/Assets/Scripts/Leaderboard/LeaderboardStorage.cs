using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace BrickBreaker
{
    public class LeaderboardStorage : ILeaderboardStorage
    {
        public const string LeaderboardPrefsKey = "leaderboard";

        public List<LeaderboardEntryData> Load()
        {
            var jsonData = PlayerPrefs.GetString(LeaderboardPrefsKey);
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
            PlayerPrefs.SetString(LeaderboardPrefsKey, jsonData);
        }

        private List<LeaderboardEntryData> CreateFakeData()
        {
            var localPlayerId = new PlayerInfo().Id;
            var entries = new List<LeaderboardEntryData>();
            for (var i = 0; i < Constants.LeaderboardEntriesLimit; i++) {
                var currentId = i;
                var entry = new LeaderboardEntryData(Random.Range(10, 17800), currentId);
                // to allow player start from 0
                if (currentId == localPlayerId) {
                    entry.Score = 0;
                }
                entries.Add(entry);
            }

            return entries;
        }
    }
}