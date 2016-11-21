using UnityEngine;
using System.Collections;



/*
public class DEFSIZE
{
	public float	SIZE_BIGGRID_BG = 5.2f;
	public float	SIZE_BOARD_BG = 15.6f;
}*/

public class Defines : Singleton<Defines>
{
	protected Defines(){}

	public enum TURN
	{
		NOTSTARTED = 0,
		P1,
		P2,
		WINANIMATION,
		GAMEOVER,
	    WAITING,
		DISCONNECTED
	};

	public enum GAMEMODE
	{
	    LOCAL,
	    AI,
	    ONLINE
	}

	public enum ICONS
	{
		EMPTY = 0,
		HIGHLIGHT,

		CIRCLE,
		CROSS,

		SPADE,
		HEART,
		CLUB,
		DIAMOND,

		TREBLE,
		TOTAL
	}

	static public Color P1_ICON_COLOR = Color.blue;
	static public Color P2_ICON_COLOR = Color.red;

	static public float EMOTE_SHOW_TIME = 2.5f;						//!< In seconds
	static public float EMOTE_SCALE_TIME = 0.25f;					//!< In seconds

	static public float MATCH_MAKE_RANDOM_RETRY_INTERVAL = 10.0f;	//!< In seconds
}

