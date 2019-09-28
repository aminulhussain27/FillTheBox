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
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
		/// <summary>
		/// not used yet.
		/// </summary>
		/// <returns>The object by name.</returns>
		/// <param name="objname">Objname.</param>
	public GameObject getObjectByName(string objname){
		GameObject rtnObj = null;
		foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
		{
			if(obj.name == objname){
				rtnObj = obj;
			}
		}
		return rtnObj;
	}
	

	
	public static GameManager instance;
	public static GameManager getInstance(){
		if(instance == null){
			instance = new GameManager();
				instance.music = GameObject.Find ("music");
				//PlayerPrefs.DeleteAll ();//uncomment this if you want to reset saved data
		}
		return instance;
	}
	
	

		GameObject music;//a instance for play music
		/// <summary>
		/// Plaies the music.
		/// </summary>
		/// <param name="str">String.</param>
		/// <param name="isforce">If set to <c>true</c> isforce.</param>
		public void playMusic(string str,bool isforce = false){

				//do not play the same music again
				if (!isforce) {
						if (bgMusic != null && musicName == str) {
								return;
						}
				}


				if (!music)
						return;


				AudioSource tmusic = null;

				AudioClip clip = (AudioClip)Resources.Load ("sound/"+str, typeof(AudioClip));

				if (GameData.getInstance ().isSoundOn == 0) {
						if (bgMusic)
								bgMusic.Stop ();
						tmusic = music.GetComponent<musicScript> ().PlayAudioClip (clip,true);
						if (str.Substring (0, 2) == "bg") {
								musicName = str;
								bgMusic = tmusic;

						}
				}

		}







		List<AudioSource> currentSFX = new List<AudioSource>();//sound fx currently playing
		Dictionary<string,int> sfxdic = new Dictionary<string,int>();//check and scan existing sound fx.
		/// <summary>
		/// Play the music effects
		/// </summary>
		/// <returns>The sfx.</returns>
		/// <param name="str">String.</param>
		public AudioSource playSfx(string str){
				AudioSource sfxSound = null;

				if (!music)
						return null;
				AudioClip clip = (AudioClip)Resources.Load ("sound/"+str, typeof(AudioClip));
				if (GameData.getInstance ().isSfxOn == 0) {
						sfxSound = music.GetComponent<musicScript> ().PlayAudioClip (clip);
						if (sfxSound != null) {
								if (sfxdic.ContainsKey (str) == false || sfxdic [str] != 1) {
										currentSFX.Add (sfxSound);
										sfxdic [str] = 1;
								}
						}	
				}	

				return sfxSound;


		}


		AudioSource bgMusic = new AudioSource();//the bgmusic instance(always only one)
		public string musicName = "";
		/// <summary>
		/// Stops the background music.
		/// </summary>
		public void stopBGMusic(){
				if(bgMusic){
						bgMusic.Stop();
						musicName = "";
				}
		}

		/// <summary>
		/// Stops all sound effects.
		/// </summary>
		public void stopAllSFX(){
				foreach(AudioSource taudio in currentSFX){
						if(taudio!=null)taudio.Stop();
				}
				currentSFX.Clear ();
				sfxdic.Clear ();
		}


		/// <summary>
		/// Stops the music.
		/// </summary>
		/// <param name="musicName">Music name.</param>
		public void stopMusic(string musicName = ""){
				if (music) {
						AudioSource[] as1 = music.GetComponentsInChildren<AudioSource> ();
						foreach (AudioSource tas in as1) {
								if(musicName == ""){
										tas.Stop ();
										break;
								}else{
										if(tas && tas.clip){
												string clipname = (tas.clip.name);
												if(clipname == musicName){
														tas.Stop();


														musicName = "";
														if(sfxdic.ContainsKey(clipname)){
																sfxdic[clipname] = 0;
														}
														break;
												}
										}
								}
						}
				}
		}

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
	
		

		GameData.getInstance().bestScore = allScore;
		GameData.getInstance().isSoundOn = (int)PlayerPrefs.GetInt("sound",0);
		GameData.getInstance().isSfxOn = (int)PlayerPrefs.GetInt("sfx",0);

		inited = true;
		
	}
	public bool noToggleSound = false;

	//=================================GameCenter======================================

	private void HandleFriendsLoaded(bool success)
	{
		//        Debug.Log("*** HandleFriendsLoaded: success = " + success);
		foreach (IUserProfile friend in Social.localUser.friends) {
			//            Debug.Log("*   friend = " + friend.ToString());
		}
	}
	
	private void HandleAchievementsLoaded(IAchievement[] achievements)
	{
		//        Debug.Log("*** HandleAchievementsLoaded");
		foreach (IAchievement achievement in achievements) {
			//            Debug.Log("*   achievement = " + achievement.ToString());
		}
	}
	
	private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
	{
		//        Debug.Log("*** HandleAchievementDescriptionsLoaded");
		foreach (IAchievementDescription achievementDescription in achievementDescriptions) {
			//            Debug.Log("*   achievementDescription = " + achievementDescription.ToString());
		}
	}
	
	// achievements
	
	public void ReportProgress(string achievementId, double progress)
	{
		if (Social.localUser.authenticated) {
			Social.ReportProgress(achievementId, progress, HandleProgressReported);
		}
	}
	
	private void HandleProgressReported(bool success)
	{
		//        Debug.Log("*** HandleProgressReported: success = " + success);
	}
	
	public void ShowAchievements()
	{
		if (Social.localUser.authenticated) {
			Social.ShowAchievementsUI();
		}
	}
	
	public void ShowLeaderboard()
	{
		Debug.Log("showLeaderboard");
		if (Social.localUser.authenticated) {
			Social.ShowLeaderboardUI();
		}
	}
	
	//=============================================GameCenter=========================

	public void showInterestitial()
    {
        if (musicScript.nTick == 0)
        {
            //AdsManager.instance.ShowInterstitial();
            musicScript.nTick = 30;//how many seconds to show a interestitial;
        }
    }
}   
