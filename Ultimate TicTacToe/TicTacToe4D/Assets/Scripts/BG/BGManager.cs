using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum BGMAPS
{
	CASTLE = 0,
	FARM,
	FOREST,
	UNDERWATER
};

public struct mBG
{
	public Sprite image;
	public bool isUnlocked;
	public int gachaChance;	// 100% is 1000
};

public class BGManager : MonoBehaviour
{
	// Main BG Stuff
	mBG [] allBG;
	int totalBG = 4;
	BGMAPS currBG;

	// Parts stuff
	public GameObject partsParent;
	public GameObject cloudTemplate;
	public GameObject sheepTemplate;
	public GameObject fishTemplate;

	const int maxNoofParts = 10;
	public int currNoofParts;
	bool hasInitOnce;

	public Sprite [] pClouds;
	public Sprite pSheep;
	public Sprite [] pFishes;

	float [] partsSpeed;
	float timeToNextPart;

	// Singleton pattern
	static BGManager instance;
	public static BGManager Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
		{
			//throw new System.Exception("You have more than 1 BGManager in the scene.");
			Destroy(this);
			return;
		}

		// Initialize the static class variables
		instance = this;
		//DontDestroyOnLoad(gameObject);
	}

	void Start ()
	{
		Init();
	}

	public void Init()
	{
		allBG = new mBG[totalBG];

		SetBGData((int)BGMAPS.CASTLE,		"BG/BG_Castle"		, true, 400);
		SetBGData((int)BGMAPS.FARM,			"BG/BG_Farm"		, true, 750);
		SetBGData((int)BGMAPS.FOREST,		"BG/BG_Forest"		, true, 900);
		SetBGData((int)BGMAPS.UNDERWATER,	"BG/BG_Underwater"	, true, 1000);

		currBG = 0;
		hasInitOnce = false;
		currNoofParts = 0;
		timeToNextPart = 0.0f;
	}

	void SetBGData(int ID, string imagePath, bool _isUnlocked, int _gachaRate)
	{
		allBG[ID].image		  = Resources.Load<Sprite>(imagePath) as Sprite;
		allBG[ID].isUnlocked  = _isUnlocked;
		allBG[ID].gachaChance = _gachaRate;
	}

	public void SetCurrBG()
	{
		currBG = BGMAPS.CASTLE;

		if(TutorialScript.Instance.isTutorial)
		{
			return;
		}

		int randomedNumber = UnityEngine.Random.Range(0, 1000);
		for(int i = 0; i < totalBG; ++i)
		{
			if(randomedNumber < allBG[i].gachaChance)
			{
				currBG = (BGMAPS)i;
				return;
			}
		}
	}

	public Sprite GetCurrBGImage()
	{
		return allBG[(int)currBG].image;
	}

	void Update ()
	{
		if (GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != Defines.TURN.P1 &&
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<TurnHandler>().turn != Defines.TURN.P2)
			return;

		timeToNextPart -= Time.deltaTime;

		if(maxNoofParts > currNoofParts && timeToNextPart <= 0.0f)
		{
			switch(currBG)
			{
			case BGMAPS.CASTLE:
				InstantiateCloud();
				break;

			case BGMAPS.FARM:
				InstantiateCloud();
				InstantiateSheep();
				break;

			case BGMAPS.FOREST:
				InstantiateCloud();
				break;

			case BGMAPS.UNDERWATER:
				InstantiateFishes();
				break;

			default:
				break;
			}

			timeToNextPart = Random.Range(5.0f, 9.0f);
		}
	}

	void InstantiateCloud()
	{
		GameObject curr = GameObject.Instantiate(cloudTemplate);
		curr.GetComponent<Image>().sprite = pClouds[Random.Range(0, pClouds.Length)];
		curr.transform.transform.SetParent(partsParent.transform, false);

		if(currBG == BGMAPS.FOREST)
			curr.GetComponent<Image>().color = new Color(0.11f, 0.2f, 0.23f, 0.8f);

		++currNoofParts;
	}

	void InstantiateSheep()
	{
		if(hasInitOnce)
			return;

		int noofSheep = Random.Range(2, 5);
		for(int i = 0; i < noofSheep; ++i)
		{
			GameObject curr = GameObject.Instantiate(sheepTemplate);
			curr.GetComponentInChildren<Image>().sprite = pSheep;
			curr.transform.transform.SetParent(partsParent.transform, false);
		}
		hasInitOnce = true;
	}

	void InstantiateFishes()
	{
		GameObject curr = GameObject.Instantiate(fishTemplate);
		int fID = Random.Range(0, pFishes.Length);

		curr.GetComponent<Image>().sprite = pFishes[fID];
		curr.GetComponent<BGPartFish>().fishIndex = fID;

		curr.transform.transform.SetParent(partsParent.transform, false);
		++currNoofParts;
	}
}

