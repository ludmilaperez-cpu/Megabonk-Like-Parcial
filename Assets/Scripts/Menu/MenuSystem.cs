using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Play() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    [SerializeField] private GameObject panelSeguro;

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
