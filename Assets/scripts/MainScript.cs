using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using DG.Tweening;
using UnityEngine.SceneManagement;
using SimpleJson;
using DG.Tweening;
using linkDot;
public class MainScript : MonoBehaviour
{

    public int timeCount = 120;//how much time you left.The more time,the better score
    Text timeTxt;//the text ui showes the count down time text
    void Start()
    {
        refreshView();
    }
		
    public void init()
    {
		Debug.Log ("init");
		UIManager.Instance().linkDotGO.GetComponent<LinkDot>().init();
		refreshView();
    }


    /// <summary>
    /// check if win every second
    /// </summary>
    /// <returns>The asecond.</returns>
    IEnumerator waitAsecond()
    {
        yield return new WaitForSeconds(1);

        if (timeCount > 0)
        {
            if (GameData.getInstance().isWin == false)
            {
                timeCount--;
                StartCoroutine("waitAsecond");
                timeTxt.text = timeCount.ToString();
            }
        }
    }
		
    /// <summary>
    /// Refreshs the view.
    /// </summary>
    public void refreshView()
    {
        GameObject.Find("CurrentLevelText").GetComponent<Text>().text = (GameData.getInstance().cLevel + 1).ToString();
       
        timeTxt = GameObject.Find("timeTxt").GetComponent<Text>();

        GameObject tIns = GameObject.Find("lb_ins");
        if (tIns != null)
        {
            tIns.GetComponent<Text>().text ="lb_ins";
        }
    }
		
    public void gameWin ()
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

        GameData.instance.levelStates[GameData.difficulty][GameData.instance.cLevel] = 1;
        PlayerPrefs.SetInt("linkdot_" + GameData.difficulty + "_" + GameData.instance.cLevel, 1);

	}


    /// <summary>
    /// camera fade out
    /// </summary>
    public Image mask;
    void fadeOut()
    {
        mask.gameObject.SetActive(true);
        mask.color = Color.black;
        ATween.ValueTo(mask.gameObject, ATween.Hash("from", 1, "to", 0, "time", 1, "onupdate", "OnUpdateTween", "onupdatetarget", this.gameObject, "oncomplete", "fadeOutOver", "oncompletetarget", this.gameObject));
    }
    /// <summary>
    /// camera fade in
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    void fadeIn(string sceneName)
    {
        if (mask.IsActive())
            return;
        mask.gameObject.SetActive(true);
        mask.color = new Color(0, 0, 0, 0);

        ATween.ValueTo(mask.gameObject, ATween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "OnUpdateTween", "onupdatetarget", this.gameObject, "oncomplete", "fadeInOver", "oncompleteparams", sceneName, "oncompletetarget", this.gameObject));
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
