using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarHandler : MonoBehaviour
{
	public GameObject currAvatar;
	public Text		  currAvatarText;
	public GameObject avatarPrefab;
	public GameObject framePrefab;
	public GameObject avatarParent;
	public GameObject frameParent;
	public GameObject scrollGrandparent;
	public GameObject scrollParent;

	public Sprite Frame1;
	public Sprite Frame2;

	public GameObject avatarLocalPlay1;
	public GameObject avatarLocalPlay2;

	public Text avatarLocalPlay1Text;
	public Text avatarLocalPlay2Text;

	public GameObject [] avatarArray;
	GameObject [] allFrames;

	// Buy Page
	public GameObject [] buyArray;
	GameObject [] buyAllFrames;
	public GameObject buyParent;
	public GameObject buyFrameParent;
	public GameObject buyScrollParent;
	int [] buyID;

	public int avatarState;
	int noofAvatars;
	int avatarColumns;

	string currAvatarName;
	public string localAvatarName_P1;
	public string localAvatarName_P2;
	public int localAvatar_P1;
	public int localAvatar_P2;

	Vector2 startPos;
	Vector2 avatarGap;

	public int[] BuyID
	{
		get { return buyID; }
	}

	// Singleton pattern
	static AvatarHandler instance;
	public static AvatarHandler Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
			throw new System.Exception("You have more than 1 AvatarHandler in the scene.");

		// Initialize the static class variables
		instance = this;
	}

	void Start()
	{
		// hack!
		SaveLoad.Load();
		for (int i = 0; i < GameData.current.icons.Count; ++i)
		{
			Defines.ICONS icon = GameData.current.icons[i];
			IconManager.Instance.SetUnlocked((int)icon, true);
		}

		noofAvatars = (int)Defines.ICONS.TOTAL;
		avatarColumns = Defines.Avatar_NoofColumns;

		startPos = new Vector3(-55.0f, 48.0f, 0.0f);
		avatarGap = new Vector3(27.0f, 27.0f, 0.0f);

		avatarArray = new GameObject[noofAvatars];
		allFrames = new GameObject[noofAvatars];

		// Avatars
		int currRowCount = 0;
		for(int i = Defines.Avatar_FirstIcon; i < noofAvatars; ++i)
		{
			avatarArray[i] = Instantiate(avatarPrefab);
			avatarArray[i].transform.SetParent(avatarParent.transform, false);
			avatarArray[i].GetComponent<Image>().sprite = GetIconManager().GetIcon(Defines.ICONS.LOCKED);
			avatarArray[i].GetComponent<AvatarClick>().SetAvatar(i, 1);

			allFrames[i] = Instantiate(framePrefab);
			//allFrames[i].GetComponent<Image>().color = Defines.ICON_COLOR_P1;
			allFrames[i].transform.SetParent(frameParent.transform, false);

			Vector3 temp = avatarArray[i].transform.localPosition;
			temp.x = startPos.x + (avatarGap.x *( (i-Defines.Avatar_FirstIcon) % avatarColumns));
			temp.y = startPos.y - (avatarGap.y *( (i-Defines.Avatar_FirstIcon) / avatarColumns));
			avatarArray[i].GetComponent<RectTransform>().localPosition = temp;
			allFrames[i].GetComponent<RectTransform>().localPosition = temp;
		}

		// Buyable Avatar Displays
		buyArray = new GameObject[GetIconManager().GetNoofBuyableIcons()];
		buyAllFrames = new GameObject[buyArray.Length];
		buyID = new int[buyArray.Length];

		currRowCount = 0;
		int count = 0;
		for(int i = Defines.Avatar_FirstIcon; i < (int)Defines.ICONS.TOTAL; ++i)
		{
			if(GetIconManager().GetIsBuy(i))
			{
				buyArray[count] = Instantiate(avatarPrefab);
				buyArray[count].transform.SetParent(buyParent.transform, false);
				buyArray[count].GetComponent<Image>().sprite = GetIconManager().GetIcon(i);
				buyArray[count].GetComponent<AvatarClick>().SetAvatar(i, 2);

				buyAllFrames[count] = Instantiate(framePrefab);
				//buyAllFrames[count].GetComponent<Image>().color = Defines.ICON_COLOR_P1;
				buyAllFrames[count].transform.SetParent(buyFrameParent.transform, false);

				buyID[count] = i;

				Vector3 temp = buyArray[count].transform.localPosition;
				temp.x = startPos.x + (avatarGap.x *( count % avatarColumns));
				temp.y = startPos.y - (avatarGap.y *( count / avatarColumns));
				buyArray[count].GetComponent<RectTransform>().localPosition = temp;
				buyAllFrames[count].GetComponent<RectTransform>().localPosition = temp;
				++count;
			}
		}

		// Set the current avatar to blue.
		//currAvatar.GetComponent<Image>().color = Defines.ICON_COLOR_P1;

		localAvatar_P1 = (int)Defines.ICON_DEFAULT_P1;
		localAvatar_P2 = (int)Defines.ICON_DEFAULT_P2;
		avatarLocalPlay1.GetComponent<Image>().sprite = IconManager.Instance.GetIcon(localAvatar_P1);
		avatarLocalPlay2.GetComponent<Image>().sprite = IconManager.Instance.GetIcon(localAvatar_P2);
	}

	void Update()
	{
	}

	public void UnlockAvatar(int i)
	{
		if(i >= noofAvatars)
		{
			Debug.Log("OUT OF RANGE!");
			return;
		}
		GetIconManager().SetUnlocked(i, true);
		avatarArray[i].GetComponent<Image>().sprite = GetIconManager().GetIcon(i);
		UpdateUnlockedAvatarsStatus();
	}

	void UnlockAllAvatars()
	{
		for (int i = Defines.Avatar_FirstIcon; i < noofAvatars; ++i)
		{
			GetIconManager().SetUnlocked(i, true);
			avatarArray[i].GetComponent<Image>().sprite = GetIconManager().GetIcon(i);
		}
		UpdateUnlockedAvatarsStatus();
	}

	public void UpdateUnlockedAvatarsStatus()
	{
		for (int i = Defines.Avatar_FirstIcon; i < noofAvatars; ++i)
		{
			if(GetIconManager().GetIsUnlocked(i))
			{
				avatarArray[i].GetComponent<Image>().sprite = GetIconManager().GetIcon(i);
				allFrames[i].GetComponent<Image>().color = Defines.ICON_COLOR_P1;
			}
			else
			{
				avatarArray[i].GetComponent<Image>().sprite = GetIconManager().GetIcon(Defines.ICONS.LOCKED);
				allFrames[i].GetComponent<Image>().color = Defines.ICON_COLOR_GREY;
			}
		}

		for (int i = 0; i < buyArray.Length; ++i)
		{
			if( GetIconManager().GetIsUnlocked(buyID[i]) )
				buyAllFrames[i].GetComponent<Image>().color = Defines.ICON_COLOR_GREY;
			else
				buyAllFrames[i].GetComponent<Image>().color = Defines.ICON_COLOR_P1;
		}
	}

	void ChangeAllAvatarColor(int player)
	{
		Color curr = Defines.ICON_COLOR_P1;
		if(player == 2)
			curr = Defines.ICON_COLOR_P2;

		for (int i = Defines.Avatar_FirstIcon; i < noofAvatars; ++i)
		{
			allFrames[i].GetComponent<Image>().color = curr;
		}
	}

	public bool UnlockedAll()
	{
		for(int i = Defines.Avatar_FirstIcon; i < noofAvatars; ++i)
		{
			if(GetIconManager().GetIsUnlocked(i) == false)
				return false;
		}
		return true;
	}

	public int GetNoofAvatars()
	{
		return noofAvatars;
	}

	public void OnClickLocalPlayIcon1(bool playSound = true)
	{
		GlobalScript.Instance.avatarState = 1;
		ChangeAllAvatarColor(1);

		if(playSound)
			AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}

	public void OnClickLocalPlayIcon2()
	{
		GlobalScript.Instance.avatarState = 2;
		ChangeAllAvatarColor(2);
		AudioManager.Instance.PlaySoundEvent(SOUNDID.CLICK);
	}
		
	public void SetAvatarIcon(int i)
	{
		if(GetIconManager().GetIsUnlocked(i))
		{
			if (GlobalScript.Instance.avatarState == 1)
			{
				avatarLocalPlay1.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;
				localAvatar_P1 = i;
			}
			else if (GlobalScript.Instance.avatarState == 2)
			{
				avatarLocalPlay2.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;
				localAvatar_P2 = i;
			}
			else if (GlobalScript.Instance.avatarState == 3)
			{
				currAvatar.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;

				SaveLoad.Load();
				GameData.current.avatarIcon = i;
				SaveLoad.Save();
			}
		}
	}

	public void SetAvatarName()
	{
		if (GlobalScript.Instance.avatarState == 1)
		{
			SetAvatarName1( avatarLocalPlay1Text.text );
		}
		else if (GlobalScript.Instance.avatarState == 2)
		{
			SetAvatarName2( avatarLocalPlay2Text.text );
		}
		else if (GlobalScript.Instance.avatarState == 3)
		{
			SetAvatarName3( currAvatarText.text );
		}
	}

	public void SetLocalAvatarName(int player)
	{
		if (player == 1)
		{
			SetAvatarName1(avatarLocalPlay1Text.text);
		}
		else if (player == 2)
		{
			SetAvatarName2(avatarLocalPlay2Text.text);
		}
	}

	public void SetAvatarName1(string name)
	{
		localAvatarName_P1 = name;
	}

	public void SetAvatarName2(string name)
	{
		localAvatarName_P2 = name;
	}

	public void SetAvatarName3(string name)
	{
		currAvatarName = name;

		SaveLoad.Load();
		GameData.current.avatarName = currAvatarName;
		SaveLoad.Save();
	}

	public void SetMyAvatarName(string name)
	{
		currAvatarName = (name == null) ? "" : name;
		//currAvatarText.text = currAvatarName;
		currAvatarText.transform.parent.GetComponent<InputField>().text = currAvatarName;
	}

	public void SetMyAvatarIcon(int icon)
	{
		currAvatar.GetComponent<Image>().sprite = avatarArray[icon].GetComponent<Image>().sprite;
	}

	public IconManager GetIconManager()
	{
		return IconManager.Instance;
	}
}