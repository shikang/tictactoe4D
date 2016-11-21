using UnityEngine;
using System.Collections;

public class HideOnNetwork : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        // Check if connected
        if ( NetworkManager.IsConnected() )
        {
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if( gameObject.GetActive() && GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>().gameMode == Defines.GAMEMODE.ONLINE )
        {
            gameObject.SetActive(false);
        }
	}
}
