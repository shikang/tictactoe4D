﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour
{
	public GameObject	bigGridObj;
	public GameObject	boardSprite;
	public GameObject	activeGridSprite;
	public GameObject [] bigGrids;
	public GameObject   scrollingText;
	public GameObject   canvas;

	int currHighlighted_BigGrid;
	int currHighlighted_Grid;

	public float 		minScale;
	public float		maxScale;
	public float		scaleLimitX;
	public float		scaleLimitY;
	public float		scaleUnit;
	public bool			isTouched;

	public Vector2		initalScaleDist;
	public Vector2		newScaleDist;
	public Vector2		initalMovePos;
	public Vector2		newMovePos;

	public float 		depth;
	public int 			gameWinner;		// 0 = Draw, 1 = Cross, 2 = Circle
	public int			activeBigGrid;	// 0-8 = respective grids, 10 = all available
    public Defines.GAMEMODE 	gameMode;
    public bool			showWinScreen;
    public int			bigGridsCompleted;

    float jerkTimerP1;
	float jerkTimerP2;

    WINMETHOD winMethod;
    int pos1,pos2,pos3;
    bool begin = false;
    float time= 0.8f;

	void Start ()
	{
		minScale = 0.5f;
		maxScale = 1.15f;
		scaleUnit = 3.3224f / (maxScale - minScale);

		// Instantiate and set positions for all grids.
		bigGrids = new GameObject[9];
		for(int i = 0; i < 9; ++i)
		{
			bigGrids[i] = (GameObject)Instantiate(bigGridObj);
			bigGrids[i].name = "BigGrids " + i;
			bigGrids[i].GetComponent<BigGridScript>().bigGridID = i;
			bigGrids[i].transform.parent = boardSprite.transform;
		}

		ResetVars();
	}

	void ResetVars()
	{
		depth = 22.0f;
		isTouched = false;
		winMethod = WINMETHOD.NIL;

        // Check if connected
        if (NetworkManager.IsConnected())
        {
			gameMode = Defines.GAMEMODE.ONLINE;
			// same as GameObject.FindGameObjectWithTag("GlobalScript").GetComponent<GlobalScript>().gameMode == 2
		}
        else
        {
			if(GlobalScript.Instance.gameMode == 0)
				gameMode = Defines.GAMEMODE.AI;
			else if(GlobalScript.Instance.gameMode == 1)
				gameMode = Defines.GAMEMODE.LOCAL;
        }

		GameData.current.matchPlayed += 1;
		SaveLoad.Save();

		activeBigGrid = 10;
		UpdateActiveGridBG(0, true);

		gameWinner = -1;
		jerkTimerP1 = Random.Range(35.0f, 80.0f);
		jerkTimerP2 = Random.Range(35.0f, 80.0f);

		currHighlighted_BigGrid = 10;
		currHighlighted_Grid = 10;
		showWinScreen = false;
		GlobalScript.Instance.isInputPaused = false;
		bigGridsCompleted = 0;
	}

	void Update ()
	{
		/*if(Input.GetKeyUp(KeyCode.P))
		{
			begin = true;
			pos1 = 2;
			pos2 = 4;
			pos3 = 6;
			//Debug.Log(bigGrids[pos1].GetComponentInChildren<Shaker>().duration + " name: " + bigGrids[pos1].GetComponentInChildren<Shaker>().name);
			//Debug.Log(bigGrids[pos1].name);
		}*/

		if(begin)
		{
			GlobalScript.Instance.isInputPaused = true;

			time -= Time.deltaTime;
			if(time <=0 && time > -1)
				time = 0;
			
			if(time ==0)
			{
				time = -1;
				bigGrids[pos1].GetComponentInChildren<Shaker>().StartShaking(0.5f);
				if(winMethod == WINMETHOD.H_TOP)
					transform.Find("H1").GetComponent<WinLine>().startLine(bigGrids[pos1].GetComponentInChildren<Shaker>().duration*3);
				else if(winMethod == WINMETHOD.H_MID)
					transform.Find("H2").GetComponent<WinLine>().startLine(bigGrids[pos1].GetComponentInChildren<Shaker>().duration*3);
				else if(winMethod == WINMETHOD.H_BOT)
					transform.Find("H3").GetComponent<WinLine>().startLine(bigGrids[pos1].GetComponentInChildren<Shaker>().duration*3);
				else if(winMethod == WINMETHOD.V_LEFT)
					transform.Find("V1").GetComponent<WinLine>().startLine(bigGrids[pos1].GetComponentInChildren<Shaker>().duration*3);
				else if(winMethod == WINMETHOD.V_MID)
					transform.Find("V2").GetComponent<WinLine>().startLine(bigGrids[pos1].GetComponentInChildren<Shaker>().duration*3);
				else if(winMethod == WINMETHOD.V_RIGHT)
					transform.Find("V3").GetComponent<WinLine>().startLine(bigGrids[pos1].GetComponentInChildren<Shaker>().duration*3);
				else if(winMethod == WINMETHOD.BACKSLASH)
					transform.Find("LeftSlash").GetComponent<WinLine>().startLine(bigGrids[pos1].GetComponentInChildren<Shaker>().duration*3);
				else if(winMethod == WINMETHOD.SLASH)
					transform.Find("RightSlash").GetComponent<WinLine>().startLine(bigGrids[pos1].GetComponentInChildren<Shaker>().duration*3);
			}
			//bigGrids[pos1].GetComponentInChildren<Shaker>().StartShaking(0.5f);
			if(bigGrids[pos1].GetComponentInChildren<Shaker>().IsShakeComplete())
				bigGrids[pos2].GetComponentInChildren<Shaker>().StartShaking(0.5f);

			else if(bigGrids[pos2].GetComponentInChildren<Shaker>().IsShakeComplete())
				bigGrids[pos3].GetComponentInChildren<Shaker>().StartShaking(0.5f);

			if(bigGrids[pos3].GetComponentInChildren<Shaker>().IsShakeComplete())
			{
				SetWinner(gameWinner);
				showWinScreen = true;
				begin = false;
				GlobalScript.Instance.isInputPaused = false;
				GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().UpdateAnalyticsGameEnd(true);
			}
		}
		else if(gameMode == Defines.GAMEMODE.AI &&
				GameObject.FindGameObjectWithTag("GUIManager") &&
				GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P2)
		{
			GameObject.FindGameObjectWithTag("AIMiniMax").GetComponent<AIMiniMax>().UpdateAI();
		}

		UpdateScaleLimit();
		UpdateJerkTimer(1, ref jerkTimerP1);
		UpdateJerkTimer(2, ref jerkTimerP2);
	}

	public void UpdateActiveGridBG(int _gridID, bool firstTime = false)
	{
		if(!firstTime)
		{
			// If next grid is already completed, next player gets to put anywhere.
			if(_gridID == 10 || bigGrids[_gridID].GetComponent<BigGridScript>().gridWinner != 0)
				activeBigGrid = 10;
			else
				activeBigGrid = _gridID;
		}

		if(activeBigGrid != 10)
		{
			Vector3 tempScale = new Vector3 (Defines.ACTIVEGRID_SIZE_SMALL, Defines.ACTIVEGRID_SIZE_SMALL, 10.0f);
			activeGridSprite.transform.localScale = tempScale;

			Vector3 tempPos = bigGrids[_gridID].transform.localPosition;
			tempPos.z = -0.1f;
			activeGridSprite.transform.localPosition = tempPos;
		}
		else
		{
			Vector3 tempScale = new Vector3 (Defines.ACTIVEGRID_SIZE_BIG, Defines.ACTIVEGRID_SIZE_BIG, 10.0f);
			activeGridSprite.transform.localScale = tempScale;

			Vector3 tempPos = Defines.ACTIVEGRID_POSITION_BIG;
			tempPos.z = -0.1f;
			activeGridSprite.transform.localPosition = tempPos;
		}
	}

	// Random pulse of animation
	void UpdateJerkTimer(int _player, ref float _animTimer)
	{
		_animTimer -= Time.deltaTime;

		if(_animTimer <= 0.0f)
		{
			_animTimer = Random.Range(18.0f, 35.0f);
			int randType = Random.Range(1, 4);

			for(int i = 0; i < 9; ++i)
			{
				for(int j = 0; j < 9; ++j)
				{
					if(bigGrids[i].GetComponent<BigGridScript>().grids[j].GetComponent<GridScript>().gridState == _player)
					{
						if(randType == 1)
							bigGrids[i].GetComponent<BigGridScript>().grids[j].GetComponent<Animator>().SetTrigger("isIconJerk1");
						else if(randType == 2)
							bigGrids[i].GetComponent<BigGridScript>().grids[j].GetComponent<Animator>().SetTrigger("isIconJerk2");
						else if(randType == 3)
							bigGrids[i].GetComponent<BigGridScript>().grids[j].GetComponent<Animator>().SetTrigger("isIconPlaced");
						else if(randType == 4)
							bigGrids[i].GetComponent<BigGridScript>().grids[j].GetComponent<Animator>().SetTrigger("isHighlighted");
					}
				}
			}
		}
	}

	public void ProcessBoardCompleted()
	{
		// Win!
		if(IsBigGridCompleted())
		{
			if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P1)
				gameWinner = 2;
			else if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P2)
				gameWinner = 1;

			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn = Defines.TURN.GAMEOVER;

			if(GameData.current.hasVibrate && VibrationManager.HasVibrator())
			{
				long [] pattern;
				pattern = new long[]{0, 100, 100, 100, 100, 350};
				VibrationManager.Vibrate(pattern, -1);
			}
			if(AudioManager.Instance)
			{
				AudioManager.Instance.PlaySoundEvent(SOUNDID.STOPBGM);
				AudioManager.Instance.PlaySoundEvent(SOUNDID.WIN_GAME);
			}

			begin = true;
			if(CanGetScore())
			{
				int _score = Defines.bigGridWin;
				if(gameMode == Defines.GAMEMODE.LOCAL)
					_score = Defines.bigGridWin_Local;
				if(gameMode == Defines.GAMEMODE.AI)
					_score = Defines.bigGridWin_AI;

				Defines.Instance.playerScore += _score;
				GameObject tmp;

				tmp = (GameObject)Instantiate(scrollingText);//.gameObject.GetComponent<FloatingText>().BeginScrolling(" + " + _score + "!");
				tmp.transform.SetParent(canvas.transform.transform);
				tmp.transform.localScale = new Vector3(1,1,1);	
				tmp.transform.localPosition =  new Vector3(0,0,0);
				tmp.GetComponent<Text>().text = "+ " + _score + "!";

				if(AudioManager.Instance)
					AudioManager.Instance.PlaySoundEvent(SOUNDID.GETPOINTS);
			}
		}
		else if(IsDraw()) // Draw. All boards filled
		{
			SetWinner(0);
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn = Defines.TURN.GAMEOVER;

			showWinScreen = true;
			begin = false;
			GlobalScript.Instance.isInputPaused = true;
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().UpdateAnalyticsGameEnd(true);

			if(GameData.current.hasVibrate && VibrationManager.HasVibrator())
			{
				long [] pattern;
				pattern = new long[]{0, 200, 200, 200, 200, 200};
				VibrationManager.Vibrate(pattern, -1);
			}

			if(AudioManager.Instance)
			{
				AudioManager.Instance.PlaySoundEvent(SOUNDID.STOPBGM);
				AudioManager.Instance.PlaySoundEvent(SOUNDID.WIN_GAME);
			}
		}
	}

	public void ResetAllHighlights()
	{
		for(int i = 0; i < 9; ++i)
		{
			foreach(Transform currSmall in bigGrids[i].transform)
			{
				if(currSmall.gameObject.tag == "Grid")
				{
					currSmall.gameObject.GetComponent<GridScript>().ResetHighlight();
				}
			}
		}
	}

	public void SetCurrentHighlight(int _bigGrid, int _grid)
	{
		ResetCurrentHighlight();
		currHighlighted_BigGrid = _bigGrid;
		currHighlighted_Grid = _grid;
	}

	public void ResetCurrentHighlight()
	{
		if(currHighlighted_BigGrid == 10 || currHighlighted_Grid == 10)
			return;

		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().gridEffect_growStage = 13;
		bigGrids[currHighlighted_BigGrid].GetComponent<BigGridScript>().grids[currHighlighted_Grid].GetComponent<GridScript>().ResetHighlight();
		currHighlighted_BigGrid = 10;
		currHighlighted_Grid = 10;
	}

	public void SetWinner(int _winner)
	{
		gameWinner = _winner;
		//GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<ScoreBoardScript>().scores[gameWinner]++;
		
		Color temp = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmNewGame.GetComponent<Image>().color;
		temp.a = 1.0f;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmNewGame.GetComponent<Image>().color = temp;

		temp = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmEndGame.GetComponent<Image>().color;
		temp.a = 1.0f;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICfmEndGame.GetComponent<Image>().color = temp;

		// Update online statistic
		if(NetworkManager.IsConnected())
		{
			if ((NetworkManager.IsPlayerOne() && _winner == 1) ||
			    (NetworkManager.IsPlayerOne() && _winner == 2))
			{
				SaveLoad.Load();
				GameData.current.win += 1;
				SaveLoad.Save();
			}
		}
	}

	void UpdateScaleLimit()
	{
		//scaleLimitX = scaleUnit * (boardSprite.transform.localScale.x - minScale);
		//scaleLimitY = scaleUnit * (boardSprite.transform.localScale.y - minScale);

		//Vector3 tempPos = boardSprite.transform.localPosition;
		//Vector3 tempScale = boardSprite.transform.localScale;

		// Moving
		/*if(Input.touchCount == 1)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				initalMovePos = Input.GetTouch(0).position;
			}

			else if(Input.GetTouch(0).phase == TouchPhase.Stationary)
			{
				initalMovePos = newMovePos;
			}

			else if(Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				newMovePos = Input.GetTouch(0).position;
				Vector2 finalPos = newMovePos - initalMovePos;

				if(finalPos.y < 0.0f)
					tempPos.y -= 0.4f;
				if(finalPos.y > 0.0f)
					tempPos.y += 0.4f;
				if(finalPos.x > 0.0f)
					tempPos.x += 0.4f;
				if(finalPos.x < 0.0f)
					tempPos.x -= 0.4f;
			}
		}*/

		// Scaling
		/*if(Input.touchCount == 2)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				initalScaleDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
			}

			else if(Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
			{
				newScaleDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
			}

			else if(Input.GetTouch(0).phase == TouchPhase.Stationary && Input.GetTouch(1).phase == TouchPhase.Stationary)
			{
				initalScaleDist = newScaleDist;
			}

			// Zoom Out
			if(initalScaleDist.magnitude < newScaleDist.magnitude)
			{
				if(tempScale.x < 1.15f)
				{
					tempScale.x += 0.05f;
					tempScale.y += 0.05f;
				}
				else
				{
					tempScale.x = tempScale.y = 1.15f;
				}
			}

			// Zoom In
			if(initalScaleDist.magnitude > newScaleDist.magnitude)
			{
				if(tempScale.x > 0.5f)
				{
					tempScale.x -= 0.05f;
					tempScale.y -= 0.05f;
				}
				else
				{
					tempScale.x = tempScale.y = 0.5f;
				}
			}
		}

		if(tempPos.x < -scaleLimitX)
			tempPos.x = -scaleLimitX;
		if(tempPos.x > scaleLimitX)
			tempPos.x = scaleLimitX;
		
		if(tempPos.y < -scaleLimitY)
			tempPos.y = -scaleLimitY;
		if(tempPos.y > scaleLimitY)
			tempPos.y = scaleLimitY;

		boardSprite.transform.localPosition = tempPos;
		boardSprite.transform.localScale = tempScale; */
	}

	public bool IsBigGridCompleted()
	{
		int turn = 1;
		if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P1)
			turn = 2;

		if(TutorialScript.Instance.isTutorial)
			turn = 1;

		if(bigGrids[0].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[1].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[2].GetComponent<BigGridScript>().gridWinner == turn)
		{
			winMethod = WINMETHOD.H_TOP;
			pos1 = 0;
			pos2 = 1;
			pos3 = 2;
		}
	    else if(bigGrids[3].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[4].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[5].GetComponent<BigGridScript>().gridWinner == turn)
	    {
	    	winMethod = WINMETHOD.H_MID;
	    	pos1 = 3;
	    	pos2 = 4;
	    	pos3 = 5;
	    }
	    else if(bigGrids[6].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[7].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[8].GetComponent<BigGridScript>().gridWinner == turn)
	    {
	    	winMethod = WINMETHOD.H_BOT;
	    	pos1 = 6;
	    	pos2 = 7;
	    	pos3 = 8;
	    }
	    else if(bigGrids[0].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[3].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[6].GetComponent<BigGridScript>().gridWinner == turn) 
	    {
	    	winMethod = WINMETHOD.V_LEFT;
	    	pos1 = 0;
	    	pos2 = 3;
	    	pos3 = 6;
	    }
	    else if(bigGrids[1].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[4].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[7].GetComponent<BigGridScript>().gridWinner == turn)
	    {
	    	winMethod = WINMETHOD.V_MID;
	    	pos1 = 1;
	    	pos2 = 4;
	    	pos3 = 7;
	    }
	    else if(bigGrids[2].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[5].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[8].GetComponent<BigGridScript>().gridWinner == turn)
	    {
	    	winMethod = WINMETHOD.V_RIGHT;
	    	pos1 = 2;
	    	pos2 = 5;
	    	pos3 = 8;
	    }
	    else if(bigGrids[0].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[4].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[8].GetComponent<BigGridScript>().gridWinner == turn)
	    {
	    	winMethod = WINMETHOD.BACKSLASH;
	    	pos1 = 0;
	    	pos2 = 4;
	    	pos3 = 8;
	    }
	    else if(bigGrids[2].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[4].GetComponent<BigGridScript>().gridWinner == turn && bigGrids[6].GetComponent<BigGridScript>().gridWinner == turn)
	    {
	    	winMethod = WINMETHOD.SLASH;
	    	pos1 = 2;
	    	pos2 = 4;
	    	pos3 = 6;
	    }

	    if (winMethod == WINMETHOD.NIL)
	    	return false;

	    return true;
	}

	public bool IsDraw()
	{
		for(int i = 0; i < 9; ++i)
		{
			if(bigGrids[i].GetComponent<BigGridScript>().gridWinner == 0)
				return false;
		}
		return true;
	}

	bool CanGetScore()
	{
		// Tutorial
		if(TutorialScript.Instance.isTutorial)
			return false;

		// If not your turn during online play
		if(gameMode == Defines.GAMEMODE.ONLINE)
		{
			if((NetworkManager.IsPlayerOne() && GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != Defines.TURN.P1)  ||
			   (!NetworkManager.IsPlayerOne() && GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != Defines.TURN.P2) )
        		return true;
        }

        // If you win AI
		if( gameMode == Defines.GAMEMODE.AI)
		{
			if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P2 )
				return true;
		}

		if(gameMode == Defines.GAMEMODE.LOCAL)
			return true;

		return false;
	}
}
