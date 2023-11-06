using System;
using UnityEngine;

public static class Actions
{
    public static event Action<Sprite> OnAvatarChange;
    public static void AvatarChange(Sprite newAvatar)
    {
        OnAvatarChange?.Invoke(newAvatar);
    }
}
