using UnityEngine;
using UnityEngine.Advertisements;

public class AdsRewarded : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

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
        Advertisement.Load(adUnitId, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(adUnitId, this);
    }

    #region LoadCallbacks

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded Ad Loaded");
    }

    public void OnUnityAdsFailedToLoad(
        string placementId,
        UnityAdsLoadError error,
        string message)
    {
        Debug.LogError($"Failed To Load Ad: {error} - {message}");
    }

    #endregion


    #region ShowCallbacks

    public void OnUnityAdsShowFailure(
        string placementId,
        UnityAdsShowError error,
        string message)
    {
        Debug.LogError($"Show Ad Failed: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Ad Started");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Ad Clicked");
    }

    public void OnUnityAdsShowComplete(
        string placementId,
        UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId &&
            showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Ads Fully Watched...");

            // Aquí das la recompensa al jugador
            // monedas += 100;
        }

        // Carga el siguiente anuncio
        LoadRewardedAd();
    }

    #endregion
}



