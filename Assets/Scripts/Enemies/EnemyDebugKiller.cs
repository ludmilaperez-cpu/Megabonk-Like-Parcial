using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyDebugKiller : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame)
        {
            Enemy enemy = FindObjectOfType<Enemy>();

            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}
