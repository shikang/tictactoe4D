
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum WINMETHOD
{
	NIL = 0,
	H_TOP,
	H_MID,
	H_BOT,
	V_LEFT,
	V_MID,
	V_RIGHT,
	SLASH,
	BACKSLASH
}

public class BigGridScript : MonoBehaviour
{
	public GameObject gridObj;
	public GameObject sWinIcon;
	public GameObject sWinFrame;
	public GameObject [] grids;
	public GameObject scrollingText;
	public GameObject canvas;

	public int bigGridID;
	public float depth;
	public int gridWinner;	// 0 = Not Done, 1 = Cross, 2 = Circle, 3 = Draw
	public WINMETHOD winMethod;	
	public bool begin = false;

	int pos1 =-1;
	int pos2 =-1;
	int pos3 =-1;

	void Start ()
	{
		grids = new GameObject[9]; 
		depth = -2.0f;

		transform.localPosition = new Vector3( (bigGridID%3) * Defines.BIGGRID_GAP_X - Defines.BIGGRID_GAP_X, 
												-(bigGridID/3) * Defines.BIGGRID_GAP_Y + Defines.BIGGRID_GAP_Y, 0);

		// Instantiate and set positions for all grids.
		float offsetX = 0.0f, offsetY = 0.0f;
		for(int i = 0; i < 9; ++i)
		{
			offsetX = ((i%3)-1) * Defines.GRID_SPACE;
			offsetY = ((i/3)-1) * -Defines.GRID_SPACE;
				
			grids[i] = (GameObject)Instantiate(gridObj);
			grids[i].transform.parent = transform;
			grids[i].transform.localScale = new Vector3(Defines.GRID_SIZE, Defines.GRID_SIZE, Defines.GRID_SIZE);
			grids[i].GetComponent<GridScript>().gridID = i;
			grids[i].GetComponent<GridScript>().parentGrid = gameObject;
			grids[i].GetComponent<GridScript>().PlaceOnGrid(0);
			grids[i].transform.position = transform.position + new Vector3(offsetX, offsetY, depth);
		}

		// Dummy placed icons for testing
		/*if(bigGridID == 0)
		{
			grids[0].GetComponent<GridScript>().PlaceOnGrid(2);
			grids[1].GetComponent<GridScript>().PlaceOnGrid(1);
			grids[2].GetComponent<GridScript>().PlaceOnGrid(2);
			grids[3].GetComponent<GridScript>().PlaceOnGrid(1);
			grids[4].GetComponent<GridScript>().PlaceOnGrid(2);
			grids[5].GetComponent<GridScript>().PlaceOnGrid(1);
			grids[6].GetComponent<GridScript>().PlaceOnGrid(1);
			grids[7].GetComponent<GridScript>().PlaceOnGrid(2);
		}*/
	}

	void ResetVars()
	{
		gridWinner = 0;
		sWinIcon.transform.localScale = new Vector3(1.8f, 1.8f, 1.0f);
		winMethod = WINMETHOD.NIL;
	}

	void Update ()
	{
		if(begin)
		{
			if(grids[pos1].GetComponent<Shaker>().IsShakeComplete())
			{
				grids[pos2].GetComponent<Shaker>().StartShaking();
			}
			else if(grids[pos2].GetComponent<Shaker>().IsShakeComplete())
			{
				grids[pos3].GetComponent<Shaker>().StartShaking();
			}
			else if(grids[pos3].GetComponent<Shaker>().IsShakeComplete())
			{
				if(gridWinner == (int)Defines.TURN.P1)
				{
					sWinIcon.GetComponent<SpriteRenderer>().sprite =
						GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().GetSpriteP1();
					//sWinIcon.GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_P1;

					sWinFrame.SetActive(true);
					sWinFrame.GetComponent<SpriteRenderer>().color =
						new Color(Defines.ICON_COLOR_P1.r, Defines.ICON_COLOR_P1.g, Defines.ICON_COLOR_P1.b, 0.7f);
				}
				else if(gridWinner == (int)Defines.TURN.P2)
				{
					sWinIcon.GetComponent<SpriteRenderer>().sprite =
						GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().GetSpriteP2();
					//sWinIcon.GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_P2;

					sWinFrame.SetActive(true);
					sWinFrame.GetComponent<SpriteRenderer>().color =
						new Color(Defines.ICON_COLOR_P2.r, Defines.ICON_COLOR_P2.g, Defines.ICON_COLOR_P2.b, 0.7f);
				}

				begin = false;
				for(int i = 0; i <9;++i)
				{
					grids[i].GetComponent<Shaker>().StartShaking(0.75f);
				}

				sWinFrame.GetComponent<Animator>().SetTrigger("isSpin");
				GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().ProcessBoardCompleted();
			}
		}
	}

	public void ProcessBigGridCompleted(Defines.TURN _turn)
	{
		if(IsGridCompleted(_turn))
		{
			if(VibrationManager.HasVibrator())
				VibrationManager.Vibrate(Defines.V_WINBIGGRID);

			if( !(TutorialScript.Instance.isTutorial && bigGridID == 7) && !(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().IsBigGridCompleted()) )
				AudioManager.Instance.PlaySoundEvent(SOUNDID.WIN_BIGGRID);
			
			//add the points
			if(CanGetScore())
			{
				int _score = Defines.smallGridWin;
				if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.LOCAL)
					_score = Defines.smallGridWin_Local;

				Defines.Instance.playerScore += _score;
				GameObject tmp;

				tmp = (GameObject)Instantiate(scrollingText);//.gameObject.GetComponent<FloatingText>().BeginScrolling(" + " + _score + "!");
				canvas = transform.parent.parent.gameObject.GetComponent<BoardScript>().canvas;
				tmp.transform.SetParent(canvas.transform);
				tmp.transform.localScale = new Vector3(1,1,1);
				tmp.transform.localPosition =  new Vector3(0,0,0);
				tmp.GetComponent<Text>().text = "+ " + _score + "!";
				AudioManager.Instance.PlaySoundEvent(SOUNDID.GETPOINTS);
			}

			//begin to do the shakings
			begin = true;
			grids[pos1].GetComponent<Shaker>().StartShaking();
			//grids[pos2].GetComponent<Shaker>().StartShaking();
			//grids[pos3].GetComponent<Shaker>().StartShaking();
			// Win!

			gridWinner = (int)_turn;
			/*GetComponent<LineRenderer>().SetPosition(0,grids[pos1].transform.position+new Vector3(0,0,1));
			GetComponent<LineRenderer>().SetPosition(1,grids[pos3].transform.position+new Vector3(0,0,1));*/
			if(winMethod == WINMETHOD.H_TOP)
				transform.Find("H1").GetComponent<WinLine>().startLine(grids[pos1].GetComponent<Shaker>().duration*3);
			else if(winMethod == WINMETHOD.H_MID)
				transform.Find("H2").GetComponent<WinLine>().startLine(grids[pos1].GetComponent<Shaker>().duration*3);
			else if(winMethod == WINMETHOD.H_BOT)
				transform.Find("H3").GetComponent<WinLine>().startLine(grids[pos1].GetComponent<Shaker>().duration*3);
			else if(winMethod == WINMETHOD.V_LEFT)
				transform.Find("V1").GetComponent<WinLine>().startLine(grids[pos1].GetComponent<Shaker>().duration*3);
			else if(winMethod == WINMETHOD.V_MID)
				transform.Find("V2").GetComponent<WinLine>().startLine(grids[pos1].GetComponent<Shaker>().duration*3);
			else if(winMethod == WINMETHOD.V_RIGHT)
				transform.Find("V3").GetComponent<WinLine>().startLine(grids[pos1].GetComponent<Shaker>().duration*3);
			else if(winMethod == WINMETHOD.BACKSLASH)
				transform.Find("LeftSlash").GetComponent<WinLine>().startLine(grids[pos1].GetComponent<Shaker>().duration*3);
			else if(winMethod == WINMETHOD.SLASH)
				transform.Find("RightSlash").GetComponent<WinLine>().startLine(grids[pos1].GetComponent<Shaker>().duration*3);
		}
		else if(IsDraw())
		{
			if(VibrationManager.HasVibrator())
				VibrationManager.Vibrate(Defines.V_WINBIGGRID);

			gridWinner = 3;
			sWinFrame.SetActive(true);
			sWinFrame.GetComponent<SpriteRenderer>().color =
				new Color(Defines.ICON_COLOR_DRAW.r, Defines.ICON_COLOR_DRAW.g, Defines.ICON_COLOR_DRAW.b, 0.5f);
			sWinFrame.GetComponent<Animator>().SetTrigger("isSpin");
			GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().ProcessBoardCompleted();
		}
	}

	public bool IsGridCompleted(Defines.TURN _turn)
	{
		if( grids[0].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[1].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[2].GetComponent<GridScript>().gridState == (int)_turn )
		{
			//Debug.Log("TUrn: " + _turn);
			pos1 = 0;
			pos2 = 1;
			pos3 = 2;
			winMethod = WINMETHOD.H_TOP;
			return true;
		}

		if( grids[3].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[4].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[5].GetComponent<GridScript>().gridState == (int)_turn )
		{
			pos1 = 3;
			pos2 = 4;
			pos3 = 5;
			winMethod = WINMETHOD.H_MID;
			return true;
		}

		if( grids[6].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[7].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[8].GetComponent<GridScript>().gridState == (int)_turn )
		{
			pos1 = 6;
			pos2 = 7;
			pos3 = 8;
			winMethod = WINMETHOD.H_BOT;
			return true;
		}

		if( grids[0].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[3].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[6].GetComponent<GridScript>().gridState == (int)_turn )
		{
			pos1 = 0;
			pos2 = 3;
			pos3 = 6;
			winMethod = WINMETHOD.V_LEFT;
			return true;
		}

		if( grids[1].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[4].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[7].GetComponent<GridScript>().gridState == (int)_turn )
		{
			pos1 = 1;
			pos2 = 4;
			pos3 = 7;
			winMethod = WINMETHOD.V_MID;
						
			return true;
		}

		if( grids[2].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[5].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[8].GetComponent<GridScript>().gridState == (int)_turn )
		{
			pos1 = 2;
			pos2 = 5;
			pos3 = 8;
			winMethod = WINMETHOD.V_RIGHT;
			return true;
		}

		if( grids[0].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[4].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[8].GetComponent<GridScript>().gridState == (int)_turn )
		{
			pos1 = 0;
			pos2 = 4;
			pos3 = 8;
			winMethod = WINMETHOD.BACKSLASH;
			return true;
		}

		if( grids[2].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[4].GetComponent<GridScript>().gridState == (int)_turn &&
			grids[6].GetComponent<GridScript>().gridState == (int)_turn )
		{
			pos1 = 2;
			pos2 = 4;
			pos3 = 6;
			winMethod = WINMETHOD.SLASH;
			return true;
		}
		return false;
	}

	public bool IsDraw()
	{
		// Draw. All grids filled	
		bool _isDraw = true;
		for(int i = 0; i < 9; ++i)
		{
			if(grids[i].GetComponent<GridScript>().gridState == 0)
			{
				_isDraw = false;
				break;
			}
		}
		return _isDraw;
	}

	bool CanGetScore()
	{
		// Tutorial
		if(TutorialScript.Instance.isTutorial)
			return true;

		// If not your turn during online play
		if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.ONLINE)
		{
			if((NetworkManager.IsPlayerOne() && GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P1)  ||
			   (!NetworkManager.IsPlayerOne() && GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P2) )
        		return true;
        }

        // If you win AI
		if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.AI)
		{
			if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn == Defines.TURN.P1 )
				return true;
		}

		if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.LOCAL)
			return true;

		return false;
	}
}
