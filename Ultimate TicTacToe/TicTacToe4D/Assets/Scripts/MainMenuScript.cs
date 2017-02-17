using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
	public GameObject InputP1;
	public GameObject InputP2;

	public GameObject MainMenuScreen;
	public GameObject StartGameLocalMultiplayerScreen;
	public GameObject StartGameNetworkedScreen;
	public GameObject OptionsScreen;
	public GameObject HowToPlayScreen;
	public GameObject AvatarScreen;
	public GameObject GachaScreen;

	public GameObject SettingsScreen;
	public GameObject CreditsPage;
	public GameObject HowToPlayPage;

	public GameObject UpdateText;
	public GameObject PasswordText;
	public GameObject PasswordField;
	public GameObject SearchBtn;

	public GameObject FindFriendGrey;
	public GameObject JoinPublicGrey;
	public GameObject SearchGrey;

	public GameObject Avatar;
	public GameObject Settings;

	public GameObject FindFriendButton;
	public GameObject JoinPublicButton;
	public GameObject PlayerNameInput;
	public GameObject PlayerIconInput;
	public GameObject RoomNameInput;

	public GameObject avatarObject;

	public GameObject PlayerIcon;
	public int screenState;

	GameObject currScreen;
	GameObject nextScreen;
	public bool moveScreen;
	int moveDirection;
	float screenMaxPosX;
	float screenMoveSpeed;

	void Start ()
	{
		screenMaxPosX = 1130.0f;
		screenMoveSpeed = 3500.0f;

		screenState = 0;
		DisplayScreen(screenState);
		currScreen = MainMenuScreen;
		//PlayerIcon.GetComponent<Image>().color = Defines.ICON_COLOR_P1;
	}

	void Update ()
	{
		/*if(screenState == 1)
		{
			foreach(Transform curr in InputP1.GetComponentsInChildren<Transform>())
			{
				if(curr.gameObject.GetComponent<Text>())
					GlobalScript.Instance.nameP1 = curr.gameObject.GetComponent<Text>().text;
			}
			foreach(Transform curr in InputP2.GetComponentsInChildren<Transform>())
			{
				if(curr.gameObject.GetComponent<Text>())
					GlobalScript.Instance.nameP2 = curr.gameObject.GetComponent<Text>().text;
			}
		}*/

		if(moveScreen)
			UpdateScreenPos();

	}

	// Change Screen is the animation; once done, then this is called to hide unwanted screens
	public void DisplayScreen(int _changeTo)
	{
		MainMenuScreen.SetActive(false);
		StartGameLocalMultiplayerScreen.SetActive(false);
		StartGameNetworkedScreen.SetActive(false);
		SettingsScreen.SetActive(false);
		AvatarScreen.SetActive(false);
		GachaScreen.SetActive(false);
		avatarObject.SetActive(false);

		//OptionsScreen.SetActive(false);
		//HowToPlayScreen.SetActive(false);
		//CreditScreen.SetActive(false);

		switch(screenState)
		{
		case 0:
			MainMenuScreen.SetActive(true);
			break;

		case 1:
		// Local Multiplayer
			StartGameLocalMultiplayerScreen.SetActive(true);
			avatarObject.SetActive(true);
			//GlobalScript.Instance.gameMode = 2;
			break;

		case 2:
		// Network
			StartGameNetworkedScreen.SetActive(true);
			UpdateText.SetActive(false);
			PasswordText.SetActive(false);
			PasswordField.SetActive(false);
			SearchBtn.SetActive(false);
			JoinPublicGrey.SetActive(false);
			FindFriendGrey.SetActive(false);
			SearchGrey.SetActive(false);
			FindFriendButton.SetActive(true);
			JoinPublicButton.SetActive(true);
			//PlayerNameInput.GetComponent<InputField>().enabled = true;
			//PlayerIconInput.GetComponent<Button>().interactable = true;
			//Color iconColor = PlayerIconInput.GetComponent<Image>().color;
			//iconColor.a = 1.0f;
			//PlayerIconInput.GetComponent<Image>().color = iconColor;
			RoomNameInput.GetComponent<InputField>().enabled = true;
			break;

		case 3:
			GachaScreen.SetActive(true);
			break;

		case 4:
			AvatarScreen.SetActive(true);
			avatarObject.SetActive(true);
			break;

		default:
			break;
		}
	}

	public void ChangeScreen(int _nextScreen, bool _moveBack = false)
	{
		moveDirection = _moveBack ? -1 : 1;
		screenState = _nextScreen;
		nextScreen = GetScreen(screenState);
		nextScreen.SetActive(true);
		nextScreen.transform.localPosition = new Vector3(screenMaxPosX * moveDirection, 0.0f, 0.0f);
		moveScreen = true;

		if(_moveBack)
			avatarObject.SetActive(false);
	}

	void UpdateScreenPos()
	{
		Vector3 temp = currScreen.transform.localPosition;
		temp.x -= screenMoveSpeed * moveDirection * Time.deltaTime;
		currScreen.transform.localPosition = temp;

		temp = nextScreen.transform.localPosition;
		temp.x -= screenMoveSpeed * moveDirection * Time.deltaTime;
		nextScreen.transform.localPosition = temp;

		if( (moveDirection == 1 && temp.x <= 0.0f) || (moveDirection == -1 && temp.x >= 0) )
		{
			nextScreen.transform.localPosition = Vector3.zero;
			moveScreen = false;
			currScreen = nextScreen;
			DisplayScreen(screenState);
		}
	}

	GameObject GetScreen(int i)
	{
		switch(i)
		{
		case 0:	return MainMenuScreen;
		case 1:	return StartGameLocalMultiplayerScreen;
		case 2: return StartGameNetworkedScreen;
		case 3: return GachaScreen;
		case 4: return AvatarScreen;
		default: return null;
		}
	}
}