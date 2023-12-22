using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BrickBreaker
{
    public class LeaderboardPopup : BasePopup
    {
        [SerializeField]
        private Button playAgainButton;
        [SerializeField]
        private Transform entriesRoot;
        [SerializeField]
        private LeaderboardEntryView entryViewPrefab;

        private int localPlayerId = 1;

        private readonly List<LeaderboardEntryView> entryViews = new();
        private LeaderboardEntryData localPlayerEntry;
        private List<LeaderboardEntryData> dataEntries;
        private ILeaderboardStorage leaderboardStorage;

        public void Inject(ILeaderboardStorage leaderboardStorage)
        {
            this.leaderboardStorage = leaderboardStorage;
        }
    
        private void Start()
        {
            this.playAgainButton.onClick.AddListener(OnPlayAgainPressed);
        }

        private void OnDestroy()
        {
            this.playAgainButton.onClick.RemoveAllListeners();
        }

        public void Warmup()
        {
            for (int i = 0; i < Constants.LeaderboardEntriesLimit; i++) {
                var entry = Instantiate(this.entryViewPrefab, this.entriesRoot);
                this.entryViews.Add(entry);
            }
            this.dataEntries = this.leaderboardStorage.Load().OrderBy(d => d.Score).ToList();
        }

        public void RegisterLocalPlayerSessionScore(int score)
        {
            CachePlayerEntry();

            this.localPlayerEntry.Score += score;
            this.dataEntries = this.dataEntries.OrderByDescending(d => d.Score).ToList();
            this.leaderboardStorage.Save(this.dataEntries);
        }

        private void CachePlayerEntry()
        {
            if (this.localPlayerEntry == null) {
                FindLocalPlayerEntry();

                if (this.localPlayerEntry == null) {
                    CreateLocalPlayerEntry();
                }
            }
        }

        private void FindLocalPlayerEntry()
        {
            this.localPlayerEntry = this.dataEntries.FirstOrDefault(data => data.PlayerId == this.localPlayerId);
        }

        private void CreateLocalPlayerEntry()
        {
            this.localPlayerEntry = new LeaderboardEntryData(score: 0, this.localPlayerId);
            this.dataEntries.Add(this.localPlayerEntry);
        }

        public override void Show()
        {
            for (int index = 0; index < this.entryViews.Count; index++) {
                this.entryViews[index].Setup(this.dataEntries[index], index);
                var isLocalPlayer = this.dataEntries[index].PlayerId == this.localPlayerId;
                if (isLocalPlayer) {
                    this.entryViews[index].SetLocalPlayerView();    
                }
                this.entryViews[index].gameObject.SetActive(true);
            }
            base.Show();
        }

        private void OnPlayAgainPressed()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}