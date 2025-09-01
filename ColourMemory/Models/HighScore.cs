namespace ColourMemory.Models;

public class HighScore
{
    public int Score { get; set; }
    public string Name { get; set; }
    public int Difficulty { get; set; }

    public HighScore(int score, string name, int difficulty)
    {
        Score = score;
        Name = name;
        Difficulty = difficulty;
    }
}
