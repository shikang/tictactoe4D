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
	public bool hasVibrate;

	public GameData()
	{
		coin = 250;
		icons = new List<Defines.ICONS>();
		win = 0;
		matchPlayed = 0;
		avatarName = RandomName();
		avatarIcon = 4;
		nextFreeRollTime = DateTime.Now;
		removeAds = false;
		finishedTutorial = false;
		shownRateApp = false;
		hasBGM = true;
		hasSFX = true;
		hasVibrate = true;
	}

	string RandomName()
	{
		int rand = UnityEngine.Random.Range(0, 7);
		switch(rand)
		{
		case 0: return "Peter";
		case 1: return "Jane";
		case 2: return "Chris";
		case 3: return "Tony";
		case 4: return "Bruce";
		case 5: return "Mary";
		case 6: return "Natasha";
		default: return "Tommy";
		}
		return "Stephen";
	}
}
