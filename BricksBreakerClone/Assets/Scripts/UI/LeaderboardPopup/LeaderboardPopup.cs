using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardPopup : MonoBehaviour
{
    [SerializeField]
    private Button playAgainButton;
    [SerializeField]
    private Transform entriesRoot;
    [SerializeField]
    private LeaderboardEntry entryPrefab;

    private const int entriesLimit = 100;
    private const string leaderboardPrefsKey = "leaderboard";
    private int localPlayerId = 1;

    private readonly List<LeaderboardEntry> entryViews = new();
    private LeaderboardEntryData localPlayerEntry;
    private List<LeaderboardEntryData> dataEntries;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(OnPlayAgainPressed);
    }

    private void OnDestroy()
    {
        playAgainButton.onClick.RemoveAllListeners();
    }

    public void RegisterLocalPlayerSessionScore(int score)
    {
        if (localPlayerEntry == null) {
            localPlayerEntry = this.dataEntries.FirstOrDefault(data => data.PlayerId == this.localPlayerId);
        }

        if (localPlayerEntry == null) {
            localPlayerEntry = new LeaderboardEntryData(score: 0, this.localPlayerId);
            dataEntries.Add(localPlayerEntry);
        }

        localPlayerEntry.Score += score;
        dataEntries = dataEntries.OrderByDescending(d => d.Score).ToList();
        SaveData(this.dataEntries);
    }

    public void Warmup()
    {
        for (int i = 0; i < entriesLimit; i++) {
            var entry = Instantiate(entryPrefab, this.entriesRoot);
            this.entryViews.Add(entry);
        }
        dataEntries = LoadData().OrderBy(d => d.Score).ToList();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        
        for (int index = 0; index < entryViews.Count; index++) {
            entryViews[index].Setup(dataEntries[index], index);
            var isLocalPlayer = dataEntries[index].PlayerId == this.localPlayerId;
            if (isLocalPlayer) {
                entryViews[index].SetLocalPlayerView();    
            }
            entryViews[index].gameObject.SetActive(true);
        }
    }

    private void OnPlayAgainPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    [ContextMenu("Load")]
    private List<LeaderboardEntryData> LoadData()
    {
        var jsonData = PlayerPrefs.GetString(leaderboardPrefsKey);
        var entries = JsonConvert.DeserializeObject<List<LeaderboardEntryData>>(jsonData);
        if (entries == null || entries.Count == 0) {
            entries = CreateFakeData();
        }
        return entries;
    }
    
    private void SaveData(List<LeaderboardEntryData> data)
    {
        var savedEntries = data.Take(entriesLimit).ToList();
        var jsonData = JsonConvert.SerializeObject(savedEntries);
        PlayerPrefs.SetString(leaderboardPrefsKey, jsonData);
    }

    [ContextMenu("TestSave")]
    private void TestSave()
    {
        SaveData(CreateFakeData());
    }

    private List<LeaderboardEntryData> CreateFakeData()
    {
        var entries = new List<LeaderboardEntryData>();
        for (int i = 0; i < entriesLimit; i++) {
            var entry = new LeaderboardEntryData(Random.Range(10, 17800), i);
            entries.Add(entry);
        }

        return entries;
    }
}
