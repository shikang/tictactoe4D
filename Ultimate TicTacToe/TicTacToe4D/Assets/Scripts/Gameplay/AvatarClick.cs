using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvatarClick : MonoBehaviour
{
	public int ID;

	void Start()
	{
	}

	void Update()
	{
	}

	public void OnClickAvatar()
	{
		GameObject.FindGameObjectWithTag("AvatarHandler").GetComponent<AvatarHandler>().SetAvatarIcon(ID);
	}

	public void SetAvatarID(int i)
	{
		ID = i;
	}
}

