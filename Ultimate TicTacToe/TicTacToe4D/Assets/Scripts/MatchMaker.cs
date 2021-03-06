﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.Events;

public class MatchMaker : Photon.PunBehaviour 
{
    public GameObject roomInputField;

    private string roomName = "";
    private bool joinedLobby = false;
    private bool joiningRoom = false;
    private bool requestRoom = false;
    private bool joinedRoom = false;
	private bool connecting = false;

    public UnityEvent continueWithGame;
    public UnityEvent stopGame;
	public UnityEvent onConnected;

	private bool bRandom = false;
	private float fRandomTimer = 0.0f;
	private float fRandomWaitTime = Defines.MATCH_MAKE_RANDOM_RETRY_INTERVAL;
	private bool bStartRandomTimer = false;

    // Use this for initialization
    void Start () 
    {
        //PhotonNetwork.ConnectUsingSettings("0.1");
        //PhotonNetwork.logLevel = PhotonLogLevel.Full;

        PhotonNetwork.player.name = "Player2";

		GlobalScript.Instance.matchMaker = this.gameObject;

		continueWithGame.AddListener( delegate{ GlobalScript.Instance.FoundFriend(); } );
		stopGame.AddListener( delegate{ GlobalScript.Instance.ResetCountdown(); } );
		onConnected.AddListener( delegate{ Camera.main.GetComponent<MainMenuScript>().OnConnected(roomName == ""); } );

		fRandomTimer = 0.0f;
		bStartRandomTimer = false;
		bRandom = false;
		fRandomWaitTime = Defines.MATCH_MAKE_RANDOM_RETRY_INTERVAL + Random.Range( 0.0f, Defines.MATCH_MAKE_RANDOM_RETRY_INTERVAL * 0.5f );
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (connecting && joinedLobby && joiningRoom && !requestRoom && !joinedRoom)
        {
            if(roomName == "")
            {
                JoinRandomRoom();
            }
            else
            {
                JoinPrivateRoom();
            }
        }
        if (joinedRoom)
        {
            // Timerlogic, if exceed, quit room, disconnect self
        }

		TryFindNewPlayer();
	}

    void JoinRandomRoom()
    {
        string log = "Joining random room\n";
        DebugLog(log);

        PhotonNetwork.JoinRandomRoom();

        requestRoom = true;

		Analytics.CustomEvent("JoinedGame_Public", new Dictionary<string, object>{});
    }

    void JoinPrivateRoom()
    {
        string log = "Joining " + roomName + " room\n";
        DebugLog(log);

        // Join or create
        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);

        requestRoom = true;

		Analytics.CustomEvent("JoinedGame_Private", new Dictionary<string, object>{});
    }

    public override void OnJoinedLobby()
    {
    	Debug.Log("JOINERS");
        base.OnJoinedLobby();

        joinedLobby = true;

		if (onConnected != null)
			onConnected.Invoke();
	}

    public override void OnConnectedToMaster()
    {
        // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
    }

    void OnPhotonRandomJoinFailed()
    {
        string log = "Can't join random room!\n";
        DebugLog(log);

		CreateRandomRoom();
	}

	void CreateRandomRoom()
	{
		fRandomTimer = 0.0f;
		bStartRandomTimer = true;
		bRandom = true;
		fRandomWaitTime = Defines.MATCH_MAKE_RANDOM_RETRY_INTERVAL + Random.Range( 0.0f, Defines.MATCH_MAKE_RANDOM_RETRY_INTERVAL * 0.5f );

		RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 2 };
		PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
	}

	void TryFindNewPlayer()
	{
		if (!bStartRandomTimer)
			return;

		fRandomTimer += Time.deltaTime;
		if ( fRandomTimer >= fRandomWaitTime )
		{
			LeaveRoom(false);
			JoinRandomRoom();
		}
	}

    public override void OnCreatedRoom()
    {
        string log = "OnCreatedRoom() : You Have Created a Room : " + PhotonNetwork.room.name + "\n";
        DebugLog(log);

        joinedRoom = true;
        PhotonNetwork.player.name = "Player1";

		Analytics.CustomEvent("LobbyCreateRoom", new Dictionary<string, object>{});
    }

    public override void OnJoinedRoom()
    {
        string log = "OnJoinedRoom() : You Have joined a Room : " + PhotonNetwork.room.name + "\n";
        DebugLog(log);

        joinedRoom = true;
        int count = 1 + PhotonNetwork.otherPlayers.Length;
        PhotonNetwork.player.name = "Player" + count;

		GlobalScript.Instance.SetMyPlayerName();
		GlobalScript.Instance.SetMyPlayerIcon();

		Analytics.CustomEvent("LobbyJoinRoom", new Dictionary<string, object>{});

		ProceedToGame();
	}

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        //int count = 1 + PhotonNetwork.otherPlayers.Length;
        string log = "OnPhotonPlayerConnected() : " + newPlayer.name + " has joined!";
        DebugLog(log);

        ProceedToGame();
	}

	public void OnPhotonMaxCcuReached()
	{
		Camera.main.GetComponent<MainMenuScript>().AlertBox.SetActive(true);
		Camera.main.GetComponent<MainMenuScript>().AlertBoxLabel.GetComponent<Text>().text = "Server is Full! Please try again later =(";
		Analytics.CustomEvent("MaxCCUReached", new Dictionary<string, object>{});
	}

	public void ConnectionHasError()
	{
		GlobalScript.Instance.LeaveRoom();
		Camera.main.GetComponent<MainMenuScript>().AlertBox.SetActive(true);
		Camera.main.GetComponent<MainMenuScript>().AlertBoxLabel.GetComponent<Text>().text = "Connection error! \n Please check your connection =(";
	}

	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		switch (cause)
		{
			/// <summary>Server actively disconnected this client.
			/// Possible cause: The server's user limit was hit and client was forced to disconnect (on connect).</summary>
			case DisconnectCause.DisconnectByServerUserLimit:
				ConnectionHasError();
				break;

			/// <summary>Connection could not be established.
			/// Possible cause: Local server not running.</summary>
			case DisconnectCause.ExceptionOnConnect:
				ConnectionHasError();
				break;

			/// <summary>Server actively disconnected this client.
			/// Possible cause: Server's send buffer full (too much data for client).</summary>
			case DisconnectCause.DisconnectByServerLogic:
				ConnectionHasError();
				break;

			/// <summary>Some exception caused the connection to close.</summary>
			case DisconnectCause.Exception:
				ConnectionHasError();
				break;

			/// <summary>(32753) The Authentication ticket expired. Handle this by connecting again (which includes an authenticate to get a fresh ticket).</summary>
			case DisconnectCause.AuthenticationTicketExpired:
				ConnectionHasError();
				break;

		}
	}

	/*
    public override void OnLeftRoom()
    {
        DebugLog("OnLeftRoom");

        if (stopGame != null)
            stopGame.Invoke();
    }
    */

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (stopGame != null)
            stopGame.Invoke();

		bStartRandomTimer = bRandom;
	}

    void ProceedToGame()
    {
        if(PhotonNetwork.otherPlayers.Length == 1)
        {
			bStartRandomTimer = false;
			bRandom = false;

			// Proceed
			string log = "Proceeding to game...\n" +
                         "[1] " + PhotonNetwork.player.name + " (Me)\n" +
                         "[2] " + PhotonNetwork.otherPlayers[0].name + "\n";
            DebugLog(log);

            //SceneManager.LoadScene("GameScene");
            // Notify Global Script
            if (continueWithGame != null)
                continueWithGame.Invoke();
        }
    }

	/*
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
	*/

    public void JoiningRoom()
    {
		StartConnecting();

		if (roomInputField != null)
        {
            roomName = roomInputField.GetComponent<InputField>().text;
        }

        joiningRoom = true;
    }

    public void LeaveRoom(bool reallyLeave = true)
    {
		if(PhotonNetwork.inRoom)
			PhotonNetwork.LeaveRoom();
        joiningRoom = false;
        requestRoom = false;
        joinedRoom = false;
		bStartRandomTimer = false;
		bRandom = false;

		if(reallyLeave)
		{
			Analytics.CustomEvent("LobbyLeftRoom", new Dictionary<string, object>{});
		}

		StartDisconnecting();
	}

    void DebugLog(string log)
    {
        Debug.Log(log);
        //GameObject debugText = GameObject.Find("DebugText");
        //debugText.GetComponent<Text>().text = log;
    }

	static public bool IsPlayerOne()
	{
		return PhotonNetwork.isMasterClient;
	}

	public void SendPlayerName(string playerName, int playerIcon)
	{
		Debug.Log("Really sending: " + playerName);
		photonView.RPC("SendPlayerName_RPC", PhotonTargets.Others, playerName, playerIcon);
	}

	public void SendPlayerNewName(string playerName)
	{
		Debug.Log("Sending player 2 new name: " + playerName);
		photonView.RPC("SendPlayerNewName_RPC", PhotonTargets.Others, playerName);
	}

	[PunRPC]
	void SendPlayerName_RPC(string playerName, int playerIcon)
	{
		string log = NetworkManager.GetPlayersInfoPrefix() + "Player name sent: " + playerName + " | Icon: " + playerIcon + "\n";
		NetworkManager.DebugLog(log);

		if ( IsPlayerOne() )
		{
			GlobalScript.Instance.iconP2 = playerIcon;

			string name = playerName;
			if ( GlobalScript.Instance.nameP1 == playerName )
			{
				name += 2;
				SendPlayerNewName( name );
			}

			GlobalScript.Instance.UpdatePlayer2Name( name );
		}
		else
		{
			GlobalScript.Instance.iconP1 = playerIcon;

			if ( GlobalScript.Instance.nameP2 == playerName )
			{
				// Wait for new name
				return;
			}
			else
			{
				GlobalScript.Instance.UpdatePlayer1Name( playerName );
			}
		}

		GlobalScript.Instance.StartCountdown();
	}

	[PunRPC]
	void SendPlayerNewName_RPC(string playerName)
	{
		string log = NetworkManager.GetPlayersInfoPrefix() + "Player new name sent: " + playerName + "\n";
		NetworkManager.DebugLog(log);

		if ( IsPlayerOne() )
		{
			NetworkManager.DebugLog( "SendPlayerNewName_RPC: this shouldn't happen" );
		}
		else
		{
			GlobalScript.Instance.UpdatePlayer2Name(playerName);
		}

		GlobalScript.Instance.StartCountdown();
	}

	public void StartConnecting()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
		connecting = true;
	}

	public void StartDisconnecting()
	{
		PhotonNetwork.Disconnect();
		connecting = false;
		joinedLobby = false;
	}

	public void OnApplicationPause(bool pause)
	{
		if (pause && connecting)
		{
			LeaveRoom();
		}
	}
}
