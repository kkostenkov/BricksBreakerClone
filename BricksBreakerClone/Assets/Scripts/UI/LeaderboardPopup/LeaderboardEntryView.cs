using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrickBreaker
{
    public class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField]
        private Image avatar;
        [SerializeField]
        private Image background;
        [SerializeField]
        private TextMeshProUGUI scoreLabel;
        [SerializeField]
        private TextMeshProUGUI placeLabel;
        [SerializeField]
        private Color regularBackColor;
        [SerializeField]
        private Color localPlayerBackColor;

        public void Setup(LeaderboardEntryData data, int index)
        {
            this.scoreLabel.text = data.Score.ToString();
            this.placeLabel.text = (index + 1).ToString();
            this.background.color = this.regularBackColor;
        }

        public void SetLocalPlayerView()
        {
            this.background.color = this.localPlayerBackColor;
        }
    }
}