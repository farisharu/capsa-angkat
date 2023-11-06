using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarButton : MonoBehaviour
{
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Image avatarIcon;
    private string avatarId;
    private int buttonIndex;
    private AvatarManager avatarManager;

    public void SetAvatarButton(Sprite avatar, string id, AvatarManager manager, int index)
    {
        avatarIcon.sprite = avatar;
        avatarId = id;
        avatarManager = manager;
        buttonIndex = index;
    }

    public void SelectThisAvatar()
    {
        avatarManager.ChooseThisAvatar(avatarId, buttonIndex);
    }

    public void ResetButton()
    {
        buttonBackground.color = Color.white;
    }

    public void ChooseThis()
    {
        buttonBackground.color = Color.green;
    }
}
