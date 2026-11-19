using UnityEngine;
using UnityEngine.InputSystem;

public class Pausa : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ScreenManager screenManager; // opcional: si esta, maneja el panel de pausa
    bool enPausa;
    bool cardsPendientes; // true si al pausar estabamos eligiendo una carta

    private const string PauseScreen = "Pause";

    private void SetPanel(bool visible)
    {
        if (screenManager != null)
        {
            if (visible) screenManager.Show(PauseScreen);
            else screenManager.HideAll();
        }
        else if (panelPausa != null) panelPausa.SetActive(visible);
    }

    private void Start()
    {
        Reanudar();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (enPausa) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        enPausa = true;
        Time.timeScale = 0f;

        // Si se estaban eligiendo cartas, se ocultan mientras dura la pausa y se recuerda para restaurarlas.
        var cards = CardManager.Instance;
        cardsPendientes = cards != null && cards.IsSelecting;
        if (cardsPendientes) cards.HideCardSelection();

        SetPanel(true);
        if (cameraController != null) cameraController.LockCamera();
    }

    public void Reanudar()
    {
        enPausa = false;
        SetPanel(false);

        // Si habia cartas pendientes, se vuelven a mostrar y el tiempo sigue frenado hasta que elija una.
        if (cardsPendientes && CardManager.Instance != null)
        {
            CardManager.Instance.ShowCardSelection();
            Time.timeScale = 0f;
            // El cursor tiene que seguir libre para elegir la carta.
            if (cameraController != null) cameraController.LockCamera();
        }
        else
        {
            Time.timeScale = 1f;
            if (cameraController != null) cameraController.UnlockCamera();
        }
        cardsPendientes = false;
    }
}
