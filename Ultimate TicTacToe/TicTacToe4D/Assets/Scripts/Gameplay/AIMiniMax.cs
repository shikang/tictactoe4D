using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	}

	public void UpdateAI()
	{
		if(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != AITurn)
			return;
	
		// UI Update not done

		// Set timer: How long AI takes to highlight a grid
		if(AIStage == 0)
		{
			AI_ThinkingTimerMax = Random.Range(1.0f, 2.5f);
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
				int gridID = 4;
				/*do
				{
					gridID = Random.Range(0, 8);
				}
				while( GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[gridID].GetComponent<BigGridScript>().IsGridCompleted(GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn )
						|| GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[gridID].GetComponent<BigGridScript>().IsDraw() );
			*/}
			currentBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().activeBigGrid;
			MiniMax();
			AIStage = 4;
		}

		// Set timer: How long AI takes to confirm a highlighted grid
		else if(AIStage == 4)
		{
			AI_ThinkingTimerMax = Random.Range(0.5f, 2.0f);
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
		if(currentBigGrid == 10)
		{
			currentBigGrid = FindBestBigGrid();
		}

		// Terminate if game ends.
		int m_iUtility = 0;
		if(TerminalTest(ref m_iUtility, depth))
		{ 
			// Print message after computer places cell.
			if(m_iUtility == MIN_VALUE)
			{
				Debug.Log("Player Wins!");
			}
			if(terminalValue == 0)
			{
				// Draw
				Debug.Log("Draw!");
				return false;
			}
			return true;
		}

		MaxVal = -INFINITE;
		minVals.Clear();
		posVals.Clear();
		//Debug.Log("Start");
		int changecount = 0;
		for(int i = 0; i < 9; ++i)
		{
			/* Places a node on the first empty spot and tests MiniMax.
			   Once done, 1unplaces it and places onthe next empty node.
			   After all of them are tested, it finds the best position
			   according to the evalution function to place the node. */
			if(GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[currentBigGrid].GetComponent<BigGridScript>().grids[i].GetComponent<GridScript>().gridState == EmptyCell)
			{
				Place(i, AITurn);
				MinVal = O(depth-1, MaxVal, INFINITE);
				UnPlace(i);
				//Debug.Log("Val: " + MinVal);

				minVals.Add(MinVal);
				posVals.Add(i);

				if(MaxVal < MinVal)
				{
					++changecount;
					FinalGrid = i;
					MaxVal = MinVal;
				}
			}
		}
		//Debug.Log("Min: "+MinVal+"\nChangeCount: " + changecount);
		if(diff == Difficulty.Easy)
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
			return true;
		}
		return false;
	}

	// Heuristic function that calculates the number of lines that are still available for Computer to use and win.
	int EvaluationTest(BigGridScript go)
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
		
		return noofLines;
	}

	int FindBestBigGrid()
	{
		// The big grid that the AI has most chance to win.
		int bestID = -1;

		for(int i = 0; i < 9; ++i)
		{
			BigGridScript go = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[i].GetComponent<BigGridScript>();
			int val = EvaluationTest(go);
			if(bestID < val)
				bestID = val;
		}
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
		return MaxVal;
	}

	int O(int depth, int alpha, int beta)
	{
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
		return MinVal;
	}
}

