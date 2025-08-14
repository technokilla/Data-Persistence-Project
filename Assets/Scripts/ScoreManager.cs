using UnityEngine;
using System.IO;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int currentScore = 0;
    public int bestScore = 0;
    public string bestScorePlayerName = "N/A"; // Nome do jogador com o melhor score
    public string currentPlayerName; // Nome do jogador atual

    public TextMeshProUGUI bestScoreText;
    public TMP_InputField playerNameInput; // Campo de entrada para o nome do jogador

    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        filePath = Path.Combine(Application.persistentDataPath, "scoredata.json");
        LoadScore();
    }

    // Método para ser chamado no InputField da primeira cena
    public void SetCurrentPlayerName(string name)
    {
        currentPlayerName = name;
    }

    public void SaveScore()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            bestScorePlayerName = currentPlayerName; // Salva o nome do jogador atual
        }

        ScoreData data = new ScoreData(bestScore, bestScorePlayerName); // Passa o nome também
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(filePath, json);

        Debug.Log("Score saved: " + bestScore + " by " + bestScorePlayerName);
    }

    private void LoadScore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);

            bestScore = data.bestScore;
            bestScorePlayerName = data.bestScorePlayerName;
            Debug.Log("Score loaded: " + bestScore + " by " + bestScorePlayerName);
        }
        else
        {
            bestScore = 0;
            bestScorePlayerName = "N/A";
            Debug.Log("No save file found. Initializing scores.");
        }

        UpdateBestScoreUI();
    }

    public void UpdateBestScoreUI()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = $"Best Score: {bestScorePlayerName} - {bestScore}";
        }
    }

    public void UpdateAndSaveScore(int finalScore)
    {
        currentScore = finalScore;
        SaveScore();
        UpdateBestScoreUI();
    }
  

    public bool UpdateAndCheckForNewBestScore(int finalScore)
    {
        currentScore = finalScore;
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            bestScorePlayerName = currentPlayerName;
            // Não salva o arquivo aqui, apenas retorna que houve uma atualização
            return true;
        }
        return false;
    }
}