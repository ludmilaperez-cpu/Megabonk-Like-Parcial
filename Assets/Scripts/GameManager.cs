using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;

    int currentLevel = 0;
    GameState currentState;
    public event Action<GameState> OnStateChanged;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.aKey.wasPressedThisFrame)
        {
            Debug.Log("A PRESIONADA");
            CardManager.Instance.ShowCardSelection();
        }
    }
    public int GetCurrentLevel() 
    { 
     return currentLevel;
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
                CardManager.Instance.HideCardSelection();
                break;
            case GameState.CardSelection:
                CardManager.Instance.ShowCardSelection();
                break;
        }
    }

    public enum GameState
    {
        Playing,

        CardSelection
    }
}
