using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

using linkDot;
public class PopUp : MonoBehaviour {
	
	public Button menuButton;
	public Button continueButton;


	void Start () 
	{
		menuButton.onClick.RemoveAllListeners ();
		menuButton.onClick.AddListener (() => {
			transform.gameObject.SetActive (false);

			UIManager.Instance().mainMenuPanel.SetActive(true);
			UIManager.Instance().all_level.SetActive(true);

			});

		continueButton.onClick.RemoveAllListeners ();
		continueButton.onClick.AddListener (() => {
				gameObject.SetActive (false);
				ContinueButtonAction ();
			});

	}

    private void OnEnable()
    {
        GameObject.Find("txtComplete").GetComponentInChildren<Text>().text = "levelComplete";

        bool isLastLevel = GameData.instance.cLevel >= GameData.totalLevel[GameData.difficulty]-1;

		continueButton.gameObject.SetActive(!isLastLevel);
	}
		
	
	public void ContinueButtonAction()
	{
		if (GameData.getInstance ().isWin) 
		{
			GameData.instance.cLevel += 1;
            GameObject.Find("linkdot").GetComponent<LinkDot>().init();
            GameObject.Find("all_game").transform.parent.GetComponent<MainScript>().refreshView();
        }
	
	}
//	public void OnRetryClick()
//	{
//		GameObject.Find("linkdot").GetComponent<LinkDot>().init();
//		GameObject.Find("all_game").transform.parent.GetComponent<MainScript>().refreshView();
//	}
//	
	
	public void showHidePanel(int nStarGet = 0)
	{
//		if (!canShow)
//			return;
//		
//		// Add event handler code here
//		if (!isShowed) {
//         
//            Transform tbg = transform.Find("bg");
//            tbg.transform.localScale = Vector2.zero;
//            tbg.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).OnComplete(OnShowCompleted);
//
//				
//				canShow = false;
//				
//            //stars not used
//				//GameObject starScore = gameObject.transform.Find("bg").Find("star"+nStarGet.ToString()).gameObject;
//				//starScore.GetComponent<Image>().enabled = true;
//				
////			}
//		
//		} else {
//            Transform tbg = transform.Find("bg");
//            tbg.transform.DOScale(Vector3.zero, .4f).OnComplete(OnHideCompleted);
//       
//				canShow = false;
//
//		
//		}



	}
}
 