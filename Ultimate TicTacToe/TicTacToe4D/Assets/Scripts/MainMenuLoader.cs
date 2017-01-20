using UnityEngine;
using System.Collections;

public class MainMenuLoader : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		SaveLoad.Load();
		AvatarHandler.Instance.SetMyAvatarName(GameData.current.avatarName);
		AvatarHandler.Instance.SetMyAvatarIcon(GameData.current.avatarIcon);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
