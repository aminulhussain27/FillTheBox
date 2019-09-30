using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {
	
	public Button menuButton;
	public Button continueButton;


	private void Start ()
	{

		//Assinging the button actions

		//Redirects to menu
		menuButton.onClick.RemoveAllListeners ();
		menuButton.onClick.AddListener (() => {

			    SoundManager.Instance ().playSound (SoundManager.SOUND_ID.CLICK);
				transform.gameObject.SetActive (false);
				UIManager.Instance ().mainMenuPanel.SetActive (true);
				UIManager.Instance ().all_level.SetActive (true);

			});

		//To next level
		continueButton.onClick.RemoveAllListeners ();
		continueButton.onClick.AddListener (() => {
				gameObject.SetActive (false);
				ContinueButtonAction ();
			});
	}

	//Checking whether its the last level or not
    private void OnEnable()
    {
        bool isLastLevel = GameData.instance.currentLevel >= GameData.totalLevel-1;

		//If last level then deactivating the NEXT button
		continueButton.gameObject.SetActive(!isLastLevel);
	}
		

	//Continuing to next level
	public void ContinueButtonAction()
	{
		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.CLICK);

		if (GameData.getInstance ().isWin) 
		{
			//Incrementing the current level it its win 
			GameData.instance.currentLevel += 1;

			//Initializing the game arena
			UIManager.Instance ().linkDotGO.GetComponent<ConnectionHandler> ().InitializeGrid ();
        }
	}
}
 