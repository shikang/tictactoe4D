using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BtnScript : MonoBehaviour
{
	public int btnType;

	void Start ()
	{
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			BtnMainMenu();
		}
	}

	public void BtnRestart()
	{
		if(TutorialScript.Instance.isTutorial)
			return;

		if(!NetworkManager.IsConnected())
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().isPaused = true;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState = 1;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().SetCfmAlpha(true);
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUIForfeitCoinText.SetActive(true);
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmText.GetComponent<Text>().text
			= "Are you sure you want to restart?";

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void BtnMainMenu()
	{
		if(TutorialScript.Instance.isTutorial)
			return;

		if(!NetworkManager.IsConnected())
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().isPaused = true;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState = 2;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().SetCfmAlpha(true);
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUIForfeitCoinText.SetActive(true);
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmText.GetComponent<Text>().text
			= "Are you sure you want to quit?";

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void BtnYes()
	{
		if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState == 1)
        {
            if(NetworkManager.IsConnected())
            {
                NetworkManager.DebugLog("Cannot restart!\n");
            }
            else
            {
				GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().UpdateAnalyticsGameEnd();
				if(AudioManager.Instance)
					AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
				BGManager.Instance.partsParent.SetActive(false);
				BackToMainMenu(2, false);
                Adverts.Instance.RandomShowAd();
            }
        }
		else if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState == 2)
        {
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().UpdateAnalyticsGameEnd();
			BGManager.Instance.partsParent.SetActive(false);

			GameObject [] allGUI = GameObject.FindGameObjectsWithTag("GUIManager");
			foreach(GameObject curr in allGUI)
			{
				Destroy(curr);
			}

            if (NetworkManager.IsConnected())
            {
				Defines.TURN turn = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn;
				Debug.Log("ttt: : " + turn);
				if (turn == Defines.TURN.P1 || turn == Defines.TURN.P2 || turn == Defines.TURN.NOTSTARTED)
				{
					NetworkManager.Disconnect();
				}
				else
				{
					NetworkGameLogic networkLogic = NetworkGameLogic.GetNetworkGameLogic();
					networkLogic.AfterActionDecision(NetworkGameLogic.GetPlayerNumber(), NetworkGameLogic.AFTERMATH_ACTION.QUIT);
					GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn = Defines.TURN.DISCONNECTED;
					NetworkManager.Disconnect();
				}
            }

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			BGManager.Instance.partsParent.SetActive(false);
			BackToMainMenu(1, false);
            Adverts.Instance.RandomShowAd();
        }
	}

	public void BtnNo()
	{
		if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState != 0)
		{
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState = 0;
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().SetCfmAlpha(false);
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmText.GetComponent<Text>().text = "";
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().isPaused = false;
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.BACK);
		}
	}

	public void BtnNewGame()
	{
        // New Game
        if (NetworkManager.IsConnected())
        {
            NetworkGameLogic networkLogic = NetworkGameLogic.GetNetworkGameLogic();
            networkLogic.AfterActionDecision(NetworkGameLogic.GetPlayerNumber(), NetworkGameLogic.AFTERMATH_ACTION.RESTART);
            GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().WaitingForOtherPlayer();
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.BGM);
		//Adverts.Instance.RandomShowAd();
    }

	public void BtnConfirmMainMenu()
	{
		NetworkManager.LeaveRoom();
		BackToMainMenu();
		//Adverts.Instance.RandomShowAd();
		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void BtnEmote()
	{
		if(!CanUseEmoji())
			return;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().isEmoteScreen = true;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ToogleEmoteMenu();

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	void BtnClickEmote(string emote)
	{
		if(NetworkManager.IsConnected())
		{
			if(NetworkManager.IsPlayerOne())
			{
				GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ShowP1EmoteSpeech(emote);
			}
			else
			{
				GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ShowP2EmoteSpeech(emote);
			}

			// Network logic
			NetworkGameLogic networkLogic = NetworkGameLogic.GetNetworkGameLogic();
			networkLogic.SendEmote(emote);
		}
		else
		{
			if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P1)
				GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ShowP1EmoteSpeech(emote);
			else
				GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ShowP2EmoteSpeech(emote);
		}

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().HideEmoteMenu();

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void BackToMainMenu(int dest = 1, bool playSound = true)
	{
		// Do fade out logic here
		if(AudioManager.Instance && playSound)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.BACK);
		GameStartAnim.Instance.FadeOut(dest);
	}

	public void BtnClickGoodGameEmote()
	{
		if(!CanUseEmoji())
			return;

		BtnClickEmote("Good Game");

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickWellPlayedEmote()
	{
		if(!CanUseEmoji())
			return;

		BtnClickEmote("Well Played!");

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickWowEmote()
	{
		if(!CanUseEmoji())
			return;

		BtnClickEmote("Wow!");
		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickOopsEmote()
	{
		if(!CanUseEmoji())
			return;

		BtnClickEmote("Oops!");
		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickThanksEmote()
	{
		if(!CanUseEmoji())
			return;

		BtnClickEmote("Thanks");
		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickGoodLuckEmote()
	{
		if(!CanUseEmoji())
			return;

		BtnClickEmote("Good Luck!");
		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	bool CanUseEmoji()
	{
		// Tutorial
		if(TutorialScript.Instance.isTutorial)
			return false;

		// If not your turn during online play
		/*if( GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.ONLINE)
		{
			if( (NetworkManager.IsPlayerOne() && GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != Defines.TURN.P1)  ||
			   (!NetworkManager.IsPlayerOne() && GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != Defines.TURN.P2) )
        		return false;
        }*/

        // If AI turn in single player
		if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.AI)
		{
			if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P2 )
				return false;
		}
		return true;
	}
}
