using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

	[HideInInspector]
	public Text btnStart, btnMore, btnReview;

	public GameObject title;
	public GameObject all_level;//levelmenu container
	public GameObject all_mainMenu;
	public GameObject  panelFade;

	public Button playButton;

	public Image mask;

	void Start()
	{
		GameManager.getInstance ().init ();

		playButton.onClick.RemoveAllListeners ();
		playButton.onClick.AddListener (() => 
			{
				StartButtonAction ();
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


	void fadeInOver(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	void fadeOutOver()
	{
		mask.gameObject.SetActive(false);
	}

	void OnUpdateTween(float value)
	{
		mask.color = new Color(0, 0, 0, value);
	}
}
