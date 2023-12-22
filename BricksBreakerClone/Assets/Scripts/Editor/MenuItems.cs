using BrickBreaker;
using UnityEditor;
using UnityEngine;

public class MenuItems
{
    [MenuItem("Cheats/Reset leaderboard")]
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(LeaderboardStorage.LeaderboardPrefsKey);
    }
    
}
