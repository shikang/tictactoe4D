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
	public DateTime nextFreeRollTime;
	public bool removeAds;
	public bool finishedTutorial;
	public bool shownRateApp;
	public bool shownLikeFacebook;
	public bool hasBGM;
	public bool hasSFX;

	public GameData()
	{
		coin = 250;
		icons = new List<Defines.ICONS>();
		win = 0;
		matchPlayed = 0;
		avatarName = "abc";
		avatarIcon = 8;
		nextFreeRollTime = DateTime.Now;
		removeAds = false;
		finishedTutorial = false;
		shownRateApp = false;
		hasBGM = true;
		hasSFX = true;
	}
}
