using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScript : MonoBehaviour
{
	float splashTime;
	public GameObject WWP;

	void Start ()
	{
		splashTime = 2.8f;
	}

	void Update ()
	{
		Color tempColor = WWP.GetComponent<SpriteRenderer>().color;
		splashTime -= Time.deltaTime;
		if(splashTime <= 1.0f)
			tempColor.a -= 1.5f * Time.deltaTime;
		if(splashTime <= 0.5f)
			SceneManager.LoadScene("MainMenu");

		WWP.GetComponent<SpriteRenderer>().color = tempColor;
	}
}
