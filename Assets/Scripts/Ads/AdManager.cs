using UnityEngine;

public class AdManager : MonoBehaviour
{
    public InitializateAd initializeAds;
    public AdsRewarded rewardedAds;

    [Header("UI del Panel de Anuncios / Game Over")]
    [SerializeField] private GameObject adPanel;

    public static AdManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //nos aseguramos de que el panel comience oculto
        if (adPanel != null)
        {
            adPanel.SetActive(false);
        }
    }

    private void Start()
    {
        if (rewardedAds != null)
        {
            rewardedAds.LoadRewardedAd();
        }

        //suscrir a los cambios de estado del GameManager 
        if (GameManager.instance != null)
        {
            GameManager.instance.OnStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
        {
            ShowAdPanel();
        }
        else
        {
            //si el juego pasa a Playing oculta el panel
            HideAdPanel();
        }
    }

    public void ShowAdPanel()
    {
        if (adPanel != null)
        {
            adPanel.SetActive(true);
        }
    }

    public void HideAdPanel()
    {
        if (adPanel != null)
        {
            adPanel.SetActive(false);
        }
    }

    public void ShowRewardedAd()
    {
        if (rewardedAds != null)
        {
            rewardedAds.ShowRewardedAd();
        }
    }
}