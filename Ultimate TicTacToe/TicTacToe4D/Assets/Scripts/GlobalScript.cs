using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GlobalScript : MonoBehaviour
{
	public string nameP1;
	public string nameP2;
	public float volSlider; // slider?
	public bool isBGMPlaying = false;
	string myName;

	public int avatarState; // 0 = None, 1 = LocalPlay_P1, 2 = LocalPlay_P2, 3 = Avatar

	/*public int startingPlayer;
	public GameObject starter1;
	public GameObject starter2;
	public Sprite tNormalP1;
	public Sprite tNormalP2;
	public Sprite tDepressedP1;
	public Sprite tDepressedP2;*/

	public int noofIcons = 5;
	int gameInit;
	public int gameMode;
	public int network_allowButtonClicks;
	public bool isInputPaused;

	public bool bStartCountdown;
	public float countdownTimerToStart;

    public GameObject matchMaker;
	public static string[] randomName = { "Tom", "Dick", "Harry", "John" };

	//public const int minIcon = (int)Defines.ICONS.C003_BOY;
	//public const int maxIcon = (int)Defines.ICONS.TREBLE;
	public int myIcon = (int)Defines.ICON_DEFAULT_P1;
	public int iconP1 = (int)Defines.ICON_DEFAULT_P1;
	public int iconP2 = (int)Defines.ICON_DEFAULT_P2;

	public float timePerTurn;

	// Singleton pattern
	static GlobalScript instance;
	public static GlobalScript Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
		{
			//throw new System.Exception("You have more than 1 GlobalScript in the scene.");
			Destroy(this);
			return;
		}

		// Initialize the static class variables
		instance = this;

		DontDestroyOnLoad(gameObject);
	}

	void Start ()
	{
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.orientation = ScreenOrientation.AutoRotation;

		ResetVars();
	}

	void ResetVars()
	{
		UpdateStartingPlayer(1);

		//nameP1 = "Player 1";
		//nameP2 = "Player 2";

		network_allowButtonClicks = 0;
		bStartCountdown = false;
		gameInit = 0;

		countdownTimerToStart = 5.0f;
	}

    public void ResetCountdown()
    {
        bStartCountdown = false;
        countdownTimerToStart = 5.0f;

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().UpdateText.SetActive(false);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().UpdateText_PublicGame.SetActive(false);
    }

	void Update ()
	{
		//if(SceneManager.GetActiveScene().name == "MainMenu" && Camera.main.GetComponent<MainMenuScript>().screenState == 1)
		//{
		//	IconP1.GetComponent<Image>().sprite = sAllIcons[playerIcon_1];
		//	IconP2.GetComponent<Image>().sprite = sAllIcons[playerIcon_2];
		//}
		//if(SceneManager.GetActiveScene().name == "MainMenu" && Camera.main.GetComponent<MainMenuScript>().screenState == 1)
		if(gameInit == 0)
		{
			//IconP1.GetComponent<Image>().sprite = iconManager.GetIcon(playerIcon_1);
			//IconP2.GetComponent<Image>().sprite = iconManager.GetIcon(playerIcon_2);
			gameInit = 1;
		}

		if (bStartCountdown)
		{
			Debug.Log("Counting downing");
			UpdateCountdownToPlay();
		}
	}

	public void SetPlayerIcon( int icon )
	{
		myIcon = icon;
		Camera.main.GetComponent<MainMenuScript>().PlayerIcon.GetComponent<Image>().sprite =
		IconManager.Instance.GetIcon(myIcon);
	}

	// Used to cycle grid sprites when choosing icons before the game starts.
	//public void CyclePlayerIcon( /*int _player*/ )
	//{
	//	myIcon = ( ( myIcon + 1 - minIcon ) % ( maxIcon - minIcon + 1 ) ) + minIcon;
	//	Camera.main.GetComponent<MainMenuScript>().PlayerIcon.GetComponent<Image>().sprite = 
	//		IconManager.Instance.GetIcon(myIcon);

		/*if(_player == 1)
		{
			playerIcon_1++;

			if(playerIcon_1 >= noofIcons)
				playerIcon_1 = 1;

			if(playerIcon_1 == playerIcon_2)
				playerIcon_1++;

			if(playerIcon_1 >= noofIcons)
				playerIcon_1 = 1;
		}
		else if(_player == 2)
		{
			playerIcon_2++;

			if(playerIcon_2 >= noofIcons)
				playerIcon_2 = 1;

			if(playerIcon_2  == playerIcon_1)
				playerIcon_2++;

			if(playerIcon_2 >= noofIcons)
				playerIcon_2 = 1;
		}*/
	//}

	public void UpdateStartingPlayer(int _player)
	{
		/*startingPlayer = _player;
		if(startingPlayer == 1)
		{
			starter1.GetComponent<Image>().sprite = tDepressedP1;
			starter2.GetComponent<Image>().sprite = tNormalP2;
		}
		else if(startingPlayer == 2)
		{
			starter1.GetComponent<Image>().sprite = tNormalP1;
			starter2.GetComponent<Image>().sprite = tDepressedP2;
		}*/
	}

	public void UpdatePlayer1Name(string str)
	{
		if(str == "")
			return;
		nameP1 = str.Substring(0, System.Math.Min(Defines.MAX_PLAYER_NAME_LENGTH, str.Length));
		//Debug.Log (nameP1);
	}

	public void UpdatePlayer2Name(string str)
	{
		if(str =="")
			return;
		nameP2 = str.Substring(0, System.Math.Min(Defines.MAX_PLAYER_NAME_LENGTH, str.Length));		
		//Debug.Log (nameP2);
	}

	static public string RandomAName()
	{
		int index = Random.Range(0, randomName.Length);
		return randomName[index];
	}

	public void SetPlayerName()
	{
		//string playerName = "Dick"; //GameObject.Find("Player Info").GetComponentInChildren<InputField>().text;
		string playerName = AvatarHandler.Instance.currAvatarText.text;
		if (playerName == "")
		{
			playerName = RandomAName();
			//GameObject.Find("Player Info").GetComponentInChildren<InputField>().text = playerName;
			AvatarHandler.Instance.currAvatarText.text = playerName;
		}

		//myName = playerName;
	}

	public void SetMyPlayerName()
	{
		if (MatchMaker.IsPlayerOne())
		{
			//UpdatePlayer1Name(myName);
			UpdatePlayer1Name(AvatarHandler.Instance.currAvatarText.text);
		}
		else
		{
			//UpdatePlayer2Name(myName);
			UpdatePlayer2Name(AvatarHandler.Instance.currAvatarText.text);
		}
	}

	public string GetMyPlayerName()
	{
		return MatchMaker.IsPlayerOne() ? nameP1 : nameP2;
	}

	public void SetMyPlayerIcon()
	{
		if (MatchMaker.IsPlayerOne())
		{
			//iconP1 = myIcon;
			iconP1 = GameData.current.avatarIcon;
		}
		else
		{
			//iconP2 = myIcon;
			iconP2 = GameData.current.avatarIcon;
		}
	}

	public void SetSinglePlayerName()
	{
		//iconP1 = myIcon;
		UpdatePlayer1Name(AvatarHandler.Instance.currAvatarText.text);
		UpdatePlayer2Name("Jerry");
	}

	public void SetSinglePlayerIcon()
	{
		//iconP1 = myIcon;
		iconP1 = GameData.current.avatarIcon;

		if(GameData.current.avatarIcon == (int)Defines.ICON_DEFAULT_P2)
			iconP2 = (int)Defines.ICON_DEFAULT_P1;
		else
			iconP2 = (int)Defines.ICON_DEFAULT_P2;
	}

	public void SetLocalMultiPlayerName()
	{
		//iconP1 = myIcon;
		UpdatePlayer1Name(AvatarHandler.Instance.localAvatarName_P1);
		UpdatePlayer2Name(AvatarHandler.Instance.localAvatarName_P2);
	}

	public void SetLocalMultiPlayerIcon()
	{
		//iconP1 = myIcon;
		iconP1 = AvatarHandler.Instance.localAvatar_P1;
		iconP2 = AvatarHandler.Instance.localAvatar_P2;
	}

	public int GetMyPlayerIcon()
	{
		//return myIcon;
		return GameData.current.avatarIcon;
	}

	static public void ShowRoomChoice(bool show)
	{
		GameObject.Find("Player Info").GetComponentInChildren<InputField>().enabled = show;
		GameObject.Find("Player Info").transform.FindChild("Image").GetComponent<Button>().interactable = show;

		Color iconColor = GameObject.Find("Player Info").transform.FindChild("Image").GetComponent<Image>().color;
		if ( !show )
		{
			iconColor.a = 0.8f;
		}
		else
		{
			iconColor.a = 1.0f;
		}
		GameObject.Find("Player Info").transform.FindChild("Image").GetComponent<Image>().color = iconColor;
	}

	public void FindPublicGame()
	{
		SetPlayerName();
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().UpdateText_PublicGame.SetActive(true);
        // This is a non update fn, so if you want to search for game here, need some kind of boolean trigger.
        //bStartCountdown = true;
        matchMaker.GetComponent<MatchMaker>().roomInputField.GetComponent<InputField>().text = "";
        matchMaker.GetComponent<MatchMaker>().JoiningRoom();

		//ShowRoomChoice(false);

		network_allowButtonClicks = 1;
	}

	public void FindFriendGame()
	{
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().SearchBtn.SetActive(true);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().PasswordText.SetActive(true);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().PasswordField.SetActive(true);

		//ShowRoomChoice(false);
		//SetPlayerName();

		network_allowButtonClicks = 1;
	}

	public void SearchFriend()
	{
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().UpdateText.SetActive(true);

        // This is a non update fn, so if you want to search for friend here, need some kind of boolean trigger.
        //bStartCountdown = true;
        matchMaker.GetComponent<MatchMaker>().JoiningRoom();
		GameObject.Find("Password").GetComponent<InputField>().enabled = false;
	}

    public void LeaveRoom()
    {
        matchMaker.GetComponent<MatchMaker>().LeaveRoom();
	}

    public void FoundFriend()
    {
		//bStartCountdown = true;
		string name = GetMyPlayerName();
		Debug.Log("Sending my friend: " + name);
		matchMaker.GetComponent<MatchMaker>().SendPlayerName(GetMyPlayerName(), GetMyPlayerIcon());
	}

	public void StartCountdown()
	{
		bStartCountdown = true;
		Debug.Log("Starting countdown...");
	}

	public void UpdateCountdownToPlay()
	{
		//Debug.Log(countdownTimerToStart);
		countdownTimerToStart -= Time.deltaTime;

		// Temp.
		if(countdownTimerToStart < 0.0f)
		{
			bStartCountdown = false;

			countdownTimerToStart = 5.0f;
			GameObject.FindGameObjectWithTag("MainMenuCanvas").GetComponent<MenuBtnScript>().isStartingGame = true;
		}
	}

	public void SetTurnTime(int i)
	{
		if(i == 1)
			timePerTurn = Defines.TIMEPERTURN_1;
		else if(i == 2)
			timePerTurn = Defines.TIMEPERTURN_2;
		else if(i == 3)
			timePerTurn = Defines.TIMEPERTURN_3;
	}

	public void SetGreyBtns()
	{
		if(timePerTurn == Defines.TIMEPERTURN_1)
		{
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn1.SetActive(false);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn2.SetActive(true);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn3.SetActive(true);
		}
		else if(timePerTurn == Defines.TIMEPERTURN_2)
		{
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn1.SetActive(true);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn2.SetActive(false);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn3.SetActive(true);
		}
		else if(timePerTurn == Defines.TIMEPERTURN_3)
		{
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn1.SetActive(true);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn2.SetActive(true);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn3.SetActive(false);
		}
	}

	public void ResetGreyBtns()
	{
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn1.SetActive(false);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn2.SetActive(false);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().greyTimerBtn3.SetActive(false);

		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().findGameAnim1.SetActive(false);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().findGameAnim2.SetActive(false);
	}
}