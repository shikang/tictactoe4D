using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarHandler : MonoBehaviour
{
	public GameObject currAvatar;
	public GameObject avatarPrefab;
	public GameObject framePrefab;
	public GameObject avatarParent;
	public GameObject frameParent;
	public GameObject scrollParent;

	public GameObject avatarLocalPlay1;
	public GameObject avatarLocalPlay2;

	GameObject [] avatarArray;
	GameObject [] allFrames;

	public int avatarState;
	int noofAvatars;
	int avatarRows;
	int avatarColumns;

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

		int currRowCount = 0;
		for(int i = 0; i < noofAvatars; ++i)
		{
			avatarArray[i] = Instantiate(avatarPrefab);
			avatarArray[i].transform.SetParent(avatarParent.transform);
			avatarArray[i].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Global").GetComponent<IconManager>().IconList[(i%7)+2];
			avatarArray[i].GetComponent<AvatarClick>().SetAvatarID(i);
			
			allFrames[i] = Instantiate(framePrefab);
			allFrames[i].transform.SetParent(frameParent.transform);

			Vector3 temp = avatarArray[i].transform.localPosition;
			temp.x = startPos.x + (avatarGap.x*(i%avatarColumns));
			temp.y = startPos.y - (avatarGap.y*(i/avatarColumns));
			avatarArray[i].GetComponent<RectTransform>().localPosition = temp;
			allFrames[i].GetComponent<RectTransform>().localPosition = temp;
		}

		// Set the current avatar to blue.
		currAvatar.GetComponent<Image>().color = Defines.P1_ICON_COLOR;

		// The Local Multiplay Icon default colors.
		avatarLocalPlay1.GetComponent<Image>().color = Defines.P1_ICON_COLOR;
		avatarLocalPlay2.GetComponent<Image>().color = Defines.P2_ICON_COLOR;
	}

	void Update()
	{
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
		if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 1)
			avatarLocalPlay1.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;

		else if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 2)
			avatarLocalPlay2.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;

		else if(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalScript>().avatarState == 3)
			currAvatar.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;
	}
}