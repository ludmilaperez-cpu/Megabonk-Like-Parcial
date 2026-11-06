using UnityEngine;

public class AdManager : MonoBehaviour
{
    public InitializateAd initializeAds;
    public AdsRewarded rewardedAds;

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
    }

    private void Start()
    {
        if (rewardedAds != null)
        {
            rewardedAds.LoadRewardedAd();
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
