﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

using linkDot;
public class WinPanel : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
			
	}
    GameObject btnBack;
    GameObject btnConti;
    GameObject btnRetry;
    private void OnEnable()
    {
        GameObject.Find("txtComplete").GetComponentInChildren<Text>().text = "levelComplete";

        bool isLastLevel = GameData.instance.cLevel >= GameData.totalLevel[GameData.difficulty]-1;

        btnBack = transform.Find("bg").Find("btnBack").gameObject;
        btnBack.SetActive(isLastLevel);

        btnConti = transform.Find("bg").Find("btnConti").gameObject;
        btnConti.SetActive(!isLastLevel);

        btnRetry = transform.Find("bg").Find("btnRetry").gameObject;


        btnConti.GetComponentInChildren<Text>().text = "next";
        btnBack.GetComponentInChildren<Text>().text = "back";
        btnRetry.GetComponentInChildren<Text>().text = "retry";

    }

    // Update is called once per frame
    void Update () {
		
	}
	//	public delegate void PanelChangedEventHandler();
	//	public event PanelChangedEventHandler showPanel;
	bool isShowed;
	bool canShow = true;
	//continue
	public void OnContinueEventHandler()
	{
				if (!isShowed)
						return;
//		GameManager.getInstance ().playSfx ("click");
		showHidePanel ();

		if (GameData.getInstance ().cLevel < GameData.totalLevel[GameData.difficulty]-1) {
			GameData.getInstance ().cLevel++;
		} else {
			GameData.getInstance ().cLevel = 0;	
		}


		
	}
	
	public void OnRetryEventHandler()
	{
//		GameManager.getInstance ().playSfx ("click");
		showHidePanel ();
		
	}

	public void OnShowCompleted()
	{
		// Add event handler code here
		//		print ("showOver");
		isShowed = true;
		canShow = true;
	}
	
	public void OnHideCompleted()
	{
		isShowed = false;
        gameObject.SetActive(false);
		canShow = true;
		if (GameData.getInstance ().isWin) {
            GameObject.Find("linkdot").GetComponent<LinkDot>().init();
            GameObject.Find("all_game").transform.parent.GetComponent<MainScript>().refreshView();
        }
	
	}
	
	
	public void showHidePanel(int nStarGet = 0){
		if (!canShow)
			return;
		
		// Add event handler code here
		if (!isShowed) {
         
            Transform tbg = transform.Find("bg");
            tbg.transform.localScale = Vector2.zero;
            tbg.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).OnComplete(OnShowCompleted);

				
				canShow = false;
				
            //stars not used
				//GameObject starScore = gameObject.transform.Find("bg").Find("star"+nStarGet.ToString()).gameObject;
				//starScore.GetComponent<Image>().enabled = true;
				
//			}
		
		} else {
            Transform tbg = transform.Find("bg");
            tbg.transform.DOScale(Vector3.zero, .4f).OnComplete(OnHideCompleted);
       
				canShow = false;

		
		}



	}
}
 