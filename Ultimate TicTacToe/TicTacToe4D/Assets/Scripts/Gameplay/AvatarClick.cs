using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvatarClick : MonoBehaviour
{
	public int ID;
	public int btnType;

	void Start()
	{
	}

	void Update()
	{
	}

	public void OnClickAvatar()
	{
		// Avatar Page
		if(btnType == 1)
			AvatarHandler.Instance.SetAvatarIcon(ID);

		// Buy Page
		else if(btnType == 2)
		{
			if( !IconManager.Instance.GetIsUnlocked(ID) )
				GameObject.FindGameObjectWithTag("Gacha").GetComponent<GachaScript>().SetBuyIcon(ID);
		}
	}

	public void SetAvatar(int i, int type)
	{
		ID = i;
		btnType = type;
	}
}

