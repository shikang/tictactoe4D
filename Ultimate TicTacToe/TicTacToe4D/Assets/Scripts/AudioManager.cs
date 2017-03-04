using UnityEngine;
using System.Collections;

public enum SOUNDID
{
	ICONPLACED = 0,
	WONBIGGRID
};

public class AudioManager : MonoBehaviour
{

	// Singleton pattern
	static AudioManager instance;
	public static AudioManager Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
			throw new System.Exception("You have more than 1 AudioManager in the scene.");

		// Initialize the static class variables
		instance = this;
	}

	void Start()
	{
	
	}

	void Update()
	{
	
	}

	public void PlaySoundEvent(SOUNDID sid, GameObject go = null)
	{
		if(!go)
			go = gameObject;

		switch(sid)
		{
		case SOUNDID.ICONPLACED:
			SoundEventWrapper("Play_IconClicked", gameObject);
			break;

		case SOUNDID.WONBIGGRID:
			SoundEventWrapper("Play_BigGridWon", go);
			break;

		default:
			Debug.Log("Sound ID " + go + " does not exist");
			break;
		}
	}

	void SoundEventWrapper(string s, GameObject go)
	{
		//AkSoundEngine.PostEvent(s, go);
	}

	public void SetMasterVol(float vol)
	{
		//AkSoundEngine.SetRTPCValue("MasterVol", vol);
	}

	public void SetBGMVol(float vol)
	{
		//AkSoundEngine.SetRTPCValue("BGMVol", vol);
	}

	public void SetSFXVol(float vol)
	{
		//AkSoundEngine.SetRTPCValue("SFXVol", vol);
	}

	//AkSoundEngine.SetState("GameState", "MainMenu");
}

