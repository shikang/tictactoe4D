using UnityEngine;
using System.Collections;

public struct mICON
{
	public Sprite image;
	public bool isUnlocked;

	public bool isBuy;
	public bool isGacha;
	public int gachaChance;	// 100% is 1000
}

public class IconManager : MonoBehaviour
{
	public mICON [] mIcon;

	// Singleton pattern
	static IconManager instance;
	public static IconManager Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
			throw new System.Exception("You have more than 1 IconManager in the scene.");

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
		mIcon = new mICON[(int)Defines.ICONS.TOTAL];

		SetIconData(Defines.ICONS.EMPTY,		"Icons/Empty",		false, false, false, 0);
		SetIconData(Defines.ICONS.HIGHLIGHT,	"Icons/Highlight",	false, false, false, 0);
		SetIconData(Defines.ICONS.INVALID,		"Icons/Invalid",	false, false, false, 0);
		SetIconData(Defines.ICONS.LOCKED,		"Icons/Locked",		false, false, false, 0);

		SetIconData(Defines.ICONS.CIRCLE,		"Icons/Circle",		true, false, true, 300);
		SetIconData(Defines.ICONS.CROSS,		"Icons/Cross",		true, false, true, 300);

		SetIconData(Defines.ICONS.SPADE,		"Icons/Spade",		false, false, true, 200);
		SetIconData(Defines.ICONS.HEART,		"Icons/Heart",		false, false, true, 100);
		SetIconData(Defines.ICONS.CLUB,			"Icons/Club",		false, false, true, 100);
		SetIconData(Defines.ICONS.DIAMOND,		"Icons/Diamond",	false, true, false, 0);

		SetIconData(Defines.ICONS.TREBLE,		"Icons/Treble",		false, true, false, 0);
	}

	void SetIconData(Defines.ICONS ID, string imagePath, bool isUnlocked_, bool isBuy_, bool isGacha_, int gachaChance_)
	{
		mIcon[(int)ID].image		= Resources.Load<Sprite>(imagePath) as Sprite;
		mIcon[(int)ID].isUnlocked 	= isUnlocked_;
		mIcon[(int)ID].isBuy 		= isBuy_;
		mIcon[(int)ID].isGacha		= isGacha_;
		mIcon[(int)ID].gachaChance	= gachaChance_;
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
		int count = 0;
		for(int i = 0; i < (int)Defines.ICONS.TOTAL; ++i)
		{
			if(mIcon[i].isBuy)
				++count;
		}
		return count;
	}

	void Update()
	{
	}
}

