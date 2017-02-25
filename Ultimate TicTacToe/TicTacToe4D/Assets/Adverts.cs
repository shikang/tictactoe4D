using UnityEngine;
using UnityEngine.Advertisements;

public enum AdVidType
{
	video = 0,
	rewardedVideo		
};
public class Adverts : MonoBehaviour
{
	//bool show = false;
	public bool freeGacha = false;
	public bool beginReward = false;
	public bool support;
	public int showChance;
	int baseChance = 20;
	int chanceIncrease = 10;
	// Singleton pattern
	static Adverts instance;
	public static Adverts Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		
		if (instance != null)
		{
			//throw new System.Exception("You have more than 1 GlobalScript in the scene.");
			Destroy(this);
			return;
		}
		Advertisement.Initialize("1268576",true);
		Debug.Log(Advertisement.isInitialized);
		Debug.Log(Advertisement.testMode);
		Debug.Log(Advertisement.isSupported);
		support = Advertisement.isSupported;
		showChance = baseChance;
		// Initialize the static class variables
		instance = this;

		DontDestroyOnLoad(gameObject);
	}
	void Start()
	{
	}
	void Update()
	{
		/*if(freeGacha)
		{
			//this means the transit has completed and we are going to call the roll
			//if(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenuScript>().moveScreen == false
			//   &&  beginReward)
			   {
			   		GameObject.FindGameObjectWithTag("Gacha").GetComponent<GachaScript>().StartGacha(true);
			   		freeGacha = beginReward = false;
			   	}
		}*/
		/*if(!show)
			if(ShowAd())
				show = true;
		Debug.Log(Advertisement.IsReady());*/

	}
	public bool GetInit()
	{
		return Advertisement.isInitialized;
	}
	public bool GetSup()
	{
		return Advertisement.isSupported;
	}
	public bool GetStatus()
	{
		return Advertisement.IsReady("rewardedVideo");
	}
	public bool ShowAd(AdVidType type , ShowOptions SO = null)
	{
		//string str;
		if(Advertisement.IsReady(type.ToString()))
		{
				//Debug.Log("showing");
			Advertisement.Show(type.ToString(),SO);
			return true;
		}
		//Debug.Log(Advertisement.IsReady());

		return false;
	}
	public void RemoveAds()
	{

		GameData.current.removeAds = true;
		SaveLoad.Save();
		Debug.Log("Removing Random Ads");
	}
	public void RandomShowAd ()
	{
		//Do not show ads if player has purchased the Ad removal
		if( GameData.current.removeAds)
			return;

		int val = Random.Range(1,101);
		if(val < showChance)
		{	//show  the skippable ad
			ShowAd(AdVidType.video);
			showChance = baseChance;
			return;
		}
		showChance+= chanceIncrease;
	}
	public void FreeGachaHandler(ShowResult result)
	{
	     switch (result)
	     {
		     case ShowResult.Finished:
		         Debug.Log ("Ad Finished. Rewarding player...");
		         if(freeGacha)
		         {
		         	//transit to gacha
		         	//freeGacha = false;
					GameObject.FindGameObjectWithTag("Gacha").GetComponent<GachaScript>().StartGacha(true);
		         }
		         //GameData.current.coin += 10;
		         //SaveLoad.Save();

		         //TODO: NEEEDIUI
		         //NEED UI HERE
		         break;
		     case ShowResult.Skipped:
		         Debug.Log ("Ad Skipped");
		         break;
		     case ShowResult.Failed:
		     		freeGacha = false;
		     		beginReward = false;
		         Debug.Log("Ad failed");
		         break;
		     
	     }
     }
  }