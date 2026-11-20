using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardSelectionUI;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardPositionOne;
    [SerializeField] Transform cardPositionTwo;
    [SerializeField] Transform cardPositionThree;
    [SerializeField] List<CardSO> deck; //cartas disponibles
    [SerializeField] private List<GameObject> particlesObjects = new List<GameObject>();

    PlayerController player;
    GameObject cardOne, cardTwo, cardThree;
    List<CardSO> alreadySelectedCards = new List<CardSO>();

    public static CardManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start() //se suscribe al evento de cambio de estado del GameManager
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnStateChanged += HandleGameStateChanged;
        }
        else
        {
            Debug.LogError("GameManager.instance es nulo en Start de CardManager.");
        }

        HideCardSelection();
        SetParticlesActive(false);
    }

    void OnDisable() //desuscribe del evento al desactivarse o destruirse para evitar errores
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
        else if (state == GameManager.GameState.Playing) //si el juego vuelve a la normalidad oculta el menu
        {
            HideCardSelection();
        }
    }

    public void RandomizeNewCards() //selecciona 3 cartas aleatorias validas y las instancia en pantalla
    {
        if (cardOne != null) Destroy(cardOne);
        if (cardTwo != null) Destroy(cardTwo);
        if (cardThree != null) Destroy(cardThree);

        List<CardSO> randomizedCards = new List<CardSO>();
        List<CardSO> availableCards = new List<CardSO>(deck);

        availableCards.RemoveAll(card => (card.isUnique && alreadySelectedCards.Contains(card)) ||
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
        GameObject cardGo = Instantiate(cardPrefab, position, false);
        RectTransform rect = cardGo.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;
        }

        Card card = cardGo.GetComponent<Card>();
        if (card != null)
        {
            card.Setup(cardSO);
        }
        return cardGo;
    }

    public void SelectCard(CardSO selectedCard) //se ejecuta cuando el jugador hace clic en una carta especifica
    {
        if (!IsSelecting) return;

        Debug.Log("Carta seleccionada: " + selectedCard.cardText);
        if (!alreadySelectedCards.Contains(selectedCard))
        {
            alreadySelectedCards.Add(selectedCard);
        }

        if (player == null) player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.ModifyStat(selectedCard.Stat, selectedCard.effectValue, selectedCard.isPercentage);
        }
        else
        {
            Debug.LogError("No se encontro un PlayerController para aplicarle la carta");
        }

        GameManager.instance.ChangeState(GameManager.GameState.Playing);
    }

    public bool IsSelecting => cardSelectionUI != null && cardSelectionUI.activeSelf;

    public void ShowCardSelection()
    {
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(true);
        }

        SetParticlesActive(true);
    }

    public void HideCardSelection()
    {
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(false);
        }

        SetParticlesActive(false);
    }

    private void SetParticlesActive(bool isActive)
    {
        foreach (GameObject particle in particlesObjects)
        {
            if (particle != null)
            {
                particle.SetActive(isActive);
            }
        }
    }
}