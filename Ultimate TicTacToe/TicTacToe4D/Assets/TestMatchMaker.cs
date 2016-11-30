using UnityEngine;
using System.Collections;
using Photon;

public class TestMatchMaker : Photon.PunBehaviour
{
    private PhotonView myPhotonView;
	// Use this for initialization
	void Start ()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
    }

    void OnGUI ()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

		/*
        if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
        {
            bool shoutMarco = TestNetworkLogic.playerWhoIsIt == PhotonNetwork.player.ID;

            if (shoutMarco && GUILayout.Button("Marco!"))
            {
                myPhotonView.RPC("Marco", PhotonTargets.All);
            }
            if (!shoutMarco && GUILayout.Button("Polo!"))
            {
                myPhotonView.RPC("Polo", PhotonTargets.All);
            }
        }
		*/
    }

    void JoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("WhyWoolTestRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //PhotonNetwork.JoinRandomRoom();
        JoinRoom();
    }

    public override void OnConnectedToMaster()
    {
        // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
        JoinRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        //PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        //GameObject monster = PhotonNetwork.Instantiate("monsterprefab", Vector3.zero, Quaternion.identity, 0);
        //monster.GetComponent<myThirdPersonController>().isControllable = true;
        //myPhotonView = monster.GetComponent<PhotonView>();
    }

	// Update is called once per frame
	void Update () 
    {
	
	}
}
