using UnityEngine;
using UnityEngine.Advertisements;

public class Adverts : MonoBehaviour
{
	bool show = false;
	void Start()
	{
		Advertisement.Initialize("1268576",true);
		Debug.Log(Advertisement.isInitialized);
		Debug.Log(Advertisement.testMode);
		Debug.Log(Advertisement.isSupported);
	}
	void Update()
	{
		if(!show)
			if(Advertisement.IsReady())
			{
				show = true;
				Debug.Log("showing");
				Advertisement.Show();
			}
		//Debug.Log(Advertisement.IsReady());
	}
  public void ShowAd()
  {
  	
    if (Advertisement.IsReady())
    {
      //Advertisement.Show();
    }
  }
}