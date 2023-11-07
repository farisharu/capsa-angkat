using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalEnum;

public class Player : MonoBehaviour
{
    public List<Card> Hand { get; private set; }
    public bool IsHuman { get; private set; }
    public string Name { get; private set; }
    public int Money { get; private set; }
    public int Bet { get; private set; }

    private PlayerUIManager uIManager;
    private string betKey = "CURRENT_BET";

    void Awake()
    {
        uIManager = GetComponent<PlayerUIManager>();
    }

    public void Initialize (string name, bool isHuman, int currentMoney)
    {
        Name = name;
        IsHuman = isHuman;
        Money = currentMoney;
        Bet = PlayerPrefs.GetInt(betKey);
        Hand = new List<Card>();

        if(IsHuman && uIManager != null)
            uIManager.SetPlayerData();
    }

    public void ReceiveCard(Card card)
    {
        Hand.Add(card);
        uIManager.ArrangeCard(card.GetComponent<Transform>());
    }
}
