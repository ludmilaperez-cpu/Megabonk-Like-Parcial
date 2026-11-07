using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isCameraLocked;


    void Update()
    {
        if (isCameraLocked)
        {
            return;
        }
        else
        {
            UnlockCamera();
        }
    }

    public void UnlockCamera()
    {
        isCameraLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LockCamera()
    {
        isCameraLocked = true;
        Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }
}
