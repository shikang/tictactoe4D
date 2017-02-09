using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour 
{

	public float fadeTime = 1.25f;
	public float floatSpeed;
	Text tmp;

	// Use this for initialization
	void Start () 
	{
		tmp = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(fadeTime >=0)
		{
			fadeTime-=Time.deltaTime;

			transform.localPosition = transform.localPosition + new Vector3(0,floatSpeed,0);
			tmp.color = new Color(tmp.color.r,tmp.color.g,tmp.color.b,tmp.color.a - Time.deltaTime / fadeTime );

			if(fadeTime <=0)
				Destroy(gameObject);
		}
	}
	public void BeginScrolling (string txt)
	{

		tmp.text = txt;

	}
}
