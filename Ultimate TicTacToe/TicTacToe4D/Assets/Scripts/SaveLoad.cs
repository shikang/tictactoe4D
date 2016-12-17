using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad
{
	public static void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, GameData.current);
		file.Close();
	}

	public static void Load()
	{
		//Debug.Log("Loading: " + Application.persistentDataPath + "/savedGames.gd");
		if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			GameData.current = (GameData)bf.Deserialize(file);
			file.Close();

			Debug.Log("Save loaded");
			Print();
		}
	}

	public static void Print()
	{
		Debug.Log("Coins: " + GameData.current.coin);
		Debug.Log("Match played: " + GameData.current.matchPlayed);
		Debug.Log("Match win: " + GameData.current.win);

		foreach (Defines.ICONS i in GameData.current.icons)
		{
			Debug.Log("Icons: " + i.ToString());
		}
	}
}
