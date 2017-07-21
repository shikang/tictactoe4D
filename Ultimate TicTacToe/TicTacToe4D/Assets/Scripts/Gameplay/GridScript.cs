using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridScript : MonoBehaviour
{
	public int gridState;	// Whose grid it belongs to
	public int gridID;

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
			if(gridState == 0)
				PlaceOnGrid(4);
			return;
		}

        if ( GetBoardScript().gameMode == Defines.GAMEMODE.ONLINE &&
            ( (  NetworkManager.IsPlayerOne() && GetTurnHandler().turn != Defines.TURN.P1 ) ||
              ( !NetworkManager.IsPlayerOne() && GetTurnHandler().turn != Defines.TURN.P2 ) ) )
        {
			if(gridState == 0)
				PlaceOnGrid(4);
			return;
        }

		//if(Input.touchCount != 1)
		//	return;

		// Don't do anything if the big grid is already won, or game hasn't started/has ended, or game is paused.
		if(parentGrid.GetComponent<BigGridScript>().gridWinner != 0 ||
			GetTurnHandler().turn == Defines.TURN.NOTSTARTED ||
			GetTurnHandler().turn == Defines.TURN.GAMEOVER ||
			GetGUIManagerScript().GUIEmoteScreen.GetActive() ||
			GetTurnHandler().pausedState != 0)
			return;


		// Special Case: Tutorials
		if(TutorialScript.Instance.isTutorial)
		{
			if( TutorialScript.Instance.tStage == TUTORIALSTAGE.PLACE_TOPLEFT_P ||
				TutorialScript.Instance.tStage == TUTORIALSTAGE.ICON_HIGHLIGHTED )
			{
				if(GetBoardScript().activeBigGrid == 0 && gridID == 5)
				{
					if(gridState == 0)
					{
						HighlightGrid();
						TutorialScript.Instance.tStage = TUTORIALSTAGE.ICON_HIGHLIGHTED;
					}
					else if(gridState == 3)
					{
						ConfirmPlacement();
						TutorialScript.Instance.tStage = TUTORIALSTAGE.OPPONENT_TURN1;
					}
				}
				else
				{
					if(gridState == 0)
					{
						TutorialScript.Instance.tStage = TUTORIALSTAGE.PLACE_TOPLEFT_P;
						PlaceOnGrid(4);
					}
				}
			}

			else if(TutorialScript.Instance.tStage == TUTORIALSTAGE.PLACE_BOTLEFT)
			{
				if(GetBoardScript().activeBigGrid == 6 && gridID == 2)
				{
					if(gridState == 0)
						HighlightGrid();
					else if(gridState == 3)
					{
						ConfirmPlacement();
						TutorialScript.Instance.tStage = TUTORIALSTAGE.OPPONENT_TURN2;
					}
				}
				else
				{
					if(gridState == 0)
						PlaceOnGrid(4);
				}
			}

			else if(TutorialScript.Instance.tStage == TUTORIALSTAGE.PLACE_BOTRIGHT_P2)
			{
				if(GetBoardScript().activeBigGrid == 10 && gridID == 1)
				{
					if(gridState == 0)
						HighlightGrid();
					else if(gridState == 3)
					{
						ConfirmPlacement();
						TutorialScript.Instance.tStage = TUTORIALSTAGE.WIN;
					}
				}
				else
				{
					if(gridState == 0)
						PlaceOnGrid(4);
				}
			}
		}

		// Check if node belongs to the activeGrid
		else
		{
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

						// Player is acting like server
						if (NetworkManager.IsPlayerOne())
						{
							ConfirmPlacement();
						}
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
	}

	void OnMouseUp()
	{
		if(gridState == 4)
			PlaceOnGrid(0);
		else if(gridState == 1 || gridState == 2)
			GetComponent<Animator>().SetTrigger("isIconPlaced");
	}

	public void HighlightGrid()
	{
		GetBoardScript().SetCurrentHighlight(parentGrid.GetComponent<BigGridScript>().bigGridID, gridID);
		//GetBoardScript().ResetAllHighlights();
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
			GetComponent<Animator>().SetTrigger("isIconPlaced");
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_P1;
			mainIcon.GetComponent<SpriteRenderer>().sprite = GetTurnHandler().GetSpriteP1();
			mainIcon.SetActive(true);

			if(GameStartAnim.Instance.GameStartAnimEnded())
				AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_CONFIRMED);

			if(VibrationManager.HasVibrator())
				VibrationManager.Vibrate(Defines.V_PLACEICON);
			break;

		case 2:	// P2 Icon
			GetComponent<Animator>().SetTrigger("isIconPlaced");
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_P2;
			mainIcon.GetComponent<SpriteRenderer>().sprite = GetTurnHandler().GetSpriteP2();
			mainIcon.SetActive(true);

			if(GameStartAnim.Instance.GameStartAnimEnded())
				AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_CONFIRMED);

			if(VibrationManager.HasVibrator())
				VibrationManager.Vibrate(Defines.V_PLACEICON);
			break;

		case 3:	// Highlighting (Yellow)
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_HIGHLIGHT;
			GetComponent<Animator>().SetTrigger("isHighlighted");
			GetGUIManagerScript().gridEffect_growStage = 10;
			GetGUIManagerScript().gridEffect.transform.position = transform.position;
			mainIcon.SetActive(false);
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_HIGHLIGHTED);
			break;

		case 4:	// Invalid (Red)
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_INVALID;
			GetComponent<Animator>().SetTrigger("isInvalid");
			//GetBoardScript().SetCurrentHighlight(10, 10);
			mainIcon.SetActive(false);
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_INVALID);
			break;

		default:
			break;
		}
	}

	public void ConfirmPlacement()
	{
		GetGUIManagerScript().gridEffect_growStage = 5;
		GetGUIManagerScript().gridEffect.transform.position = transform.position;

		PlaceOnGrid((int)GetTurnHandler().turn);
		parentGrid.GetComponent<BigGridScript>().ProcessBigGridCompleted(GetTurnHandler().turn);
		GetTurnHandler().ChangeTurn();

		GetGUIManagerScript().ChangeEmoteButtonColors();
		GetGUIManagerScript().ResetTimer();

		GetBoardScript().UpdateActiveGridBG(gridID);
		GetGUIManagerScript().UpdateClick();
		//GameObject.FindGameObjectWithTag("AIMiniMax").GetComponent<AIMiniMax>().UpdateAI();
		//AudioManager.Instance.PlaySoundEvent(SOUNDID.ICONPLACED);

		if(TutorialScript.Instance.isTutorial)
		{
			if(TutorialScript.Instance.tStage == TUTORIALSTAGE.PLACE_MIDRIGHT_C)
				TutorialScript.Instance.tStage = TUTORIALSTAGE.PLACE_BOTLEFT;
			else if(TutorialScript.Instance.tStage == TUTORIALSTAGE.PLACE_TOPRIGHT_C)
				TutorialScript.Instance.tStage = TUTORIALSTAGE.PLACE_BOTRIGHT_P1;
		}
	}

	public void ResetHighlight()
	{
		if(gridState == 3)
		{
			gridState = 0;
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_GREY;
			GetComponent<Animator>().SetBool("isHighlighted", false);
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
