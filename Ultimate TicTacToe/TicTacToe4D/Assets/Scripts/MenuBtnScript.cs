using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public enum BUTTONTYPES
{
	MAIN_SINGLEPLAYER = 0,
	MAIN_LOCALPLAY,
	MAIN_NETWORK,
	NETWORK_BACKTOMAINMENU,

	MAIN_SETTINGS,	//4
	MAIN_AVATAR,
	MAIN_GACHA,

	NETWORK_PUBLICGAME,	//7
	NETWORK_PRIVATEGAME,
	NETWORK_PRIVATEGAME_SEARCH,
	LOCALPLAY_START,

	SETTINGS_HOWTOPLAY,	//11
	SETTINGS_CREDITS,

	LOCAL_BACKTOMAINMENU
};

public class MenuBtnScript : MonoBehaviour
{
	public BUTTONTYPES menuType;
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
		switch((BUTTONTYPES)btn_)
		{
		case BUTTONTYPES.MAIN_SINGLEPLAYER:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().gameMode = 0;
			SceneManager.LoadScene("GameScene");
			break;

		case BUTTONTYPES.MAIN_LOCALPLAY:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().gameMode = 1;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(1);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState = 1;
			break;

		case BUTTONTYPES.MAIN_NETWORK:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().gameMode = 2;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(2);
			break;

		case BUTTONTYPES.MAIN_GACHA:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(3);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			break;

		case BUTTONTYPES.MAIN_SETTINGS:
			isShowSettingsScreen = !isShowSettingsScreen;
			Camera.main.GetComponent<MainMenuScript>().SettingsScreen.SetActive(isShowSettingsScreen);
			break;

		case BUTTONTYPES.MAIN_AVATAR:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(4);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState = 3;
			break;

		// Back To Main Menu
		case BUTTONTYPES.LOCAL_BACKTOMAINMENU:
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(0);

			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState = 0;

	        //GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().LeaveRoom();
	        //GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().ResetCountdown();
	        break;

		case BUTTONTYPES.LOCALPLAY_START:
			SceneManager.LoadScene("GameScene");
			break;

		case BUTTONTYPES.NETWORK_PUBLICGAME:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().FindPublicGame();
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			break;

		case BUTTONTYPES.NETWORK_PRIVATEGAME:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().FindFriendGame();
			break;

		case BUTTONTYPES.NETWORK_PRIVATEGAME_SEARCH:
			GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().SearchFriend();
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SearchGrey.SetActive(true);
			break;

		case BUTTONTYPES.SETTINGS_CREDITS:
			isShowCreditsScreen = !isShowCreditsScreen;
			Camera.main.GetComponent<MainMenuScript>().CreditsPage.SetActive(isShowCreditsScreen);
			break;

		case BUTTONTYPES.SETTINGS_HOWTOPLAY:
			isShowHowToPlayScreen = !isShowHowToPlayScreen;
			Camera.main.GetComponent<MainMenuScript>().HowToPlayPage.SetActive(isShowHowToPlayScreen);
			break;

		case BUTTONTYPES.NETWORK_BACKTOMAINMENU:
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

				GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().LeaveRoom();
		        GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().ResetCountdown();
			}
			else
			{
				Camera.main.GetComponent<MainMenuScript>().GoToScreen(0);
				Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
				Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);

		        //GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().LeaveRoom();
		        //GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().ResetCountdown();
			}
			break;

		default:
			break;
		}
	}
}
