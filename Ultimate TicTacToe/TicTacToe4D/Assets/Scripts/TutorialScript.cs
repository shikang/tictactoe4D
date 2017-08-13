using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum TUTORIALSTAGE
{
	STARTANIM = 0,
	WELCOME,
	PLACE_TOPLEFT_P,
	ICON_HIGHLIGHTED,
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
	float winCountdown;

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
		winCountdown = 2.2f;

		if(isTutorial)
		{
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnMainMenu.SetActive(false);
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnEmote.SetActive(false);
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().BtnRestart.SetActive(false);
		}
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
			//tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -40.0f, 0.0f);
			tText_Btn.GetComponent<Text>().text = "Welcome! Let's run through the basics of the game!";
		}
		else if(tStage == TUTORIALSTAGE.PLACE_TOPLEFT_P)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "See that glowing board on the big <color=green>top left</color> grid? That shows where you can place your piece. Try placing in the <color=blue>middle-right</color> box!";

			GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[0].GetComponent<BigGridScript>().grids[5].GetComponent<GridScript>().
				GetComponent<Animator>().SetTrigger("isIconPlaced");

		}
		else if(tStage == TUTORIALSTAGE.ICON_HIGHLIGHTED)
		{
			tText_NoBtn.GetComponent<Text>().text = "The first tap highlights the box. Tap on it again to <color=green>confirm</color> placement!";

			GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[0].GetComponent<BigGridScript>().grids[5].GetComponent<GridScript>().
				GetComponent<Animator>().SetTrigger("isIconPlaced");
		}
		else if(tStage == TUTORIALSTAGE.OPPONENT_TURN1)
		{
			tButton.SetActive(true);
			tText_Btn.SetActive(true);
			tText_NoBtn.SetActive(false);
			tText_Btn.GetComponent<Text>().text = "By placing your piece on the <color=green>middle-right</color> box of this grid, you've sent the opponent to the big <color=green>middle-right</color> grid!";

			//tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -80.0f, 0.0f);
		}
		else if(tStage == TUTORIALSTAGE.PLACE_MIDRIGHT_C)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "Waiting for opponent...";
		}
		else if(tStage == TUTORIALSTAGE.PLACE_BOTLEFT)
		{
			tFrame.SetActive(true);
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "The opponent has sent you to the big <color=green>bottom-left grid!</color> Win it!!";

			//tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -10.0f, 0.0f);

			GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[6].GetComponent<BigGridScript>().grids[2].GetComponent<GridScript>().
				GetComponent<Animator>().SetTrigger("isIconPlaced");
		}
		else if(tStage == TUTORIALSTAGE.OPPONENT_TURN2)
		{
			tButton.SetActive(true);
			tText_Btn.SetActive(true);
			tText_NoBtn.SetActive(false);
			tText_Btn.GetComponent<Text>().text = "Again, by placing your piece on the <color=green>top-right</color> box of this grid, you've sent the opponent to the big <color=green>top-right</color> grid!";

			//tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -50.0f, 0.0f);
		}
		else if(tStage == TUTORIALSTAGE.PLACE_TOPRIGHT_C)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "Waiting for opponent...";
		}
		else if(tStage == TUTORIALSTAGE.PLACE_BOTRIGHT_P1)
		{
			tFrame.SetActive(true);
			tButton.SetActive(true);
			tText_Btn.SetActive(true);
			tText_NoBtn.SetActive(false);
			tText_Btn.GetComponent<Text>().text = "Your opponent has placed on the <color=green>bottom-center</color> box of the grid, which is already completed! So, you can now place <color=blue>ANYWHERE</color>!";

			//tFrame.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -15.0f, 0.0f);
		}
		else if(tStage == TUTORIALSTAGE.PLACE_BOTRIGHT_P2)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "Win this game by winning the bottom big grids!";

			GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().bigGrids[8].GetComponent<BigGridScript>().grids[1].GetComponent<GridScript>().
				GetComponent<Animator>().SetTrigger("isIconPlaced");

		}
		else if(tStage == TUTORIALSTAGE.WIN)
		{
			tButton.SetActive(false);
			tText_Btn.SetActive(false);
			tText_NoBtn.SetActive(true);
			tText_NoBtn.GetComponent<Text>().text = "Congratulations! You are now ready to challenge other players!";

			winCountdown -= Time.deltaTime;
			if(winCountdown <= 0.0f)
			{
				tFrame.SetActive(false);
			}

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

		AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
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

