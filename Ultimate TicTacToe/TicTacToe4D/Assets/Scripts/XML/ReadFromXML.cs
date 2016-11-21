using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.UI;
//using System.Xml.Serialization;
public class ReadFromXML : MonoBehaviour 
{
	// Use this for initialization
	public TextAsset XMLScript;
	public GameObject toDup;
	public Image img;
	//string[] array;
	public Sprite[] spriteArray;
	//public InfoPanel ip;

	float ratio;
	void Start ()
	{	
		//array = new string[999];
		XMLParser parser =new XMLParser();
		XMLNode tmp = parser.Parse (XMLScript.ToString());
		ratio = img.rectTransform.rect.width / img.rectTransform.rect.height;
	//	Debug.Log (XMLScript.ToString());
	//	Debug.Log (tmp.Count);
		/*ip.img.sprite = spriteArray [4];
		int height = spriteArray [4].texture.height;
		int width = spriteArray [4].texture.width;
		Debug.Log (spriteArray [4].name);
		Debug.Log (spriteArray [4].texture.height);
		Debug.Log (spriteArray [4].texture.width);
		//this is width /   height
		float ratio = 180.0f / 120.0f;
		float otherratio = (float)width / (float)height;
		Debug.Log ("Ratio: " + ratio);
		Debug.Log ("other ratio: " + otherratio);
		if (otherratio > ratio)
		{
			Debug.Log (otherratio / ratio);
			ip.img.transform.localScale = new Vector3 (1, 1 / (otherratio / ratio), 1);
		}*/
	/*	if(width > 180)
		{
			//if the width is the one that excceds
			//we resize the width so that it remains in 180 range
			int dif = width - 180;
			//now we need to translate this diff for the height as well
			//we get the ratio of how much height to move per width unit
			int newHeight = height -(int)( (float)height/(float)width * (float)dif);
			spriteArray[0].texture.Resize(180,newHeight);
		}*/
		//ip.img.transform.localScale = new Vector3 (1, 1, 1);
		/*
		//EXAMPLE  ON HOW TO GET A SINGLE VAL
		string val = tmp.GetValue ("Test1>0>Info>0>Title>0>_text");
		Debug.Log (val);
		val = tmp.GetValue ("Test1>0>Info>1>Title>0>_text");
		Debug.Log (val);

		//THIS GETS THE ARRAY OF INFO , WHICH MEANS HOW MANY PANELS
		//I NEED TO MAKE
		XMLNodeList arr =tmp.GetNodeList("Test1>0>Info");
		Debug.Log (arr.Count);
	*/
		XMLNodeList arr =tmp.GetNodeList("Test1>0>Info");
		Vector3 pos = toDup.transform.position;
		for(int i =0 ;i< arr.Count;++i)
		{

			//i need to change the way the image is positioned
			GameObject newobj = (GameObject)Instantiate(toDup,pos,Quaternion.identity);
			newobj.transform.parent = this.transform;
			newobj.transform.localScale = new Vector3(1,1,1);
			pos = toDup.transform.localPosition;
			pos.x += 300*i;
			newobj.transform.localPosition = pos;
			//InfoPanel nIP = newobj.GetComponent<InfoPanel>();
			//GrabAllData (tmp, i,nIP);
		}
		toDup.SetActive (false);
	}

	// Update is called once per frame
	void Update () 
	{

	}
/*	void GrabAllData(XMLNode node, int curNo,InfoPanel ip)
	{

	//	Debug.Log ("BEgin test");
		string str = "Test1>0>Info>" + curNo.ToString () + ">";

		//grab the object title
		//Debug.Log( node.GetValue(str+"Title>0>_text"));
		ip.objTitle.text = node.GetValue (str + "Title>0>_text");
		//get the object sprite name
	//	Debug.Log( node.GetValue(str+"Sprite>0>_text"));
		string name = node.GetValue(str+"Sprite>0>_text");
		for(int i=0; i < spriteArray.Length;++i)
		{
			if(spriteArray[i].name.Contains(name))
			{
				ip.img.sprite = spriteArray[i];
				int height = spriteArray [i].texture.height;
				int width = spriteArray [i].texture.width;
				float otherratio = (float)width / (float)height;

				//this is for us to assign the correct image to view
				//when going to the extended level
				if(ip.but!=null)
					ip.but.gameObject.GetComponent<ChangeLevelOnClick>().environNo = i;
				//Debug.Log ("o: "+otherratio +" "+ ratio);
				if (otherratio > ratio)
				{
				//	Debug.Log (otherratio / ratio);
					ip.img.transform.localScale = new Vector3 (1, 1 / (otherratio / ratio), 1);
				}
				else if(otherratio<ratio && ratio <=0.201f)
				{
					ip.img.transform.localScale = new Vector3 (1 / (ratio / otherratio ),1, 1);
				}
				break;
			}
		}
		//get the object description
		//Debug.Log( node.GetValue(str+"Desc>0>_text"));
		ip.objDesc.text = node.GetValue(str+"Desc>0>_text");
/*		//get the object 1st Stats
		Debug.Log( node.GetValue(str+"Stats>0>_text"));
		//get the object 2nd Stats
		Debug.Log( node.GetValue(str+"Stats>1>_text"));
		Debug.Log ("End test");
	}*/
}
