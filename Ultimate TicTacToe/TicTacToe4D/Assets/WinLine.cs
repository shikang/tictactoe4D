using UnityEngine;
using System.Collections;

public class WinLine : MonoBehaviour 
{
	public float duration=-1;
	public float distance;
	//public float st
	//Transform tf;1
	//bool start = false;
	//1.6
	// Use this for initialization
	float amtToAdd;
	Vector3 nScale;
	void Start () 
	{
		//tf = transform;
		amtToAdd = distance / duration;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(duration > 0)
		{
			duration -= Time.deltaTime;
			nScale = transform.localScale;
			nScale.x += amtToAdd * Time.deltaTime;
			transform.localScale = nScale;
			if(duration <=0)
				duration = -1;
		}
	}
	public void startLine(float dur)
	{
		/*if(gameObject.GetComponentInParent<BigGridScript>().gridWinner == 1)
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_P1;
		else if(gameObject.GetComponentInParent<BigGridScript>().gridWinner == 2)
			GetComponent<SpriteRenderer>().color = Defines.ICON_COLOR_P2;*/

//		Debug.Log("????");
		duration = dur;
		amtToAdd = distance / dur;
	}
}