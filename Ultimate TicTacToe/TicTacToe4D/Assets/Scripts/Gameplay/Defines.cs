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

	// Player Colors
	static public Color P1_ICON_COLOR = Color.blue;
	static public Color P2_ICON_COLOR = Color.red;

	// Emotes
	static public float EMOTE_SHOW_TIME = 2.5f;						//!< In seconds
	static public float EMOTE_SCALE_TIME = 0.25f;					//!< In seconds

	static public float MATCH_MAKE_RANDOM_RETRY_INTERVAL = 10.0f;	//!< In seconds

	// Grid Layout
	static public float GRID_SPACE = 1.1f;
	static public float GRID_SIZE = 0.5f;
	static public float	BIGGRID_GAP_X = 3.3f;
	static public float	BIGGRID_GAP_Y = 3.4f;

	// Active Grid Settings
	static public Vector3	ACTIVEGRID_POSITION_BIG = new Vector3(0.0f, 0.0f, 0.0f);
	static public float		ACTIVEGRID_SIZE_BIG = 16.3f;
	static public float		ACTIVEGRID_SIZE_SMALL = 5.2f;

	//player point stuff
	public int playerScore;
	static public int smallGridWin = 1;
	static public int bigGridWin = 3;

	void Start()
	{
		//load playerscoree
	}
}
