using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
//using AppAdvisory.Ads;
/// <summary>
/// The main controller singleton class
/// </summary>
public class GameManager{
	
	// Use this for initialization
	void Start () {
		
	}
	
//
//	public GameObject getObjectByName(string objname){
//		GameObject rtnObj = null;
//		foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
//		{
//			if(obj.name == objname){
//				rtnObj = obj;
//			}
//		}
//		return rtnObj;
//	}
//	
//
	
	public static GameManager instance;
	public static GameManager getInstance(){
		if(instance == null){
			instance = new GameManager();
		}
		return instance;
	}
	
	

		GameObject music;//a instance for play music
		/// <summary>
		/// Plaies the music.
		/// </summary>
		/// <param name="str">String.</param>
		/// <param name="isforce">If set to <c>true</c> isforce.</param>
		


	public static bool inited;//check if inited
		/// <summary>
		/// Init this controller instance.only once.
		/// </summary>
	public void init(){
		//get data
		if (inited)
			return;
	
		int allScore = 0;

        //stars not use for this game
        //for(int i = 0;i<GameData.totalLevel[GameData.difficulty];i++){
        //	int tScore = PlayerPrefs.GetInt("levelScore_"+i.ToString(),0);
        //	allScore += tScore;
        //	//save star state to gameobject
        //	int tStar = PlayerPrefs.GetInt("levelStar_"+i.ToString(),0);
        //	GameData.getInstance().lvStar.Add(tStar);
        //}


        GameData.getInstance().levelStates = new List<List<int>>();
        for (int i = 0; i < GameData.totalLevel.Length; i++)
        {
            GameData.instance.levelStates.Add(new List<int>());
            for (int j = 0; j < GameData.totalLevel[i]; j++)
            {
               
                int tState = PlayerPrefs.GetInt("linkdot_" + i + "_" + j, 0);
                GameData.instance.levelStates[i].Add(tState);
                GameData.getInstance().levelStates[i][j] = tState;


                if (tState == 1)
                {
                    allScore++;
                }
            }
        }

        GameData.instance.bestScore = allScore;

        Debug.Log("bestScore is:"+allScore);
		//GameData.getInstance ().levelPassed = PlayerPrefs.GetInt("levelPassed",0);
		//Debug.Log ("current passed level = " + GameData.getInstance ().levelPassed);
		
		//for continue,set default to lastest level
		//GameData.getInstance ().cLevel = GameData.getInstance ().levelPassed;


		inited = true;
		
	}
}   
