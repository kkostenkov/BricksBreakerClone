using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardPopup : MonoBehaviour
{
    [SerializeField]
    private Button playAgainButton;
    private void Awake()
    {
        playAgainButton.onClick.AddListener(OnPlayAgainPressed);
    }

    private void OnDestroy()
    {
        playAgainButton.onClick.RemoveAllListeners();
    }

    private void OnPlayAgainPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
