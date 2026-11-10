using UnityEngine;
using System.Collections.Generic;


public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardSelectionUI;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardPositionOne;
    [SerializeField] Transform cardPositionTwo;
    [SerializeField] Transform cardPositionThree;
    [SerializeField] List<CardSO> deck; //cartas disponibles

    GameObject cardOne, cardTwo, cardThree;
    List<CardSO> alreadySelectedCards = new List<CardSO>();

    public static CardManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start() 
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnStateChanged += HandleGameStateChanged;
        }
        else
        {
            Debug.LogError("GameManager.instance es nulo en Start de CardManager.");
        }
    }

    void OnDisable() 
    {
        if (GameManager.instance != null)
            GameManager.instance.OnStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.CardSelection)
        {
            RandomizeNewCards();
            ShowCardSelection();
        }
        else if (state == GameManager.GameState.Playing)
        {
            HideCardSelection();
        }
    }

    public void RandomizeNewCards()
    {
        if (cardOne != null) Destroy(cardOne);
        if (cardTwo != null) Destroy(cardTwo);
        if (cardThree != null) Destroy(cardThree);

        List<CardSO> randomizedCards = new List<CardSO>();
        List<CardSO> availableCards = new List<CardSO>(deck);

        availableCards.RemoveAll(card =>
            (card.isUnique && alreadySelectedCards.Contains(card)) ||
            (card.unlockLevel > GameManager.instance.GetCurrentLevel()));

        if (availableCards.Count < 3)
        {
            Debug.LogWarning("No hay suficientes cartas disponibles para este nivel.");
            return;
        }

        while (randomizedCards.Count < 3)
        {
            CardSO randomCard = availableCards[Random.Range(0, availableCards.Count)];
            if (!randomizedCards.Contains(randomCard))
            {
                randomizedCards.Add(randomCard);
            }
        }

        cardOne = InstantiateCard(randomizedCards[0], cardPositionOne);
        cardTwo = InstantiateCard(randomizedCards[1], cardPositionTwo);
        cardThree = InstantiateCard(randomizedCards[2], cardPositionThree);
    }

    GameObject InstantiateCard(CardSO cardSO, Transform position)
    {
        GameObject cardGo = Instantiate(cardPrefab, position.position, Quaternion.identity, position);
        Card card = cardGo.GetComponent<Card>();
        if (card != null)
        {
            card.Setup(cardSO);
        }
        return cardGo;
    }

    public void SelectCard(CardSO selectedCard)
    {
        Debug.Log("Carta seleccionada: " + selectedCard.cardText);
        if (!alreadySelectedCards.Contains(selectedCard))
        {
            alreadySelectedCards.Add(selectedCard);
        }

        switch (selectedCard.effectType)
        {
            case CardEffect.DamageIncrease:
                Debug.Log("Boost de atq");
                break;
            case CardEffect.HealthIncrease:
                Debug.Log("Boost de health");
                break;
            case CardEffect.SpeedIncrease:
                Debug.Log("Boost de speed");
                break;
        }

        GameManager.instance.ChangeState(GameManager.GameState.Playing);
    }

    public void ShowCardSelection()
    {
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(true);
        }
        else
        {
            Debug.LogError("CardSelectionUI no esta asignado en el Inspector de CardManager");
        }
    }

    public void HideCardSelection()
    {
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(false);
        }
    }
}
