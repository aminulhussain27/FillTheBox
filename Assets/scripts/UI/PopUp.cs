using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {
	
	public Button menuButton;
	public Button continueButton;


	private void Start ()
	{
		menuButton.onClick.RemoveAllListeners ();
		menuButton.onClick.AddListener (() => {
				transform.gameObject.SetActive (false);

				UIManager.Instance ().mainMenuPanel.SetActive (true);
				UIManager.Instance ().all_level.SetActive (true);

			});

		continueButton.onClick.RemoveAllListeners ();
		continueButton.onClick.AddListener (() => {
				gameObject.SetActive (false);
				ContinueButtonAction ();
			});
	}

    private void OnEnable()
    {
        bool isLastLevel = GameData.instance.currentLevel >= GameData.totalLevel-1;

		continueButton.gameObject.SetActive(!isLastLevel);
	}
		
	
	public void ContinueButtonAction()
	{
		if (GameData.getInstance ().isWin) 
		{
			GameData.instance.currentLevel += 1;

			UIManager.Instance ().linkDotGO.GetComponent<linkDot.ConnectionHandler> ().InitializeGrid ();
        }
	}
}
 