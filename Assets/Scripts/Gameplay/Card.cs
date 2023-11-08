using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalEnum;
using TMPro;

public class Card : MonoBehaviour
{
    public CardSuit Suit { get; private set; }
    public CardRank Rank { get; private set; }

    public TextMeshProUGUI cardRankText;
    public Image cardSuitIcon;
    public Image cardBackground;

    public GameObject backCover;

    public void SetCard(CardSuit suit, CardRank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public void SetCardRank(string rankText)
    {
        cardRankText.text = rankText;
    }

    public void SetCardSuit(Sprite suitIcon)
    {
        cardSuitIcon.sprite = suitIcon;
    }

    public void FlipCard(bool isBack)
    {
        backCover.SetActive(isBack);
    }

    public void ChooseCard()
    {
        cardBackground.color = Color.green;
    }
    
    public void ResetCard()
    {
        cardBackground.color = Color.white;
    }

    public int CompareTo(Card other)
    {
        int rankComparison = this.Rank.CompareTo(other.Rank);
        if (rankComparison != 0)
        {
            return rankComparison;
        }

        return this.Suit.CompareTo(other.Suit);
    }
}
