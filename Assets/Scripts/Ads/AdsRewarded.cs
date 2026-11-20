using UnityEngine;
using UnityEngine.Advertisements;

public class AdsRewarded : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;
    [SerializeField] private float coinMultiplier = 1.75f;

    private string adUnitId;

    private void Awake()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID || UNITY_EDITOR
        adUnitId = androidAdUnitId;
#endif
    }

    public void LoadRewardedAd()
    {
        if (string.IsNullOrEmpty(adUnitId))
        {
            Debug.LogWarning("AdUnitId está vacío en AdsRewarded.");
            return;
        }

        Advertisement.Load(adUnitId, this);
    }

    public void ShowRewardedAd()
    {
        Time.timeScale = 0f;
        Advertisement.Show(adUnitId, this);
    }

    private void RestoreTimeScaleAfterAd()
    {
        bool isPlaying = GameManager.instance != null && GameManager.instance.GetCurrentState() == GameManager.GameState.Playing;

        Time.timeScale = isPlaying ? 1f : 0f;
    }

    #region LoadCallbacks

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded Ad Loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed To Load Ad: {error} - {message}");
    }

    #endregion

    #region ShowCallbacks

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Show Ad Failed: {error} - {message}");
        RestoreTimeScaleAfterAd();
        LoadRewardedAd();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Ad Started");
        Time.timeScale = 0f;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Ad Clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log($"[Debug] Anuncio completado: multiplicar monedas por x{coinMultiplier}");
        }

        RestoreTimeScaleAfterAd();
        LoadRewardedAd();
    }

    #endregion
}


