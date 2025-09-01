using System.Text.Json;
using ColourMemory.Models;

namespace ColourMemory.Services;

public static class HighScoreSerializer
{
    private static readonly string filePath = "highscores.json";

    /// <summary>
    /// Saves a new high score to the persistent storage.
    /// </summary>
    public static void SaveHighScore(int score, string name, int difficulty)
    {
        var highScores = LoadHighScore();
        var highScore = new HighScore(score, name, difficulty);
        highScores.Add(highScore);
        var json = JsonSerializer.Serialize(highScores);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Loads the list of high scores from persistent storage.
    /// </summary>
    public static IList<HighScore> LoadHighScore()
    {
        if (!File.Exists(filePath))
            return new List<HighScore>();

        var json = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<List<HighScore>>(json) ?? new List<HighScore>();
    }
}
