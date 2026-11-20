using UnityEngine;
using UnityEngine.InputSystem;
using ScreenManager;
using ScreenStack = ScreenManager.ScreenManager;

public class Pausa : MonoBehaviour
{
    [SerializeField] private SimpleUiScreen pauseScreen;
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private CameraController cameraController;

    private bool enPausa;
    private bool cardsPendientes;

    private bool UsaScreens => pauseScreen != null && ScreenStack.Instance != null;

    private void Start()
    {
        if (!UsaScreens && panelPausa != null) panelPausa.SetActive(false);
        Reanudar();
    }

    private void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;

        if (!enPausa) Pausar();
        else if (UsaScreens && ScreenStack.Instance.Count > 2) ScreenStack.Instance.Pop();
        else Reanudar();
    }

    public void Pausar()
    {
        if (enPausa) return;
        enPausa = true;
        Time.timeScale = 0f;

        var cards = CardManager.Instance;
        //cardsPendientes = cards != null && cards.IsSelecting;
        if (cardsPendientes) cards.HideCardSelection();

        if (UsaScreens) ScreenStack.Instance.Push(pauseScreen);
        else if (panelPausa != null) panelPausa.SetActive(true);

        if (cameraController != null) cameraController.LockCamera();
    }

    public void Reanudar()
    {
        enPausa = false;

        if (UsaScreens)
        {
            var screens = ScreenStack.Instance;
            while (screens.Count > 1) screens.Pop();
        }
        else if (panelPausa != null) panelPausa.SetActive(false);

        if (cardsPendientes && CardManager.Instance != null)
        {
            CardManager.Instance.ShowCardSelection();
            Time.timeScale = 0f;
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
