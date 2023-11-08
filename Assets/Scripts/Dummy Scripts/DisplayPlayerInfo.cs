using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Player Id: " + PlayerInfo.Instance.GetPlayerId);
        //Debug.Log("Player Cash: " + PlayerInfo.Instance.GetPlayerCash());
        for(int i = 0; i < PlayerInfo.Instance.GetMatchHistory().Count; i++)
        {
            //Debug.Log("Match " + PlayerInfo.Instance.GetMatchHistory()[i].battleID + " result is " + PlayerInfo.Instance.GetMatchHistory()[i].battleResult.ToString());
        }
    }

    public void AddPlayerCash()
    {
        PlayerInfo.Instance.UpdatePlayerCash(55000);

        //Debug.Log("Player Id: " + PlayerInfo.Instance.GetPlayerId);
        //Debug.Log("Player Cash: " + PlayerInfo.Instance.GetPlayerCash());
        for(int i = 0; i < PlayerInfo.Instance.GetMatchHistory().Count; i++)
        {
            //Debug.Log("Match " + PlayerInfo.Instance.GetMatchHistory()[i].battleID + " result is " + PlayerInfo.Instance.GetMatchHistory()[i].battleResult.ToString());
        }
    }
}
