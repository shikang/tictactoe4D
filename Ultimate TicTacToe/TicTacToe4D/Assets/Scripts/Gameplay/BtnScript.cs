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

		AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void BtnMainMenu()
	{
		if(TutorialScript.Instance.isTutorial)
			return;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().pausedState = 2;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().SetCfmAlpha(true);
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmText.GetComponent<Text>().text
			= "Are you sure you want to quit?";

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
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
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
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
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
			AudioManager.Instance.PlaySoundEvent(SOUNDID.BACK);
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
		AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void BtnEmote()
	{
		if(TutorialScript.Instance.isTutorial)
			return;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ToogleEmoteMenu();
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
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().ShowP1EmoteSpeech(emote);
		}

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().HideEmoteMenu();
		AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void BackToMainMenu(int dest = 1)
	{
		// Do fade out logic here
		AudioManager.Instance.PlaySoundEvent(SOUNDID.BACK);
		GameStartAnim.Instance.FadeOut(dest);
	}

	public void BtnClickGoodGameEmote()
	{
		BtnClickEmote("Good Game");
		AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickWellPlayedEmote()
	{
		BtnClickEmote("Well Played!");
		AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickWowEmote()
	{
		BtnClickEmote("Wow!");
		AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickOopsEmote()
	{
		BtnClickEmote("Oops!");
		AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickThanksEmote()
	{
		BtnClickEmote("Thanks");
		AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}

	public void BtnClickGoodLuckEmote()
	{
		BtnClickEmote("Good Luck!");
		AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
	}
}
