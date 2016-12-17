using UnityEngine;
using System.Collections;

public class DevTools : MonoBehaviour
{
	bool godMode;

	void Start ()
	{
		godMode = true;
	}

	void Update ()
	{
		if(godMode)
		{
			if(Input.GetKeyDown(KeyCode.K))
				AddMoneySmall();
			else if(Input.GetKeyDown(KeyCode.L))
				AddMoneyBig();
		}
	}

	void AddMoneySmall()
	{
		GameData.current.coin += 500;
	}

	void AddMoneyBig()
	{
		GameData.current.coin += 10000;
	}
}

