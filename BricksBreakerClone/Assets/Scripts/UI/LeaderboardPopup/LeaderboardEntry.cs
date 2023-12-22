using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
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
        scoreLabel.text = data.Score.ToString();
        placeLabel.text = (index + 1).ToString();
        background.color = this.regularBackColor;
    }

    public void SetLocalPlayerView()
    {
        background.color = this.localPlayerBackColor;
    }
}