using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnHandler : MonoBehaviour
{
	public Defines.ICONS spriteID_P1;
	public Defines.ICONS spriteID_P2;
	public Defines.ICONS spriteID_Empty;
	public Defines.ICONS spriteID_Highlight;
	public IconManager allIcons;

	public Color ColorP1;
	public Color ColorP2;

	public int pausedState;		// 0 = Gameplay, 1 = RestartClicked, 2 = MenuClicked
	public float countdownToStart;
	public Defines.TURN turn;

	// Use this for initialization
	void Start ()
	{
		allIcons = GameObject.FindGameObjectWithTag("Global").GetComponent<IconManager>();

		ColorP1 = Defines.P1_ICON_COLOR;
		ColorP2 = Defines.P2_ICON_COLOR;

		if ( NetworkManager.IsConnected() )
		{
			spriteID_P1 = (Defines.ICONS)GameObject.Find("Global").GetComponent<GlobalScript>().iconP1;
			spriteID_P2 = (Defines.ICONS)GameObject.Find("Global").GetComponent<GlobalScript>().iconP2;
		}

		ResetVars();
	}

	void ResetVars()
	{
		turn = Defines.TURN.NOTSTARTED;
		pausedState = 0;
		countdownToStart = 1.5f;
	}

	// Update is called once per frame
	void Update ()
	{
		if(turn == Defines.TURN.NOTSTARTED)
		{
			countdownToStart -= Time.deltaTime;
			if(countdownToStart < 0.5f)
			{
				Color temp = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICenterText.GetComponent<Text>().color;
				temp.a -= 0.1f;
				GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUICenterText.GetComponent<Text>().color = temp;
			}
			if(countdownToStart <= 0.0f)
			{
				turn = Defines.TURN.P1;
			}
		}
	}

	public void ChangeTurn()
	{
		if(turn == Defines.TURN.P1)
			turn = Defines.TURN.P2;
		else if(turn == Defines.TURN.P2)
			turn = Defines.TURN.P1;
	}

    public void WaitingForOtherPlayer()
    {
        turn = Defines.TURN.WAITING;
    }

	public void InformDisconnect()
	{
		turn = Defines.TURN.DISCONNECTED;
	}

	public void UpdatePlayerIcons()
	{
		spriteID_Empty = Defines.ICONS.EMPTY;
		spriteID_Highlight = Defines.ICONS.HIGHLIGHT;

		// Read from main menus
		spriteID_P1 = Defines.ICONS.SPADE;
		spriteID_P2 = Defines.ICONS.HEART;
	}

	public Sprite GetSpriteEmpty()
	{
		return allIcons.GetIcon(Defines.ICONS.EMPTY);
	}

	public Sprite GetSpriteP1()
	{
		return allIcons.GetIcon(spriteID_P1);
	}

	public Sprite GetSpriteP2()
	{
		return allIcons.GetIcon(spriteID_P2);
	}

	public Sprite GetSpriteHighlight()
	{
		return allIcons.GetIcon(spriteID_Highlight);
	}
}
