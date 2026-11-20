using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void WatchAd()
    {
        if (AdManager.instance != null)
        {
            AdManager.instance.ShowRewardedAd();
        }
        else
        {
            Debug.LogWarning("AdManager.instance no encontrado en la escena.");
        }
    }
}
