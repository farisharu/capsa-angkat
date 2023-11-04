using GlobalEnum;

[System.Serializable]
public class MatchHistory
{
    public string battleID;
    public long battleTime;
    public BattleResult battleResult;

    public MatchHistory(string id, long time, BattleResult result)
    {
        battleID = id;
        battleTime = time;
        battleResult = result;
    }
}