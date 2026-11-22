using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopItem : MonoBehaviour
{
    [SerializeField] ItemSO item;
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI priceText;
    [Tooltip("Opcional: muestra el nombre del item")]
    [SerializeField] TextMeshProUGUI nameText;
    [Tooltip("Panel que se muestra por encima de Buy y Prize cuando el item ya esta comprado")]
    [SerializeField] GameObject purchasedPanel;
    [Tooltip("Boton Desc. de la fila")]
    [SerializeField] Button descButton;
    [Tooltip("Panel unico de detalle que se comparte entre todas las filas")]
    [SerializeField] ItemDetailPanel detailPanel;

    private void Awake()
    {
        if (buyButton != null) buyButton.onClick.AddListener(Buy);
        if (descButton != null) descButton.onClick.AddListener(ShowDetail);
    }

    private void OnEnable()
    {
        EventManager.OnCoinsChanged += OnCoinsChanged;
        ItemShop.OnItemsChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        EventManager.OnCoinsChanged -= OnCoinsChanged;
        ItemShop.OnItemsChanged -= Refresh;
    }

    private void OnCoinsChanged(float _) => Refresh();

    public void Buy()
    {
        if (item == null) return;
        ItemShop.TryBuy(item);
    }

    public void ShowDetail()
    {
        if (item == null || detailPanel == null) return;
        detailPanel.Show(item);
    }

    private void Refresh()
    {
        if (item == null) return;

        bool owned = item.IsUnlocked;
        if (nameText != null) nameText.text = item.itemName;
        if (priceText != null) priceText.text = Mathf.RoundToInt(item.price).ToString();
        if (buyButton != null) buyButton.interactable = !owned && ItemShop.CanAfford(item);
        if (purchasedPanel != null) purchasedPanel.SetActive(owned);
    }
}
