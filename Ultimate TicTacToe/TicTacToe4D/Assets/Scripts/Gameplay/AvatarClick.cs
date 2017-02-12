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
			GameObject gacha = GameObject.FindGameObjectWithTag("Gacha");

			Transform buyPage = gacha.transform.FindChild("BuyPage");
			Transform confirmBuy = buyPage.FindChild("ConfirmBuy");
			Button confirmBuyButton = confirmBuy.GetComponent<Button>();
			Text confirmBuyText = confirmBuy.GetComponentInChildren<Text>();

			if (!IconManager.Instance.GetIsUnlocked(ID))
			{
				gacha.GetComponent<GachaScript>().SetBuyIcon(ID);
				confirmBuyButton.enabled = true;

				string productIdentifier = InAppProductList.GetProductIdentifier( InAppProductList.ProductType.AVATAR, ID );
				if (InAppProductList.Instance.NonConsumableList.ContainsKey(productIdentifier))
				{
					confirmBuyText.text = "Buy for " + InAppProductList.Instance.NonConsumableList[productIdentifier].m_sPrice + "!";
				}
			}
			else
			{
				confirmBuyText.text = "Already bought!";
				confirmBuyButton.enabled = false;
			}
		}
	}

	public void SetAvatar(int i, int type)
	{
		ID = i;
		btnType = type;
	}
}

