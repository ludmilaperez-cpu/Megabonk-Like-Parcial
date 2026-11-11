using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int currentLevel = 1;
    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    GameState currentState;
    public event Action<GameState> OnStateChanged; //avisa de cambio de estado

    private void Awake()
    {
        instance = this;
    }

    public int GetCurrentLevel() //devuelve lvl para q otros puedan acceder
    {
        return currentLevel;
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(newState);
        HandleStateChanged();
    }

    private void HandleStateChanged() //decide q hacer segun el estado
    {
        switch (currentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f; //reanuda el tiempo del juego
                if (CardManager.Instance != null)
                    CardManager.Instance.HideCardSelection();
                break;

            case GameState.CardSelection:
                Time.timeScale = 0f; //pausa el tiempo del juego (enemigos, movimiento, etc.)
                if (CardManager.Instance != null)
                    CardManager.Instance.ShowCardSelection();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f; //pausa el juego también al morir
                break;
        }
    }
    private void Update()
    {
        //SOLO PARA PRUEBAS: G para simular que el jugador murio
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            Debug.Log("Simulando Game Over con la tecla G");
            ChangeState(GameState.GameOver);
        }
    }

    public enum GameState //estados posibles
    {
        Playing,
        CardSelection,
        GameOver
    }
}
