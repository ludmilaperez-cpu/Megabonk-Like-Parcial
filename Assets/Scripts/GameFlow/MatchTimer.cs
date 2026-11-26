using TMPro;
using UnityEngine;

/// <summary>
/// Cuenta regresiva de la partida. Al llegar a 0 se gana (GameManager -> Victory).
/// Usa tiempo escalado: se frena solo en pausa, seleccion de cartas y GameOver.
/// </summary>
public class MatchTimer : MonoBehaviour
{
    [SerializeField, Min(1f)] private float durationSeconds = 600f;
    [SerializeField] private TextMeshProUGUI timerText;

    private float remaining;
    private int shownSeconds = -1;
    private bool finished;

    private void Start()
    {
        remaining = durationSeconds;
        UpdateText();
    }

    private void Update()
    {
        if (finished) return;

        var gm = GameManager.instance;
        if (gm != null && gm.GetCurrentState() != GameManager.GameState.Playing) return;

        remaining -= Time.deltaTime;
        if (remaining <= 0f)
        {
            remaining = 0f;
            finished = true;
            UpdateText();
            if (gm != null) gm.ChangeState(GameManager.GameState.Victory);
            return;
        }

        UpdateText();
    }

    private void UpdateText()
    {
        if (timerText == null) return;

        int seconds = Mathf.CeilToInt(remaining);
        if (seconds == shownSeconds) return; // solo reescribe el texto cuando cambia el segundo
        shownSeconds = seconds;
        timerText.text = $"{seconds / 60:00}:{seconds % 60:00}";
    }
}
