using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Si algo crea un EventSystem en runtime con el StandaloneInputModule (input viejo),
/// lo reemplaza por InputSystemUIInputModule. Se ejecuta solo, no hay que ponerlo en ninguna escena.
/// </summary>
public static class InputModuleFixer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Fix()
    {
        foreach (var old in Object.FindObjectsByType<StandaloneInputModule>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            GameObject go = old.gameObject;
            Debug.LogWarning($"[InputModuleFixer] '{go.name}' tenia StandaloneInputModule; lo reemplazo por InputSystemUIInputModule.", go);
            Object.DestroyImmediate(old);
            if (go.GetComponent<InputSystemUIInputModule>() == null)
                go.AddComponent<InputSystemUIInputModule>();
        }
    }
}
