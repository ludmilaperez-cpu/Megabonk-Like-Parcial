using UnityEngine;

public class BetwenScenes : MonoBehaviour
{
    public static BetwenScenes Instance { get; private set; }

    [SerializeField] private GameObject panelOpciones;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        panelOpciones.SetActive(false);
    }

    public void MostrarOpciones()
    {
        panelOpciones.SetActive(true);
    }

    public void OcultarOpciones()
    {
        panelOpciones.SetActive(false);
    }
}
