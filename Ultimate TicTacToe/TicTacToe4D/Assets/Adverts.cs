using UnityEngine;
using UnityEngine.Advertisements;

public enum AdVidType
{
	video = 0,
	rewardedVideo		
};
public class Adverts : MonoBehaviour
{
	bool show = false;

	public bool support;
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

		// Initialize the static class variables
		instance = this;

		DontDestroyOnLoad(gameObject);
	}
	void Start()
	{
		
	}
	void Update()
	{
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
	public bool ShowAd(AdVidType type = AdVidType.rewardedVideo)
	{
		//Debug.Log(Advertisement.IsReady());
		if(Advertisement.IsReady("rewardedVideo"))
		{
			//Debug.Log("showing");
				Advertisement.Show("rewardedVideo");
			return true;
		}
		return false;
	}
	void AdCallbackhandler(ShowResult result)
	{
	     switch (result)
	     {
		     case ShowResult.Finished:
		         Debug.Log ("Ad Finished. Rewarding player...");
		         GameData.current.coin += 10;
		         SaveLoad.Save();
		         //TODO: NEEEDIUI
		         //NEED UI HERE
		         break;
		     case ShowResult.Skipped:
		         Debug.Log ("Ad Skipped");
		         break;
		     case ShowResult.Failed:
		         Debug.Log("Ad failed");
		         break;
		     
	     }
     }
  }