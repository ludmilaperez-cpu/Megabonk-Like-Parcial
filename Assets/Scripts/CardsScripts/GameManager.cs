using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winScreen;

    public static GameManager instance;

    private int currentLevel = 1;
    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    private GameState currentState;
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (winScreen != null)
            winScreen.SetActive(false);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public GameState GetCurrentState()
    {
        return currentState;
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(newState);
        HandleStateChanged();
    }

    private void HandleStateChanged()
    {
        switch (currentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                if (gameOverPanel != null)
                    gameOverPanel.SetActive(false);
                if (CardManager.Instance != null)
                    CardManager.Instance.HideCardSelection();
                break;

            case GameState.CardSelection:
                Time.timeScale = 0f;
                if (CardManager.Instance != null)
                    CardManager.Instance.ShowCardSelection();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                if (gameOverPanel != null)
                    gameOverPanel.SetActive(true);
                break;

            case GameState.Victory:
                Time.timeScale = 0f;
                if (winScreen != null)
                    winScreen.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        // SOLO PARA PRUEBAS: G para simular que el jugador murió
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            Debug.Log("Simulando Game Over con la tecla G");
            ChangeState(GameState.GameOver);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu(string mainMenuSceneName = "MainMenu")
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public enum GameState
    {
        Playing,
        CardSelection,
        GameOver,
        Victory
    }
}
