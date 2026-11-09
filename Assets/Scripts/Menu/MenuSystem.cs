using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject panelSeguro;
    [SerializeField] private Pausa pausa;

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void Start()
    {
        if (panelSeguro != null) panelSeguro.SetActive(false);
    }
    public void Exit()
    {
        panelSeguro.SetActive(true);
    }
    public void Credits() 
    {
       SceneManager.LoadScene("Credits");
    }
    public void Options()
    {
        if (BetwenScenes.Instance != null) BetwenScenes.Instance.MostrarOpciones();
    }
    public void SalirOptions()
    {
        if (BetwenScenes.Instance != null) BetwenScenes.Instance.OcultarOpciones();
    }
    public void Store() 
    {
        SceneManager.LoadScene("Store");
    }
    public void Reanudar()
    {
        pausa.Reanudar();
    }
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    public void Si()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void No()
    {
        panelSeguro.SetActive(false);
    }
}
