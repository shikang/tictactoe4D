﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScript : MonoBehaviour
{
	public GameObject WWP;

	void Start ()
	{
	}

	void Update ()
	{
		if(WWP.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EndAnim"))
			SceneManager.LoadScene("MainMenu");
	}
}
