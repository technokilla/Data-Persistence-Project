using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            // O texto de "SCORE" será o nome do jogador atual + a pontuação
            ScoreText.text = $"{ScoreManager.Instance.currentPlayerName}: {m_Points}";

            // O texto de "BEST SCORE" será o nome do melhor jogador + a melhor pontuação
            BestScoreText.text = $"Best Score: {ScoreManager.Instance.bestScorePlayerName}: {ScoreManager.Instance.bestScore}";
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("StartMenu");
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{ScoreManager.Instance.currentPlayerName}: {m_Points}";

        // Adicionamos esta lógica aqui para verificar o recorde a cada ponto
        if (ScoreManager.Instance.UpdateAndCheckForNewBestScore(m_Points))
        {
            // Se for um novo recorde, atualiza o texto do Best Score imediatamente
            BestScoreText.text = $"Best Score: {ScoreManager.Instance.bestScorePlayerName}: {ScoreManager.Instance.bestScore}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        // Não precisamos mais salvar aqui, pois a atualização já ocorre a cada ponto
        // Mas podemos adicionar uma chamada para salvar o arquivo quando o jogo acaba
        ScoreManager.Instance.SaveScore();
    }
}