using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;
using Assets.SimpleAndroidNotifications;

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

	LOCAL_BACKTOMAINMENU,

	ADS_WATCH_VIDEO, //14

	SETTIME_1,
	SETTIME_2,
	SETTIME_3,

	SETTINGS_DIABLE_ADS, //18

	RATE_OUR_APP,
	NO_RATE_OUR_APP,
	LIKE_OUR_FACEBOOK,
	NO_LIKE_OUR_FACEBOOK,
};

public class MenuBtnScript : MonoBehaviour
{
	public BUTTONTYPES menuType;
	public bool isShowSettingsScreen;
	public bool isShowCreditsScreen;
	public bool isShowHowToPlayScreen;
	public GameObject MenuHandler;

	public bool isStartingGame;
	public GameObject blackScreen;

	void Start ()
	{
		isShowSettingsScreen = false;
		isStartingGame = false;
		blackScreen.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		blackScreen.SetActive(false);
	}

	void Update ()
	{
		UpdateAnim();

		if(!GameData.current.finishedTutorial)
			BtnClick((int)BUTTONTYPES.MAIN_SINGLEPLAYER);
	}

	void UpdateAnim()
	{
		if(isStartingGame)
		{
			blackScreen.SetActive(true);
			Color temp = blackScreen.GetComponent<Image>().color;
			temp.a += Time.deltaTime * 3.0f;
			blackScreen.GetComponent<Image>().color = temp;

			if(blackScreen.GetComponent<Image>().color.a >= 1.0f)
			{
				SceneManager.LoadScene("GameScene");
			}
		}
	}

	// NGUI Mouse Handler
	public void BtnClick(int btn_)
	{
		// No buttons should work while the screen is transiting.
		if(MenuHandler.GetComponent<MainMenuScript>().moveScreen)
			return;

		switch((BUTTONTYPES)btn_)
		{
		case BUTTONTYPES.MAIN_SINGLEPLAYER:
			isStartingGame = true;
			GlobalScript.Instance.gameMode = 0;
			GlobalScript.Instance.SetSinglePlayerName();
			GlobalScript.Instance.SetSinglePlayerIcon();
			break;

		case BUTTONTYPES.MAIN_LOCALPLAY:
			GlobalScript.Instance.gameMode = 1;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(1);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GlobalScript.Instance.avatarState = 1;
			break;

		case BUTTONTYPES.MAIN_NETWORK:
			GlobalScript.Instance.gameMode = 2;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(2);
			break;

		case BUTTONTYPES.MAIN_GACHA:
			GlobalScript.Instance.network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(3);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			break;

		case BUTTONTYPES.MAIN_SETTINGS:
			isShowSettingsScreen = !isShowSettingsScreen;
			Camera.main.GetComponent<MainMenuScript>().SettingsScreen.SetActive(isShowSettingsScreen);
			break;

		case BUTTONTYPES.MAIN_AVATAR:
			GlobalScript.Instance.network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(4);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GlobalScript.Instance.avatarState = 3;
			break;

		// Back To Main Menu
		case BUTTONTYPES.LOCAL_BACKTOMAINMENU:
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(0, true);

			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);
			GlobalScript.Instance.avatarState = 0;

			AvatarHandler.Instance.OnClickLocalPlayIcon1();

	        //GlobalScript.Instance.LeaveRoom();
	        //GlobalScript.Instance.ResetCountdown();
	        break;

		case BUTTONTYPES.LOCALPLAY_START:
			isStartingGame = true;
			GlobalScript.Instance.SetLocalMultiPlayerName();
			GlobalScript.Instance.SetLocalMultiPlayerIcon();
			break;

		case BUTTONTYPES.NETWORK_PUBLICGAME:
			GlobalScript.Instance.FindPublicGame();
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().networkMenuAnimStage = 1;
			break;

		case BUTTONTYPES.NETWORK_PRIVATEGAME:
			GlobalScript.Instance.FindFriendGame();
			Camera.main.GetComponent<MainMenuScript>().networkMenuAnimStage = 1;
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
			GameData.current.finishedTutorial = false;
			SaveLoad.Save();
			isStartingGame = true;
			//isShowHowToPlayScreen = !isShowHowToPlayScreen;
			//Camera.main.GetComponent<MainMenuScript>().HowToPlayPage.SetActive(isShowHowToPlayScreen);
			break;

		case BUTTONTYPES.ADS_WATCH_VIDEO:
			if(GachaScript.Instance.CanGacha())
			{
				var options = new ShowOptions { resultCallback = Adverts.Instance.FreeGachaHandler};
				Adverts.Instance.ShowAd(AdVidType.video,options);
				Adverts.Instance.freeGacha = true;
				GachaScript.Instance.SetGacha();

				#if UNITY_ANDROID && !UNITY_EDITOR
				NotificationManager.SendWithAppIcon(System.TimeSpan.FromSeconds(Defines.FREE_ROLL_TIMER), "Ultimate Tic Tac Toe", "Get your free roll now!", Color.black);
				#endif

				#if UNITY_IOS && !UNITY_EDITOR
				// @todo apple notification here
				#endif
			}
			break;

		case BUTTONTYPES.NETWORK_BACKTOMAINMENU:
			if(GlobalScript.Instance.network_allowButtonClicks == 1)
			{
				Camera.main.GetComponent<MainMenuScript>().networkMenuAnimStage = 2;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SearchGrey.SetActive(false);

				Camera.main.GetComponent<MainMenuScript>().UpdateText.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().UpdateText_PublicGame.SetActive(false);
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
				Camera.main.GetComponent<MainMenuScript>().ChangeScreen(0, true);
				Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
				Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);

		        //GlobalScript.Instance.LeaveRoom();
		        //GlobalScript.Instance.ResetCountdown();
			}
			break;

		case BUTTONTYPES.SETTIME_1:
			GlobalScript.Instance.SetTurnTime(1);
			Camera.main.GetComponent<MainMenuScript>().SetTimerImage(GlobalScript.Instance.gameMode, 1);
			break;

		case BUTTONTYPES.SETTIME_2:
			GlobalScript.Instance.SetTurnTime(2);
			Camera.main.GetComponent<MainMenuScript>().SetTimerImage(GlobalScript.Instance.gameMode, 2);
			break;

		case BUTTONTYPES.SETTIME_3:
			GlobalScript.Instance.SetTurnTime(3);
			Camera.main.GetComponent<MainMenuScript>().SetTimerImage(GlobalScript.Instance.gameMode, 3);
			break;

		case BUTTONTYPES.SETTINGS_DIABLE_ADS:
			Camera.main.GetComponent<MainMenuScript>().EnableDisableAdsButton(false);

			GameObject IAPManager = GameObject.Find("IAPManager");
			InAppPurchaser iapPurchaser = IAPManager.GetComponent<InAppPurchaser>();
			iapPurchaser.BuyProduct( InAppProductList.ProductType.ADS, (int)Defines.AdsInAppPurchase.DISABLE );
			break;
		case BUTTONTYPES.RATE_OUR_APP:
			PlatformUtilies.Instance.DisplayRateUs();
			Camera.main.GetComponent<MainMenuScript>().RateAppScreen.SetActive(false);
			break;
		case BUTTONTYPES.NO_RATE_OUR_APP:
			Camera.main.GetComponent<MainMenuScript>().RateAppScreen.SetActive(false);
			break;
		case BUTTONTYPES.LIKE_OUR_FACEBOOK:
			PlatformUtilies.Instance.DisplayFacebookPage();
			Camera.main.GetComponent<MainMenuScript>().LikeFacebookScreen.SetActive(false);
			break;
		case BUTTONTYPES.NO_LIKE_OUR_FACEBOOK:
			Camera.main.GetComponent<MainMenuScript>().LikeFacebookScreen.SetActive(false);
			break;
		default:
			break;
		}
	}
}
