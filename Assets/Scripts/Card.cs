using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image cardImageRenderer;
    [SerializeField] TextMeshProUGUI cardTextRenderer;

    private CardSO cardInfo;

    public void Setup(CardSO card)
    {
        cardInfo = card;

        cardImageRenderer.sprite = card.cardImage;
        cardTextRenderer.text = card.cardText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("elegiste una carta");
        CardManager.Instance.SelectCard(cardInfo);
    }
}