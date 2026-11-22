using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descText;
    [SerializeField] Button closeButton;

    private void Awake()
    {
        if (closeButton != null) closeButton.onClick.AddListener(Hide);
    }

    public void Show(ItemSO item)
    {
        nameText.text = item.itemName;
        descText.text = item.desc;
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);
}
