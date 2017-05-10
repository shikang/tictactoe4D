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

		/*CIRCLE,
		CROSS,

		SPADE,
		HEART,
		CLUB,
		DIAMOND,

		TREBLE,*/

		C001_BOY,
		C002_PRINCESS,
		C003_KNIGHT,
		C004_KING,
		C005_PRINCE,
		C006_OLDMAN,

		U001_CHICK,
		U002_FROG,
		U003_BEAR,
		U004_OCTOPUS,
		U005_PUFFER,

		R001_DEER,
		R002_RACCOON,
		R003_FOX,

		L001_PIG,
		L002_UNICORN,

		B001_COW,
		B002_CHICKEN,
		B003_CLAM,
		B004_RAINBOWGIRL,

		TOTAL
	}
	static public int Avatar_NoofColumns = 5;
	static public int Avatar_FirstIcon = (int)ICONS.LOCKED + 1;	// Need to exclude the first few non avatar icons.

	// Player Colors
	static public Color ICON_COLOR_P1 = new Color(0.102f, 0.675f, 0.686f);
	static public Color ICON_COLOR_P2 = new Color(0.871f, 0.447f, 0.204f);
	static public Color ICON_COLOR_DRAW = new Color(0.075f, 0.737f, 0.376f);
	static public Color ICON_COLOR_HIGHLIGHT = new Color(0.984f, 0.851f, 0.098f);
	static public Color ICON_COLOR_INVALID = new Color(0.886f, 0.118f, 0.043f);
	static public Color ICON_COLOR_GREY = Color.grey;
	static public Color ICON_COLOR_WHITE = Color.white;

	static public ICONS ICON_DEFAULT_P1 = ICONS.C001_BOY;
	static public ICONS ICON_DEFAULT_P2 = ICONS.C002_PRINCESS;

	// Emotes
	static public float EMOTE_SHOW_TIME = 2.5f;						//!< In seconds
	static public float EMOTE_SCALE_TIME = 0.25f;					//!< In seconds

	static public float MATCH_MAKE_RANDOM_RETRY_INTERVAL = 10.0f;	//!< In seconds

	// Grid Layout
	static public float GRID_SPACE = 1.05f;
	static public float GRID_SIZE = 0.5f;
	static public float	BIGGRID_GAP_X = 3.3f;
	static public float	BIGGRID_GAP_Y = 3.35f;

	// Active Grid Settings
	static public Vector3	ACTIVEGRID_POSITION_BIG = new Vector3(0.0f, 0.0f, -0.1f);
	static public float		ACTIVEGRID_SIZE_BIG = 16.3f;
	static public float		ACTIVEGRID_SIZE_SMALL = 5.2f;

	//player point stuff
	public int playerScore;
	static public int smallGridWin = 5;
	static public int bigGridWin = 30;

	// Money stuff
	static public int GACHACOST = 100;
	static public float FREE_ROLL_TIMER = 14400.0f;	//4 hours

	static public float TIMEPERTURN_1 = 20.0f;
	static public float TIMEPERTURN_2 = 30.0f;
	static public float TIMEPERTURN_3 = 40.0f;

	static public int TIMES_TO_SHOW_RATE_APP = 5;
	static public int TIMES_TO_SHOW_LIKE_FB = 3;

	static public int V_STARTGAME = 150;
	static public int V_PLACEICON = 80;
	static public int V_WINBIGGRID = 350;

	static public int MAX_PLAYER_NAME_LENGTH = 10;

	//Gacha Stuff
	static public int GACHARATE_TOTAL_C = 55000;	// Common
	static public int GACHARATE_TOTAL_U = 30000;	// Uncommon
	static public int GACHARATE_TOTAL_R = 10000;	// Rare
	static public int GACHARATE_TOTAL_L = 5000;		// Legenrdary

	public enum AdsInAppPurchase
	{
		DISABLE,

		TOTAL
	}

	void Start()
	{
		//load playerscoree
	}
}
