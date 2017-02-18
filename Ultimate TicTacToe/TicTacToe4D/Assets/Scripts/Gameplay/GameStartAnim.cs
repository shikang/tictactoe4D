using UnityEngine;
using UnityEngine.UI;
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

public class GameStartAnim : MonoBehaviour
{
	STAGE currStage;
	public GameObject blackScreen;
	public GameObject whiteScreen;
	float scaleTimer;
	bool fadeOut;

	public GameObject playerGroup1;
	public GameObject playerGroup2;

	public GameObject playerIcon1;
	public GameObject playerIcon2;
	public GameObject playerName1;
	public GameObject playerName2;

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
		scaleTimer = 2.5f;
		fadeOut = false;
	}

	void Update ()
	{
		if(currStage == STAGE.BlackBG)
		{
			Color tmp = blackScreen.GetComponent<Image>().color;
			tmp.a += Time.deltaTime * 1.0f;
			blackScreen.GetComponent<Image>().color = tmp;

			if(tmp.a >= 0.5f)
				currStage = STAGE.PlayerLerpIn;
		}

		else if(currStage == STAGE.PlayerLerpIn)
		{
			playerGroup1.transform.localPosition = Vector3.Lerp(playerGroup1.transform.localPosition, new Vector3(-100.0f, 0.0f, 0.0f), Time.deltaTime * 3.0f);
			playerGroup2.transform.localPosition = Vector3.Lerp(playerGroup2.transform.localPosition, new Vector3(100.0f, 0.0f, 0.0f), Time.deltaTime * 3.0f);

			if(Vector3.Distance(playerGroup1.transform.localPosition, new Vector3(-100.0f, 0.0f, 0.0f)) < 10.0f)
			{
				currStage = STAGE.ScreenBlink;
			}
		}

		else if(currStage == STAGE.ScreenBlink)
		{
			Color tmp;

			if(!fadeOut)
			{
				tmp = whiteScreen.GetComponent<Image>().color;
				tmp.a += Time.deltaTime * 5.0f;
				whiteScreen.GetComponent<Image>().color = tmp;

				if(tmp.a >= 0.8f)
					fadeOut = true;
			}
			else
			{
				tmp = whiteScreen.GetComponent<Image>().color;
				tmp.a -= Time.deltaTime * 1.3f;
				whiteScreen.GetComponent<Image>().color = tmp;

				if(tmp.a <= 0.2f)
				{
					currStage = STAGE.IconScaling;
					whiteScreen.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
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
			playerGroup1.transform.localPosition = Vector3.Lerp(playerGroup1.transform.localPosition, new Vector3(-240.0f, 0.0f, 0.0f), Time.deltaTime * 8.0f);
			playerGroup2.transform.localPosition = Vector3.Lerp(playerGroup2.transform.localPosition, new Vector3(240.0f, 0.0f, 0.0f), Time.deltaTime * 8.0f);

			if(Vector3.Distance(playerGroup1.transform.localPosition, new Vector3(-240.0f, 0.0f, 0.0f)) < 10.0f)
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
				currStage = STAGE.EndAnim;
		}
	}

	public bool GameStartAnimEnded()
	{
		return currStage == STAGE.EndAnim;
	}
}
