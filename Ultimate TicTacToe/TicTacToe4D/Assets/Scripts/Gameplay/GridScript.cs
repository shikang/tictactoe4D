﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridScript : MonoBehaviour
{
	public int gridState;	// Whose grid it belongs to
	public int gridID;

	public GameObject gridEffect;
	public GameObject parentGrid;
	public GameObject mainIcon;

	void Start ()
	{
	}

	void Update()
	{
	}

	void OnMouseDown()
	{
		if( GetBoardScript().gameMode == Defines.GAMEMODE.AI &&
			GetTurnHandler().turn == GameObject.FindGameObjectWithTag("AIMiniMax").GetComponent<AIMiniMax>().AITurn )
		{
			return;
		}


        if ( GetBoardScript().gameMode == Defines.GAMEMODE.ONLINE &&
            ( (  NetworkManager.IsPlayerOne() && GetTurnHandler().turn != Defines.TURN.P1 ) ||
              ( !NetworkManager.IsPlayerOne() && GetTurnHandler().turn != Defines.TURN.P2 ) ) )
        {
            return;
        }

		//if(Input.touchCount != 1)
		//	return;

		// Don't do anything if the big grid is already won, or game hasn't started/has ended, or game is paused.
		if(parentGrid.GetComponent<BigGridScript>().gridWinner != 0 ||
			GetTurnHandler().turn == Defines.TURN.NOTSTARTED ||
			GetTurnHandler().turn == Defines.TURN.GAMEOVER ||
			GetTurnHandler().pausedState != 0)
			return;

		// Check if node belongs to the activeGrid
		if(GetBoardScript().activeBigGrid == parentGrid.GetComponent<BigGridScript>().bigGridID ||
		   GetBoardScript().activeBigGrid == 10)
		{
			// Highlight grid
			if(gridState == 0)
			{
                if ( GetBoardScript().gameMode == Defines.GAMEMODE.ONLINE )
                {
                    NetworkGameLogic networkLogic = NetworkGameLogic.GetNetworkGameLogic();
                    networkLogic.HighlightGrid(parentGrid.GetComponent<BigGridScript>().bigGridID, gridID);
                }
                else
                {
                    HighlightGrid();
                }
			}

			// Only allowed if grid is highlighted
			else if(gridState == 3)
			{
                if ( GetBoardScript().gameMode == Defines.GAMEMODE.ONLINE )
                {
                    NetworkGameLogic networkLogic = NetworkGameLogic.GetNetworkGameLogic();
                    networkLogic.ConfirmPlacement(parentGrid.GetComponent<BigGridScript>().bigGridID, gridID,
                                                  NetworkManager.IsPlayerOne() ? Defines.TURN.P1 : Defines.TURN.P2,
												  NetworkManager.IsPlayerOne() ? GetGUIManagerScript().timerP1 : GetGUIManagerScript().timerP2);
                }
                else
                {
                    ConfirmPlacement();
                }
			}
		}
		else
		{
			if(gridState == 0)
				PlaceOnGrid(4); // Current grid is invalid (red)
		}
	}

	void OnMouseUp()
	{
		if(gridState == 4)
			PlaceOnGrid(0);
			
	}

	public void HighlightGrid()
	{
		GetBoardScript().ResetAllHighlights();
		PlaceOnGrid(3);
	}

	public void PlaceOnGrid(int _newState)
	{
		gridState = _newState;

		switch(gridState)
		{
		case 0:	// Nothing (Black)
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_GREY;
			mainIcon.SetActive(false);
			break;

		case 1:	// P1 Icon
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_P1;
			mainIcon.GetComponent<SpriteRenderer>().sprite = GetTurnHandler().GetSpriteP1();
			mainIcon.SetActive(true);
			GetComponent<Animator>().SetTrigger("isIconPlaced");
			break;

		case 2:	// P2 Icon
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_P2;
			mainIcon.GetComponent<SpriteRenderer>().sprite = GetTurnHandler().GetSpriteP2();
			mainIcon.SetActive(true);
			GetComponent<Animator>().SetTrigger("isIconPlaced");
			break;

		case 3:	// Highlighting (Yellow)
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_HIGHLIGHT;
			GetComponent<Animator>().SetTrigger("isHighlighted");
			mainIcon.SetActive(false);
			break;

		case 4:	// Invalid (Red)
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_INVALID;
			GetComponent<Animator>().SetTrigger("isHighlighted");
			mainIcon.SetActive(false);
			break;

		default:
			break;
		}
	}

	public void ConfirmPlacement()
	{
		GetGUIManagerScript().gridEffect_growStage = 1;
		GetGUIManagerScript().gridEffect.transform.position = transform.position;

		PlaceOnGrid((int)GetTurnHandler().turn);
		parentGrid.GetComponent<BigGridScript>().ProcessBigGridCompleted(GetTurnHandler().turn);
		GetTurnHandler().ChangeTurn();

		GetBoardScript().UpdateActiveGridBG(gridID);
		GetGUIManagerScript().UpdateClick();
		GameObject.FindGameObjectWithTag("AIMiniMax").GetComponent<AIMiniMax>().UpdateAI();
		//AudioManager.Instance.PlaySoundEvent(SOUNDID.ICONPLACED);
	}

	public void ResetHighlight()
	{
		if(gridState == 3)
		{
			gridState = 0;
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_GREY;
		}
	}

    BoardScript GetBoardScript()
    {
        return GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>();
    }

	TurnHandler GetTurnHandler()
    {
		return GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>();
    }

	GUIManagerScript GetGUIManagerScript()
    {
		return GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>();
    }
}
