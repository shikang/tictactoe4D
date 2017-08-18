using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BGPartSheep : MonoBehaviour
{
	float timerNextMove;

	void Start()
	{
		int yPos = Random.Range(-865, -790);
		int xPos = Random.Range(-400, 500);
		transform.localPosition = new Vector3((float)xPos, yPos);

		timerNextMove = 0.0f;
	}

	void Update()
	{
		timerNextMove -= Time.deltaTime;

		if(timerNextMove <= 0.0f)
		{
			if(Random.Range(1, 3) == 1)
				GoLeft();
			else
				GoRight();

			timerNextMove = Random.Range(9.0f, 20.0f);
		}
	}

	void GoLeft()
	{
		if(GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			GetComponentInChildren<Transform>().localScale = new Vector3(5.65f, 5.65f, 5.65f);
			GetComponentInChildren<Animator>().SetInteger("animType", 1);
			GetComponentInChildren<Animator>().SetTrigger("canPlay");
			transform.localPosition += new Vector3(6.0f, 0.0f, 0.0f);
		}
	}

	void GoRight()
	{
		if(GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			GetComponentInChildren<Transform>().localScale = new Vector3(-5.65f, 5.65f, 5.65f);
			GetComponentInChildren<Animator>().SetInteger("animType", 2);
			GetComponentInChildren<Animator>().SetTrigger("canPlay");
			transform.localPosition += new Vector3(-6.0f, 0.0f, 0.0f);
		}
	}
}
