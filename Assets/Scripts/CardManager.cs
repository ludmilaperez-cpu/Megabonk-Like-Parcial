using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

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

    void Awake() //se subscribe al gamemanager si existe
    {
        Instance = this;
        if (GameManager.instance != null)
            GameManager.instance.OnStateChanged += HandleGameStateChanged;
        
        
        
    }

    void onDisable() //se desuscribe cuando se destruye el obj
    {
        if (GameManager.instance != null)
            GameManager.instance.OnStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState state) //recibe cambio de estado en el juego
    {
        if (state == GameManager.GameState.CardSelection) ;
    }

    

    public void RandomizeNewCards() //destruye las cartas anteriores si existen
    {
        if (cardOne != null) Destroy(cardOne);
        if (cardTwo != null) Destroy(cardTwo);
        if (cardThree != null) Destroy(cardThree);

        List<CardSO> randomizedCards = new List<CardSO>(); //cartas elegibles



        List<CardSO> availableCards = new List<CardSO>(deck); //filtrar cartas disponibles


        availableCards.RemoveAll(card =>
            card.isUnique && alreadySelectedCards.Contains(card)
            || card.unlockLevel > GameManager.instance.GetCurrentLevel());  //elimina cartas unicas ya elegidas y las q su nivel de desbloqueo sean mas altas que el nivel del jugador

        if (availableCards.Count < 3)
        {
            Debug.Log("No hay suficientes cartas");
            return;
        }

        while (randomizedCards.Count < 3) //elige 3 cartas aleatorias sin repetir
        {
            CardSO randomCard = availableCards[Random.Range(0, availableCards.Count)];

            if (!randomizedCards.Contains(randomCard))
            {
                randomizedCards.Add(randomCard);
            }
        }

        cardOne = InstantiateCard(randomizedCards[0], cardPositionOne); //crea las cartas en sus posiciones
        cardTwo = InstantiateCard(randomizedCards[1], cardPositionTwo);
        cardThree = InstantiateCard(randomizedCards[2], cardPositionThree);
    }

    GameObject InstantiateCard(CardSO cardSO, Transform position) //le asisgna los datos en su posicion a la carta
    {
        GameObject cardGo = Instantiate(
            cardPrefab,
            position.position,
            Quaternion.identity,
            position);

        Card card = cardGo.GetComponent<Card>();

        if (card != null)
        {
            card.Setup(cardSO);
        }

        return cardGo;
    }

    public void SelectCard(CardSO selectedCard) // guarda la carta elegida

    {
        Debug.Log("Carta seleccionada: " + selectedCard.cardText);
        if (!alreadySelectedCards.Contains(selectedCard))
        {
            alreadySelectedCards.Add(selectedCard);
        }

        //uestra un texto segun el tipo de carta elegida
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



        GameManager.instance.ChangeState(GameManager.GameState.Playing); //vuelve al estado normal y oculta la seleccion 

    }

    public void ShowCardSelection() //interfaz seleccion 
    {
        cardSelectionUI.SetActive(true);
    }

    public void HideCardSelection()  //la oculta
    {
        cardSelectionUI.SetActive(false);
    }

}
