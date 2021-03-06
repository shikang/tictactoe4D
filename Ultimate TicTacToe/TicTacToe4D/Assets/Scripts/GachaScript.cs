﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleAndroidNotifications;

public class GachaScript : MonoBehaviour
{
	public GameObject currIcon;
	public GameObject IconFX;
	int unlockedIcon;

	public GameObject gachaStartButton;
	public GameObject BuyIcon;
	public GameObject BuyIconFX;
	int BuyID;

	public GameObject moneyText;
	public GameObject gachaInfoText;
	public GameObject gachaInfoSpecial;

	int [] randomList;
	float [] changeTimer;

	int currCounter;
	int noofChanges;
	float currTime;

	bool isGachaing;
	bool isAnimating;
	bool isAnimatingBuy;

	public GameObject GreyBG;
	int GreyFadeState;

	public GameObject GachaPage;
	public GameObject BuyPage;

	bool isSpecialTextGolding;

	// Free Roll Stuff
	public GameObject freeRollButton;
	public GameObject freeRollButtonGrey;

	// Singleton pattern
	static GachaScript instance;
	public static GachaScript Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
		{
			//throw new System.Exception("You have more than 1 GachaScript in the scene.");
			Destroy(this);
			return;
		}

		// Initialize the static class variables
		instance = this;
	}

	void Start()
	{
		currIcon.GetComponent<Image>().sprite =
			IconManager.Instance.GetIcon(Defines.ICONS.LOCKED);
		//currIcon.GetComponent<Image>().color = IconFX.GetComponent<Image>().color =
		//BuyIcon.GetComponent<Image>().color = BuyIconFX.GetComponent<Image>().color = Defines.ICON_COLOR_P1;

		IconFX.SetActive(false);
		BuyIcon.SetActive(false);
		BuyIconFX.SetActive(false);

		noofChanges = 20;
		unlockedIcon = 0;
		randomList = new int[noofChanges];

		isGachaing = false;
		isAnimating = false;

		InitTiming();
		SetGreyBG(false);
		ResetGachaText();

		UpdateGachaButton();
		ResetSpecialText();

		moneyText.GetComponent<Text>().text = GameData.current.coin.ToString();
	}

	void Update()
	{
		if(isGachaing)
			UpdateGacha();

		if(isAnimating)
		{
			UpdateAnimation(IconFX);
		}
		else
		{
			IconFX.GetComponent<Image>().color = Color.white;
			IconFX.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
		}

		if(isAnimatingBuy)
		{
			UpdateAnimation(BuyIconFX);
		}
		else
		{
			BuyIconFX.GetComponent<Image>().color = Color.white;
			BuyIconFX.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
		}

		UpdateGreyBG();
		UpdateFreeRoll();

		Color tmp = gachaInfoSpecial.GetComponent<Text>().color;
		if(isSpecialTextGolding)
		{
			if(tmp.r > 0.7f)
			{
				tmp.r -= 0.33f * Time.deltaTime;
				tmp.g -= 0.33f * Time.deltaTime;
				tmp.b -= 1.0f * Time.deltaTime;
			}
			else
			{
				isSpecialTextGolding = false;
			}
		}
		else
		{
			if(tmp.r < 1.0f)
			{
				tmp.r += 0.33f * Time.deltaTime;
				tmp.g += 0.33f * Time.deltaTime;
				tmp.b += 1.0f * Time.deltaTime;
			}
			else
			{
				isSpecialTextGolding = true;
			}
		}
		gachaInfoSpecial.GetComponent<Text>().color = tmp;

		if(currIcon.GetActive() && !currIcon.GetComponent<Animator>().GetBool("isRot"))
		{
			Quaternion temp = new Quaternion(0, 0, 0, 0);
			currIcon.GetComponent<RectTransform>().rotation = temp;
		}
	}

	void InitTiming()
	{
		changeTimer = new float[noofChanges];
		changeTimer[0] = 0.08f;
		changeTimer[1] = 0.09f;
		changeTimer[2] = 0.1f;
		changeTimer[3] = 0.1f;
		changeTimer[4] = 0.12f;
		changeTimer[5] = 0.12f;
		changeTimer[6] = 0.12f;
		changeTimer[7] = 0.14f;
		changeTimer[8] = 0.15f;
		changeTimer[9] = 0.15f;
		changeTimer[10] = 0.18f;
		changeTimer[11] = 0.2f;
		changeTimer[12] = 0.22f;
		changeTimer[13] = 0.28f;
		changeTimer[14] = 0.4f;
		changeTimer[15] = 0.45f;
		changeTimer[16] = 0.5f;
		changeTimer[17] = 0.6f;
		changeTimer[18] = 0.7f;
		changeTimer[19] = 0.4f;
	}

	public void StartGacha(bool free = false)
	{
		if(GameData.current.coin >= Defines.GACHACOST || free)
		{
			ResetSpecialText();

			// Set Grey BG
			SetGreyBG(true);

			// Generate Random List
			if(AvatarHandler.Instance.UnlockedAll())
			{
				Debug.Log("Unlocked All!");
			}

			for (int i = 0; i < noofChanges; ++i)
			{
				int prev = 0;
				if(i > 0)
					prev = randomList[i-1];
				do
				{
					randomList[i] = IconManager.Instance.FindGachaIcon(UnityEngine.Random.Range(0, 99999));
					//randomList[i] = UnityEngine.Random.Range(Defines.Avatar_FirstIcon, AvatarHandler.Instance.GetNoofAvatars());
				}
				while (IconManager.Instance.GetIsBuy(randomList[i]) || randomList[i] == prev );
			}

			currTime = 0.0f;
			currCounter = -1;
			isGachaing = true;

			// Deduct Money
			if(!free)
				GameData.current.coin -= Defines.GACHACOST;
			moneyText.GetComponent<Text>().text = GameData.current.coin.ToString();
			UpdateGachaButton();

			// Unlock avatar
			int unlockIcon = randomList[noofChanges - 1];
			unlockedIcon = unlockIcon;
			//Debug.Log("Bought token: " + unlockIcon);

			gachaInfoText.SetActive(false);
			gachaInfoSpecial.SetActive(false);
			if(IconManager.Instance.GetIsUnlocked(unlockIcon))
			{
				gachaInfoText.GetComponent<Text>().text = "You already have this Tac!";
				gachaInfoSpecial.GetComponent<Text>().text = "";
			}
			else
			{
				if(((Defines.ICONS)unlockIcon).ToString().Substring(0, 1) == "R")
				{
					gachaInfoText.GetComponent<Text>().text = "";
					gachaInfoSpecial.GetComponent<Text>().text = "YOU GOT RARE " +  ((Defines.ICONS)unlockIcon).ToString().Substring(5) + " TAC!!";
				}
				else if(((Defines.ICONS)unlockIcon).ToString().Substring(0, 1) == "L")
				{
					gachaInfoText.GetComponent<Text>().text = "";
					gachaInfoSpecial.GetComponent<Text>().text = "YOU GOT LEGENDARY " +  ((Defines.ICONS)unlockIcon).ToString().Substring(5) + " TAC!!!";
				}
				else
				{
					gachaInfoSpecial.GetComponent<Text>().text = "";
					gachaInfoText.GetComponent<Text>().text = "You got a new " +  ((Defines.ICONS)unlockIcon).ToString().Substring(5) + " Tac!";
				}

				Analytics.CustomEvent("Unlocked_Tac", new Dictionary<string, object>
				{
					{((Defines.ICONS)unlockIcon).ToString().Substring(0, 4), 1}
				});
			}
				
			AvatarHandler.Instance.UnlockAvatar(unlockIcon);
			if(!GameData.current.icons.Contains((Defines.ICONS)unlockIcon))
			{
				GameData.current.icons.Add((Defines.ICONS)unlockIcon);
			}
			SaveLoad.Save();
		}
		else
		{
			Debug.Log("No Money!");
		}
	}

	void UpdateGacha()
	{
		currTime -= Time.deltaTime;
		if(currTime <= 0.0f)
		{
			// Reached the last icon
			if(currCounter == noofChanges-1)
			{
				isGachaing = false;

				// Starts FX
				isAnimating = true;
				IconFX.SetActive(true);
				IconFX.GetComponent<Image>().sprite = currIcon.GetComponent<Image>().sprite;

				// Reset Grey BG
				SetGreyBG(false);

				if(AudioManager.Instance)
					AudioManager.Instance.PlaySoundEvent(SOUNDID.GACHAEND);
			}

			// Keep looping icons
			else
			{
				currCounter += 1;
				currIcon.GetComponent<Image>().sprite =
					IconManager.Instance.GetIcon(randomList[currCounter]);
				currTime = changeTimer[currCounter];

				if(AudioManager.Instance)
					AudioManager.Instance.PlaySoundEvent(SOUNDID.GACHACOUNT);
			}
		}
	}

	void UpdateAnimation(GameObject curr)
	{
		curr.transform.localScale += new Vector3(0.008f, 0.008f, 0.008f);
		curr.GetComponent<Image>().color -= new Color(0.0f, 0.0f, 0.0f, 0.03f);

		if(curr.GetComponent<Image>().color.a <= 0f)
		{
			curr.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
			//curr.GetComponent<Image>().color = Defines.ICON_COLOR_P1;
			curr.SetActive(false);

			isAnimating = isAnimatingBuy = false;
			gachaInfoText.SetActive(true);
			gachaInfoSpecial.SetActive(true);

			if(((Defines.ICONS)unlockedIcon).ToString().Substring(0, 1) == "R" || ((Defines.ICONS)unlockedIcon).ToString().Substring(0, 1) == "L")
				currIcon.GetComponent<Animator>().SetBool("isRot", true);
		}
	}

	void UpdateGreyBG()
	{
		if(GreyFadeState == 1)
		{
			// Fade In
			if(GreyBG.GetComponent<Image>().color.a < 0.5f)
				GreyBG.GetComponent<Image>().color += new Color(0.0f, 0.0f, 0.0f, 0.05f);
			else
				GreyFadeState = 0;
		}
		else if(GreyFadeState == 2)
		{
			// Fade Out
			if(GreyBG.GetComponent<Image>().color.a > 0.0f)
				GreyBG.GetComponent<Image>().color -= new Color(0.0f, 0.0f, 0.0f, 0.05f);
			else
				GreyFadeState = 0;
		}
	}

	void UpdateFreeRoll()
	{
		if(CanFreeRoll())
		{
			freeRollButtonGrey.SetActive(false);
		}
		else
		{
			freeRollButtonGrey.SetActive(true);

			int hour  = GameData.current.nextFreeRollTime.Subtract(DateTime.Now).Hours;
			int min = GameData.current.nextFreeRollTime.Subtract(DateTime.Now).Minutes;
			int secs = GameData.current.nextFreeRollTime.Subtract(DateTime.Now).Seconds;

			freeRollButtonGrey.GetComponentInChildren<Text>().text = hour.ToString("00") + ":" + min.ToString("00") + ":" + secs.ToString("00") + " to free roll"; 
		}
	}

	void UpdateGachaButton()
	{
		gachaStartButton.GetComponent<Image>().color = Defines.ICON_COLOR_P1;
		gachaStartButton.GetComponentInChildren<Text>().text = "Roll for Tacs!";
		if(GameData.current.coin < Defines.GACHACOST)
		{
			gachaStartButton.GetComponent<Image>().color = Defines.ICON_COLOR_GREY;
			gachaStartButton.GetComponentInChildren<Text>().text = "Not Enough Coins!";
		}
	}

	public void SetGreyBG (bool setter)
	{
		GreyBG.SetActive(setter);
		if(setter)
			GreyFadeState = 1;
		else
			GreyFadeState = 2;
	}

	public void ResetSpecialText()
	{
		isSpecialTextGolding = true;
		gachaInfoSpecial.GetComponent<Text>().color = Color.white;
		gachaInfoSpecial.SetActive(false);
		gachaInfoText.SetActive(true);

		currIcon.GetComponent<Animator>().SetBool("isRot", false);
		Quaternion tmp = new Quaternion(0, 0, 0, 0);
		currIcon.GetComponent<RectTransform>().rotation = tmp;
		currIcon.GetComponent<Image>().sprite = IconManager.Instance.GetIcon(Defines.ICONS.LOCKED);
	}

	public void BuyButtonClick()
	{
		GameObject.FindGameObjectWithTag("MainMenuCanvas").GetComponent<MenuBtnScript>().currScreen = SCREENS.GACHA_MONEY;
		GachaPage.SetActive(false);
		BuyPage.SetActive(true);
		ResetSpecialText();

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void BackBuyButtonClick()
	{
		GameObject.FindGameObjectWithTag("MainMenuCanvas").GetComponent<MenuBtnScript>().currScreen = SCREENS.GACHA_POINTS;
		GachaPage.SetActive(true);
		BuyPage.SetActive(false);
		BuyIcon.SetActive(false);

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.BACK);
	}

	public void SetBuyIcon(int i)
	{
		BuyIcon.SetActive(true);
		BuyIcon.GetComponent<Image>().sprite = IconManager.Instance.GetIcon(i);
		BuyID = i;

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.AVATARSELECT);
	}

	public void BuyCurrentIcon()
	{
		if(IconManager.Instance.GetIsUnlocked(BuyID))
		{
			return;
		}

		if(BuyIcon.GetActive())
		{
			// Disable UI
			EnableBuyUI(false);

			// Then buy
			GameObject go = GameObject.FindGameObjectWithTag("IAPManager");
			InAppPurchaser purchaser = go.GetComponent<InAppPurchaser>();
			purchaser.BuyProduct(InAppProductList.ProductType.AVATAR, BuyID);

			Analytics.CustomEvent("PreBuy_Tac", new Dictionary<string, object>
			{
				{((Defines.ICONS)BuyID).ToString().Substring(0, 4), 1}
			});
		}
	}

	public void ProcessBuyIcon(int buyID)
	{
		if (buyID != BuyID)
		{
			Debug.Log("BuyID not match! | buyID: " + buyID + " | BuyID: " + BuyID);
		}
		else
		{
			Debug.Log("BuyID match! | buyID&BuyID: " + buyID);
		}

		AvatarHandler.Instance.UnlockAvatar(BuyID);
		if (!GameData.current.icons.Contains((Defines.ICONS)BuyID))
		{
			GameData.current.icons.Add((Defines.ICONS)BuyID);
		}
		SaveLoad.Save();

		BuyIconFX.SetActive(true);
		BuyIconFX.GetComponent<Image>().sprite = BuyIcon.GetComponent<Image>().sprite;
		isAnimatingBuy = true;

		if(AudioManager.Instance)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.ICON_BOUGHT);

		// Enable UI
		EnableBuyUI(true);

		// Update UI
		Transform confirmBuy = BuyPage.transform.FindChild("ConfirmBuy");
		Button confirmBuyButton = confirmBuy.GetComponent<Button>();
		Text confirmBuyText = confirmBuy.GetComponentInChildren<Text>();
		confirmBuyText.text = "Already bought!";
		confirmBuyButton.enabled = false;
	}

	public void EnableBuyUI(bool enable)
	{
		Transform back = BuyPage.transform.FindChild("Back");
		Transform confirmBuy = BuyPage.transform.FindChild("ConfirmBuy");

		GameObject backObject = back.gameObject;
		GameObject confirmBuyObject = confirmBuy.gameObject;

		Button backButton = backObject.GetComponent<Button>();
		Button confirmBuyButton = confirmBuyObject.GetComponent<Button>();

		backButton.enabled = enable;
		confirmBuyButton.enabled = enable;

		for ( int i = 0; i < AvatarHandler.Instance.buyArray.Length; ++i )
		{
			Button button = AvatarHandler.Instance.buyArray[i].GetComponent<Button>();
			button.enabled = enable;
		}
	}

	public bool CanFreeRoll()
	{
		if(DateTime.Now > GameData.current.nextFreeRollTime)
			return true;
		return false;
	}

	public void SetGacha()
	{
		GameData.current.nextFreeRollTime = DateTime.Now.AddSeconds(Defines.FREE_ROLL_TIMER);
		SaveLoad.Save();
	}

	public void ResetGachaText()
	{
		gachaInfoText.GetComponent<Text>().text = "Cost: 100 coins";
	}
}

