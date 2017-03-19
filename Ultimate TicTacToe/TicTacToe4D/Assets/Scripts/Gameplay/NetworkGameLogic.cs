using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class NetworkGameLogic : Photon.PunBehaviour
{
    //PhotonView photonView;

    public enum AFTERMATH_ACTION
    {
        INVALID = 0,
        NOT_EQUAL = 0,
        RESTART = 1,
        QUIT = 1 << 1,
    };

    AFTERMATH_ACTION player1AfterAction = AFTERMATH_ACTION.INVALID;
    AFTERMATH_ACTION player2AfterAction = AFTERMATH_ACTION.INVALID;

    // Use this for initialization
    void Start () 
    {
		Debug.Log("NetworkGameLogic");
        //photonView = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*
            if (PhotonNetwork.player.name == "Player1" && loop <= 60)
            {
                ++loop;
                Debug.Log("loop: " + loop);
                if (loop > 60)
                {
                    SendMessage(PhotonNetwork.player.name + ": loop > 60");
                }
            }
            */
	}

    public static NetworkGameLogic GetNetworkGameLogic()
    {
        return GameObject.Find("NetworkManager").GetComponent<NetworkGameLogic>();
    }

    public static int GetPlayerNumber()
    {
        return NetworkManager.IsPlayerOne() ? 1 : 2;
    }

    void ProcessAfterAction()
    {
        if( player1AfterAction == AFTERMATH_ACTION.INVALID ||
            player2AfterAction == AFTERMATH_ACTION.INVALID )
        {
            return;
        }

        if( ( player1AfterAction & player2AfterAction ) == AFTERMATH_ACTION.RESTART )
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
			NetworkManager.LeaveRoom();
			SceneManager.LoadScene("MainMenu");
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
		GameObject.FindGameObjectsWithTag("GUIManager")[0].GetComponent<TurnHandler>().InformDisconnect();
		if(GameObject.Find("Board").GetComponent<BoardScript>().gameWinner == -1)
		{
			SaveLoad.Load();
			GameData.current.win += 1;
			SaveLoad.Save();
		}
	}

	public void OnApplicationPause(bool pause)
	{
		if (pause && NetworkManager.IsConnected())
		{
			NetworkManager.Disconnect();
			SceneManager.LoadScene("MainMenu");
		}
	}

	// Network game logic
	public void SendMessage(string message)
    {
        photonView.RPC("SendMessage_RPC", PhotonTargets.All, message);
    }

	public void SendEmote(string emote)
	{
		photonView.RPC("SendEmote_RPC", PhotonTargets.Others, emote);
	}

	public void HighlightGrid(int bigID, int smallID)
    {
        photonView.RPC("HighlightGrid_RPC", PhotonTargets.All, bigID, smallID);
    }

    public void ConfirmPlacement(int bigID, int smallID, Defines.TURN turn, float time)
    {
        photonView.RPC("ConfirmPlacement_RPC", PhotonTargets.Others, bigID, smallID, turn, time);
    }

	// Player 1 to Player 2 only
	public void ChangeTurn(Defines.TURN prev, Defines.TURN current)
	{
		if (!NetworkManager.IsPlayerOne())
			return;

		photonView.RPC("ChangeTurn_RPC", PhotonTargets.Others, prev, current);
	}

	public void AfterActionDecision(int player, AFTERMATH_ACTION action)
    {
        if ( NetworkManager.IsPlayerOne() && player == 1 )
        {
            player1AfterAction = action;
            ProcessAfterAction();
        }
        else if ( !NetworkManager.IsPlayerOne() && player == 2 )
        {
            player2AfterAction = action;
            ProcessAfterAction();
        }

        photonView.RPC("AfterActionDecision_RPC", PhotonTargets.Others, player, action);
    }

    [PunRPC]
    void SendMessage_RPC(string message)
    {
        string log = NetworkManager.GetPlayersInfoPrefix() + message + "\n";
        NetworkManager.DebugLog(log);
    }

    [PunRPC]
    void HighlightGrid_RPC(int bigID, int smallID)
    {
        BoardScript board = GameObject.Find("Board").GetComponent<BoardScript>();
        BigGridScript bigGrid = board.bigGrids[bigID].GetComponent<BigGridScript>();
        GridScript smallGrid = bigGrid.grids[smallID].GetComponent<GridScript>();
        smallGrid.HighlightGrid();
    }

    [PunRPC]
    void ConfirmPlacement_RPC(int bigID, int smallID, Defines.TURN turn, float time)
    {
		// If not correct turn
		if (GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != turn)
			return;

        BoardScript board = GameObject.Find("Board").GetComponent<BoardScript>();
        BigGridScript bigGrid = board.bigGrids[bigID].GetComponent<BigGridScript>();
        GridScript smallGrid = bigGrid.grids[smallID].GetComponent<GridScript>();
		GUIManagerScript guiScript = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>();

        smallGrid.ConfirmPlacement();
		//guiScript.SetTimer(turn, time);
		guiScript.ResetTimer();

		// Confirms p2 action and ask p2 to execute it.
		if (NetworkManager.IsPlayerOne() && turn == Defines.TURN.P2)
		{
			GetNetworkGameLogic().ConfirmPlacement(bigID, smallID, turn, time);
		}
    }

	[PunRPC]
	public void ChangeTurn_RPC(Defines.TURN prev, Defines.TURN current)
	{
		// Ignore
		if (GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != prev)
			return;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ChangeTurn();
	}

	[PunRPC]
    void AfterActionDecision_RPC(int player, AFTERMATH_ACTION action)
    {
        switch(player)
        {
            case 1:
                {
                    if ( !NetworkManager.IsPlayerOne() )
                    {
                        player1AfterAction = action;
                    }
                }
                break;
            case 2:
                {
                    if ( NetworkManager.IsPlayerOne() )
                    {
                        player2AfterAction = action;
                    }
                }
                break;
            default:
                {
                    string log = "[AfterAction_RPC] Invalid player: " + player + "\n";
                    NetworkManager.DebugLog(log);
                }
                break;
        }

        ProcessAfterAction();
    }

	[PunRPC]
	void SendEmote_RPC(string emote)
	{
		if (NetworkManager.IsPlayerOne())
		{
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ShowP2EmoteSpeech(emote);
		}
		else
		{
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ShowP1EmoteSpeech(emote);
		}
	}
}
