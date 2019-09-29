using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	//Singleton for UImanager
	private static UIManager instance;
	public static UIManager Instance()
	{
		if (instance == null) 
		{
			instance = FindObjectOfType<UIManager> ();
		}
		return instance;
	}
		
	[Header("GameObjects")]
	public GameObject mainMenuPanel;
	public GameObject buttonPanel;
	public GameObject all_level;

	public GameObject gameCanvas;
	public GameObject gamePanel;
	public GameObject popUp;

	public GameObject linkDotGO;
	public GameObject linkDotContainer;

	[Header("Buttons")]
	public Button playButton;
	public Button home;

	public Button refreshButton;//Its in the Game panel torefresh the current level
	public Button selectLevelButton;//Its in the Game panel to go back to select a different level

	[Header("Images")]
	public Image mask;

	[Header("Text")]
	public Text levelText;

	void Start()
	{
		Init ();

		playButton.onClick.RemoveAllListeners ();
		playButton.onClick.AddListener (() => {
				StartButtonAction ();
			});

		selectLevelButton.onClick.RemoveAllListeners ();
		selectLevelButton.onClick.AddListener (() => {
			mainMenuPanel.SetActive (true);
			});
		
		home.onClick.RemoveAllListeners ();
		home.onClick.AddListener (() => {
				HomeButtonAction ();
			});
	}

	public void StartButtonAction()
	{

		all_level.SetActive (true);

		if (GameData.instance.mode == 1) 
		{
			fadeIn ("levelMenu");
		} 
		else 
		{
			all_level.gameObject.SetActive (true);
		}
	}

	void fadeOut()
	{
		mask.gameObject.SetActive(true);

		mask.color = Color.black;
	}

	void fadeIn(string sceneName)
	{
		if (mask.IsActive ()) 
		{
			return;
		}

		mask.gameObject.SetActive (true);

		mask.color = new Color(0, 0, 0, 0);
	}


	void fadeOutOver()
	{
		mask.gameObject.SetActive(false);
	}

	void OnUpdateTween(float value)
	{
		mask.color = new Color(0, 0, 0, value);
	}

	public void HomeButtonAction()
	{
		mainMenuPanel.SetActive (true);

		all_level.SetActive (false);
	}

	public void Init()
	{
		Debug.Log ("init in UIManager");

		GameData.getInstance().levelStates = new List<int>();

		for (int j = 0; j < GameData.totalLevel ; j++) 
		{
			int tState = PlayerPrefs.GetInt ("linkdot_" + "_" + j, 0);

			GameData.instance.levelStates.Add (tState);

			GameData.getInstance ().levelStates[j] = tState;
		}
	}

	public void GameWin ()
	{
		StartCoroutine (WinPopUp ());
	}


	IEnumerator WinPopUp()
	{
		yield return new WaitForSeconds(1.2f);

		UIManager.Instance().linkDotContainer.SetActive (false);

		GameData.getInstance().isWin = true;

		UIManager.Instance().popUp.SetActive(true);

		GameData.instance.levelStates[GameData.instance.cLevel] = 1;

		PlayerPrefs.SetInt("linkdot_" + 0 + "_" + GameData.instance.cLevel, 1);

	}
}
