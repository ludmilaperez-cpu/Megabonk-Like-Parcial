using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Se pone en el panel de un upgrade de la store (ej: "+ Vida  Buy  N").
/// Conecta el boton Buy con el UpgradeManager y mantiene actualizado el precio.
/// </summary>
public class UpgradeShopItem : MonoBehaviour
{
    [SerializeField] UpgradeSO upgrade;
    [SerializeField] Button buyButton;
    [Tooltip("El texto 'N' donde se muestra el precio")]
    [SerializeField] TextMeshProUGUI priceText;
    [Tooltip("Opcional: muestra cuantas veces se compro")]
    [SerializeField] TextMeshProUGUI levelText;

    private void Awake()
    {
        if (buyButton != null) buyButton.onClick.AddListener(Buy);
    }

    private void OnEnable()
    {
        EventManager.OnCoinsChanged += OnCoinsChanged;
        UpgradeManager.OnUpgradesChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        EventManager.OnCoinsChanged -= OnCoinsChanged;
        UpgradeManager.OnUpgradesChanged -= Refresh;
    }

    private void OnCoinsChanged(float _) => Refresh();

    public void Buy()
    {
        if (upgrade == null) return;
        UpgradeManager.TryBuy(upgrade);
    }

    private void Refresh()
    {
        if (upgrade == null) return;
        if (priceText != null) priceText.text = Mathf.RoundToInt(UpgradeManager.GetPrice(upgrade)).ToString();
        if (levelText != null) levelText.text = "Nv. " + UpgradeManager.GetLevel(upgrade);
        if (buyButton != null) buyButton.interactable = UpgradeManager.CanAfford(upgrade);
    }
}
