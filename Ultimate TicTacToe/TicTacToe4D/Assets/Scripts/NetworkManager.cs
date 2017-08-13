using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class NetworkManager : UnityEngine.MonoBehaviour
{
    // Use this for initialization
	void Start () 
    {
        //string log = GetPlayersInfoPrefix() + "GameStart!\n";
        //DebugLog(log);
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    static public bool IsConnected()
    {
        return PhotonNetwork.inRoom;
    }

    static public bool IsPlayerOne()
    {
        return PhotonNetwork.isMasterClient;
    }

    static public string GetPlayersInfoPrefix()
    {
        /* DEBUG ONLY
        if (IsConnected())
        {
            return "[1] " + PhotonNetwork.player.name + " (Me)\n" +
                   "[2] " + PhotonNetwork.otherPlayers[0].name + "\n";
        }
        else
        {
            return "Not in room! Playing alone!\n";
        }*/
        return "";
    }

    static public void DebugLog(string log)
    {
        /*Debug.Log(log);
        GameObject debugText = GameObject.Find("DebugText");
        debugText.GetComponent<Text>().text = log;*/
    }

	static public void LeaveRoom()
	{
		if (PhotonNetwork.inRoom)
			PhotonNetwork.LeaveRoom();
	}

	static public void Disconnect()
	{
		PhotonNetwork.Disconnect();
	}
}
