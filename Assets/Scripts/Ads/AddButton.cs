using UnityEngine;

public class AddButton : MonoBehaviour
{
    public void ExecuteButton()
    {
        AdManager.instance.ShowRewardedAd();
    }
}
