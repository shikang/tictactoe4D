using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScript : MonoBehaviour
{
	public GameObject WWP;

	void Start ()
	{
	}

	void Update ()
	{
		if(WWP.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EndAnim") || Input.touchCount > 0)
			SceneManager.LoadScene("MainMenu");
	}
}
