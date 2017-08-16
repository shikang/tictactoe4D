using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BGPartFish : MonoBehaviour
{
	float yPos;
	float speed;
	bool isLeft;
	Vector3 currPos;

	public int fishIndex;

	void Start()
	{
		gameObject.layer = 9;

		// Different fish swims at different height
		if(fishIndex != 3)
			yPos = (float)Random.Range(250, 1050);
		else
			yPos = (float)Random.Range(-1000, -650);

		isLeft = false;
		if(Random.Range(0, 2) == 0)
			isLeft = true;

		float xPos = isLeft ? -1220.0f : 1220.0f;
		transform.localPosition = new Vector3(xPos, yPos);
		transform.localScale *= 5.0f;

		// Set speed, and invert speed if it goes from left to right
		// Flip the image as well
		speed = Random.Range(40.0f, 100.0f);
		if(isLeft)
		{
			speed = speed * -2.0f;
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}

		// Fish sprites do not have same size, so need some adjustements.
		if(fishIndex == 0)
		{
			transform.localScale *= Random.Range(0.8f, 1.5f);
		}
		else if(fishIndex == 3)
		{
			transform.localScale *= Random.Range(0.8f, 1.5f);

			Color tmp = GetComponent<Image>().color;
			tmp.a = 1.0f;
			GetComponent<Image>().color = tmp;
		}
	}

	void Update () 
	{
		currPos = transform.localPosition;
		currPos.x -= speed * Time.deltaTime;
		transform.localPosition = currPos;

		if( (isLeft && transform.localPosition.x > 1220.0f) ||
			(!isLeft && transform.localPosition.x < -1220.0f) )
		{
			BGManager.Instance.currNoofParts -= 1;
			Destroy(gameObject);
		}
	}
}
