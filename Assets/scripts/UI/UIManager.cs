﻿using System.Collections;
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

			all_level.transform.GetComponent<LevelMenu> ().refreshLevel ();
		}
	}

	void fadeOut()
	{
		mask.gameObject.SetActive(true);
		mask.color = Color.black;
	}

	void fadeIn(string sceneName)
	{
		if (mask.IsActive())
			return;
		mask.gameObject.SetActive(true);
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

//		GameData.instance.levelStates.Add (new int);//new List<int> ());

		for (int j = 0; j < GameData.totalLevel ; j++) 
		{
			int tState = PlayerPrefs.GetInt ("linkdot_" + "_" + j, 0);
			GameData.instance.levelStates.Add (tState);
			GameData.getInstance ().levelStates[j] = tState;
		}

		/*	for (int i = 0; i < GameData.totalLevel.Length; i++) 
		{
			GameData.instance.levelStates.Add (new List<int> ());
			for (int j = 0; j < GameData.totalLevel [i]; j++) {
				int tState = PlayerPrefs.GetInt ("linkdot_" + i + "_" + j, 0);
				GameData.instance.levelStates [i].Add (tState);
				GameData.getInstance ().levelStates [i] [j] = tState;
			}
		}*/
	}

	public void GameWin ()
	{
		Debug.Log ("gameWin");
		StartCoroutine (WinPopUp ());
	}


	IEnumerator WinPopUp()
	{
		yield return new WaitForSeconds(2);

		UIManager.Instance().linkDotContainer.SetActive (false);

		GameData.getInstance().isWin = true;

		UIManager.Instance().popUp.SetActive(true);

//		GameData.instance.levelStates[GameData.difficulty][GameData.instance.cLevel] = 1;
		GameData.instance.levelStates[GameData.instance.cLevel] = 1;
		PlayerPrefs.SetInt("linkdot_" + GameData.difficulty + "_" + GameData.instance.cLevel, 1);

	}
}
