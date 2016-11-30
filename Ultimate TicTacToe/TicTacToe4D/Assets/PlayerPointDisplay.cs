using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPointDisplay : MonoBehaviour 
{
	public Text pointDisplay;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		pointDisplay.text = "Points: " + Defines.Instance.playerScore;
	}
}
