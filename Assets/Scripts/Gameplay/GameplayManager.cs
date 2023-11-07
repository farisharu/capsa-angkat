using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private List<Player> players;
    [SerializeField] private List<Card> initialDeck = new List<Card>();

    [SerializeField] private CardFactory cardFactory;

    private const int CARDS_PER_PLAYER = 13;

    // Start is called before the first frame update
    void Start()
    {
        // initialize players
        InitializePlayer();
        
        // initiate the deck
        initialDeck = cardFactory.CreateDeck();

        // shuffle the deck
        ShuffleDeck();

        // distribute cards to the player
        DistributeCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializePlayer()
    {
        for(int i = 0; i < players.Count; i++)
        {
            players[i].Initialize(PlayerInfo.Instance.GetPlayerId, true, PlayerInfo.Instance.GetPlayerCash());
        }
    }
    
    private void DistributeCards()
    {
        for(int i = 0; i < CARDS_PER_PLAYER; i++)
        {
            players[0].ReceiveCard(initialDeck[i]);
        }
    }

    private void ShuffleDeck()
    {
        int n = initialDeck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card value = initialDeck[k];
            initialDeck[k] = initialDeck[n];
            initialDeck[n] = value;
        }
    }
}
