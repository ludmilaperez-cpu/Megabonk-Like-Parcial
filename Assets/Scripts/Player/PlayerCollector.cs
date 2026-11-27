using UnityEngine;

/// <summary>
/// Acumula las monedas de la partida y las suma al guardado cuando la partida termina
/// (GameOver o Victory) o cuando el player se destruye, es decir, al salir de la escena
/// por cualquier camino (pausa, pantalla de Win/GameOver, reiniciar...).
/// </summary>
public class PlayerCollector : MonoBehaviour
{
    public float RunCoins { get; private set; }

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        if (gameManager != null) gameManager.OnStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        if (gameManager != null) gameManager.OnStateChanged -= OnStateChanged;
        BankCoins();
    }

    private void OnStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver || state == GameManager.GameState.Victory)
            BankCoins();
    }

    public void AddCoins(float amount)
    {
        RunCoins += amount;
        Debug.Log($"[PlayerCollector] +{amount} (total de la partida: {RunCoins})");
    }

    public void BankCoins()
    {
        if (RunCoins <= 0f) return;

        float banked = RunCoins;
        RunCoins = 0f; // antes de guardar, asi nunca se suma dos veces
        EventManager.RaiseCoinsChanged(SaveManager.LastLoadedGameData.coins + banked);
        Debug.Log($"[PlayerCollector] {banked} monedas sumadas al guardado");
    }
}
