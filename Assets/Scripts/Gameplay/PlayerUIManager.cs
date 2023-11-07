using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GlobalEnum;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI playerCash;
    [SerializeField] TextMeshProUGUI playerBet;
    [SerializeField] Transform playerHand;

    private string betKey = "CURRENT_BET";
    private string currentAvatarId;

    public void SetPlayerData()
    {
        playerNameText.text = PlayerInfo.Instance.GetPlayerId;
        playerCash.text = "IDR " + PlayerInfo.Instance.GetPlayerCash().ToString("N0");
        playerBet.text = "BET: IDR " + PlayerPrefs.GetInt(betKey).ToString("N0");
        
        currentAvatarId = PlayerInfo.Instance.GetPlayerAvatarId();
        Actions.AvatarChange(PlayerInfo.Instance.GetSmileFaceById(currentAvatarId));
    }

    public void UpdatePlayerCash()
    {
        playerCash.text = "IDR " + PlayerInfo.Instance.GetPlayerCash().ToString("N0");
    }

    public void UpdatePlayerBet()
    {
        playerBet.text = "BET: IDR " + PlayerPrefs.GetInt(betKey).ToString("N0");
    }

    public void UpdatePlayerAvatar(PlayerEmotion emotion)
    {
        switch(emotion)
        {
            case PlayerEmotion.SMILE:
                Actions.AvatarChange(PlayerInfo.Instance.GetSmileFaceById(currentAvatarId));
                break;
            case PlayerEmotion.SAD:
                Actions.AvatarChange(PlayerInfo.Instance.GetSadFaceById(currentAvatarId));
                break;
            default:
                Actions.AvatarChange(PlayerInfo.Instance.GetSmileFaceById(currentAvatarId));
                break;
        }
    }

    public void ArrangeCard(Transform card)
    {
        card.SetParent(playerHand);
        card.gameObject.SetActive(true);
    }
}
