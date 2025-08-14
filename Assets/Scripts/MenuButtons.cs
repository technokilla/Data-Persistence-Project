using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Adicione uma referência ao campo de input para validar se o nome foi inserido
    public TMP_InputField playerNameInput;

    public void StartGame()
    {
        // Verifica se o jogador inseriu um nome antes de iniciar
        if (string.IsNullOrWhiteSpace(playerNameInput.text))
        {
            Debug.LogError("Por favor, insira um nome de jogador.");
            // Opcional: Você pode exibir uma mensagem na tela para o usuário
            return;
        }

        // Salva o nome do jogador no ScoreManager antes de transicionar
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetCurrentPlayerName(playerNameInput.text);
        }

        // Carrega a cena do jogo. Certifique-se de que o nome da cena está correto.
        SceneManager.LoadScene("main");
    }

    public void QuitGame()
    {
        // Sai do aplicativo
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}