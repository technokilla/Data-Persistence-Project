using System;

[Serializable]
public class ScoreData
{
    public int bestScore;
    public string bestScorePlayerName; // Adicionamos esta linha

    public ScoreData(int initialScore, string playerName)
    {
        bestScore = initialScore;
        bestScorePlayerName = playerName;
    }
}