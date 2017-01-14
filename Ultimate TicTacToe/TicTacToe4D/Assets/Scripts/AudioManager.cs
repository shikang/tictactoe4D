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

		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
	
	}

	void Update()
	{
	
	}

	public void PlaySoundEvent(SOUNDID go)
	{
		switch(go)
		{
		case SOUNDID.ICONPLACED:
			SoundEventWrapper("IconPlaced");
			break;

		case SOUNDID.WONBIGGRID:
			SoundEventWrapper("WonBigGrid");
			break;

		default:
			Debug.Log("Sound ID " + go + " does not exist");
			break;
		}
	}

	void SoundEventWrapper(string s)
	{
		AkSoundEngine.PostEvent(s, gameObject);
	}
}

