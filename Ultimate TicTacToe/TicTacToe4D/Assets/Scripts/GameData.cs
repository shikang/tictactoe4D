using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class GameData
{
	public static GameData current = new GameData();
	public int coin;
	public List<Defines.ICONS> icons;
	public int win;
	public int matchPlayed;
	public string avatarName;
	public int avatarIcon;
	public DateTime lastFreeRollTime;
	public bool RandomAds;

	public GameData()
	{
		coin = 1000;
		icons = new List<Defines.ICONS>();
		win = 0;
		matchPlayed = 0;
		avatarName = "abc";
		avatarIcon = 8;
		lastFreeRollTime = DateTime.Now;
		RandomAds = true;
	}
}
