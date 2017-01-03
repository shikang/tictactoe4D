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
	public GameObject avatarObject;

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
		// Default set to not appear until the case updates it
		avatarObject.SetActive(false);

		switch((BUTTONTYPES)btn_)
		{
		case BUTTONTYPES.MAIN_SINGLEPLAYER:
			GlobalScript.Instance.gameMode = 0;
			SceneManager.LoadScene("GameScene");
			break;

		case BUTTONTYPES.MAIN_LOCALPLAY:
			GlobalScript.Instance.gameMode = 1;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(1);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GlobalScript.Instance.avatarState = 1;
			avatarObject.SetActive(true);
			break;

		case BUTTONTYPES.MAIN_NETWORK:
			GlobalScript.Instance.gameMode = 2;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(2);
			break;

		case BUTTONTYPES.MAIN_GACHA:
			GlobalScript.Instance.network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(3);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			break;

		case BUTTONTYPES.MAIN_SETTINGS:
			isShowSettingsScreen = !isShowSettingsScreen;
			Camera.main.GetComponent<MainMenuScript>().SettingsScreen.SetActive(isShowSettingsScreen);
			break;

		case BUTTONTYPES.MAIN_AVATAR:
			GlobalScript.Instance.network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(4);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GlobalScript.Instance.avatarState = 3;
			avatarObject.SetActive(true);
			break;

		// Back To Main Menu
		case BUTTONTYPES.LOCAL_BACKTOMAINMENU:
			Camera.main.GetComponent<MainMenuScript>().GoToScreen(0);

			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);
			GlobalScript.Instance.avatarState = 0;

			AvatarHandler.Instance.OnClickLocalPlayIcon1();

	        //GlobalScript.Instance.LeaveRoom();
	        //GlobalScript.Instance.ResetCountdown();
	        break;

		case BUTTONTYPES.LOCALPLAY_START:
			SceneManager.LoadScene("GameScene");
			break;

		case BUTTONTYPES.NETWORK_PUBLICGAME:
			GlobalScript.Instance.FindPublicGame();
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			break;

		case BUTTONTYPES.NETWORK_PRIVATEGAME:
			GlobalScript.Instance.FindFriendGame();
			break;

		case BUTTONTYPES.NETWORK_PRIVATEGAME_SEARCH:
			GlobalScript.Instance.SearchFriend();
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
			if(GlobalScript.Instance.network_allowButtonClicks == 1)
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

				GlobalScript.Instance.network_allowButtonClicks = 0;

				GlobalScript.Instance.LeaveRoom();
		        GlobalScript.Instance.ResetCountdown();
			}
			else
			{
				Camera.main.GetComponent<MainMenuScript>().GoToScreen(0);
				Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
				Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);

		        //GlobalScript.Instance.LeaveRoom();
		        //GlobalScript.Instance.ResetCountdown();
			}
			break;

		default:
			break;
		}
	}
}
