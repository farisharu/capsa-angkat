using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GlobalEnum;
using System.Data.SqlTypes;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI playerCash;
    [SerializeField] TextMeshProUGUI playerBet;
    [SerializeField] Image playerAvatar;
    [SerializeField] Transform playerHand;

    private Player player;

    public void SetPlayer(Player playerRef)
    {
        player = playerRef;
    }
    public void SetPlayerData()
    {
        playerNameText.text = player.Name;
        playerCash.text = "IDR " + player.Money.ToString("N0");
        playerBet.text = "BET: IDR " + player.Bet.ToString("N0");
        
        playerAvatar.sprite = PlayerInfo.Instance.GetSmileFaceById(player.AvatarId);
    }

    public void UpdatePlayerCash()
    {
        playerCash.text = "IDR " + player.Money.ToString("N0");
    }

    public void UpdatePlayerBet()
    {
        playerBet.text = "BET: IDR " + player.Bet.ToString("N0");
    }

    public void UpdatePlayerAvatar(PlayerEmotion emotion)
    {
        switch(emotion)
        {
            case PlayerEmotion.SMILE:
                Actions.AvatarChange(PlayerInfo.Instance.GetSmileFaceById(player.AvatarId));
                break;
            case PlayerEmotion.SAD:
                Actions.AvatarChange(PlayerInfo.Instance.GetSadFaceById(player.AvatarId));
                break;
            default:
                Actions.AvatarChange(PlayerInfo.Instance.GetSmileFaceById(player.AvatarId));
                break;
        }
    }

    public void ArrangeCard(Transform card)
    {
        card.SetParent(playerHand);
        card.gameObject.SetActive(true);
    }
}
