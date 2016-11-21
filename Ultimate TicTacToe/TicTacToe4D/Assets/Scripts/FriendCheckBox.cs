using UnityEngine;
using System.Collections;

public class FriendCheckBox : MonoBehaviour 
{
    private bool playWithFriend = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void TooglePlayWithFriend()
    {
        playWithFriend = !playWithFriend;
    }

    public bool IsPlayingWithFriend()
    {
        return playWithFriend;
    }
}
