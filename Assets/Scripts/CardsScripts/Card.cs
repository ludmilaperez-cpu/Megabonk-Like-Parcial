using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image cardImageRenderer;
    [SerializeField] TextMeshProUGUI cardTextRenderer;

    private CardSO cardInfo; //guarda info de carta

    public void Setup(CardSO card) //carta con imagen y el texto scriptableobject
    {
        cardInfo = card;

        cardImageRenderer.sprite = card.cardImage;
        cardTextRenderer.text = card.cardText;
    }

    public void OnPointerClick(PointerEventData eventData) //cuando el jugador hace click se eejcuta
    {
        Debug.Log("elegiste una carta");
        CardManager.Instance.SelectCard(cardInfo); //avisa q carta se eligio
    }
}