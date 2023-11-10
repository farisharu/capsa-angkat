using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalEnum;

public class CardFactory : MonoBehaviour
{
    [SerializeField] private List<Sprite> cardSuitIcons = new List<Sprite>();
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardHolder;
    public List<Card> CreateDeck()
    {
        var deck = new List<Card>();

        foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardRank rank in System.Enum.GetValues(typeof(CardRank)))
            {
                Card newCard = Instantiate(cardPrefab, Vector2.zero, Quaternion.identity, cardHolder);
                newCard.SetCard(suit, rank);
                newCard.SetCardRank(CardRankToSymbol(rank));
                newCard.SetCardSuit(CardSuitToIcon(suit));
                newCard.gameObject.SetActive(false);
                deck.Add(newCard);
            }
        }

        return deck;
    }

    public string CardRankToSymbol(CardRank rank)
    {
        switch (rank)
        {
            case CardRank.Three:    return "3";
            case CardRank.Four:     return "4";
            case CardRank.Five:     return "5";
            case CardRank.Six:      return "6";
            case CardRank.Seven:    return "7";
            case CardRank.Eight:    return "8";
            case CardRank.Nine:     return "9";
            case CardRank.Ten:      return "10";
            case CardRank.Jack:     return "J";
            case CardRank.Queen:    return "Q";
            case CardRank.King:     return "K";
            case CardRank.Ace:      return "A";
            case CardRank.Two:      return "2";
            default: return "";
        }
    }

    public Sprite CardSuitToIcon(CardSuit suit)
    {
        if((int)suit < cardSuitIcons.Count)
            return cardSuitIcons[(int)suit];
        else
            return null;
    }
}
