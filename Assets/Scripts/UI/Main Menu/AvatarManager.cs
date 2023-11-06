using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AvatarManager : MonoBehaviour
{
    [SerializeField] private AvatarButton avatarButtonPrefab;
    [SerializeField] private Transform avatarButtonParent;
    [SerializeField] private GameObject chooseAvatarPanel;
    private List<AvatarButton> avatarButtons = new List<AvatarButton>();

    private string currentSelectedAvatarId;

    // Start is called before the first frame update
    void Start()
    {
        InitiateAvatars();
    }

    private void InitiateAvatars()
    {
        List<AvatarData> datas = PlayerInfo.Instance.GetAllAvatarData();
        for(int i = 0; i < datas.Count; i++)
        {
            AvatarButton button = Instantiate(avatarButtonPrefab, Vector3.zero, Quaternion.identity, avatarButtonParent);
            button.SetAvatarButton(PlayerInfo.Instance.GetSmileFaceById(datas[i].avatarId), datas[i].avatarId, this, i);
            avatarButtons.Add(button);
        }

        if(string.IsNullOrEmpty(PlayerInfo.Instance.GetPlayerAvatarId()))
        {
            ResetAvatarButtonColor();
            ShowChooseAvatarPanel();
        }
        else
        {
            currentSelectedAvatarId = PlayerInfo.Instance.GetPlayerAvatarId();
            Actions.AvatarChange(PlayerInfo.Instance.GetSmileFaceById(currentSelectedAvatarId));
        }
        
    }

    private void ResetAvatarButtonColor()
    {
        for(int i = 0; i < avatarButtons.Count; i++)
        {
            avatarButtons[i].ResetButton();
        }
    }

    public void ChooseThisAvatar(string avatarId, int buttonIndex)
    {
        ResetAvatarButtonColor();
        currentSelectedAvatarId = avatarId;
        avatarButtons[buttonIndex].ChooseThis();
    }

    public void SelectAvatar()
    {
        // set action to change avatar image of the player
        PlayerInfo.Instance.ChangeAvatar(currentSelectedAvatarId);
        Actions.AvatarChange(PlayerInfo.Instance.GetSmileFaceById(currentSelectedAvatarId));
    }

    public void ShowChooseAvatarPanel()
    {
        ResetAvatarButtonColor();
        chooseAvatarPanel.SetActive(true);
    }
}
