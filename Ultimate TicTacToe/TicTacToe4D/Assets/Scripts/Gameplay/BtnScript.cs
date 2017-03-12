using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState = 1;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().SetCfmAlpha(true);
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmText.GetComponent<Text>().text
			= "Are you sure you want to restart?";
	}

	public void BtnMainMenu()
	{
		if(TutorialScript.Instance.isTutorial)
			return;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState = 2;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().SetCfmAlpha(true);
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmText.GetComponent<Text>().text
			= "Are you sure you want to quit?";
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
				BackToMainMenu(2);
                Adverts.Instance.RandomShowAd();
            }
        }
		else if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState == 2)
        {
            if (NetworkManager.IsConnected())
            {
                NetworkGameLogic networkLogic = NetworkGameLogic.GetNetworkGameLogic();
                networkLogic.AfterActionDecision(NetworkGameLogic.GetPlayerNumber(), NetworkGameLogic.AFTERMATH_ACTION.QUIT);
            }
            else
            {
				BackToMainMenu();
            }
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
		Adverts.Instance.RandomShowAd();
    }

	public void BtnConfirmMainMenu()
	{
		NetworkManager.LeaveRoom();
		BackToMainMenu();
		Adverts.Instance.RandomShowAd();
	}

	public void BtnEmote()
	{
		if(TutorialScript.Instance.isTutorial)
			return;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ToogleEmoteMenu();
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
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ShowP1EmoteSpeech(emote);
		}

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().HideEmoteMenu();
	}

	public void BackToMainMenu(int dest = 1)
	{
		// Do fade out logic here
		GameStartAnim.Instance.FadeOut(dest);
	}

	public void BtnClickGoodGameEmote()
	{
		BtnClickEmote("Good Game");
	}

	public void BtnClickWellPlayedEmote()
	{
		BtnClickEmote("Well Played!");
	}

	public void BtnClickWowEmote()
	{
		BtnClickEmote("Wow!");
	}

	public void BtnClickOopsEmote()
	{
		BtnClickEmote("Oops!");
	}

	public void BtnClickThanksEmote()
	{
		BtnClickEmote("Thanks");
	}

	public void BtnClickGoodLuckEmote()
	{
		BtnClickEmote("Good Luck!");
	}
}
