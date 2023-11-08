using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalEnum;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] CanvasGroup playerInputs;
    private int currentIndex = -1;
    private int currentPairIndex = -1;

    public void ToggleSingleCombinationInHand()
    {
        ResetAllCards();

        // Move to the next card
        currentIndex = (currentIndex + 1) % player.Hand.Count;

        // Select the card at currentIndex
        var selectedCard = player.Hand[currentIndex];
        selectedCard.ChooseCard();

        // Clear previous selections and add the new one
        player.cardToBeSubmitted.Clear();
        player.cardToBeSubmitted.Add(selectedCard);
    }

    public void TogglePairCombinationInHand()
    {
        ResetAllCards();

        for (int i = currentPairIndex + 1; i < player.Hand.Count; i++)
        {
            for (int j = i + 1; j < player.Hand.Count; j++)
            {
                if (player.Hand[i].Rank == player.Hand[j].Rank)
                {
                    // Select the pair
                    player.Hand[i].ChooseCard();
                    player.Hand[j].ChooseCard();

                    // Clear previous selections and add the new pair
                    player.cardToBeSubmitted.Clear();
                    player.cardToBeSubmitted.Add(player.Hand[i]);
                    player.cardToBeSubmitted.Add(player.Hand[j]);

                    currentPairIndex = i;
                    return;
                }
            }
        }

        currentPairIndex = -1;
    }

    public void SubmitSelectedCards()
    {
        if (player.cardToBeSubmitted.Count == 0)
        {
            // Nothing is selected, so do nothing.
            return;
        }

        List<Card> lastCardsOnTable = gameplayManager.GetCardInTheTable();
        
        if (lastCardsOnTable.Count == 0 || IsCombinationHigher(player.cardToBeSubmitted, lastCardsOnTable))
        {
            gameplayManager.SubmitCard(new List<Card>(player.cardToBeSubmitted));
            player.RemoveCardsFromHand(player.cardToBeSubmitted);
            player.cardToBeSubmitted.Clear();
        }
    }

    private bool IsCombinationHigher(List<Card> cardsToCheck, List<Card> lastCardsOnTable)
    {
        if (lastCardsOnTable.Count == 0) return true;

        CombinationType lastCombinationType = lastCardsOnTable.Count == 1 ? CombinationType.Single : CombinationType.Pair;

        // Compare single cards
        if (lastCombinationType == CombinationType.Single && cardsToCheck.Count == 1)
        {
            return IsCardHigher(cardsToCheck[0], lastCardsOnTable[0]);
        }

        // Compare pairs
        if (lastCombinationType == CombinationType.Pair && cardsToCheck.Count == 2)
        {
            return AreCardsHigher(cardsToCheck, lastCardsOnTable);
        }

        return false;
    }


    private bool IsCardHigher(Card cardToCheck, Card lastCardOnTable)
    {
        return cardToCheck.CompareTo(lastCardOnTable) > 0;
    }

    private bool AreCardsHigher(List<Card> cardsToCheck, List<Card> lastCardsOnTable)
    {
        return cardsToCheck[1].CompareTo(lastCardsOnTable[1]) > 0;
    }

    private void ResetAllCards()
    {
        foreach (var card in player.Hand)
        {
            card.ResetCard();
        }
    }

    public void EnableButton(bool isEnable)
    {
        playerInputs.interactable = isEnable;
    }
}
