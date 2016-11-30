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

	GameObject [] avatarArray;
	GameObject [] allFrames;
	float iconSize;

	int noofAvatars;
	int avatarRows;
	int avatarColumns;
	int rowsPerPage;

	Vector2 startPos;
	Vector2 avatarGap;
	Vector2 initalMovePos;
	Vector2 newMovePos;

	float vTop;
	float vBot;

	void Start()
	{
		noofAvatars = 60;
		avatarRows = 5;
		avatarColumns = 5;
		rowsPerPage = 4;

		startPos = new Vector3(-55.0f, 10.0f, 0.0f);
		avatarGap = new Vector3(28.0f, 28.0f, 0.0f);

		vTop = 100.0f;
		vBot = 100.0f;

		avatarArray = new GameObject[noofAvatars];
		allFrames = new GameObject[noofAvatars];
		iconSize = 0.75f;

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
			temp.x = startPos.x + (avatarGap.x*(i%5));
			temp.y = startPos.y - (avatarGap.y*(i/5));
			avatarArray[i].GetComponent<RectTransform>().localPosition = temp;
			allFrames[i].GetComponent<RectTransform>().localPosition = temp;
		}

		// Set the current avatar to blue.
		currAvatar.GetComponent<Image>().color = Defines.P1_ICON_COLOR;

	}

	void Update()
	{
		// Moving
		/*if(Input.touchCount == 1)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				initalMovePos = Input.GetTouch(0).position;
			}

			else if(Input.GetTouch(0).phase == TouchPhase.Stationary)
			{
				initalMovePos = newMovePos;
			}

			else if(Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				newMovePos = Input.GetTouch(0).position;
				Vector2 finalPos = newMovePos - initalMovePos;

				float vPos = 0.0f;
				if(finalPos.y < 0.0f)
					vPos -= 0.4f;
				if(finalPos.y > 0.0f)
					vPos += 0.4f;

				for(int i = 0; i < noofAvatars; ++i)
				{
					avatarArray[i].GetComponent<RectTransform>().position += new Vector3(0, vPos, 0);
					allFrames[i].GetComponent<RectTransform>().position += new Vector3(0, vPos, 0);
				}
			}
		}*/
	}
		
	public void SetAvatarIcon(int i)
	{
		currAvatar.GetComponent<Image>().sprite = avatarArray[i].GetComponent<Image>().sprite;
	}
}