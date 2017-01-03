using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardScript : MonoBehaviour
{
	public GameObject	bigGridObj;
	public GameObject	boardSprite;
	public GameObject	activeGridSprite;
	public GameObject [] bigGrids;

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

    WINMETHOD winMethod = WINMETHOD.NIL;
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

        // Check if connected
        if (NetworkManager.IsConnected())
        {
			gameMode = Defines.GAMEMODE.ONLINE;
			// same as GameObject.FindGameObjectWithTag("GlobalScript").GetComponent<GlobalScript>().gameMode == 2

			SaveLoad.Load();
			GameData.current.matchPlayed += 1;
			SaveLoad.Save();
		}
        else
        {
			if(GlobalScript.Instance.gameMode == 0)
				gameMode = Defines.GAMEMODE.AI;
			else if(GlobalScript.Instance.gameMode == 1)
				gameMode = Defines.GAMEMODE.LOCAL;
        }

		activeBigGrid = 10;
		UpdateActiveGridBG(0, true);

		gameWinner = -1;
	}

	void Update ()
	{
		if(Input.GetKeyUp(KeyCode.P))
		{
			begin = true;
			pos1 = 2;
			pos2 = 4;
			pos3 = 6;
						//Debug.Log(bigGrids[pos1].GetComponentInChildren<Shaker>().duration + " name: " + bigGrids[pos1].GetComponentInChildren<Shaker>().name);
						//Debug.Log(bigGrids[pos1].name);
			
		}
		if(begin)
		{
			time-=Time.deltaTime;
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

			else if(bigGrids[pos3].GetComponentInChildren<Shaker>().IsShakeComplete())
			{
				SetWinner(gameWinner);
				begin = false;
			}

		}
		UpdateScaleLimit();
		if(Input.GetKeyDown("t"))
			SetWinner(0);

		// AI's turn if applicable
		if (gameMode == Defines.GAMEMODE.AI)
		{
			GameObject.FindGameObjectWithTag("AIMiniMax").GetComponent<AIMiniMax>().UpdateAI();
		}
	}

	public void UpdateActiveGridBG(int _gridID, bool firstTime = false)
	{
		if(!firstTime)
		{
			// If next grid is already completed, next player gets to put anywhere.
			if(bigGrids[_gridID].GetComponent<BigGridScript>().gridWinner != 0)
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

	public void ProcessBoardCompleted()
	{
		// Win!
		if(IsBigGridCompleted())
		{
			gameWinner = (int)GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn;
			begin = true;
			if (GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P1)
			{	
				Defines.Instance.playerScore += Defines.bigGridWin;
			}
			//SetWinner((int)GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn);
		}
		else
		{
			// Draw. All boards filled
			if(IsDraw())
				SetWinner(0);
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

	public void SetWinner(int _winner)
	{
		gameWinner = _winner;
		GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn = Defines.TURN.GAMEOVER;
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
		scaleLimitX = scaleUnit * (boardSprite.transform.localScale.x - minScale);
		scaleLimitY = scaleUnit * (boardSprite.transform.localScale.y - minScale);

		Vector3 tempPos = boardSprite.transform.localPosition;
		Vector3 tempScale = boardSprite.transform.localScale;

		// Moving
		if(Input.touchCount == 1)
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
		}

		// Scaling
		if(Input.touchCount == 2)
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
		boardSprite.transform.localScale = tempScale;
	}

	bool IsBigGridCompleted()
	{
		int turn = (int)GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn;


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

	    if ( winMethod == WINMETHOD.NIL)
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
}
