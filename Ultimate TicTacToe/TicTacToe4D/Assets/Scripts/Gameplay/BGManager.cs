using UnityEngine;
using System.Collections;

public struct mBG
{
	public Sprite image;
	public bool isUnlocked;
	public int gachaChance;	// 100% is 1000
}

public class BGManager : MonoBehaviour
{
	mBG [] allBG;
	int totalBG = 4;
	int currBG;

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

		DontDestroyOnLoad(gameObject);
	}

	void Start ()
	{
		Init();
	}

	public void Init()
	{
		allBG = new mBG[totalBG];

		SetBGData(0, "BG/01", true, 500);
		SetBGData(1, "BG/02", true, 800);
		SetBGData(2, "BG/03", true, 950);
		SetBGData(3, "BG/04", true, 1000);

		currBG = 0;
	}

	void SetBGData(int ID, string imagePath, bool _isUnlocked, int _gachaRate)
	{
		allBG[ID].image		  = Resources.Load<Sprite>(imagePath) as Sprite;
		allBG[ID].isUnlocked  = _isUnlocked;
		allBG[ID].gachaChance = _gachaRate;
	}

	public void SetCurrBG()
	{
		currBG = 0;
		int randomedNumber = UnityEngine.Random.Range(0, 1000);

		for(int i = 0; i < totalBG; ++i)
		{
			if(randomedNumber < allBG[i].gachaChance)
			{
				currBG = i;
				return;
			}
		}
	}

	public Sprite GetCurrBGImage()
	{
		return allBG[currBG].image;
	}

	void Update ()
	{
	}
}

