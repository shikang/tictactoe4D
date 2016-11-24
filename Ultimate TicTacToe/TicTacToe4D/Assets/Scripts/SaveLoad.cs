using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad
{
	public static void Save()
	{
		// Test
		GameData.current.coin += 100;

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, GameData.current);
		file.Close();
	}

	public static void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			GameData.current = (GameData)bf.Deserialize(file);
			file.Close();
		}
	}
}
