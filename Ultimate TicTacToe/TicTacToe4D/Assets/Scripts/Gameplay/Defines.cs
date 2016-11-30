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

	static public float GRID_SPACE = 1.25f;
	static public float GRID_STARTPOS_X = 1.00f;
	static public float GRID_STARTPOS_Y = 1.0f;

	static public float BIGGRID_SPACE = 4.0f;
	static public float BIGGRID_STARTPOS_X = 1.35f;
	static public float BIGGRID_STARTPOS_Y = 1.2f;

	static public Vector3	ACTIVEGRID_POSITION_BIG = new Vector3(0f, -0.5f, 0.0f);
	static public float		ACTIVEGRID_SIZE_BIG = 16.3f;

	static public float		GRID_LINE_X = 4.0f;
	static public float		GRID_LINE_Y = 4.0f;
}
