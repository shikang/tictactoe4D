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

