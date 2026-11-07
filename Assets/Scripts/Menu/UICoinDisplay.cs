using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICoinDisplay : MonoBehaviour
{
    TextMeshProUGUI displayTarget;

    [Header("Debug")]
    [SerializeField] bool createDebugButton = true;

    private void Awake()
    {
        displayTarget = GetComponent<TextMeshProUGUI>();
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
        if (createDebugButton) CreateDebugButton();
    }
    public void UpdateDisplay(float coins)
    {
        displayTarget.text = Mathf.RoundToInt(coins).ToString();
    }
    public void AddCoin()
    {
        EventManager.RaiseCoinsChanged(SaveManager.LastLoadedGameData.coins + 1);
    }

    void CreateDebugButton()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return;

        GameObject buttonObject = new GameObject("DebugAddCoinButton", typeof(RectTransform), typeof(Image), typeof(Button));
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.SetParent(canvas.transform, false);
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(1f, 0f);
        rect.anchoredPosition = new Vector2(-20f, 20f);
        rect.sizeDelta = new Vector2(180f, 60f);
        buttonObject.GetComponent<Button>().onClick.AddListener(AddCoin);

        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.SetParent(rect, false);
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = labelRect.offsetMax = Vector2.zero;
        TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();
        label.text = "+1 Moneda";
        label.color = Color.black;
        label.alignment = TextAlignmentOptions.Center;
        label.fontSize = 24;
    }
}
