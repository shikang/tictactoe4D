using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum TUTORIALSTAGE
{
	STARTANIM = 0,
	WELCOME,
	PLACE_TOPLEFT_P,
	OPPONENT_TURN1,
	PLACE_MIDRIGHT_C,
	PLACE_BOTLEFT,
	OPPONENT_TURN2,
	PLACE_TOPRIGHT_C,
	PLACE_BOTRIGHT_P1,
	PLACE_BOTRIGHT_P2,
	WIN,
	END
};

public class TutorialScript : MonoBehaviour
{
	bool isInit;
	public bool isTutorial;
	public TUTORIALSTAGE tStage;

	public GameObject tFrame;
	public GameObject tText_Btn;
	public GameObject tText_NoBtn;
	public GameObject tButton;

	// Singleton pattern
	static TutorialScript instance;
	public static TutorialScript Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
			throw new System.Exception("You have more than 1 TutorialScript in the scene.");

		// Initialize the static class variables
		instance = this;
	}

	void Start()
	{
		isInit = false;
		isTutorial = !GameData.current.finishedTutorial;
	}

	void Update()
	{
		if(!isInit && isTutorial)
		{
			PrePlaceBoard();
			isInit = true;
		}

		// Tutorial Stages. Some are triggered by clicks in fn OnTutorialClick, some are triggered after putting a node (GridScript.cs, Special Case: Tutorials)
		if(tStage == TUTORIALSTAGE.STARTANIM)
		{
			tFrame.SetActive(false);
			if(isTutorial && GameStartAnim.Instance.GameStartAnimEnded())
				tStage = TUTORIALSTAGE.WELCOME;
		}
		else if(tStage == TUTORIALSTAGE.WELCOME)
		{
			tFrame.SetActive(true);
			tText_NoBtn.SetActive(false);
			tText_Btn.GetComponent<Text>().text = "Welcome! In this tutorial, you will learn how to play the game!";
		}
		else if(tStage == TUTORIALSTAGE.PLACE_TOPLEFT_P)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "See that glowing big grid on the top left? That's where you can place. Try placing the middle-right grid!";
		}
		else if(tStage == TUTORIALSTAGE.PLACE_TOPLEFT_P)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "See that glowing big grid on the top left? That's where you can place. Try placing the middle-right grid!";
		}
		else if(tStage == TUTORIALSTAGE.OPPONENT_TURN1)
		{
			tButton.SetActive(true);
			tText_Btn.SetActive(true);
			tText_NoBtn.SetActive(false);
			tText_Btn.GetComponent<Text>().text = "The opponent can now only place on the middle-right big grid, because that's where you sent it to!";

			tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -85.0f, 0.0f);
		}
		else if(tStage == TUTORIALSTAGE.PLACE_MIDRIGHT_C)
		{
			tFrame.SetActive(false);
		}
		else if(tStage == TUTORIALSTAGE.PLACE_BOTLEFT)
		{
			tFrame.SetActive(true);
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "Win the grid!!";

			tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -10.0f, 0.0f);
		}
		else if(tStage == TUTORIALSTAGE.OPPONENT_TURN2)
		{
			tButton.SetActive(true);
			tText_Btn.SetActive(true);
			tText_NoBtn.SetActive(false);
			tText_Btn.GetComponent<Text>().text = "Again, by placing on the top right grid, you've sent the opponent to the top right big grid!";

			tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -50.0f, 0.0f);
		}
		else if(tStage == TUTORIALSTAGE.PLACE_TOPRIGHT_C)
		{
			tFrame.SetActive(false);
		}
		else if(tStage == TUTORIALSTAGE.PLACE_BOTRIGHT_P1)
		{
			tFrame.SetActive(true);
			tButton.SetActive(true);
			tText_Btn.SetActive(true);
			tText_NoBtn.SetActive(false);
			tText_Btn.GetComponent<Text>().text = "The opponent has placed on the bottom-center! It is already completed, so you can place anywhere!";

			tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 10.0f, 0.0f);
		}
		else if(tStage == TUTORIALSTAGE.PLACE_BOTRIGHT_P2)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "Win this game by winning the bottom row!";

			tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -10.0f, 0.0f);
		}
		else if(tStage == TUTORIALSTAGE.WIN)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "Congratulations! You are now ready to play the game!";

			tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -5.0f, 0.0f);
			isTutorial = false;
			GameData.current.finishedTutorial = true;
			SaveLoad.Save();
		}
	}

	public void OnTutorialClick()
	{
		if(tStage == TUTORIALSTAGE.WELCOME)
			tStage = TUTORIALSTAGE.PLACE_TOPLEFT_P;

		else if(tStage == TUTORIALSTAGE.OPPONENT_TURN1)
			tStage = TUTORIALSTAGE.PLACE_MIDRIGHT_C;

		else if(tStage == TUTORIALSTAGE.OPPONENT_TURN2)
			tStage = TUTORIALSTAGE.PLACE_TOPRIGHT_C;

		else if(tStage == TUTORIALSTAGE.PLACE_BOTRIGHT_P1)
			tStage = TUTORIALSTAGE.PLACE_BOTRIGHT_P2;
	}

	void PrePlaceBoard()
	{
		GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().UpdateActiveGridBG(0, false);

		BigGridScript currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[0].GetComponent<BigGridScript>();
		currBigGrid.grids[1].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[3].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[4].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[6].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[8].GetComponent<GridScript>().PlaceOnGrid(2);

		currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[1].GetComponent<BigGridScript>();
		currBigGrid.grids[0].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[1].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[2].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[5].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[7].GetComponent<GridScript>().PlaceOnGrid(2);

		currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[2].GetComponent<BigGridScript>();
		currBigGrid.grids[0].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[1].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[2].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[3].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[4].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[5].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[6].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[8].GetComponent<GridScript>().PlaceOnGrid(2);

		currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[3].GetComponent<BigGridScript>();
		currBigGrid.grids[4].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[6].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[7].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[8].GetComponent<GridScript>().PlaceOnGrid(1);

		currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[4].GetComponent<BigGridScript>();
		currBigGrid.grids[1].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[2].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[3].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[7].GetComponent<GridScript>().PlaceOnGrid(2);

		currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[5].GetComponent<BigGridScript>();
		currBigGrid.grids[1].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[2].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[4].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[8].GetComponent<GridScript>().PlaceOnGrid(2);

		currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[6].GetComponent<BigGridScript>();
		currBigGrid.grids[3].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[4].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[6].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[8].GetComponent<GridScript>().PlaceOnGrid(2);

		currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[7].GetComponent<BigGridScript>();
		currBigGrid.grids[1].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[3].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[4].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[5].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[7].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.ProcessBigGridCompleted(Defines.TURN.P1);

		currBigGrid = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[8].GetComponent<BigGridScript>();
		currBigGrid.grids[0].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[4].GetComponent<GridScript>().PlaceOnGrid(1);
		currBigGrid.grids[5].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[6].GetComponent<GridScript>().PlaceOnGrid(2);
		currBigGrid.grids[7].GetComponent<GridScript>().PlaceOnGrid(1);
	}

	public bool HasTutorialEnded()
	{
		return tStage == TUTORIALSTAGE.WIN;
	}
}

