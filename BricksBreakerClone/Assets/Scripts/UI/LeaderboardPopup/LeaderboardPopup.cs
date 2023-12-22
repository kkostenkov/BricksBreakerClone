using System.Collections.Generic;
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

        private readonly List<LeaderboardEntryView> entryViews = new();
        private ILeaderboardController leaderboard;
        private IPlayerInfoProvider player;

        public void Inject(ILeaderboardController leaderboard, IPlayerInfoProvider player)
        {
            this.leaderboard = leaderboard;
            this.player = player;
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
        }

        public override void Show()
        {
            var dataEntries = this.leaderboard.Get();
                
            for (int index = 0; index < this.entryViews.Count; index++) {
                this.entryViews[index].Setup(dataEntries[index], index);
                var isLocalPlayer = dataEntries[index].PlayerId == player.Id;
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