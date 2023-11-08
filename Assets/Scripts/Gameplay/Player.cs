using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalEnum;
using System.Linq;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Card> Hand { get; private set; }
    public bool IsHuman { get; private set; }
    public string Name { get; private set; }
    public int Money { get; private set; }
    public int Bet { get; private set; }
    public string AvatarId {get; private set; }
    public bool isPass;

    public List<Card> cardToBeSubmitted = new List<Card>();

    public PlayerUIManager UIManager { get; private set; }

    private GameplayManager gameplayManager;
    private PlayerInputController inputController;
    private string betKey = "CURRENT_BET";

    public int currentSelectedIndex = -1;

    void Awake()
    {
        UIManager = GetComponent<PlayerUIManager>();
        gameplayManager = FindObjectOfType<GameplayManager>();
        inputController = FindObjectOfType<PlayerInputController>();
    }

    public void Initialize (string name, bool isHuman, int currentMoney, string avatarId)
    {
        Name        = name;
        IsHuman     = isHuman;
        Money       = currentMoney;
        AvatarId    = avatarId;
        Hand = new List<Card>();

        if(IsHuman)
            Bet = PlayerPrefs.GetInt(betKey);
        else
            Bet = CalculateBet(Money);

        UIManager.SetPlayer(this);
        UIManager.SetPlayerData();
    }

    public void ReceiveCard(Card card)
    {
        Hand.Add(card);
        if(IsHuman)
            card.FlipCard(false);
        UIManager?.ArrangeCard(card.transform);
    }

    private int CalculateBet(int moneyHas)
    {
        if(moneyHas > 1000)
            return Random.Range(100, 1000);
        else
            return 10;
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        UIManager.UpdatePlayerCash();

        if(IsHuman)
            PlayerInfo.Instance.UpdatePlayerCash(amount);
    }

    public void PenaltyMoney()
    {
        Money -= Bet;

        UIManager.UpdatePlayerCash();

        if(IsHuman)
            PlayerInfo.Instance.UpdatePlayerCash(Bet);
    }

    public void PlayerPass()
    {
        isPass = true;
        gameplayManager.PlayerPass(this);
    }

    #region Player_Moves
    public List<Card> GetCardsToBeSubmitted()
    {
        return cardToBeSubmitted;
    }

    public void CheckCardInTheTable()
    {
        // check if there is card in the table
        if(gameplayManager.GetCardInTheTable().Count == 0)
        {
            // can submit anything
        }
        else
        {
            // check what is the combination
            CombinationType combinationInTable = CheckCombination(gameplayManager.GetCardInTheTable());
        }
    }

    public void SortCard()
    {
        Hand.Sort((card1, card2) =>
        {
            int rankComparison = card1.Rank.CompareTo(card2.Rank);
            if (rankComparison != 0)
            {
                return rankComparison;
            }
    
            return card1.Suit.CompareTo(card2.Suit);
        });

        for(int i = 0; i < Hand.Count; i++)
        {
            Hand[i].transform.SetAsLastSibling();
        }
    }

    private CombinationType CheckCombination(List<Card> cards)
    {
        cards.Sort((card1, card2) =>
        {
            int rankComparison = card1.Rank.CompareTo(card2.Rank);
            if (rankComparison != 0)
            {
                return rankComparison;
            }
    
            return card1.Suit.CompareTo(card2.Suit);
        });

        if(cards.Count == 1)
            return CombinationType.Single;

        if(cards.Count == 2)
            return CombinationType.Pair;

        if(cards.Count == 3)
            return CombinationType.ThreeOfAKind;

        if(HasStraight(cards))
            return CombinationType.Straight;
        
        if(HasFlush(cards))
            return CombinationType.Flush;

        if(HasFullHouse(cards))
            return CombinationType.FullHouse;

        if(HasFourOfAKind(cards))
            return CombinationType.FourOFAKind;

        if(HasStraightFlush(cards))
            return CombinationType.StraightFlush;

        if(HasRoyalFlush(cards))
            return CombinationType.RoyalFlush;

        return CombinationType.None;
    }

    private bool HasStraight(List<Card> cards)
    {
        if (cards.Count < 5) return false;

        int straightCount = 1;
    
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].Rank == CardRank.Ace && cards[i + 1].Rank == CardRank.Two)
            {
                straightCount++;
            }
            else if (cards[i + 1].Rank - cards[i].Rank == 1)
            {
                straightCount++;
            }
            else
            {
                straightCount = 1;
            }

            if (straightCount >= 5)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasFlush(List<Card> cards)
    {
        if (cards.Count < 5) return false;
        
        var suitCounts = new Dictionary<CardSuit, int>();
        foreach (var card in cards)
        {
            if (suitCounts.ContainsKey(card.Suit))
            {
                suitCounts[card.Suit]++;
            }
            else
            {
                suitCounts.Add(card.Suit, 1);
            }
        }

        foreach (var suitCount in suitCounts)
        {
            if (suitCount.Value >= 5)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasFullHouse(List<Card> cards)
    {
        if (cards.Count < 5) return false;

        var rankCounts = new Dictionary<CardRank, int>();
        foreach (var card in cards)
        {
            if (rankCounts.ContainsKey(card.Rank))
            {
                rankCounts[card.Rank]++;
            }
            else
            {
                rankCounts.Add(card.Rank, 1);
            }
        }

        bool hasThreeOfAKind = false;
        bool hasPair = false;

        foreach (var rankCount in rankCounts)
        {
            if (rankCount.Value == 3)
            {
                hasThreeOfAKind = true;
            }
            else if (rankCount.Value == 2)
            {
                hasPair = true;
            }

            if (hasThreeOfAKind && hasPair)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasFourOfAKind(List<Card> cards)
    {
        if (cards.Count != 5) return false;

        var rankCounts = new Dictionary<CardRank, int>();
        foreach (var card in cards)
        {
            if (rankCounts.ContainsKey(card.Rank))
            {
                rankCounts[card.Rank]++;
            }
            else
            {
                rankCounts.Add(card.Rank, 1);
            }
        }

        return rankCounts.Any(rankCount => rankCount.Value == 4);
    }

    public bool HasStraightFlush(List<Card> cards)
    {
        if (cards.Count != 5) return false;

        bool sameSuit = cards.All(c => c.Suit == cards[0].Suit);
        if (!sameSuit)
        {
            return false;
        }

        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i + 1].Rank - cards[i].Rank != 1)
            {
                return false;
            }
        }

        return true;
    }

    public bool HasRoyalFlush(List<Card> cards)
    {
        if (!HasStraightFlush(cards))
        {
            return false;
        }

        return cards[0].Rank == CardRank.Ten && cards[4].Rank == CardRank.Ace;
    }

    #endregion

    #region BOT_Move
    public List<Card> GetPlayableCards(List<Card> cardsOnTable)
    {
        List<Card> playableCards = new List<Card>();

        if (cardsOnTable.Count == 0)
        {
            return Hand;
        }

        // Determine if the cards on the table are a pair or a single card.
        bool isPairOnTable = cardsOnTable.Count == 2 && cardsOnTable[0].Rank == cardsOnTable[1].Rank;

        // Find all higher rank cards or pairs in hand depending on the table.
        foreach (var card in Hand)
        {
            if (isPairOnTable)
            {
                var matchingCards = Hand.Where(c => c.Rank == card.Rank).ToList();
                if (matchingCards.Count >= 2 && card.Rank > cardsOnTable[0].Rank)
                {
                    playableCards.Add(matchingCards[0]);
                    playableCards.Add(matchingCards[1]);
                    break;
                }
            }
            else
            {
                if (card.Rank > cardsOnTable[0].Rank || (card.Rank == cardsOnTable[0].Rank && card.Suit > cardsOnTable[0].Suit))
                {
                    playableCards.Add(card);
                    break;
                }
            }
        }

        return playableCards;
    }
    #endregion
    public void RemoveCardsFromHand(List<Card> cardsToRemove)
    {
        foreach (var card in cardsToRemove)
        {
            Hand.Remove(card);
        }
    }
}
