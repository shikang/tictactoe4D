using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManagerScript : MonoBehaviour
{
	public GameObject ScoreBoardObj;
	public GameObject GUITurn;
	public GameObject GUIAIText;
	public GameObject GUICenterText;

	public GameObject GUICfmText;
	public GameObject GUICfmYes;
	public GameObject GUICfmNo;
	public GameObject GUICfmNewGame;
	public GameObject GUICfmEndGame;
	public GameObject GUICfmFrame;
	public GameObject GUICfmMainMenu;

	// Emote
	public GameObject GUIEmoteScreen;
	public GameObject GUIP1EmoteSpeech;
	public GameObject GUIP2EmoteSpeech;
	private float p1EmoteTimer = 0.0f;
	private float p2EmoteTimer = 0.0f;

	public GameObject GUIScoreDraw;
	public GameObject GUIScoreP1;
	public GameObject GUIScoreP2;

	public GameObject GUITimerP1;
	public GameObject GUITimerP2;
	public GameObject GUINameP1;
	public GameObject GUINameP2;

	public GameObject FrameLeft;
	public GameObject FrameRight;
	public GameObject ImageLeft;
	public GameObject ImageRight;
  
	public float currMin;
	public float currMax;
	public float frameScaleSpeed;

	public Sprite EmptySprite;
	public Sprite HighlightSprite;

	public float textScaleSpeed;
	public string nameP1 = "abc";
	public string nameP2 = "xyz";

	public float timerP1;
	public float timerP2;
	public float startTime;

	public GameObject	gridEffect;
	float				gridEffect_minSize;
	float				gridEffect_maxSize;
	float				gridEffect_scaleSpeed;
	public int			gridEffect_growStage;

	float AITimer;

	bool emoteP1UpAnimation = false;
	bool emoteP1DownAnimation = false;
	bool emoteP2UpAnimation = false;
	bool emoteP2DownAnimation = false;
	float emoteP1AnimTimer = 0.0f;
	float emoteP2AnimTimer = 0.0f;

	void Start ()
	{
		textScaleSpeed = 2.0f;
		Application.targetFrameRate = 60;

		//if(GameObject.FindGameObjectWithTag("ScoreBoard") == null)
		//	Instantiate(ScoreBoardObj);

		AITimer = 0.0f;
		currMin = 1.0f;
		currMax = 1.3f;
		frameScaleSpeed = 1.0f;

		gridEffect_minSize = 0.95f;
		gridEffect_maxSize = 1.2f;
		gridEffect_scaleSpeed = 3.0f;

		ResetVars();
	}

	void ResetVars()
	{
		gridEffect_growStage = 0;
		startTime = 300.0f;
		timerP1 = timerP2 = startTime;

		nameP1 = GlobalScript.Instance.nameP1;
		nameP2 = GlobalScript.Instance.nameP2;

		GUINameP1.GetComponent<Text>().text = nameP1;
		GUINameP2.GetComponent<Text>().text = nameP2;

		GUICfmNewGame.SetActive(false);
		GUICfmEndGame.SetActive(false);
		GUICfmMainMenu.SetActive(false);
		SetCfmAlpha(false);

		GUICenterText.transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);

		Color tmp = GUICfmFrame.GetComponent<Image>().color;
		tmp.a = 0.8f;
		GUICfmFrame.GetComponent<Image>().color = tmp;
		GUICfmFrame.SetActive(false);

		GUIEmoteScreen.SetActive(false);
		//GUIP1EmoteSpeech.SetActive(false);
		//GUIP2EmoteSpeech.SetActive(false);
		GUIP1EmoteSpeech.transform.localScale = Vector3.zero;
		GUIP2EmoteSpeech.transform.localScale = Vector3.zero;
		p1EmoteTimer = Defines.EMOTE_SHOW_TIME;
		p2EmoteTimer = Defines.EMOTE_SHOW_TIME;

		emoteP1UpAnimation = false;
		emoteP1DownAnimation = false;
		emoteP2UpAnimation = false;
		emoteP2DownAnimation = false;
		emoteP1AnimTimer = 0.0f;
		emoteP2AnimTimer = 0.0f;

		if (GetComponent<TurnHandler>() != null)
		{
			GetComponent<TurnHandler>().UpdatePlayerIcons();
			SetAvatar();
		}
	}

	void Update()
	{
		ScaleText(GUITurn.transform, 0.7f);
		ScaleText(GUICenterText.transform, 1.2f);

		UpdateTimer();
		GridEffectAnim();

		//UpdateTurnGUI();
		UpdateAIGUI();

		// Center Text
		//   Game Start
		if (GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.NOTSTARTED)
		{
			GUITurn.GetComponent<Text>().text = "";
			GUICenterText.GetComponent<Text>().text = "GAME START!";
		}

		//   Game End
		else if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.GAMEOVER)
		{
			if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameWinner == 0)
				GUICenterText.GetComponent<Text>().text = "Draw!";
			else if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameWinner == 1)
				GUICenterText.GetComponent<Text>().text = "Player 1 Wins!";
			else if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameWinner == 2)
				GUICenterText.GetComponent<Text>().text = "Player 2 Wins!";

			GUITurn.GetComponent<Text>().text = "";
			GUICfmFrame.SetActive(true);
			GUICfmNewGame.SetActive(true);
			GUICfmEndGame.SetActive(true);

			GUICenterText.SetActive(true);
			Color temp = GUICenterText.GetComponent<Text>().color;
			temp.a = 1.0f;
			GUICenterText.GetComponent<Text>().color = temp;

			GUIEmoteScreen.SetActive(false);

			// Add money
			Debug.Log("Recieve coin: " + Defines.Instance.playerScore);
			GameData.current.coin += Defines.Instance.playerScore;
			SaveLoad.Save();
		}
		else if (GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.WAITING)
		{
			GUICenterText.GetComponent<Text>().text = "Waiting for other player...";
			GUICfmNewGame.SetActive(false);
			GUICfmEndGame.SetActive(false);

			GUIEmoteScreen.SetActive(false);
		}
		else if (GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.DISCONNECTED)
		{
			GUICfmText.GetComponent<Text>().text = "Other player has disconnected! T.T";

			GUITurn.GetComponent<Text>().text = "";
			GUICfmFrame.SetActive(true);
			GUICfmMainMenu.SetActive(true);

			GUICfmText.SetActive(true);
			Color temp = GUICenterText.GetComponent<Text>().color;
			temp.a = 1.0f;
			GUICfmText.GetComponent<Text>().color = temp;

			GUICfmNewGame.SetActive(false);
			GUICfmEndGame.SetActive(false);
			GUICfmYes.SetActive(false);
			GUICfmNo.SetActive(false);
			GUICenterText.SetActive(false);

			GUIEmoteScreen.SetActive(false);
		}
		else
		{
			GUICenterText.GetComponent<Text>().text = "";
			UpdateEmote();
		}
	}

	/*void UpdateTurnGUI()
	{
		// Whose Turn
		if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P1)
		{
			timerP1 -= Time.deltaTime;
			if(timerP1 <= 0.0f)
				GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().SetWinner((int)Defines.TURN.P2);

			GUINameP1.GetComponent<Text>().color = Color.green;
			GUITimerP1.GetComponent<Text>().color = Color.green;
			GUINameP2.GetComponent<Text>().color = Color.grey;
			GUITimerP2.GetComponent<Text>().color = Color.grey;
			GUITurn.GetComponent<Text>().text = nameP1 + "'s Turn";

			if(FrameLeft.transform.localScale.x < currMax)
			{
				FrameLeft.transform.localScale = new Vector3(FrameLeft.transform.localScale.x + (frameScaleSpeed * Time.deltaTime),
										FrameLeft.transform.localScale.y + (frameScaleSpeed * Time.deltaTime),
										FrameLeft.transform.localScale.z);
										
				ImageLeft.transform.localScale = new Vector3(ImageLeft.transform.localScale.x + (frameScaleSpeed * Time.deltaTime),
										ImageLeft.transform.localScale.y + (frameScaleSpeed * Time.deltaTime),
										ImageLeft.transform.localScale.z);
			}

			if(FrameRight.transform.localScale.y > currMin)
			{
				FrameRight.transform.localScale = new Vector3(FrameRight.transform.localScale.x + (frameScaleSpeed * Time.deltaTime),
										FrameRight.transform.localScale.y - (frameScaleSpeed * Time.deltaTime),
										FrameRight.transform.localScale.z);
										
				ImageRight.transform.localScale = new Vector3(ImageRight.transform.localScale.x + (frameScaleSpeed * Time.deltaTime),
										ImageRight.transform.localScale.y - (frameScaleSpeed * Time.deltaTime),
										ImageRight.transform.localScale.z);
			}
		}
		else if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P2)
		{
			timerP2 -= Time.deltaTime;
			if(timerP2 <= 0.0f)
				GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().SetWinner((int)Defines.TURN.P1);

			GUINameP1.GetComponent<Text>().color = Color.grey;
			GUITimerP1.GetComponent<Text>().color = Color.grey;
			GUINameP2.GetComponent<Text>().color = Color.green;
			GUITimerP2.GetComponent<Text>().color = Color.green;
			GUITurn.GetComponent<Text>().text = nameP2 + "'s Turn";
	
			if(FrameLeft.transform.localScale.y > currMin)
			{
				FrameLeft.transform.localScale = new Vector3(FrameLeft.transform.localScale.x - (frameScaleSpeed * Time.deltaTime),
										FrameLeft.transform.localScale.y - (frameScaleSpeed * Time.deltaTime),
										FrameLeft.transform.localScale.z);
										
				ImageLeft.transform.localScale = new Vector3(ImageLeft.transform.localScale.x - (frameScaleSpeed * Time.deltaTime),
										ImageLeft.transform.localScale.y - (frameScaleSpeed * Time.deltaTime),
										ImageLeft.transform.localScale.z);
			}

			if(FrameRight.transform.localScale.y < currMax)
			{
				FrameRight.transform.localScale = new Vector3(FrameRight.transform.localScale.x - (frameScaleSpeed * Time.deltaTime),
										FrameRight.transform.localScale.y + (frameScaleSpeed * Time.deltaTime),
										FrameRight.transform.localScale.z);
										
				ImageRight.transform.localScale = new Vector3(ImageRight.transform.localScale.x - (frameScaleSpeed * Time.deltaTime),
										ImageRight.transform.localScale.y + (frameScaleSpeed * Time.deltaTime),
										ImageRight.transform.localScale.z);
			}
		}
		else
		{
			GUITurn.GetComponent<Text>().text = "";
		}
	}*/

	void UpdateAIGUI()
	{
		if (GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.AI &&
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == GameObject.FindGameObjectWithTag("AIMiniMax").GetComponent<AIMiniMax>().AITurn )
		{
			AITimer += Time.deltaTime;
			if(AITimer < 0.5f)
				GUIAIText.GetComponent<Text>().text = "Placing";
			else if(AITimer < 1.0f)
				GUIAIText.GetComponent<Text>().text = "Placing.";
			else if(AITimer < 1.5f)
				GUIAIText.GetComponent<Text>().text = "Placing..";
			else if(AITimer < 2.0f)
				GUIAIText.GetComponent<Text>().text = "Placing...";
			else if(AITimer < 2.5f)
				AITimer = 0.0f;
		}
		else
		{
			GUIAIText.GetComponent<Text>().text = "";
			AITimer = 0.0f;
		}
	}

	public void UpdateClick()
	{
		float scaleSize = 1.0f;
		GUITurn.transform.localScale = new Vector3(scaleSize, scaleSize, 1.0f);
		//GUICurrentGrid.transform.localScale = new Vector3(scaleSize, scaleSize, 1.0f);
	}

	public void UpdateWin()
	{
		ScaleText(GUICenterText.transform, 1.2f);
	}

	public void SetCfmAlpha(bool _Setter)
	{
		GUICfmText.SetActive(_Setter);
		GUICfmYes.SetActive(_Setter);
		GUICfmNo.SetActive(_Setter);
		GUICfmFrame.SetActive(_Setter);
	}

	void ScaleText(Transform _label, float _normalSize)
	{
		Vector3 tempScale = _label.localScale;
		if(tempScale.x > _normalSize)
		{
			tempScale.x -= textScaleSpeed * Time.deltaTime;
			tempScale.y -= textScaleSpeed * Time.deltaTime;
		}
		else
		{
			tempScale.x = _normalSize;
			tempScale.y = _normalSize;
		}
		_label.localScale = tempScale;
	}

	public void SetTimer(Defines.TURN turn, float time)
    {
        if (turn == Defines.TURN.P1)
        {
            timerP1 = time;
        }
        else if (turn == Defines.TURN.P2)
        {
            timerP2 = time;
        }
    }

	void UpdateTimer()
	{
		int min = 0;
		int currTime = (int)timerP1;

		while(currTime >= 60)
		{
			++min;
			currTime -= 60;
		}
		GUITimerP1.GetComponent<Text>().text = min + ":" + currTime.ToString("00");

		min = 0;
		currTime = (int)timerP2;

		while(currTime >= 60)
		{
			++min;
			currTime -= 60;
		}
		GUITimerP2.GetComponent<Text>().text = min + ":" + currTime.ToString("00");
	}

	public void GridEffectAnim()
	{
		if(gridEffect_growStage == 0)
		{
			// Do Nothing
		}
		else if(gridEffect_growStage == 1)
		{
			// Setup
			if(GetComponent<TurnHandler>().turn == Defines.TURN.P1)
			{
				gridEffect.GetComponent<SpriteRenderer>().sprite =
					GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().GetSpriteP2();
				gridEffect.GetComponent<SpriteRenderer>().color = 
					GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().ColorP2;
			}
			else if(GetComponent<TurnHandler>().turn == Defines.TURN.P2)
			{
				gridEffect.GetComponent<SpriteRenderer>().sprite =
					GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().GetSpriteP1();
				gridEffect.GetComponent<SpriteRenderer>().color = 
					GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().ColorP1;
			}

			// Scale
			gridEffect.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

			//Opacity
			Color tmp = gridEffect.GetComponent<Renderer>().material.color;
			tmp.a = 1.0f;
			gridEffect.GetComponent<Renderer>().material.color = tmp;

			// Next stage
			gridEffect_growStage = 2;
		}
		else if(gridEffect_growStage == 2)
		{
			// Go smaller by abit
			gridEffect.transform.localScale = new Vector3(
				gridEffect.transform.localScale.x - gridEffect_scaleSpeed * Time.deltaTime,
				gridEffect.transform.localScale.y - gridEffect_scaleSpeed * Time.deltaTime,
				gridEffect.transform.localScale.z );

			if(gridEffect.transform.localScale.x <= gridEffect_minSize)
				gridEffect_growStage = 3;
		}
		else if(gridEffect_growStage == 3)
		{
			// Enlarge
			gridEffect.transform.localScale = new Vector3(
				gridEffect.transform.localScale.x + gridEffect_scaleSpeed * Time.deltaTime,
				gridEffect.transform.localScale.y + gridEffect_scaleSpeed * Time.deltaTime,
				gridEffect.transform.localScale.z );

			if(gridEffect.transform.localScale.x >= gridEffect_maxSize)
				gridEffect_growStage = 4;
		}
		else if(gridEffect_growStage == 4)
		{
			// Scale down and disappear
			gridEffect.transform.localScale = new Vector3(
				gridEffect.transform.localScale.x - gridEffect_scaleSpeed * Time.deltaTime,
				gridEffect.transform.localScale.y - gridEffect_scaleSpeed * Time.deltaTime,
				gridEffect.transform.localScale.z );

			Color tmp = gridEffect.GetComponent<Renderer>().material.color;
			tmp.a -= 0.1f;
			gridEffect.GetComponent<Renderer>().material.color = tmp;

			if(gridEffect.GetComponent<Renderer>().material.color.a <= 0.1f)
			{
				gridEffect_growStage = 0;
				tmp = gridEffect.GetComponent<Renderer>().material.color;
				tmp.a = 0.0f;
				gridEffect.GetComponent<Renderer>().material.color = tmp;
			}
		}
	}

	public void SetAvatar()
	{
		ImageLeft.GetComponent<Image>().sprite =
			GetComponent<TurnHandler>().GetSpriteP1();

		ImageRight.GetComponent<Image>().sprite =
			GetComponent<TurnHandler>().GetSpriteP2();
	}

	public void ToogleEmoteMenu()
	{
		GUIEmoteScreen.SetActive( !GUIEmoteScreen.GetActive() );
	}

	public void HideEmoteMenu()
	{
		GUIEmoteScreen.SetActive( false );
	}

	public void ShowP1EmoteSpeech( string emote )
	{
		//GUIP1EmoteSpeech.SetActive( true );

		emoteP1UpAnimation = true;
		emoteP1DownAnimation = false;
		emoteP1AnimTimer = 0.0f;
		GUIP1EmoteSpeech.transform.localScale = Vector3.zero;

		GUIP1EmoteSpeech.GetComponentInChildren<Text>().text = emote;
		p1EmoteTimer = 0.0f;
	}

	public void ShowP2EmoteSpeech( string emote )
	{
		//GUIP2EmoteSpeech.SetActive(true);

		emoteP2UpAnimation = true;
		emoteP2DownAnimation = false;
		emoteP2AnimTimer = 0.0f;
		GUIP2EmoteSpeech.transform.localScale = Vector3.zero;

		GUIP2EmoteSpeech.GetComponentInChildren<Text>().text = emote;
		p2EmoteTimer = 0.0f;
	}

	public void HideP1EmoteSpeech()
	{
		//GUIP1EmoteSpeech.SetActive( false );

		emoteP1UpAnimation = false;
		emoteP1DownAnimation = true;
		emoteP1AnimTimer = 0.0f;
		GUIP1EmoteSpeech.transform.localScale = Vector3.one;
	}

	public void HideP2EmoteSpeech()
	{
		//GUIP2EmoteSpeech.SetActive( false );

		emoteP2UpAnimation = false;
		emoteP2DownAnimation = true;
		emoteP2AnimTimer = 0.0f;
		GUIP2EmoteSpeech.transform.localScale = Vector3.one;
	}

	float GetFactor( float f )
	{
		return f * f;
	}

	void UpdateEmote()
	{
		if ( p1EmoteTimer < Defines.EMOTE_SHOW_TIME )
		{
			p1EmoteTimer += Time.deltaTime;

			if ( p1EmoteTimer >= Defines.EMOTE_SHOW_TIME )
			{
				HideP1EmoteSpeech();
			}
		}

		if ( p2EmoteTimer < Defines.EMOTE_SHOW_TIME )
		{
			p2EmoteTimer += Time.deltaTime;

			if ( p2EmoteTimer >= Defines.EMOTE_SHOW_TIME )
			{
				HideP2EmoteSpeech();
			}
		}

		if ( emoteP1UpAnimation || emoteP1DownAnimation )
		{
			emoteP1AnimTimer += Time.deltaTime;

			if ( emoteP1UpAnimation )
			{
				GUIP1EmoteSpeech.transform.localScale = GetFactor( Mathf.Clamp( emoteP1AnimTimer / Defines.EMOTE_SCALE_TIME, 0.0f, 1.0f ) ) * Vector3.one;
			}
			else if ( emoteP1DownAnimation )
			{
				GUIP1EmoteSpeech.transform.localScale = GetFactor( Mathf.Clamp( 1.0f - ( emoteP1AnimTimer / Defines.EMOTE_SCALE_TIME ), 0.0f, 1.0f ) ) * Vector3.one;
			}
		}
		else if ( emoteP2UpAnimation || emoteP2DownAnimation )
		{
			emoteP2AnimTimer += Time.deltaTime;

			if ( emoteP2UpAnimation )
			{
				GUIP2EmoteSpeech.transform.localScale = GetFactor( Mathf.Clamp( emoteP2AnimTimer / Defines.EMOTE_SCALE_TIME, 0.0f, 1.0f ) ) * Vector3.one;
			}
			else if ( emoteP1DownAnimation )
			{
				GUIP2EmoteSpeech.transform.localScale = GetFactor( Mathf.Clamp( 1.0f - ( emoteP2AnimTimer / Defines.EMOTE_SCALE_TIME ), 0.0f, 1.0f ) ) * Vector3.one;
			}
		}
	}
}
