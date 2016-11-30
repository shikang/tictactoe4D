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
	public GameObject scrollParent;

	public GameObject avatarLocalPlay1;
	public GameObject avatarLocalPlay2;

	public Text avatarLocalPlay1Text;
	public Text avatarLocalPlay2Text;

	GameObject [] avatarArray;
	GameObject [] allFrames;
	bool	   [] isUnlocked;

	public int avatarState;
	int noofAvatars;
	int avatarRows;
	int avatarColumns;

	string currAvatarName;
	string localAvatarName_P1;
	string localAvatarName_P2;

	Vector2 startPos;
	Vector2 avatarGap;

	void Start()
	{
		noofAvatars = 60;
		avatarRows = 5;
		avatarColumns = 5;

		startPos = new Vector3(-48.0f, 45.0f, 0.0f);
		avatarGap = new Vector3(25.0f, 25.0f, 0.0f);

		avatarArray = new GameObject[noofAvatars];
		allFrames = new GameObject[noofAvatars];
		isUnlocked = new bool[noofAvatars];

		int currRowCount = 0;
		for(int i = 0; i < noofAvatars; ++i)
		{
			avatarArray[i] = Instantiate(avatarPrefab);
			avatarArray[i].transform.SetParent(avatarParent.transform);
			avatarArray[i].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Global").GetComponent<IconManager>().IconList[(int)Defines.ICONS.LOCKED];
			avatarArray[i].GetComponent<AvatarClick>().SetAvatarID(i);
			
			allFrames[i] = Instantiate(framePrefab);
			allFrames[i].transform.SetParent(frameParent.transform);

			Vector3 temp = avatarArray[i].transform.localPosition;
			temp.x = startPos.x + (avatarGap.x*(i%avatarColumns));
			temp.y = startPos.y - (avatarGap.y*(i/avatarColumns));
			avatarArray[i].GetComponent<RectTransform>().localPosition = temp;
			allFrames[i].GetComponent<RectTransform>().localPosition = temp;

			isUnlocked[i] = false;
		}

		// Set the current avatar to blue.
		currAvatar.GetComponent<Image>().color = Defines.P1_ICON_COLOR;

		// The Local Multiplay Icon default colors.
		avatarLocalPlay1.GetComponent<Image>().color = Defines.P1_ICON_COLOR;
		avatarLocalPlay2.GetComponent<Image>().color = Defines.P2_ICON_COLOR;

		// Unlock the first 3 avatars
		UnlockAvatar(0);
		UnlockAvatar(1);
		UnlockAvatar(2);
	}

	void Update()
	{
	}

	void UnlockAvatar(int i)
	{
		isUnlocked[i] = true;
		avatarArray[i].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Global").GetComponent<IconManager>().IconList[(i%7)+3];
	}

	void UnlockAllAvatars()
	{
		for (int i = 0; i < noofAvatars; ++i)
		{
			avatarArray[i].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Global").GetComponent<IconManager>().IconList[(i%7)+3];
		}
	}

	void UpdateUnlockedAvatarsStatus()
	{
		for (int i = 0; i < noofAvatars; ++i)
		{
			if(isUnlocked[i])
				avatarArray[i].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Global").GetComponent<IconManager>().IconList[(i%7)+3];
			else
				avatarArray[i].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Global").GetComponent<IconManager>().IconList[(int)Defines.ICONS.LOCKED];
		}
	}

	void ChangeAllAvatarColor(Color newColor)
	{
		for (int i = 0; i < noofAvatars; ++i)
		{
			avatarArray[i].GetComponent<Image>().color = newColor;
		}
	}

	public void OnClickLocalPlayIcon1()
	{
		GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState = 1;
		ChangeAllAvatarColor(Defines.P1_ICON_COLOR);
	}

	public void OnClickLocalPlayIcon2()
	{
		GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState = 2;
		ChangeAllAvatarColor(Defines.P2_ICON_COLOR);
	}
		
	public void SetAvatarIcon(int i)
	{
		if(isUnlocked[i])
		{
			if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 1)
				avatarLocalPlay1.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;

			else if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 2)
				avatarLocalPlay2.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;

			else if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 3)
				currAvatar.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;
		}
	}

	public void SetAvatarName()
	{
		if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 1)
			localAvatarName_P1 = avatarLocalPlay1Text.text;

		else if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 2)
			localAvatarName_P2 = avatarLocalPlay2Text.text;

		else if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 3)
			currAvatarName = currAvatarText.text;
	}
}