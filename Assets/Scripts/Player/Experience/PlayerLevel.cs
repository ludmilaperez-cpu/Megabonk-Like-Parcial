using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLevel : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int level = 1;

    [SerializeField] private int experience;

    //private void Start()
   // {
    //    AddExperience(3000);
   // }

    //experiencia necesaria para subir.
    private int TO_LEVEL_UP
    {
        get
        {
            return level * 1500;
        }
    }

    public void AddExperience(int amount)
    {
        experience += amount;

        Debug.Log("EXP: " + experience);

        CheckLevelUp();
    }

    public void CheckLevelUp() //comprueba si la experiencia actual es suficiente para subir de nivel
    {
        while (experience >= TO_LEVEL_UP)
        {
            experience -= TO_LEVEL_UP;

            level++;

            GameManager.instance.SetCurrentLevel(level); //actualiza el nivel en el GameManager global para que otros sistemas lo sepan

            Debug.Log("LEVEL UP " + level);

            if (GameManager.instance != null) //cambia el estado del juego a seleccion de cartas (abre el menu y pausa el juego)
            {
                GameManager.instance.ChangeState(GameManager.GameState.CardSelection);
            }
        }
    }
    private void Update()
    {
        //SOLO PARA PRUEBAS: L para sumar la experiencia necesaria y subir de nivel
        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            AddExperience(TO_LEVEL_UP);
        }
    }
    public int GetCurrentLevel() //lvl actual
    {
        return level;
    }

    public int GetCurrentExperience() //exp actual
    {
        return experience;
    }

    public int GetExpRequired() //exp requerida para completar el nivel 
    {
        return TO_LEVEL_UP;
    }
}