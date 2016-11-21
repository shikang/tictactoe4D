using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FriendPassword : MonoBehaviour 
{
    public GameObject playButton;

	// Use this for initialization
	void Start () 
    {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(GetComponent<InputField>().text == "")
        {
            playButton.GetComponent<Button>().enabled = false;
        }
        else
        {
            playButton.GetComponent<Button>().enabled = true;
        }
	}

    public void ToogleVisible()
    {
        gameObject.SetActive(!gameObject.GetActive());

        if(gameObject.GetActive() && playButton != null)
        {
            playButton.GetComponent<Button>().enabled = false;
        }
        else if(playButton != null)
        {
            playButton.GetComponent<Button>().enabled = true;
        }
    }
}
