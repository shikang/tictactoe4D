using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

enum Difficulty 
{
	None=0,
	Easy,
	Medium,
	Hard
};
public class AIMiniMax : MonoBehaviour
{
	int MIN_VALUE = -9;
	int MAX_VALUE = 9;
	int INFINITE = 1000;
	int depth;
	int terminalValue;
	public int FinalGrid;

	public int currentBigGrid;
	int EmptyCell;
	public Defines.TURN PlayerTurn;
	public Defines.TURN AITurn;
	List<int> minVals,posVals;
	int MaxVal;
	int MinVal;
	int aiMin;
	int aiMax;
	bool highlighted;
	Difficulty diff;
	int fakeTimesMax;
	int fakeTimesCurr;

	int AIStage;
	float AI_ThinkingTimerMax;
	float AI_ThinkingTimerCurr;

	void Start()
	{
		minVals = new List<int>();
		posVals = new List<int>();

		fakeTimesMax = 3;
		ResetVars();
	}

	void ResetVars()
	{
		highlighted = false;
		depth = 9;
		currentBigGrid = 10;
		EmptyCell = 0;

		AIStage = 0;
		diff = Difficulty.Hard;
		AITurn = Defines.TURN.P2;	// TEMPORARY. MUST CHANGE!!
		PlayerTurn = Defines.TURN.P1;

		fakeTimesCurr = 0;
	}

	void Update()
	{
		if( GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.AI &&
			GameObject.FindGameObjectWithTag("GUIManager") &&
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnEmote.GetActive())
		{
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnEmote.SetActive(false);

			Vector3 tmp = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnMainMenu.GetComponent<RectTransform>().localPosition;
			tmp.x = 110.0f;
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnMainMenu.GetComponent<RectTransform>().localPosition = tmp;

			tmp = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnRestart.GetComponent<RectTransform>().localPosition;
			tmp.x = -110.0f;
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnRestart.GetComponent<RectTransform>().localPosition = tmp;

		}
	}

	public void UpdateAI()
	{
		if(TutorialScript.Instance.isTutorial)
		{
			if( TutorialScript.Instance.tStage != TUTORIALSTAGE.PLACE_MIDRIGHT_C &&
				TutorialScript.Instance.tStage != TUTORIALSTAGE.PLACE_TOPRIGHT_C )
				return;
		}

		if( GameObject.FindGameObjectWithTag("GUIManager") &&
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != AITurn)
			return;

		// Set timer: How long AI takes to highlight a grid
		if(AIStage == 0)
		{
			AI_ThinkingTimerMax = UnityEngine.Random.Range(0.5f, 1.7f);
			//AI_ThinkingTimerMax = 0.0f;
			AI_ThinkingTimerCurr = 0.0f;
			AIStage = 1;
		}

		// Countdown
		else if(AIStage == 1)
		{
			AI_ThinkingTimerCurr += Time.deltaTime;
			if(AI_ThinkingTimerCurr >= AI_ThinkingTimerMax)
				AIStage = 2;
		}

		// Highlight grid
		else if(AIStage == 2)
		{
			// Free to go any big grid
			if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().activeBigGrid == 10)
			{
				currentBigGrid = FindBestBigGrid();
			}
			else
			{
				currentBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().activeBigGrid;
			}
			MiniMax();
			AIStage = 4;
		}

		// Set timer: How long AI takes to confirm a highlighted grid
		else if(AIStage == 4)
		{
			AI_ThinkingTimerMax = UnityEngine.Random.Range(0.5f, 2.0f);
			//AI_ThinkingTimerMax = 0.0f;
			AI_ThinkingTimerCurr = 0.0f;
			AIStage = 5;
		}

		// Countdown
		else if(AIStage == 5)
		{
			AI_ThinkingTimerCurr += Time.deltaTime;
			if(AI_ThinkingTimerCurr >= AI_ThinkingTimerMax)
				AIStage = 6;
		}

		// Place
		else if(AIStage == 6)
		{
			GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].
					GetComponent<BigGridScript>().grids[FinalGrid].
					GetComponent<GridScript>().ConfirmPlacement();
			AIStage = 0;
		}
	}

	public bool MiniMax()
	{
		// Terminate if game ends.
		int m_iUtility = 0;
		if(TerminalTest(ref m_iUtility, depth))
		{ 
			// Print message after computer places cell.
			if(m_iUtility == MIN_VALUE)
			{
			}
			if(terminalValue == 0)
			{
				return false;
			}
			return true;
		}

		MaxVal = -INFINITE;
		//minVals.Clear();
		//posVals.Clear();
		//Debug.Log("Start");
		int changecount = 0;
		int maxEmptySlots=0;
		int []tmpval = new int[9];
		int []tmppos = new int[9];
		for(int i = 0; i < 9; ++i)
		{
			/* Places a node on the first empty spot and tests MiniMax.
			   Once done, 1unplaces it and places onthe next empty node.
			   After all of them are tested, it finds the best position
			   according to the evalution function to place the node. */
			if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().grids[i].GetComponent<GridScript>().gridState == EmptyCell)
			{
				++maxEmptySlots;
				Place(i, AITurn);
				MinVal = O(depth-1, MaxVal, INFINITE);
				UnPlace(i);
				//Debug.Log(i+ " > Val: " + MinVal);
				tmpval[i] = MinVal;
				//minVals.Add(MinVal);
				//posVals.Add(i);

				if(MaxVal < MinVal)
				{
					FinalGrid = i;
					MaxVal = MinVal;
				}
				/*else if (MaxVal == MinVal)
					++changecount;*/
			}
			else
			{
				tmpval[i] = -INFINITE;
			}
		}

		//Debug.Log("Min: "+MinVal+"\nChangeCount: " + changecount);
		/*if(diff == Difficulty.Easy)
		{
			aiMin = MaxVal - 6;
			aiMax = MaxVal + 6;
		}
		else if ( diff == Difficulty.Medium)
		{
			aiMin = MaxVal - 3;
			aiMax = MaxVal + 3;
		}
		else if( diff == Difficulty.Hard)
		{
			aiMin = MaxVal;
			aiMax = MaxVal;
		}

		if(changecount < 2 && MinVal == 0)
		{
			//purge the list of values != minval
			int removedNo = 0;
			for(int i =0; i < minVals.Count; ++i)
			{
				if(minVals[i] != MaxVal)
				{
					posVals.RemoveAt(i-removedNo);
					++removedNo;
				}
			}
			minVals.RemoveAll(RemoveLargerNum);
			int tmpval = Random.Range(0,minVals.Count);
			int tmpposval = posVals[tmpval];
			FinalGrid =  tmpposval;
		}*/

		/*string s="";
		for(int j =0 ; j <9 ; ++j)
		{
			s += "[" + tmpval[j] + "] ";
			if(j%3 == 2)
				s+= "\n";
		}
		Debug.Log(s);*/

		//Debug.Log("Empty Slots: " + maxEmptySlots);
		//if some or all of the positions are equally viable, they will have the same values
		//so we randomnize it between them
		changecount=0;
		Debug.Log("Finalgrid: " + FinalGrid);
		for (int k =0; k < 9; ++k)
		{
			if(k == FinalGrid)
				continue;
			//we check if there are duplicate values
			else if (tmpval[FinalGrid] == tmpval[k])
			{
				//we assign the duplicate position to an arroy
				tmppos[changecount] = k;
				++changecount;
			}
		}
		Debug.Log("Changecount: "+changecount);
		//this means there is at least 1 duplicate
		if(changecount >0)
		{
			tmppos[changecount] = FinalGrid;
			++changecount;
			Debug.Log("Randomizing because there are duplicate max vals");
			int rand = UnityEngine.Random.Range(0,changecount+1);
			Debug.Log("Rand: " + rand);
			FinalGrid = tmppos[rand];
			Debug.Log("Randomed placement: " + FinalGrid);
		}
		Place(FinalGrid, AITurn);
		GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().grids[FinalGrid].GetComponent<GridScript>().HighlightGrid();
		return true;
	}
	bool RemoveLargerNum(int val)
	{
		//if(val > MaxVal )
		if(val >= aiMax ||  val <= aiMin)
				return  true;
		return false;
	}
	//Tests if a goal node is achieved or if the max depth is reached.
	bool TerminalTest(ref int terminalValue, int depth)
	{
		// If AI can put anywhere, find the one that it has bext chance to win.
		if(currentBigGrid == 10)
		{
			currentBigGrid = FindBestBigGrid();
		}
		//Debug.Log(currentBigGrid);
		// Free to go any big grid
		if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().IsGridCompleted(AITurn))
		{
			terminalValue = MAX_VALUE;
			return true;
		}
		if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().IsGridCompleted(PlayerTurn))
		{
			terminalValue = MIN_VALUE;
			return true;
		}
		if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().IsDraw())
		{
			terminalValue = 0;
			return true;
		}

		// Called when max depth is reached
		if(depth == 0)
		{
			BigGridScript go = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>();
			terminalValue = EvaluationTest(go);
			//Debug.Log("ES: " + terminalValue);
			return true;
		}
		return false;
	}

	// Heuristic function that calculates the number of lines that are still available for Computer to use and win.
	int EvaluationTest(BigGridScript go,bool terminal = true)
	{
		int noofLines = 0;
		// Checks for horizontal wins
		if( go.grids[0].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[1].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[2].GetComponent<GridScript>().gridState != (int)PlayerTurn)
			noofLines += 1;

		if( go.grids[3].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[4].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[5].GetComponent<GridScript>().gridState != (int)PlayerTurn)
			noofLines += 1;

		if( go.grids[6].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[7].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[8].GetComponent<GridScript>().gridState != (int)PlayerTurn)
			noofLines += 1;

		if( go.grids[0].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[3].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[6].GetComponent<GridScript>().gridState != (int)PlayerTurn)
			noofLines += 1;

		if( go.grids[1].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[4].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[7].GetComponent<GridScript>().gridState != (int)PlayerTurn)
			noofLines += 1;

		if( go.grids[2].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[5].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[8].GetComponent<GridScript>().gridState != (int)PlayerTurn)
			noofLines += 1;

		if( go.grids[0].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[4].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[8].GetComponent<GridScript>().gridState != (int)PlayerTurn)
			noofLines += 1;

		if( go.grids[2].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[4].GetComponent<GridScript>().gridState != (int)PlayerTurn &&
			go.grids[6].GetComponent<GridScript>().gridState != (int)PlayerTurn)
			noofLines += 1;

		if(terminal == false)
		{
			for(int i =0; i <9; ++i)
			{
				if(go.grids[i].GetComponent<GridScript>().gridState == (int)AITurn)
				{
					if(UnityEngine.Random.Range(1,101) <=60)
						++noofLines;
				}
				//add a chance to add to the no of lines
			}
		}

		return noofLines;
	}

	int FindBestBigGrid()
	{
		//Debug.Log("COMEHERE");	
		// The big grid that the AI has most chance to win.
		int[] vals ;
		vals = new int[9];
		int bestID = -1;
		int bestVal = -1;
		int sameweight = 0;

		for(int i = 0; i < 9; ++i)
		{
			BigGridScript go = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[i].GetComponent<BigGridScript>();
			//Debug.Log(go.gridWinner);
			//this is so that we do not place into won already grids?
			if(go.gridWinner == 0)
			{
				int val = EvaluationTest(go,false);
				vals[i] = val;
				//Debug.Log(i + ": " + val);
				if(bestVal < val)
				{
					bestID = i;
					bestVal = val;
				}
				else if( bestVal == val)
					++sameweight;
			}
			else
			{
				vals[i] = -1;	// completed grids should have no chance.
			}
		}
		/*string s="";
		for(int j =0 ; j <9 ; ++j)
		{
			s += "[" + vals[j] + "] ";
			if(j%3 == 2)
				s+= "\n";
		}
		Debug.Log(s);*/
		//Debug.Log("Same weight: " + sameweight);
		//if this number is 8, it means all the board have the same chance of winning
		//so we will random a board between all of them
		if(sameweight == 8)
			bestID = UnityEngine.Random.Range(0,9);

		return bestID;
	}

	void Place(int gridID, Defines.TURN turn)
	{
		GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().grids[gridID].GetComponent<GridScript>().gridState = (int)turn;
	}

	void UnPlace(int gridID)
	{
		GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().grids[gridID].GetComponent<GridScript>().gridState= EmptyCell;
	}

	int X(int depth, int alpha, int beta)
	{
		int MaxVal = -INFINITE;
		if(TerminalTest(ref MaxVal, depth))
			return MaxVal;

		// Place in every cell and check value.
		for(int i = 0; i < 9; ++i)
		{
			if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().grids[i].GetComponent<GridScript>().gridState == EmptyCell)
			{
				Place(i, AITurn);
				MaxVal = Mathf.Max(MaxVal, O(depth-1, alpha, beta));
				UnPlace(i);

				// Get the best value
				if(MaxVal >= beta)
					return MaxVal;
				alpha = Mathf.Max(alpha, MaxVal);
			}
		}

		//original 
		//return MaxVal;
		//testing might break!!!
		return MaxVal + depth;
	}

	int O(int depth, int alpha, int beta)
	{
		//Debug.Log("Depth: " + depth);
		int MinVal = INFINITE;
		if(TerminalTest(ref MinVal, depth))
			return MinVal;

		// Place in every cell and check value.
		for(int i = 0; i < 9; ++i)
		{
			if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().grids[i].GetComponent<GridScript>().gridState == EmptyCell)
			{
				Place(i, PlayerTurn);
				MinVal = Mathf.Min(MinVal, X(depth-1, alpha, beta));
				UnPlace(i);

				// Get the best value
				if(MinVal <= alpha)
					return MinVal;
				beta = Mathf.Min(beta, MinVal);
			}
		}

		//original
		//return MinVal;
		//TESTING might break!!
		return MinVal - depth;
	}
}

