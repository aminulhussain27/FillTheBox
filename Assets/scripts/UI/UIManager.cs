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
		//Assigning the button action in Menu panel
		playButton.onClick.RemoveAllListeners ();
		playButton.onClick.AddListener (() => {
			SoundManager.Instance ().playSound (SoundManager.SOUND_ID.CLICK);
				StartButtonAction ();
			});

		selectLevelButton.onClick.RemoveAllListeners ();
		selectLevelButton.onClick.AddListener (() => {
			SoundManager.Instance ().playSound (SoundManager.SOUND_ID.CLICK);
			mainMenuPanel.SetActive (true);
			});
		
		home.onClick.RemoveAllListeners ();
		home.onClick.AddListener (() => {
			SoundManager.Instance ().playSound (SoundManager.SOUND_ID.CLICK);
				HomeButtonAction ();
			});

		//Refres button will refresh a loaded level
		refreshButton.onClick.RemoveAllListeners ();
		refreshButton.onClick.AddListener (() => 
		{
			SoundManager.Instance ().playSound (SoundManager.SOUND_ID.CLICK);
			UIManager.Instance ().linkDotGO.GetComponent<ConnectionHandler> ().InitializeGrid ();
		});

	
		//Background music started from launch of the game
		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.LOOP_BACKGROUND, 0.25f, true);
	}

	//Play button in menu panel leads to play the game
	public void StartButtonAction()
	{
		all_level.SetActive (true);
	}
		
	//Home button redirects to menu screen
	public void HomeButtonAction()
	{
		mainMenuPanel.SetActive (true);

		all_level.SetActive (false);
	}

	//Winning popup will show
	public void GameWin ()
	{
		StartCoroutine (WinPopUp ());
	}

	//Calling the winning popup after some delay using coroutine
	IEnumerator WinPopUp()
	{
		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.WIN);

		//waiting for some time to show the finishing move properly
		yield return new WaitForSeconds(0.8f);

		UIManager.Instance().linkDotContainer.SetActive (false);

		GameData.getInstance().isWin = true;

		UIManager.Instance().popUp.SetActive(true);

		//This level completed Hence storing as 1 in level state List
		GameData.instance.levelStates[GameData.instance.currentLevel] = 1;

		//Storing the finished level to player prefab
		PlayerPrefs.SetInt("FinishedLevel_" + 0 + "_" + GameData.instance.currentLevel, 1);

	}
}
