using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System;
using System.Collections;
using UnityEngine.Advertisements;
using System.Collections.Generic;
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

	SETTINGS_BGM,	//23
	SETTINGS_SFX,
	SETTINGS_VIBRATION,

	ALERT_BACK
};

public enum SCREENS
{
	MAINMENU = 0,
	ONLINEPLAY,
	LOCALPLAY,
	AVATAR,
	GACHA_POINTS,
	GACHA_MONEY,
	SETTINGS_MAIN,
	SETTINGS_CREDITS,
	INGAME
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
	public SCREENS currScreen;

	public GameObject BGM_On;
	public GameObject BGM_Off;
	public GameObject SFX_On;
	public GameObject SFX_Off;
	public GameObject Vibrate_On;
	public GameObject Vibrate_Off;

	void Start ()
	{
		isShowSettingsScreen = false;
		isStartingGame = false;
		blackScreen.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		blackScreen.SetActive(false);
		currScreen = SCREENS.MAINMENU;

		SaveLoad.Load();
		UpdateBGMButton();
		UpdateSFXButton();
		UpdateVibrateButton();
	}

	void Update ()
	{
		UpdateAnim();

		if(!GameData.current.finishedTutorial)
			BtnClick((int)BUTTONTYPES.MAIN_SINGLEPLAYER);

		// Android Back Button
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(currScreen == SCREENS.MAINMENU)
				Application.Quit();
			else if(currScreen == SCREENS.ONLINEPLAY)
				BtnClick((int)BUTTONTYPES.NETWORK_BACKTOMAINMENU);
			else if(currScreen == SCREENS.LOCALPLAY)
				BtnClick((int)BUTTONTYPES.LOCAL_BACKTOMAINMENU);
			else if(currScreen == SCREENS.AVATAR)
				BtnClick((int)BUTTONTYPES.NETWORK_BACKTOMAINMENU);
			else if(currScreen == SCREENS.GACHA_POINTS)
				BtnClick((int)BUTTONTYPES.LOCAL_BACKTOMAINMENU);
			else if(currScreen == SCREENS.GACHA_MONEY)
				GachaScript.Instance.BackBuyButtonClick();
			else if(currScreen == SCREENS.SETTINGS_MAIN)
				BtnClick((int)BUTTONTYPES.MAIN_SETTINGS);
			else if(currScreen == SCREENS.SETTINGS_CREDITS)
				BtnClick((int)BUTTONTYPES.SETTINGS_CREDITS);
		}
	}

	void UpdateAnim()
	{
		if(isStartingGame)
		{
			if(GameData.current.hasVibrate && VibrationManager.HasVibrator())
				VibrationManager.Vibrate(Defines.V_STARTGAME);

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

	void UpdateBGMButton()
	{
		if(GameData.current.hasBGM)
		{
			if(!GlobalScript.Instance.isBGMPlaying)
			{
				BGM_On.SetActive(true);
				BGM_Off.SetActive(false);

				if(AudioManager.Instance)
					AudioManager.Instance.PlaySoundEvent(SOUNDID.BGM);
			}
		}
		else
		{
			BGM_On.SetActive(false);
			BGM_Off.SetActive(true);

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.STOPBGM);
		}
	}

	void UpdateSFXButton()
	{
		if(GameData.current.hasSFX)
		{
			SFX_On.SetActive(true);
			SFX_Off.SetActive(false);

			if(AudioManager.Instance)
				AudioManager.Instance.SetSFXVol(100.0f);
		}
		else
		{
			SFX_On.SetActive(false);
			SFX_Off.SetActive(true);

			if(AudioManager.Instance)
				AudioManager.Instance.SetSFXVol(0.001f);
		}
	}

	void UpdateVibrateButton()
	{
		if(GameData.current.hasVibrate)
		{
			Vibrate_On.SetActive(true);
			Vibrate_Off.SetActive(false);
		}
		else
		{
			Vibrate_On.SetActive(false);
			Vibrate_Off.SetActive(true);
		}

		if(GameData.current.hasVibrate && VibrationManager.HasVibrator())
			VibrationManager.Vibrate(Defines.V_PLACEICON);
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

			if(GameData.current.finishedTutorial && AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.STARTGAME);

			break;

		case BUTTONTYPES.MAIN_LOCALPLAY:
			currScreen = SCREENS.LOCALPLAY;
			GlobalScript.Instance.gameMode = 1;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(1);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GlobalScript.Instance.avatarState = 1;
			AvatarHandler.Instance.UpdateUnlockedAvatarsStatus();
			AvatarHandler.Instance.SetAvatarPlaceHolderText();

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.MAIN_NETWORK:
			currScreen = SCREENS.ONLINEPLAY;
			GlobalScript.Instance.gameMode = 2;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(2);

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.MAIN_GACHA:
			currScreen = SCREENS.GACHA_POINTS;
			GlobalScript.Instance.network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(3);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			AvatarHandler.Instance.UpdateUnlockedAvatarsStatus();

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.MAIN_SETTINGS:
			if(currScreen == SCREENS.MAINMENU)
				currScreen = SCREENS.SETTINGS_MAIN;
			else
				currScreen = SCREENS.MAINMENU;
			isShowSettingsScreen = !isShowSettingsScreen;
			Camera.main.GetComponent<MainMenuScript>().SettingsScreen.SetActive(isShowSettingsScreen);

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.MAIN_AVATAR:
			currScreen = SCREENS.AVATAR;
			GlobalScript.Instance.network_allowButtonClicks = 0;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(4);
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			GlobalScript.Instance.avatarState = 3;
			AvatarHandler.Instance.UpdateUnlockedAvatarsStatus();

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		// Back To Main Menu
		case BUTTONTYPES.LOCAL_BACKTOMAINMENU:
			currScreen = SCREENS.MAINMENU;
			Camera.main.GetComponent<MainMenuScript>().ChangeScreen(0, true);

			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);
			GlobalScript.Instance.avatarState = 0;

			if(GachaScript.Instance)
			{
				GachaScript.Instance.ResetGachaText();
				GachaScript.Instance.ResetSpecialText();
			}

			AvatarHandler.Instance.OnClickLocalPlayIcon1(false);

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.BACK);

	        //GlobalScript.Instance.LeaveRoom();
	        //GlobalScript.Instance.ResetCountdown();
	        break;

		case BUTTONTYPES.LOCALPLAY_START:
			isStartingGame = true;
			GlobalScript.Instance.SetLocalMultiPlayerName();
			GlobalScript.Instance.SetLocalMultiPlayerIcon();

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.STARTGAME);
			break;

		case BUTTONTYPES.NETWORK_PUBLICGAME:
			GlobalScript.Instance.FindPublicGame();
			Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
			Camera.main.GetComponent<MainMenuScript>().networkMenuAnimStage = 1;
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SetAnim(1);
			GlobalScript.Instance.SetGreyBtns();

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.NETWORK_PRIVATEGAME:
			GlobalScript.Instance.FindFriendGame();
			Camera.main.GetComponent<MainMenuScript>().networkMenuAnimStage = 1;

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.NETWORK_PRIVATEGAME_SEARCH:

			if(GameObject.Find("Password").GetComponent<InputField>().text == "")
			{
				Camera.main.GetComponent<MainMenuScript>().UpdateText.GetComponent<Text>().text = "Password field cannot be blank!";
				Camera.main.GetComponent<MainMenuScript>().UpdateText.SetActive(true);
			}
			else
			{
				Camera.main.GetComponent<MainMenuScript>().UpdateText.SetActive(false);
				GlobalScript.Instance.SearchFriend();
				Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(false);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SearchGrey.SetActive(true);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SetAnim(2);
				GlobalScript.Instance.SetGreyBtns();
			}

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.SETTINGS_CREDITS:
			if(currScreen == SCREENS.SETTINGS_MAIN)
				currScreen = SCREENS.SETTINGS_CREDITS;
			else
				currScreen = SCREENS.SETTINGS_MAIN;
			isShowCreditsScreen = !isShowCreditsScreen;
			Camera.main.GetComponent<MainMenuScript>().CreditsPage.SetActive(isShowCreditsScreen);
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.SETTINGS_HOWTOPLAY:
			GameData.current.finishedTutorial = false;
			SaveLoad.Save();
			isStartingGame = true;

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			//isShowHowToPlayScreen = !isShowHowToPlayScreen;
			//Camera.main.GetComponent<MainMenuScript>().HowToPlayPage.SetActive(isShowHowToPlayScreen);
			break;

		case BUTTONTYPES.ADS_WATCH_VIDEO:
			if(GachaScript.Instance.CanFreeRoll())
			{
				#if UNITY_ANDROID || UNITY_IOS
				var options = new ShowOptions { resultCallback = Adverts.Instance.FreeGachaHandler};
				Adverts.Instance.ShowAd(AdVidType.video,options);
				Adverts.Instance.freeGacha = true;

				GameData.current.nextFreeRollTime = DateTime.Now.Add(TimeSpan.FromHours(4.0));
				NotificationManager.SendWithAppIcon(TimeSpan.FromHours(4.0), "Free roll ready!", "Come and roll for a new Tac!", Color.black);
				SaveLoad.Save();

				Analytics.CustomEvent("FreeRoll_Used", new Dictionary<string, object>{});
				#endif

				#if UNITY_ANDROID && !UNITY_EDITOR
				//NotificationManager.SendWithAppIcon(System.TimeSpan.FromSeconds(Defines.FREE_ROLL_TIMER), "Ultimate Tic Tac Toe", "Get your free roll now!", Color.black);
				#endif

				#if UNITY_IOS && !UNITY_EDITOR
				// @todo apple notification here
				#endif

				Adverts.Instance.freeGacha = true;
			}

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.NETWORK_BACKTOMAINMENU:
			if(GlobalScript.Instance.network_allowButtonClicks == 1)
			{
				currScreen = SCREENS.ONLINEPLAY;
				Camera.main.GetComponent<MainMenuScript>().networkMenuAnimStage = 2;

				if(GameObject.Find("Password"))
					GameObject.Find("Password").GetComponent<InputField>().interactable = true;

				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SearchGrey.SetActive(false);

				Camera.main.GetComponent<MainMenuScript>().UpdateText.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().UpdateConnectingText.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().UpdateText_PublicGame.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().PasswordText.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().PasswordField.SetActive(false);
				Camera.main.GetComponent<MainMenuScript>().SearchBtn.SetActive(false);

				Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
				Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);

				GlobalScript.Instance.network_allowButtonClicks = 0;

				GlobalScript.Instance.LeaveRoom();
		        GlobalScript.Instance.ResetCountdown();
				GlobalScript.Instance.ResetGreyBtns();

				if(AudioManager.Instance)
					AudioManager.Instance.PlaySoundEvent(SOUNDID.BACK);
			}
			else
			{
				currScreen = SCREENS.MAINMENU;
				Camera.main.GetComponent<MainMenuScript>().ChangeScreen(0, true);
				Camera.main.GetComponent<MainMenuScript>().Avatar.SetActive(true);
				Camera.main.GetComponent<MainMenuScript>().Settings.SetActive(true);

				if(AudioManager.Instance)
					AudioManager.Instance.PlaySoundEvent(SOUNDID.BACK);

		        //GlobalScript.Instance.LeaveRoom();
		        //GlobalScript.Instance.ResetCountdown();
			}
			break;

		case BUTTONTYPES.SETTIME_1:
			GlobalScript.Instance.SetTurnTime(1);
			Camera.main.GetComponent<MainMenuScript>().SetTimerImage(GlobalScript.Instance.gameMode, 1);

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.AVATARSELECT);
			break;

		case BUTTONTYPES.SETTIME_2:
			GlobalScript.Instance.SetTurnTime(2);
			Camera.main.GetComponent<MainMenuScript>().SetTimerImage(GlobalScript.Instance.gameMode, 2);

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.AVATARSELECT);
			break;

		case BUTTONTYPES.SETTIME_3:
			GlobalScript.Instance.SetTurnTime(3);
			Camera.main.GetComponent<MainMenuScript>().SetTimerImage(GlobalScript.Instance.gameMode, 3);

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.AVATARSELECT);
			break;

		case BUTTONTYPES.SETTINGS_DIABLE_ADS:
			Camera.main.GetComponent<MainMenuScript>().EnableDisableAdsButton(false);

			GameObject IAPManager = GameObject.Find("IAPManager");
			InAppPurchaser iapPurchaser = IAPManager.GetComponent<InAppPurchaser>();
			iapPurchaser.BuyProduct( InAppProductList.ProductType.ADS, (int)Defines.AdsInAppPurchase.DISABLE );

			Analytics.CustomEvent("PreBuy_Ads", new Dictionary<string, object>{});

			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.RATE_OUR_APP:
			PlatformUtilies.Instance.DisplayRateUs();
			Camera.main.GetComponent<MainMenuScript>().RateAppScreen.SetActive(false);
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.NO_RATE_OUR_APP:
			Camera.main.GetComponent<MainMenuScript>().RateAppScreen.SetActive(false);
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.LIKE_OUR_FACEBOOK:
			PlatformUtilies.Instance.DisplayFacebookPage();
			Camera.main.GetComponent<MainMenuScript>().LikeFacebookScreen.SetActive(false);
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.NO_LIKE_OUR_FACEBOOK:
			Camera.main.GetComponent<MainMenuScript>().LikeFacebookScreen.SetActive(false);
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.SETTINGS_BGM:
			GameData.current.hasBGM = !GameData.current.hasBGM;
			SaveLoad.Save();
			UpdateBGMButton();
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.SETTINGS_SFX:
			GameData.current.hasSFX = !GameData.current.hasSFX;
			SaveLoad.Save();
			UpdateSFXButton();
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.SETTINGS_VIBRATION:
			GameData.current.hasVibrate = !GameData.current.hasVibrate;
			SaveLoad.Save();
			UpdateVibrateButton();
			if(AudioManager.Instance)
				AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
			break;

		case BUTTONTYPES.ALERT_BACK:
			Camera.main.GetComponent<MainMenuScript>().AlertBox.SetActive(false);
			BtnClick((int)BUTTONTYPES.NETWORK_BACKTOMAINMENU);
			break;

		default:
			break;
		}
	}
}
