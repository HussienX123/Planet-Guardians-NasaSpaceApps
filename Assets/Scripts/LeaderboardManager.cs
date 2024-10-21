using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class LeaderboardManager : MonoBehaviour
{
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class Leaderboard
    {
        public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    }

    public TextMeshProUGUI leaderboardText;
    private Leaderboard leaderboard = new Leaderboard();

    private string leaderboardFilePath;

    private void Awake()
    {
        leaderboardFilePath = Application.persistentDataPath + "/leaderboard.json";
        LoadLeaderboard();
    }

    public void SaveEntry(string playerName, int playerScore)
    {
        // Add the new entry
        LeaderboardEntry newEntry = new LeaderboardEntry { name = playerName, score = playerScore };
        leaderboard.entries.Add(newEntry);

        // Sort the leaderboard by score in descending order
        leaderboard.entries.Sort((entry1, entry2) => entry2.score.CompareTo(entry1.score));

        // Save the updated leaderboard
        SaveLeaderboard();

        // Update the leaderboard UI
        UpdateLeaderboardUI();
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboard);
        File.WriteAllText(leaderboardFilePath, json);
    }

    public void LoadLeaderboard()
    {
        if (File.Exists(leaderboardFilePath))
        {
            string json = File.ReadAllText(leaderboardFilePath);
            leaderboard = JsonUtility.FromJson<Leaderboard>(json);
        }

        // Update the leaderboard UI
        UpdateLeaderboardUI();
    }

    private void UpdateLeaderboardUI()
    {
        leaderboardText.text = "<b>Leaderboard:</b>\n";  // Clear the current text

        int index = 0;
        foreach (var entry in leaderboard.entries)
        {
            if(index >= 10) break;  // Only show the top 10 entries

            leaderboardText.text += $"{entry.name}: {entry.score}\n";
            index++;
        }
    }
}
