using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

enum STAGE
{
	BlackBG = 0,
	PlayerLerpIn,
	ScreenBlink,
	IconScaling,
	PlayerLerpOut,
	FadeOut,
	EndAnim
};

enum HEADER
{
	MoveDown = 0,
	BounceUp,
	End
};

public class GameStartAnim : MonoBehaviour
{
	STAGE currStage;
	public GameObject blackScreen;
	public GameObject whiteScreen;
	float scaleTimer;
	bool fadeOut;

	bool fadeToMenu;
	int nextScreen;

	public GameObject playerGroup1;
	public GameObject playerGroup2;

	public GameObject playerIcon1;
	public GameObject playerIcon2;
	public GameObject playerName1;
	public GameObject playerName2;

	public GameObject header;
	HEADER headerStage;

	// Singleton pattern
	static GameStartAnim instance;
	public static GameStartAnim Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
			throw new System.Exception("You have more than 1 GameStartAnim in the scene.");

		// Initialize the static class variables
		instance = this;
	}

	void Start ()
	{
		currStage = STAGE.BlackBG;
		headerStage = HEADER.MoveDown;
		scaleTimer = 2.5f;
		fadeOut = false;
		fadeToMenu = false;

		playerIcon1.GetComponent<Image>().sprite = IconManager.Instance.GetIcon((Defines.ICONS)GlobalScript.Instance.iconP1);
		playerIcon2.GetComponent<Image>().sprite = IconManager.Instance.GetIcon((Defines.ICONS)GlobalScript.Instance.iconP2);
		playerName1.GetComponent<Text>().text = GlobalScript.Instance.nameP1;
		playerName2.GetComponent<Text>().text = GlobalScript.Instance.nameP2;
	}

	void Update ()
	{
		UpdateGameStartAnim();
		UpdateHeaderAnim();

		if(fadeToMenu)
		{
			Color tmp = blackScreen.GetComponent<Image>().color;
			tmp.a += Time.deltaTime * 1.0f;
			blackScreen.GetComponent<Image>().color = tmp;

			if(tmp.a >= 1.0f)
			{
				if(nextScreen == 1)
					SceneManager.LoadScene("MainMenu");
				else if(nextScreen == 2)
					SceneManager.LoadScene("GameScene");
			}
		}


	}

	void UpdateGameStartAnim()
	{
		if(currStage == STAGE.BlackBG)
		{
			blackScreen.SetActive(true);

			Color tmp = blackScreen.GetComponent<Image>().color;
			tmp.a -= Time.deltaTime * 1.0f;
			blackScreen.GetComponent<Image>().color = tmp;

			if(tmp.a <= 0.5f)
				currStage = STAGE.PlayerLerpIn;
		}

		else if(currStage == STAGE.PlayerLerpIn)
		{
			playerGroup1.transform.localPosition = Vector3.Lerp(playerGroup1.transform.localPosition, new Vector3(-100.0f, 0.0f, 0.0f), Time.deltaTime * 3.0f);
			playerGroup2.transform.localPosition = Vector3.Lerp(playerGroup2.transform.localPosition, new Vector3(100.0f, 0.0f, 0.0f), Time.deltaTime * 3.0f);

			if(Vector3.Distance(playerGroup1.transform.localPosition, new Vector3(-100.0f, 0.0f, 0.0f)) < 10.0f)
			{
				currStage = STAGE.ScreenBlink;

				if(AudioManager.Instance)
					AudioManager.Instance.PlaySoundEvent(SOUNDID.STARTGAME_FLASH);
			}
		}

		else if(currStage == STAGE.ScreenBlink)
		{
			Color tmp;

			if(!fadeOut)
			{
				tmp = whiteScreen.GetComponent<Image>().color;
				tmp.a += Time.deltaTime * 7.0f;
				whiteScreen.GetComponent<Image>().color = tmp;

				if(tmp.a >= 0.8f)
					fadeOut = true;
			}
			else
			{
				tmp = whiteScreen.GetComponent<Image>().color;
				tmp.a -= Time.deltaTime * 1.6f;
				whiteScreen.GetComponent<Image>().color = tmp;

				if(tmp.a <= 0.2f)
				{
					currStage = STAGE.IconScaling;
					whiteScreen.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

					if(AudioManager.Instance)
						AudioManager.Instance.PlaySoundEvent(SOUNDID.STARTGAME_SCALING);
				}
			}
		}

		else if(currStage == STAGE.IconScaling)
		{
			playerIcon1.GetComponent<Animator>().SetBool("isPlaying", true);
			playerIcon2.GetComponent<Animator>().SetBool("isPlaying", true);
			//playerName1.GetComponent<Animator>().SetBool("isPlaying", true);
			//playerName2.GetComponent<Animator>().SetBool("isPlaying", true);

			scaleTimer -= Time.deltaTime;
			if(scaleTimer <= 0.0f)
			{
				currStage = STAGE.PlayerLerpOut;

				playerIcon1.GetComponent<Animator>().SetBool("isPlaying", false);
				playerIcon2.GetComponent<Animator>().SetBool("isPlaying", false);
				//playerName1.GetComponent<Animator>().SetBool("isPlaying", false);
				//playerName2.GetComponent<Animator>().SetBool("isPlaying", false);
			}
		}

		else if(currStage == STAGE.PlayerLerpOut)
		{
			playerGroup1.transform.localPosition = Vector3.Lerp(playerGroup1.transform.localPosition, new Vector3(-300.0f, 0.0f, 0.0f), Time.deltaTime * 8.0f);
			playerGroup2.transform.localPosition = Vector3.Lerp(playerGroup2.transform.localPosition, new Vector3(300.0f, 0.0f, 0.0f), Time.deltaTime * 8.0f);

			if(Vector3.Distance(playerGroup1.transform.localPosition, new Vector3(-300.0f, 0.0f, 0.0f)) < 10.0f)
			{
				currStage = STAGE.FadeOut;
			}
		}

		else if(currStage == STAGE.FadeOut)
		{
			Color tmp = blackScreen.GetComponent<Image>().color;
			tmp.a -= Time.deltaTime * 1.5f;
			blackScreen.GetComponent<Image>().color = tmp;

			if(tmp.a <= 0.0f)
			{
				blackScreen.SetActive(false);
				whiteScreen.SetActive(false);
				currStage = STAGE.EndAnim;
			}
		}
	}

	void UpdateHeaderAnim()
	{
		if(headerStage == HEADER.MoveDown)
		{
			header.GetComponent<Transform>().localPosition = Vector3.Lerp(header.GetComponent<Transform>().localPosition, new Vector3(0.0f, -900.0f, 0.0f), Time.deltaTime * 3.0f);
			if( Vector3.Distance(header.GetComponent<Transform>().localPosition, new Vector3(0.0f, -900.0f, 0.0f)) < 50.0f)
				headerStage = HEADER.BounceUp;
		}
		else if(headerStage == HEADER.BounceUp)
		{
			header.GetComponent<Transform>().localPosition = Vector3.Lerp(header.GetComponent<Transform>().localPosition, new Vector3(0.0f, -680.0f, 0.0f), Time.deltaTime * 2.0f);
			if( Vector3.Distance(header.GetComponent<Transform>().localPosition, new Vector3(0.0f, -680.0f, 0.0f)) < 5.0f)
			{
				header.GetComponent<Transform>().localPosition = new Vector3(0.0f, -680.0f, 0.0f);
				headerStage = HEADER.End;
			}
		}
	}

	public bool GameStartAnimEnded()
	{
		return currStage == STAGE.EndAnim;
	}

	public void FadeOut(int dest = 1)
	{
		nextScreen = dest;
		fadeToMenu = true;
		blackScreen.SetActive(true);
	}
}
