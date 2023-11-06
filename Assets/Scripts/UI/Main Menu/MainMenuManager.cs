using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI playerIdText;
    [SerializeField] private TextMeshProUGUI playerCashText;
    [SerializeField] private List<Button> betButtons = new List<Button>();
    [SerializeField] private List<TextMeshProUGUI> betAmountButtonTexts = new List<TextMeshProUGUI>();

    [Header("Data")]
    [SerializeField] private List<int> betAmounts = new List<int>();
    private int currentPlayerCash;
    private string betKey = "CURRENT_BET";

    // Start is called before the first frame update
    void Start()
    {
        InitMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void InitMainMenu()
    {
        playerIdText.text   = PlayerInfo.Instance.GetPlayerId;
        currentPlayerCash = PlayerInfo.Instance.GetPlayerCash();
        playerCashText.text = "IDR " + currentPlayerCash.ToString("N0");

        UpdateBetButtonStatus();
    }

    public void ChooseThisBet(int amount)
    {
        PlayerPrefs.SetInt(betKey, amount);
    }

    public void UpdateButtonSelected(int buttonIndex)
    {
        ResetBetButtonColor();

        betButtons[buttonIndex].GetComponent<Image>().color = Color.green;
    }

    private void UpdateBetButtonStatus()
    {
        for(int i = 0; i < betButtons.Count; i++)
        {
            betAmountButtonTexts[i].text = "IDR " + betAmounts[i].ToString("N0");
            if(currentPlayerCash < betAmounts[i])
            {
                betButtons[i].interactable = false;
            }
            else
            {
                betButtons[i].interactable = true;
            }
        }

    }

    private void ResetBetButtonColor()
    {
        for(int i = 0; i < betButtons.Count; i++)
        {
            betButtons[i].GetComponent<Image>().color = Color.white;
        }
    }
}
