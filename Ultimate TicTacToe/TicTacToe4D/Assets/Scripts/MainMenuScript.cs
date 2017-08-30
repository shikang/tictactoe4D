using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
	public GameObject InputP1;
	public GameObject InputP2;

	public GameObject MainMenuScreen;
	public GameObject StartGameLocalMultiplayerScreen;
	public GameObject StartGameNetworkedScreen;
	public GameObject OptionsScreen;
	public GameObject HowToPlayScreen;
	public GameObject AvatarScreen;
	public GameObject GachaScreen;

	public GameObject SettingsScreen;
	public GameObject CreditsPage;
	public GameObject HowToPlayPage;
	public GameObject RateAppScreen;
	public GameObject LikeFacebookScreen;

	public GameObject UpdateText;
	public GameObject UpdateConnectingText;
	public GameObject UpdateText_PublicGame;
	public GameObject UpdateText_Connecting;
	public GameObject PasswordText;
	public GameObject PasswordField;
	public GameObject SearchBtn;

	public GameObject greyTimerBtn1;
	public GameObject greyTimerBtn2;
	public GameObject greyTimerBtn3;
	public GameObject findGameAnim1;
	public GameObject findGameAnim2;

	public GameObject FindFriendBtn;
	public GameObject JoinPublicBtn;
	public GameObject SearchGrey;

	public GameObject Avatar;
	public GameObject Settings;

	public GameObject FindFriendButton;
	public GameObject JoinPublicButton;
	public GameObject PlayerNameInput;
	public GameObject PlayerIconInput;
	public GameObject RoomNameInput;

	public GameObject DisableAdsButton;
	public GameObject avatarObject;
	public GameObject AlertBox;
	public GameObject AlertBoxLabel;

    public GameObject PlayerIcon;
	public int screenState;

    public GameObject BlackOverlay;

	GameObject currScreen;
	GameObject nextScreen;
	public bool moveScreen;
	int moveDirection;
	float screenMaxPosX;
	float screenMoveSpeed;

    // variables to lerp screen fade in out
    Image blackOverlayImage;
    float currentFadeTime;
    float faderDuration;
    bool isFading;

    // variables to lerp screen left right
    float currentMoveTime;
    float moveDuration;
    bool isMoving;

	bool shownRate;
	bool shownLike;

    public GameObject setTimer_local_1;
	public GameObject setTimer_local_2;
	public GameObject setTimer_local_3;
	public GameObject setTimer_network_1;
	public GameObject setTimer_network_2;
	public GameObject setTimer_network_3;
	public Sprite normalButton;
	public Sprite depressedButton;

	public int networkMenuAnimStage;

    enum MenuScreens
    {
        eMainMenuScreen,
        eLocalPlay,
        eNetworkPlay,
        eGatcha,
        eAvatarScreen,
    }

	void Start ()
	{
		screenMaxPosX = 1130.0f;
		screenMoveSpeed = 3500.0f;

        blackOverlayImage = BlackOverlay.GetComponent(typeof(Image)) as Image;

        screenState = 0;
		DisplayScreen(screenState);
		currScreen = MainMenuScreen;

        //PlayerIcon.GetComponent<Image>().color = Defines.ICON_COLOR_P1;

        networkMenuAnimStage = 0;

		GlobalScript.Instance.SetTurnTime(3);
		setTimer_local_3.GetComponent<Image>().sprite = depressedButton;
		setTimer_network_3.GetComponent<Image>().sprite = depressedButton;

		shownRate = false;
		shownLike = false;

		//SetupFade(2.0f);
		BlackOverlay.SetActive(true);
		BlackOverlay.GetComponent<Image>().color = Color.black;
		isFading = true;

		if(!GlobalScript.Instance.isBGMPlaying && GameData.current.hasBGM && AudioManager.Instance)
		{
			AudioManager.Instance.PlaySoundEvent(SOUNDID.BGM);
		}

		// @debug
		//Assets.SimpleAndroidNotifications.NotificationManager.SendWithAppIcon(System.TimeSpan.FromSeconds(Defines.FREE_ROLL_TIMER), "Ultimate Tic Tac Toe", "Get your free roll now!", Color.black);
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.K))
			AlertBox.SetActive(true);

		/*if(screenState == 1)
		{
			foreach(Transform curr in InputP1.GetComponentsInChildren<Transform>())
			{
				if(curr.gameObject.GetComponent<Text>())
					GlobalScript.Instance.nameP1 = curr.gameObject.GetComponent<Text>().text;
			}
			foreach(Transform curr in InputP2.GetComponentsInChildren<Transform>())
			{
				if(curr.gameObject.GetComponent<Text>())
					GlobalScript.Instance.nameP2 = curr.gameObject.GetComponent<Text>().text;
			}
		}*/

		if(moveScreen)
		{
			UpdateScreenPos();
		}
		else
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}

        //Debug.Log(blackOverlayImage.color.a + "," + isFading + "," + faderDuration);
        if(isFading)
        {
        	Color temp = BlackOverlay.GetComponent<Image>().color;
        	temp.a -= Time.deltaTime * 0.8f;
			BlackOverlay.GetComponent<Image>().color = temp;

			if(temp.a <= 0.0f)
			{
				isFading = false;
				BlackOverlay.SetActive(false);
			}
        }

		if (!isFading && !shownLike && !GameData.current.shownRateApp && GameData.current.matchPlayed >= Defines.TIMES_TO_SHOW_RATE_APP)
		{
			GameData.current.shownRateApp = true;
			RateAppScreen.SetActive(true);
			SaveLoad.Save();

			shownRate = true;
		}

		if (!isFading && !shownRate && !GameData.current.shownLikeFacebook && GameData.current.matchPlayed >= Defines.TIMES_TO_SHOW_LIKE_FB)
		//if (!isFading)
		{
			GameData.current.shownLikeFacebook = true;
			LikeFacebookScreen.SetActive(true);
			SaveLoad.Save();

			shownLike = true;
		}

		if (networkMenuAnimStage != 0)
			UpdateNetworkMenuAnim();

        currentFadeTime += Time.deltaTime;
        currentMoveTime += Time.deltaTime;

		SpinIcons();
    }

	// Change Screen is the animation; once done, then this is called to hide unwanted screens
	public void DisplayScreen(int _changeTo)
	{
		MainMenuScreen.SetActive(false);
		StartGameLocalMultiplayerScreen.SetActive(false);
		StartGameNetworkedScreen.SetActive(false);
		SettingsScreen.SetActive(false);
		AvatarScreen.SetActive(false);
		GachaScreen.SetActive(false);
		RateAppScreen.SetActive(false);
		LikeFacebookScreen.SetActive(false);
		avatarObject.SetActive(false);
        //OptionsScreen.SetActive(false);
        //HowToPlayScreen.SetActive(false);
        //CreditScreen.SetActive(false);

        switch (screenState)
		{
		case 0:
            MainMenuScreen.SetActive(true);
            //kelvin: this is supposed to fade in only from intro screen
            //SetupFade(2.0f);
            break;

		case 1:
		// Local Multiplayer
			StartGameLocalMultiplayerScreen.SetActive(true);
			avatarObject.SetActive(true);
			//GlobalScript.Instance.gameMode = 2;

			AvatarHandler.Instance.scrollGrandparent.GetComponent<RectTransform>().localPosition = new Vector3(0.3f, -65.5f, 0.0f);
			AvatarHandler.Instance.scrollGrandparent.GetComponent<RectTransform>().sizeDelta = new Vector2(163.4f, 68.1f);
			AvatarHandler.Instance.scrollParent.GetComponent<RectTransform>().localPosition = new Vector3(-0.0f, -34.0f, 0.0f);
			AvatarHandler.Instance.scrollParent.GetComponent<RectTransform>().sizeDelta = new Vector2(314.0f, 200.6f);
			break;

		case 2:
		// Network
			StartGameNetworkedScreen.SetActive(true);
			UpdateText.SetActive(false);
			UpdateConnectingText.SetActive(false);
			UpdateText_PublicGame.SetActive(false);
			UpdateText_Connecting.SetActive(false);
			PasswordText.SetActive(false);
			PasswordField.SetActive(false);
			SearchBtn.SetActive(false);
			SearchGrey.SetActive(false);
			FindFriendButton.SetActive(true);
			JoinPublicButton.SetActive(true);
				
			GlobalScript.Instance.ResetGreyBtns();
			//PlayerNameInput.GetComponent<InputField>().enabled = true;
			//PlayerIconInput.GetComponent<Button>().interactable = true;
			//Color iconColor = PlayerIconInput.GetComponent<Image>().color;
			//iconColor.a = 1.0f;
			//PlayerIconInput.GetComponent<Image>().color = iconColor;
			RoomNameInput.GetComponent<InputField>().enabled = true;
			break;

		case 3:
			GachaScreen.SetActive(true);
			break;

		case 4:
			AvatarScreen.SetActive(true);
			avatarObject.SetActive(true);

			AvatarHandler.Instance.scrollGrandparent.GetComponent<RectTransform>().localPosition = new Vector3(0.3f, -44.3f, 0.0f);
			AvatarHandler.Instance.scrollGrandparent.GetComponent<RectTransform>().sizeDelta = new Vector2(163.4f, 110.7f);
			AvatarHandler.Instance.scrollParent.GetComponent<RectTransform>().localPosition = new Vector3(-0.0f, -57.3f, 0.0f);
			AvatarHandler.Instance.scrollParent.GetComponent<RectTransform>().sizeDelta = new Vector2(314.0f, 197.8f);
			break;

		default:
			break;
		}
	}

	public void ChangeScreen(int _nextScreen, bool _moveBack = false)
	{
		moveDirection = _moveBack ? -1 : 1;
		screenState = _nextScreen;
		nextScreen = GetScreen(screenState);
		nextScreen.SetActive(true);
		nextScreen.transform.localPosition = new Vector3(screenMaxPosX * moveDirection, 0.0f, 0.0f);
		moveScreen = true;

		if(_moveBack)
			avatarObject.SetActive(false);

        SetupMove(1.0f);
	}

	void UpdateScreenPos()
	{
        
		Vector3 temp = currScreen.transform.localPosition;
		temp.x -= screenMoveSpeed * moveDirection * Time.deltaTime;
		currScreen.transform.localPosition = temp;

		temp = nextScreen.transform.localPosition;
		temp.x -= screenMoveSpeed * moveDirection * Time.deltaTime;
		nextScreen.transform.localPosition = temp;
        
		if( (moveDirection == 1 && temp.x <= 0.0f) || (moveDirection == -1 && temp.x >= 0) )
		{
			nextScreen.transform.localPosition = Vector3.zero;
			moveScreen = false;
			currScreen = nextScreen;
			DisplayScreen(screenState);
		}
        
        
        /*
        if (currentMoveTime <= moveDuration)
        {
            Vector3 temp = currScreen.transform.localPosition;
            //temp.x -= PennerEasing.Instance.Linear(currentMoveTime, 0.0f, -30.0f, moveDuration);
            //currScreen.transform.localPosition = temp;

            temp = nextScreen.transform.localPosition;
            temp.x -= PennerEasing.Instance.easeInElastic(currentMoveTime, 30.0f, 0.0f, moveDuration);
            nextScreen.transform.localPosition = temp;

            Debug.Log(nextScreen.transform.localPosition);
        }
        else //(currentFadeTime > faderDuration)
        {
            Debug.Log(nextScreen.transform.localPosition);
            //nextScreen.transform.localPosition = Vector3.zero;
            currScreen = nextScreen;
            isMoving = false;
            DisplayScreen(screenState);
        }
        */
    }

	GameObject GetScreen(int i)
	{
		switch(i)
		{
		case 0:	return MainMenuScreen;
		case 1:	return StartGameLocalMultiplayerScreen;
		case 2: return StartGameNetworkedScreen;
		case 3: return GachaScreen;
		case 4: return AvatarScreen;
		default: return null;
		}
	}

	public void SetTimerImage(int mode, int i)
	{
		if(i == 1)
		{
			if(mode == 1) // local
			{
				setTimer_local_1.GetComponent<Image>().sprite = depressedButton;
				setTimer_local_2.GetComponent<Image>().sprite = normalButton;
				setTimer_local_3.GetComponent<Image>().sprite = normalButton;
			}
			else if(mode == 2) // network
			{
				setTimer_network_1.GetComponent<Image>().sprite = depressedButton;
				setTimer_network_2.GetComponent<Image>().sprite = normalButton;
				setTimer_network_3.GetComponent<Image>().sprite = normalButton;
			}
		}
		else if(i == 2)
		{
			if(mode == 1) // local
			{
				setTimer_local_1.GetComponent<Image>().sprite = normalButton;
				setTimer_local_2.GetComponent<Image>().sprite = depressedButton;
				setTimer_local_3.GetComponent<Image>().sprite = normalButton;
			}
			else if(mode == 2) // network
			{
				setTimer_network_1.GetComponent<Image>().sprite = normalButton;
				setTimer_network_2.GetComponent<Image>().sprite = depressedButton;
				setTimer_network_3.GetComponent<Image>().sprite = normalButton;
			}
		}
		else if(i == 3)
		{
			if(mode == 1) // local
			{
				setTimer_local_1.GetComponent<Image>().sprite = normalButton;
				setTimer_local_2.GetComponent<Image>().sprite = normalButton;
				setTimer_local_3.GetComponent<Image>().sprite = depressedButton;
			}
			else if(mode == 2) // network
			{
				setTimer_network_1.GetComponent<Image>().sprite = normalButton;
				setTimer_network_2.GetComponent<Image>().sprite = normalButton;
				setTimer_network_3.GetComponent<Image>().sprite = depressedButton;
			}
		}
	}

	void UpdateNetworkMenuAnim()
	{
		if(networkMenuAnimStage == 1)
		{
			float yPos = JoinPublicBtn.GetComponent<RectTransform>().localPosition.y;
			JoinPublicBtn.GetComponent<RectTransform>().localPosition =
				Vector3.Lerp(JoinPublicBtn.GetComponent<RectTransform>().localPosition, new Vector3(-160.0f, yPos, 0.0f), Time.deltaTime * 4.0f);

			yPos = FindFriendBtn.GetComponent<RectTransform>().localPosition.y;
			FindFriendBtn.GetComponent<RectTransform>().localPosition =
				Vector3.Lerp(FindFriendBtn.GetComponent<RectTransform>().localPosition, new Vector3(160.0f, yPos, 0.0f), Time.deltaTime * 4.0f);
		}

		else if(networkMenuAnimStage == 2)
		{
			float yPos = JoinPublicBtn.GetComponent<RectTransform>().localPosition.y;
			JoinPublicBtn.GetComponent<RectTransform>().localPosition =
				Vector3.Lerp(JoinPublicBtn.GetComponent<RectTransform>().localPosition, new Vector3(0.0f, yPos, 0.0f), Time.deltaTime * 6.0f);

			yPos = FindFriendBtn.GetComponent<RectTransform>().localPosition.y;
			FindFriendBtn.GetComponent<RectTransform>().localPosition =
				Vector3.Lerp(FindFriendBtn.GetComponent<RectTransform>().localPosition, new Vector3(0.0f, yPos, 0.0f), Time.deltaTime * 6.0f);
		}
	}

	public void EnableDisableAdsButton(bool enable)
	{
		DisableAdsButton.GetComponent<Button>().enabled = enable;
	}

	public void DisableDisableAdsButton()
	{
		DisableAdsButton.GetComponent<Button>().interactable = false;
	}

    public void SetupFade(float duration)
    {
        faderDuration = duration;
        currentFadeTime = 0.0f;
        isFading = true;
        blackOverlayImage.rectTransform.gameObject.SetActive(true);
    }

    public void SetupMove(float duration)
    {
        moveDuration = duration;
        currentMoveTime = 0.0f;
        isMoving = true;
    }

    /* void blackFadeTransition()
    {
        if (currentFadeTime <= faderDuration)
        {
            blackOverlayImage.color = new Color(blackOverlayImage.color.r, blackOverlayImage.color.g, blackOverlayImage.color.b, PennerEasing.Instance.LinearInOut(currentFadeTime, 0.0f, 1.0f, faderDuration));
        }
        else //(currentFadeTime > faderDuration)
        {
            isFading = false;
            blackOverlayImage.rectTransform.gameObject.SetActive(false);
        } 
    }*/

    public void SetAnim(int id)
    {
    	int rand = Random.Range((int)Defines.Avatar_FirstIcon+1, (int)(Defines.ICONS.TOTAL)-1);

    	if(id == 1)
    	{
			findGameAnim1.SetActive(true);
			findGameAnim1.GetComponentInChildren<Image>().sprite = IconManager.Instance.GetIcon(rand);
		}
		else if(id == 2)
		{
			findGameAnim2.SetActive(true);
			findGameAnim2.GetComponentInChildren<Image>().sprite = IconManager.Instance.GetIcon(rand);
		}
    }

    public void SpinIcons()
    {
    	if(findGameAnim1.activeInHierarchy)
    	{
			foreach(Transform curr in findGameAnim1.GetComponent<Transform>())
			{
				curr.Rotate(0.0f, 0.0f, 6.0f);
			}
		}

		if(findGameAnim2.activeInHierarchy)
    	{
			foreach(Transform curr in findGameAnim2.GetComponent<Transform>())
			{
				curr.Rotate(0.0f, 0.0f, 6.0f);
			}
		}
    }

	public void OnConnected(bool pub)
	{
		if (pub)
		{
			UpdateText_PublicGame.SetActive(true);
			UpdateText_Connecting.SetActive(false);
		}
		else
		{
			UpdateText.SetActive(true);
			UpdateText.GetComponent<Text>().text = "Finding Game...";
			UpdateConnectingText.SetActive(false);
		}
	}
}