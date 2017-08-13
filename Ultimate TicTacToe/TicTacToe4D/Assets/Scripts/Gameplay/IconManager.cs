using UnityEngine;
using System.Collections;

public enum TIERS
{
	COMMON = 0,
	UNCOMMON,
	RARE,
	LEGENDARY,
	BUY,
	NIL
}

public struct mICON
{
	public Sprite image;
	public bool isUnlocked;
	public string name;

	public bool isBuy;
	public bool isGacha;
	public TIERS gachaTier;
	public int gachaChance;	// 100% is 1000
}

public class IconManager : MonoBehaviour
{
	public mICON [] mIcon;

	int counterGacha_Common;
	int counterGacha_Uncommon;
	int counterGacha_Rare;
	int counterGacha_Legendary;
	int counterBuyable;

	int gachaRate_Indiv_C;
	int gachaRate_Indiv_U;
	int gachaRate_Indiv_R;
	int gachaRate_Indiv_L;

	// Singleton pattern
	static IconManager instance;
	public static IconManager Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
		{
			//throw new System.Exception("You have more than 1 IconManager in the scene.");
			Destroy(this);
			return;
		}

		// Initialize the static class variables
		instance = this;

		DontDestroyOnLoad(gameObject);
	}

	void Start ()
	{
		Init();
	}

	public void Init()
	{
		counterGacha_Common = counterGacha_Uncommon = counterGacha_Rare = counterGacha_Legendary = 0;
		mIcon = new mICON[(int)Defines.ICONS.TOTAL];

		SetIconData(Defines.ICONS.EMPTY,		"Icons/Empty",		false, false, false, TIERS.NIL);
		SetIconData(Defines.ICONS.HIGHLIGHT,	"Icons/Highlight",	false, false, false, TIERS.NIL);
		SetIconData(Defines.ICONS.INVALID,		"Icons/Invalid",	false, false, false, TIERS.NIL);
		SetIconData(Defines.ICONS.LOCKED,		"Icons/Locked",		false, false, false, TIERS.NIL);

		SetIconData(Defines.ICONS.C001_BOY,		"Icons/C001_Boy",		true, false, true, TIERS.COMMON);
		SetIconData(Defines.ICONS.C002_PRINCESS,"Icons/C002_Princess",	true, false, true, TIERS.COMMON);
		SetIconData(Defines.ICONS.C003_KNIGHT,	"Icons/C003_Knight",	false, false, true, TIERS.COMMON);
		SetIconData(Defines.ICONS.C004_KING,	"Icons/C004_King",		false, false, true, TIERS.COMMON);
		SetIconData(Defines.ICONS.C005_PRINCE,	"Icons/C005_Prince",	false, false, true, TIERS.COMMON);
		SetIconData(Defines.ICONS.C006_OLDMAN,	"Icons/C006_OldMan",	false, false, true, TIERS.COMMON);

		SetIconData(Defines.ICONS.U001_COW,		"Icons/U001_Cow",		false, false, true, TIERS.UNCOMMON);
		SetIconData(Defines.ICONS.U002_FROG,	"Icons/U002_Frog",		false, false, true, TIERS.UNCOMMON);
		SetIconData(Defines.ICONS.U003_BEAR,	"Icons/U003_Bear",		false, false, true, TIERS.UNCOMMON);
		SetIconData(Defines.ICONS.U004_RACCOON,	"Icons/U004_Raccoon",	false, false, true, TIERS.UNCOMMON);
		SetIconData(Defines.ICONS.U005_DEER,	"Icons/U005_Deer",		false, false, true, TIERS.UNCOMMON);

		SetIconData(Defines.ICONS.R001_CHICKEN,	"Icons/R001_Chicken",	false, false, true, TIERS.RARE);
		SetIconData(Defines.ICONS.R002_CHICK,	"Icons/R002_Chick",		false, false, true, TIERS.RARE);
		SetIconData(Defines.ICONS.R003_FOX,		"Icons/R003_Fox",		false, false, true, TIERS.RARE);

		SetIconData(Defines.ICONS.L001_RAINBOWGIRL,	"Icons/L001_RainbowGirl",		false, false, true, TIERS.LEGENDARY);
		SetIconData(Defines.ICONS.L002_UNICORN,	"Icons/L002_Unicorn",	false, false, true, TIERS.LEGENDARY);

		SetIconData(Defines.ICONS.B001_PIG,		"Icons/B001_Pig",		false, true, false, TIERS.BUY);
		SetIconData(Defines.ICONS.B002_PUFFER,	"Icons/B002_Puffer",	false, true, false, TIERS.BUY);
		SetIconData(Defines.ICONS.B003_CLAM,	"Icons/B003_Clam",		false, true, false, TIERS.BUY);
		SetIconData(Defines.ICONS.B004_OCTOPUS,	"Icons/B004_Octopus",	false, true, false, TIERS.BUY);

		CalculateGachaRates();

		//SetIconData(Defines.ICONS.CIRCLE,		"Icons/Circle",		true, false, true, 300);
		//SetIconData(Defines.ICONS.CROSS,		"Icons/Cross",		true, false, true, 300);

		//SetIconData(Defines.ICONS.SPADE,		"Icons/Spade",		false, false, true, 200);
		//SetIconData(Defines.ICONS.HEART,		"Icons/Heart",		false, false, true, 100);
		//SetIconData(Defines.ICONS.CLUB,		"Icons/Club",		false, false, true, 100);
		//SetIconData(Defines.ICONS.DIAMOND,	"Icons/Diamond",	false, true, false, 0);

		//SetIconData(Defines.ICONS.TREBLE,		"Icons/Treble",		false, true, false, 0);
	}

	void SetIconData(Defines.ICONS ID, string imagePath, bool isUnlocked_, bool isBuy_, bool isGacha_, TIERS gachaTier_)
	{
		mIcon[(int)ID].image		= Resources.Load<Sprite>(imagePath) as Sprite;
		mIcon[(int)ID].isUnlocked 	= isUnlocked_;
		mIcon[(int)ID].isBuy 		= isBuy_;
		mIcon[(int)ID].isGacha		= isGacha_;
		mIcon[(int)ID].gachaTier	= gachaTier_;
		mIcon[(int)ID].name			= imagePath.ToString().Substring(6);

		if(mIcon[(int)ID].gachaTier == TIERS.COMMON)
			counterGacha_Common++;
		else if(mIcon[(int)ID].gachaTier == TIERS.UNCOMMON)
			counterGacha_Uncommon++;
		else if(mIcon[(int)ID].gachaTier == TIERS.RARE)
			counterGacha_Rare++;
		else if(mIcon[(int)ID].gachaTier == TIERS.LEGENDARY)
			counterGacha_Legendary++;
		else if(mIcon[(int)ID].gachaTier == TIERS.BUY)
			counterBuyable++;
	}

	public Sprite GetIcon(Defines.ICONS currIcon)
	{
		return mIcon[(int)currIcon].image;
	}

	public Sprite GetIcon(int i)
	{
		//return mIcon[(currIcon%7)+3].image;
		return mIcon[i].image;
	}

	public int GetNoofIcons()
	{
		return (int)Defines.ICONS.TOTAL;
	}

	public void SetUnlocked(int ID, bool setter)
	{
		mIcon[ID].isUnlocked = setter;
	}

	public bool GetIsUnlocked(int i)
	{
		return mIcon[i].isUnlocked;
	}

	public bool GetIsBuy(int i)
	{
		return mIcon[i].isBuy;
	}

	public int GetNoofBuyableIcons()
	{
		return counterBuyable;
	}

	public string GetName(Defines.ICONS i)
	{
		return mIcon[(int)i].name;
	}

	public void CalculateGachaRates()
	{
		int stackRates = 0;
		gachaRate_Indiv_C = (int)(Defines.GACHARATE_TOTAL_C / counterGacha_Common);
		gachaRate_Indiv_U = (int)(Defines.GACHARATE_TOTAL_U / counterGacha_Uncommon);
		gachaRate_Indiv_R = (int)(Defines.GACHARATE_TOTAL_R / counterGacha_Rare);
		gachaRate_Indiv_L = (int)(Defines.GACHARATE_TOTAL_L / counterGacha_Legendary);

		for(int i = (int)Defines.Avatar_FirstIcon; i < (int)Defines.ICONS.TOTAL; ++i)
		{
			if(mIcon[i].gachaTier == TIERS.COMMON)
			{
				mIcon[i].gachaChance = stackRates + gachaRate_Indiv_C;
				stackRates += gachaRate_Indiv_C;
			}
			else if(mIcon[i].gachaTier == TIERS.UNCOMMON)
			{
				mIcon[i].gachaChance = stackRates + gachaRate_Indiv_U;
				stackRates += gachaRate_Indiv_U;
			}
			else if(mIcon[i].gachaTier == TIERS.RARE)
			{
				mIcon[i].gachaChance = stackRates + gachaRate_Indiv_R;
				stackRates += gachaRate_Indiv_R;
			}
			else if(mIcon[i].gachaTier == TIERS.LEGENDARY)
			{
				mIcon[i].gachaChance = stackRates + gachaRate_Indiv_L;
				stackRates += gachaRate_Indiv_L;
			}
			else
			{
				mIcon[i].gachaChance = 0;
			}
		}
	}

	public int FindGachaIcon(int randomedNumber)
	{
		for(int i = (int)Defines.Avatar_FirstIcon; i < (int)Defines.ICONS.TOTAL; ++i)
		{
			if(randomedNumber < mIcon[i].gachaChance)
				return i;
		}
		return (int)Defines.ICONS.L001_RAINBOWGIRL;
	}

	void Update()
	{
	}
}

