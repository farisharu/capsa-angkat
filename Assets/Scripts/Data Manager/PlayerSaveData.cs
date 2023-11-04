using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData
{
    public string playerId;
    public string uid;
    public int playerCash;
    public List<MatchHistory> matchHistoryData;
}
