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
	
		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.LOOP_BACKGROUND, 1, true);
	}

	public void StartButtonAction()
	{
		all_level.SetActive (true);
	}

//	void fadeOut()
//	{
//		mask.gameObject.SetActive(true);
//
//		mask.color = Color.black;
//	}

//	void fadeIn(string sceneName)
//	{
//		if (mask.IsActive ()) 
//		{
//			return;
//		}
//
//		mask.gameObject.SetActive (true);
//
//		mask.color = new Color(0, 0, 0, 0);
//	}
//
//
//	void fadeOutOver()
//	{
//		mask.gameObject.SetActive(false);
//	}
//
//	void OnUpdateTween(float value)
//	{
//		mask.color = new Color(0, 0, 0, value);
//	}

	public void HomeButtonAction()
	{
		mainMenuPanel.SetActive (true);

		all_level.SetActive (false);
	}

	public void GameWin ()
	{
		StartCoroutine (WinPopUp ());
	}


	IEnumerator WinPopUp()
	{
		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.WIN);

		yield return new WaitForSeconds(0.8f);

		UIManager.Instance().linkDotContainer.SetActive (false);

		GameData.getInstance().isWin = true;

		UIManager.Instance().popUp.SetActive(true);

		GameData.instance.levelStates[GameData.instance.currentLevel] = 1;

		PlayerPrefs.SetInt("linkdot_" + 0 + "_" + GameData.instance.currentLevel, 1);

	}
}
