using UnityEngine;

[CreateAssetMenu(fileName = "Avatar Data", menuName = "ScriptableObjects/Avatar")]
public class AvatarData : ScriptableObject
{
    public string avatarId;
    public Sprite smileFace;
    public Sprite sadFace;
}
