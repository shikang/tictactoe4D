using UnityEngine;
using System.Collections;

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
		INVALID,
		LOCKED,

		CIRCLE,
		CROSS,

		SPADE,
		HEART,
		CLUB,
		DIAMOND,

		TREBLE,
		TOTAL
	}
	static public int Avatar_NoofColumns = 5;
	static public int Avatar_FirstIcon = 4;	// Need to exclude the first few non avatar icons.

	// Player Colors
	static public Color ICON_COLOR_P1 = Color.blue;
	static public Color ICON_COLOR_P2 = Color.red;
	static public Color ICON_COLOR_GREY = Color.grey;

	// Emotes
	static public float EMOTE_SHOW_TIME = 2.5f;						//!< In seconds
	static public float EMOTE_SCALE_TIME = 0.25f;					//!< In seconds

	static public float MATCH_MAKE_RANDOM_RETRY_INTERVAL = 10.0f;	//!< In seconds

	// Grid Layout
	static public float GRID_SPACE = 1.1f;
	static public float GRID_SIZE = 0.5f;
	static public float	BIGGRID_GAP_X = 3.3f;
	static public float	BIGGRID_GAP_Y = 3.3f;

	// Active Grid Settings
	static public Vector3	ACTIVEGRID_POSITION_BIG = new Vector3(0.0f, 0.0f, -0.1f);
	static public float		ACTIVEGRID_SIZE_BIG = 16.3f;
	static public float		ACTIVEGRID_SIZE_SMALL = 5.2f;

	//player point stuff
	public int playerScore;
	static public int smallGridWin = 1;
	static public int bigGridWin = 3;

	// Money stuff
	static public int GACHACOST = 100;

	void Start()
	{
		//load playerscoree
	}
}
