using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuBtnScript : MonoBehaviour
{
	/*
	0 = 1P
	1 = 2P,
	2 = HowToPlay,
	3 = Options,
	4 = Credits,

	5 = Icon1P,
	6 = Icon2P,
	7 = Start Player 1P,
	8 = Start Player 2P,
	9 = Back,
	10 = Back,
	11 = Start */
	public int menuType;
	public bool isShowSettingsScreen;
	public bool isShowCreditsScreen;
	public bool isShowHowToPlayScreen;

	void Start ()
	{
		isShowSettingsScreen = false;
	}

	void Update ()
	{
	}

	// NGUI Mouse Handler
	public void BtnClick(int btn_)
	{
		switch(btn_)
		{
		// Single Player
		case 0:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().gameMode = 0;
			SceneManager.LoadScene("GameScene");
			break;

		// Local Multiplayer Screen
		case 1:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().gameMode = 1;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(1);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			break;

		// Networked Screen
		case 2:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().gameMode = 2;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(2);
			break;

		// Go To Options Screen
		case 3:
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(3);
			break;
		
		// Go To Credits Screen
		case 4:
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(4);
			break;

		// Change P1 Icon
		case 5:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().CyclePlayerIcon();
			break;

		// Change P2 Icon
		case 6:
			//GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().CyclePlayerIcon(2);
			Debug.Log( "[SHOULD NOT HAVE] - cycling 2nd player icon" );
			break;

		// Change Starting Player to P1
		case 7:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().UpdateStartingPlayer(1);
			break;
		
		// Change Starting Player to P2
		case 8:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().UpdateStartingPlayer(2);
			break;

		// Back To Main Menu
		case 9:
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(0);

			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);

	        GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().LeaveRoom();
	        GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().ResetCountdown();
	        break;

		// Start Game!
		case 11:
			SceneManager.LoadScene("GameScene");
			break;

		// Play Public
		case 12:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().FindPublicGame();
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			break;

		// Play With Friends
		case 13:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().FindFriendGame();
			break;

		// Search (after keying in password)
		case 14:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().SearchFriend();
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SearchGrey.SetActive(true);
			break;

		// Settings
		case 15:
			isShowSettingsScreen = !isShowSettingsScreen;
			Camera.main.GetComponent<MainMenuScript>().SettingsScreen.SetActive(isShowSettingsScreen);
			break;

		// Settings: Credits
		case 16:
			isShowCreditsScreen = !isShowCreditsScreen;
			Camera.main.GetComponent<MainMenuScript>().CreditsPage.SetActive(isShowCreditsScreen);
			break;

		// Settings: How To Play
		case 17:
			isShowHowToPlayScreen = !isShowHowToPlayScreen;
			Camera.main.GetComponent<MainMenuScript>().HowToPlayPage.SetActive(isShowHowToPlayScreen);
			break;

		// Avatar
		case 18:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(4);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			break;

		// Back (Online Play)
		case 19:
			if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().network_allowButtonClicks == 1)
			{
				Camera.main.GetComponent<MainMenuScript>().JoinPublicGrey.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().FindFriendGrey.SetActive(false);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SearchGrey.SetActive(false);

				Camera.main.GetComponent<MainMenuScript>().UpdateText.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().PasswordText.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().PasswordField.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().SearchBtn.SetActive(false);

				Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
				Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);

				GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().network_allowButtonClicks = 0;
			}
			else
			{
				Camera.main.GetComponent<MainMenuScript>().GoToScreen(0);
				Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
				Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);

		        GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().LeaveRoom();
		        GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().ResetCountdown();
			}
			break;

		default:
			break;
		}
	}
}
