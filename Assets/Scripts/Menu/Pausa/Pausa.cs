using UnityEngine;
using UnityEngine.InputSystem;

public class Pausa : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ScreenManager screenManager; // opcional: si esta, maneja el panel de pausa
    bool enPausa;

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
        SetPanel(true);
        if (cameraController != null) cameraController.LockCamera();
    }

    public void Reanudar()
    {
        enPausa = false;
        Time.timeScale = 1f;
        SetPanel(false);
        if (cameraController != null) cameraController.UnlockCamera();
    }
}
