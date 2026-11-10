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

    public void CheckLevelUp()
    {
        while (experience >= TO_LEVEL_UP)
        {
            experience -= TO_LEVEL_UP;

            level++;

            GameManager.instance.SetCurrentLevel(level);

            Debug.Log("LEVEL UP " + level);

            if (GameManager.instance != null)
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
    public int GetCurrentLevel()
    {
        return level;
    }

    public int GetCurrentExperience()
    {
        return experience;
    }

    public int GetExpRequired()
    {
        return TO_LEVEL_UP;
    }
}