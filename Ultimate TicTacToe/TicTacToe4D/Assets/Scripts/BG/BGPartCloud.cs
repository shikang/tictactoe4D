using UnityEngine;
using System.Collections;

public class BGPartCloud : MonoBehaviour
{
	float yPos;
	float speed;
	bool isLeft;
	Vector3 currPos;

	void Start()
	{
		gameObject.layer = 9;

		yPos = (float)Random.Range(250, 1050);
		isLeft = false;

		if(Random.Range(0, 2) == 0)
			isLeft = true;

		float xPos = isLeft ? -950.0f : 950.0f;
		transform.localPosition = new Vector3(xPos, yPos);

		// Set speed, and invert speed if it goes from left to right
		speed = Random.Range(40.0f, 70.0f);
		if(isLeft)
			speed = speed * -2.0f;

		transform.localScale *= Random.Range(5.8f, 6.5f);
	}

	void Update () 
	{
		currPos = transform.localPosition;
		currPos.x -= speed * Time.deltaTime;
		transform.localPosition = currPos;

		if( (isLeft && transform.localPosition.x > 950.0f) ||
			(!isLeft && transform.localPosition.x < -950.0f) )
		{
			BGManager.Instance.currNoofParts -= 1;
			Destroy(gameObject);
		}
	}
}
