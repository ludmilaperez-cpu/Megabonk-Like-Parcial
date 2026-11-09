using UnityEngine;
using UnityEngine.InputSystem;

public class Pausa : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private CameraController cameraController;
    bool enPausa;

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
        panelPausa.SetActive(true);
        if (cameraController != null) cameraController.LockCamera();
    }

    public void Reanudar()
    {
        enPausa = false;
        Time.timeScale = 1f;
        panelPausa.SetActive(false);
        if (cameraController != null) cameraController.UnlockCamera();
    }
}
