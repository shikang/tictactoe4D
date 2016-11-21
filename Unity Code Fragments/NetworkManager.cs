using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour 
{
	[SerializeField] Text connectionText;
	[SerializeField] Camera sceneCamera;

	[SerializeField] GameObject serverWindow;
	[SerializeField] InputField username;
	[SerializeField] InputField roomName;
	[SerializeField] InputField roomList;
	[SerializeField] Text messageWindow;

	[SerializeField] GameObject[] GemSP_;
	[SerializeField] GameObject[] PowerUpSP_;
	[SerializeField] GameObject[] Goals_;

	GameObject player;
	Queue<string> messages;
	const int messageCount = 6;

	public int[] playerIds;
	public string[] playerNames;
	public int selfID;

	PhotonView photonView;

	// Use this for initialization
	void Start () 
	{
		photonView = GetComponent<PhotonView> ();

		messages = new Queue<string> (messageCount);

		// Outputs everything (maximum debugging, not required)
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;

		PhotonNetwork.ConnectUsingSettings ("0.2");
		StartCoroutine ("UpdateConnectionString");
		playerIds = new int[4];
		playerNames = new string[4];
	}

	IEnumerator UpdateConnectionString () 
	{
		while(true)
		{
			//connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
			yield return null;
		}
	}

	public void AddMessage(string message)
	{
		photonView.RPC ("AddMessage_RPC", PhotonTargets.All, message);
	}
	

	[RPC]
	void AddMessage_RPC(string message)
	{
		messages.Enqueue (message);
		if (messages.Count > messageCount)
			messages.Dequeue ();

		messageWindow.text = "";
		foreach (string m in messages)
			messageWindow.text += m + "\n";

	}

	public void StartGame (int playerID, int[] playerIds_)
	{
		//Screen.lockCursor = true;
		GameObject lobby = GameObject.FindGameObjectWithTag ("Lobby");
		GameObject player = PhotonNetwork.Instantiate ("Zombie", Vector3.zero, Quaternion.identity, 0);
		player.GetComponent<PlayerScript>().InitPlayer(playerID + 1, true);

		selfID = playerID;
		GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelManager>().gameState = 0;

		if(playerID == 0)
		{
			foreach (GameObject go in GemSP_)
			{
				go.GetComponent<GemSP>().StartLevel();
			}
			foreach (GameObject go in PowerUpSP_)
			{
				go.GetComponent<PowerUpSP>().StartLevel();
			}
		}

		for(int i = 0; i < 4; ++i)
		{
			playerIds[i] = playerIds_[i];
		}

		gameObject.GetComponent<LevelManager> ().StartLevel();

		lobby.SetActive (false);

		for(int i = 0; i <= 3; ++i)
		{
			if(playerIds[i] == 0)
			{
				Goals_[i].SetActive(false);
			}
			else
			{
				Goals_[i].SetActive(true);
			}
		}
	}
	

	public void SpawnName(string prefabname, Vector3 pos, Quaternion rot)
	{
		PhotonNetwork.Instantiate(prefabname,
		                                  pos,
		                                  rot,
		                                  0);
	}
}
