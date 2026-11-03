using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;

    int currentLevel = 0;
    GameState currentState;
    public event Action<GameState> OnStateChanged; //avisa de cambio de estado
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.aKey.wasPressedThisFrame)
        {
            Debug.Log("A presionada");

            CardManager.Instance.RandomizeNewCards(); //genera 3 cartas y muestra la seleccion
            CardManager.Instance.ShowCardSelection();
        }
    }
    public int GetCurrentLevel()  //devuelve lvl para q otos puedan acceder
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
                CardManager.Instance.HideCardSelection(); //oculta las cartas durante el juego
                break;
            case GameState.CardSelection:
                CardManager.Instance.ShowCardSelection(); //muestra las cartas durante la seleccion
                break;
        }
    }

    public enum GameState //estados posibles
    {
        Playing,

        CardSelection
    }
}
