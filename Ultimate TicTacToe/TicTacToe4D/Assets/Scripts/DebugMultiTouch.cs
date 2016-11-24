using UnityEngine;
using System.Collections.Generic;

public class DebugMultiTouch : MonoBehaviour
{
	List<string> touchInfos = new List<string>();

	void Start()
	{
	}

	void Update()
	{
		touchInfos.Clear ();

		for (int i = 0; i < Input.touchCount; ++i)
		{
			Touch touch = Input.GetTouch(i);
			string tmp = "Touch #" + (i + 1) + " at " + touch.position.ToString () + ", " + touch.radius;
			touchInfos.Add(tmp);
		}
	}

	void OnGUI()
	{
		foreach (string s in touchInfos)
		{
			GUILayout.Label(s);
		}
	}
}
