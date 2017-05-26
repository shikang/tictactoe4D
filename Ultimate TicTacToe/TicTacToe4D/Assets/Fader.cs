using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public enum FadeType
{
	Nothing=0,
	FadeIn_Out,
	SplitDisappear,
	ScrollUpDown,
	Total
};
public class Fader : MonoBehaviour 
{
	Color col;
	Canvas cv;
	public FadeType type;
	[Header("Fade Time in Seconds")]
	public float fadeTime;
	float fadeTimer;
	bool  fadeIn = true;//scrollup is the same as fadein
	[Header("No of Squares")]
	public int squareCount;
	// Use this for initialization
	float canvasHeight;
	List<GameObject> clones = new List<GameObject>();
	void Start () 
	{
		fadeTimer = fadeTime;
		if(transform.parent!=null)
		{
			cv = this.GetComponentInParent<Canvas>();
			GetComponent<RectTransform>().sizeDelta = new Vector2(cv.GetComponent<RectTransform>().rect.width, cv.GetComponent<RectTransform>().rect.height);
		}
		if(type == FadeType.FadeIn_Out)
		{
			col = GetComponent<Image>().color;
			if(col.a ==1)
				fadeIn = false;
		}
		else if (type == FadeType.ScrollUpDown)
		{
			//fadein will mean scrollup

			if(GetComponent<RectTransform>().anchoredPosition.y !=0)
				fadeIn = false;
			//Debug.Log(GetComponent<RectTransform>().anchoredPosition);

		}
		else if(type == FadeType.SplitDisappear)
		{
			col = GetComponent<Image>().color;
			//Debug.Log("AAAAAAA");
			clones.Add(this.gameObject);
			float newwidth = cv.GetComponent<RectTransform>().rect.width / squareCount;
			float newheight = cv.GetComponent<RectTransform>().rect.height / squareCount;
			if(col.a ==1)
				fadeIn = false;
			if(squareCount > 1)
			{
				this.GetComponent<RectTransform>().sizeDelta = new Vector2(newwidth,newheight);
				//GameObject clone;
				/*for ( int x = 0; x < squareCount ; ++x)
				{
					for ( int y = 0 ; y < squareCount ; ++y)
					{
						if (x == 0 && y == 0)
						{}
						else
						{
							//clone = GameObject.Instantiate(this.gameObject);
							//remove the script of the cloned gameobject

							//Debug.Log("AAAAAAAAAAAAAAA  " + clone.transform.parent);
							//clone.transform.SetParent(this.transform.parent);//why?	
							//if (transform.parent == null)
							//	Debug.Log("WTF");
							//clone.transform.parent = transform.parent;
							//clone.transform.parent = transform.parent;
							//clone.GetComponent<RectTransform>().localPosition = new Vector2(newwidth*x,newheight*y);
							//clones.Add(clone);
						}
					}
				}*/
			}
		}
		//Debug.Log("Height: "+cv.GetComponent<RectTransform>().rect.height);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(fadeTimer >0)
		{
			fadeTimer-=Time.deltaTime;
			if(type == FadeType.FadeIn_Out)
				FadeInOut();
			else if( type == FadeType.ScrollUpDown)
				ScrollUpDown();
			else if( type == FadeType.SplitDisappear)
				SplitDisappear();
		}
	}
	void SplitDisappear()
	{
		//Debug.Log(clones.Count);
		for (int i = 0; i < clones.Count; ++i)
		{
			col = clones[i].GetComponent<Image>().color;
			if(!fadeIn)
			{				
				//Debug.Log("HUH");
				col.a -= Time.deltaTime/fadeTime;
				if(col.a <=0)
					col.a = 0;
			}
			else
			{
				//Debug.Log("why");
				col.a += Time.deltaTime/fadeTime;
				if(col.a >=1)
					col.a = 1;	
			}
			clones[i].GetComponent<Image>().color=col;
		}
		if(fadeTimer<=0)
		{
			fadeIn = !fadeIn;
		}
	}
	void FadeInOut()
	{
		col = GetComponent<Image>().color;
		if(!fadeIn)
		{
			col.a -= Time.deltaTime/fadeTime;
			if(col.a <=0)
				col.a = 0;
		}
		else
		{
			col.a += Time.deltaTime/fadeTime;
			if(col.a >=1)
				col.a = 1;	
		}
			GetComponent<Image>().color=col;
		if(fadeTimer<=0)
		{
			fadeIn = !fadeIn;
		}
	}
	void ScrollUpDown()
	{
		canvasHeight = cv.GetComponent<RectTransform>().rect.height;
		Vector3 tmpvec;
		tmpvec = GetComponent<RectTransform>().anchoredPosition3D;
		if(fadeIn)
		{
			tmpvec.y += canvasHeight*Time.deltaTime/fadeTime;
			//Debug.Log(tmpvec.y);
			if(tmpvec.y >= canvasHeight)
				tmpvec.y  = canvasHeight;

			GetComponent<RectTransform>().anchoredPosition3D = tmpvec;
			//Debug.Log("AAA");
		}
		else
		{
			tmpvec.y -= canvasHeight*Time.deltaTime/fadeTime;
			//Debug.Log(tmpvec.y);
			if(tmpvec.y <= 0)
				tmpvec.y  = 0;

			GetComponent<RectTransform>().anchoredPosition3D = tmpvec;
		}
		if(fadeTimer<=0)
		{
			fadeIn = !fadeIn;	
		}
	}
}
