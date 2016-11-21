using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour
{
	const int PER_BUY = 100;

	// Use this for initialization
	void Start ()
	{
		SaveLoad.Load();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( Input.GetMouseButtonDown(0) )
		{
			Buy();
		}
	}

	public bool Buy()
	{
		if ( GameData.current.coin >= PER_BUY )
		{
			Debug.Log( GameData.current.coin.ToString() + " -> " + (GameData.current.coin - PER_BUY).ToString() );

			Defines.ICONS icon = (Defines.ICONS)Random.Range( (int)Defines.ICONS.EMPTY + 1, (int)Defines.ICONS.TOTAL - 1 );
			GameData.current.coin -= PER_BUY;

			Debug.Log("Bought icon: " + icon.ToString());

			if( GameData.current.icons == null )
			{
				GameData.current.icons = new System.Collections.Generic.List<Defines.ICONS>();
			}

			if( !GameData.current.icons.Contains( icon ) )
			{
				GameData.current.icons.Add( icon );
			}

			SaveLoad.Save();
			return true;
		}
		else
		{
			Debug.Log( "Not enough money!" );
			return false;
		}
	}
}
