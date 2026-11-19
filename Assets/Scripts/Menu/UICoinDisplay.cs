using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICoinDisplay : MonoBehaviour
{
    TextMeshProUGUI displayTarget;


    private void Awake()
    {
        displayTarget = GetComponentInChildren<TextMeshProUGUI>(true);
    }
    private void OnEnable()
    {
        EventManager.OnCoinsChanged += UpdateDisplay;
        UpdateDisplay(SaveManager.LastLoadedGameData.coins);
    }
    private void OnDisable()
    {
        EventManager.OnCoinsChanged -= UpdateDisplay;
    }
    private void Start()
    {
       
    }
    public void UpdateDisplay(float coins)
    {
        displayTarget.text = Mathf.RoundToInt(coins).ToString();
    }
    public void AddCoin()
    {
        EventManager.RaiseCoinsChanged(SaveManager.LastLoadedGameData.coins + 1);
    }

    
}
