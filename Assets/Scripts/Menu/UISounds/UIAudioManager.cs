using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip openMenuSound;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Botones normales
    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }

    // Cerrar ventanas/paneles
    public void PlayClose()
    {
        audioSource.PlayOneShot(closeSound);
    }

    // Abrir tienda, opciones, inventario, etc.
    public void PlayOpenMenu()
    {
        audioSource.PlayOneShot(openMenuSound);
    }
}
