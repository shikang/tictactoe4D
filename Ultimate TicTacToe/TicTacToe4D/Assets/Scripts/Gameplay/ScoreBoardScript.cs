using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreBoardScript : MonoBehaviour
{
	public int [] scores;

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		scores = new int[3];		// 0 = Draws, 1 = P1 Wins, 2 = P2 Wins
		scores[0] = scores[1] = scores[2] = 0;
	}

	void Update()
	{
		if(SceneManager.GetActiveScene().name == "GameScene")
		{
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUIScoreDraw.GetComponent<Text>().text = "" + scores[0];
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUIScoreP1.GetComponent<Text>().text = "" + scores[1];
			GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManagerScript>().GUIScoreP2.GetComponent<Text>().text = "" + scores[2];
		}
	}
}
