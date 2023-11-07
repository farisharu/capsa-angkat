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
}
