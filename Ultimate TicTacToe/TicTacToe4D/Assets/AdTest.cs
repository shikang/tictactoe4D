using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AdTest : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
//		Debug.Log(Adverts.Instance.GetSup());
		if(Adverts.Instance.support == false)
			GetComponent<Image>().color = new Color(1,0,0);
		/*else
			GetComponent<Image>().color = new Color(0,1,0);*/
	}
}
