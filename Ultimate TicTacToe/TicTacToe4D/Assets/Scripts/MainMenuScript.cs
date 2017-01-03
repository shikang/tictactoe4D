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

	public GameObject PlayerIcon;

	public int screenState;

	void Start ()
	{
		GoToScreen(0);
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

	}

	public void GoToScreen(int _changeTo)
	{
		screenState = _changeTo;

		MainMenuScreen.SetActive(false);
		StartGameLocalMultiplayerScreen.SetActive(false);
		StartGameNetworkedScreen.SetActive(false);
		SettingsScreen.SetActive(false);
		AvatarScreen.SetActive(false);
		GachaScreen.SetActive(false);

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
			break;

		default:
			break;
		}
	}
}
