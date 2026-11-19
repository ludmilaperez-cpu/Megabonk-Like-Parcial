using UnityEngine;

/// <summary>
/// "Lock" = camara bloqueada y cursor libre (menus, pausa, cartas).
/// "Unlock" = camara libre y cursor oculto y centrado (jugando).
/// </summary>
public class CameraController : MonoBehaviour
{
    public bool isCameraLocked;

    private void Start()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnStateChanged -= HandleGameStateChanged;
    }

    // Seleccion de cartas / game over: hace falta el cursor para tocar la UI.
    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing) UnlockCamera();
        else LockCamera();
    }

    public void UnlockCamera()
    {
        isCameraLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LockCamera()
    {
        isCameraLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
