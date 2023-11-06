using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarDisplay : MonoBehaviour
{
    [SerializeField] private Image avatarIcon;

    void Awake()
    {
        Actions.OnAvatarChange += ChangeAvatar;
    }

    void OnDestroy()
    {
        Actions.OnAvatarChange -= ChangeAvatar;
    }

    private void ChangeAvatar(Sprite newAvatar)
    {
        Debug.Log("Avatar Changed");
        avatarIcon.sprite = newAvatar;
    }
}
