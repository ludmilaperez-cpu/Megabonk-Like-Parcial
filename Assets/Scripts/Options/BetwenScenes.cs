using UnityEngine;

public class BetwenScenes : MonoBehaviour
{
    private void Awake()
    {
        var noDestruirEntreEscenas = FindObjectsByType<BetwenScenes>(FindObjectsSortMode.None);
        if (noDestruirEntreEscenas.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
