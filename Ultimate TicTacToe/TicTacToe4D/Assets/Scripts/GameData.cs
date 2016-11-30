using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
	public static GameData current = new GameData();
	public int coin;
	public List<Defines.ICONS> icons;
	public int win;
	public int matchPlayed;

	public GameData()
	{
		coin = 0;
		icons = new List<Defines.ICONS>();
		win = 0;
		matchPlayed = 0;
	}
}
