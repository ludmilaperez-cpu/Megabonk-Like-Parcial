using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public float RunCoins { get; private set; }

    public void AddCoins(float amount)
    {
        RunCoins += amount;
        Debug.Log($"[PlayerCollector] +{amount} (total de la partida: {RunCoins})");
    }
    public void BankCoins()
    {
        if (RunCoins <= 0f) return;
        EventManager.RaiseCoinsChanged(SaveManager.LastLoadedGameData.coins + RunCoins); 
        Debug.Log($"[PlayerCollector] {RunCoins} monedas sumadas al guardado");
        RunCoins = 0f;
    }
}
