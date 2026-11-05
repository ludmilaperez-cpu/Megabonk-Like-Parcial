using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject panelSeguro;

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
        SceneManager.LoadScene("Options");
    }
    public void Store() 
    {
        SceneManager.LoadScene("Store");
    }
    public void BackToMenu()
    {
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
