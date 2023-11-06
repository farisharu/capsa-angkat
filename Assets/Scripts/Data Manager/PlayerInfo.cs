using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalEnum;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public class PlayerInfo : Singleton<PlayerInfo>
{
    private string savePath;

    #region PLAYER_DATA
    [SerializeField] private string playerId;
    private string uid;
    [SerializeField] private int playerCash;
    [SerializeField] private List<MatchHistory> matchHistories = new List<MatchHistory>();
    #endregion
    
    private void Initialize()
    {
        savePath = Application.persistentDataPath + "/playerSaveData.dat";

        PlayerSaveData playerData = LoadPlayerData();
        PopulatePlayerData(playerData);

        // load main menu
        StartCoroutine(GoToMainMenu());
    }

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public void SavePlayerData(PlayerSaveData playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(savePath);

        string json = JsonUtility.ToJson(playerData);
        byte[] encryptedData = Encoding.UTF8.GetBytes(json);

        formatter.Serialize(file, encryptedData);
        file.Close();
    }

    public PlayerSaveData LoadPlayerData()
    {
        if(File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);

            byte[] encryptedData = (byte[])formatter.Deserialize(file);
            string json = Encoding.UTF8.GetString(encryptedData);

            file.Close();

            return JsonUtility.FromJson<PlayerSaveData>(json);
        }
        else
        {
            Debug.Log("Save data is not exist, create new default player data");
            PlayerSaveData playerdata = new PlayerSaveData
            {
                playerId = "CA1000001",
                uid = SystemInfo.deviceUniqueIdentifier,
                playerCash = 500000,
                matchHistoryData = new List<MatchHistory>()
            };

            SavePlayerData(playerdata);

            return playerdata;
        }
    }

    private void PopulatePlayerData(PlayerSaveData data)
    {
        playerId        = data.playerId;
        uid             = data.uid;
        playerCash      = data.playerCash;
        matchHistories  = data.matchHistoryData;
    }

    private void SaveToPlayerData()
    {
        PlayerSaveData playerData = new PlayerSaveData
        {
            playerId = playerId,
            uid = uid,
            playerCash = playerCash,
            matchHistoryData = matchHistories
        };

        SavePlayerData(playerData);
    }

    private IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(2);
        SceneController.Instance.LoadScene("Main Menu");
    }

    #region  GETTER/SETTER

    public string GetPlayerId
    {
        get { return playerId;}
    }

    public int GetPlayerCash()
    {
        return playerCash;
    }

    public void UpdatePlayerCash(int cashChange)
    {
        playerCash += cashChange;

        // save to local
        SaveToPlayerData();
    }

    public List<MatchHistory> GetMatchHistory()
    {
        return matchHistories;
    }

    public void AddMatchHistory(BattleResult result)
    {
        int battleNumber = matchHistories.Count + 1;
        string battleId = "CA1000" + battleNumber.ToString();
        long battleTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        MatchHistory newMatch = new MatchHistory(battleId, battleTime, result);
        matchHistories.Add(newMatch);

        // save to local
        SaveToPlayerData();
    }

    #endregion

}
