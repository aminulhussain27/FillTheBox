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

    // Use this for initialization

    //data
    public int timeCount = 120;//how much time you left.The more time,the better score
    Text timeTxt;//the text ui showes the count down time text
    void Start()
    {

        //initData();
        refreshView();
        //StartCoroutine("waitAsecond");

        //fadeOut ();
        all_game = GameObject.Find("all_game");
        all_level = GameObject.Find("all_level");
        if (all_level != null)//only when level menu loaded
        {

//            float tx = GameObject.Find("Back_UI_Cam").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
//            all_game.transform.Translate(tx, 0, 0);

        }
        else
        {
            init();//for test
        }
        StartCoroutine("nextframe");
    }

    GameObject all_level;//levelmenu container
    GameObject all_game;
    IEnumerator nextframe()
    {
        yield return new WaitForEndOfFrame();

    }
		
    public void init()
    {
		Debug.Log ("init");
        GameData.getInstance().currentScene = 2;
        GameObject.Find("linkdot").GetComponent<LinkDot>().init();
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


    //	GameObject nodeOrigin;//the origin node gameobject on the stage for duplicate
    //	GameObject linkLine;// the oirigin line gameobject on the stage for duplicate
    //	//	public Vector3 list;

    //		List<GameObject> nodes;//store all nodes

    //		SimpleJSON.JSONNode levelData;
    //	public string[] lvAnswerData;//the list of solve sequences
    //		/// <summary>
    //		/// parase the data get from level data to build the level.
    //		/// </summary>
    //	void initData(){

    //        GameData.getInstance().resetData();
    //        GameData.getInstance().main = this;

    //        return;


    //        if (nodes == null) {
    //			nodes = new List<GameObject>();		
    //		}
    //		GameData.getInstance ().resetData();
    //		GameData.getInstance ().main = this;

    //		nodeOrigin = GameObject.Find ("nodeOri");
    //		linkLine = GameObject.Find ("linkLine");




    //        string[] tDatas = Datas.Instance.getData("energy");
    //        string tData = tDatas[GameData.getInstance ().cLevel];

    //				levelData = SimpleJSON.JSONArray.Parse(tData);
    //				initLevelData();
    //	}


    //		void initLevelData(){





    //				lvAnswerData = new string[levelData [2].Count];
    //				for (int i = 0; i < levelData [2].Count; i++) {
    //						lvAnswerData[i] = levelData [2][i];
    //				}

    //				string tno = "";//0;
    //				float tx = 0;
    //				float ty = 0;


    //				GameData.getInstance().nLink = lvAnswerData.Length - 1;



    //				float zoom = Screen.height / 1000f;//for resolution
    //				//construct map;
    ////				for (int i = 0; i <lvMapData.Length; i++) {
    //				for (int i = 0; i <levelData[0].Count; i++) {
    //						for (int j = 0; j < levelData [0] [i].Count; j++) {

    //								if (j == 0) {
    //										if (levelData [0] [i] [j] == "" || levelData [1] [i] [j] == 0)
    //												continue;
    //										tno = (i+1).ToString();

    ////										tno = t0;
    //								} else if (j == 1) {
    //										tx = levelData [0][i][j]*zoom;
    ////										print (tx);
    //								} else{
    //										ty = levelData [0] [i][j]*zoom;
    //								}
    //								if(j%3 == 2){
    //										GameObject tnode = Instantiate (nodeOrigin, nodeOrigin.transform.parent) as GameObject;
    //										tnode.transform.position = new Vector3(Screen.width/2 -tx-4630*zoom,Screen.height/2 + ty - 2120*zoom ,0);

    //										tnode.name = "node_"+ tno;

    //										//add reference for kill;

    //										nodes.Add(tnode);
    //										ty =  levelData [0] [i][j]*zoom;
    //								}
    //						}

    //				}
    //				//link map;
    //				string linka = "";string linkb="";
    //				for (int i = 0; i < levelData [1].Count; i++) {
    //						for (int j = 0; j < levelData [1] [i].Count; j++) {
    //								if (j == 0) {
    //										if (levelData [1] [i] [j] == "" || levelData [1] [i] [j] == 0)
    //												continue;
    //										linka = levelData [1] [i] [j];

    //								} else if (j == 1) {
    //										linkb = levelData [1] [i] [j];
    //								}
    //								if (j == 1) {

    //										GameObject obja = GameObject.Find ("node_" + linka);	
    //										GameObject objb = GameObject.Find ("node_" + linkb);	

    //										Vector3 tlinkLinePos = (obja.transform.position + objb.transform.position) / 2;
    //										//				dfControl tlinkline =  dfpanel_.AddPrefab(linkLine);
    //										GameObject tlinkline = GameObject.Instantiate (linkLine, linkLine.transform.parent) as GameObject;

    //										tlinkline.transform.position = tlinkLinePos;

    //										int tlinka = int.Parse (linka);
    //										int tlinkb = int.Parse (linkb);

    //										tlinkline.name = "linkLine" + "_" + Mathf.Min (tlinka, tlinkb) + "_" + Mathf.Max (tlinka, tlinkb);
    //										//				tlinkline.SendToBack();
    //										GameObject tChild = tlinkline.transform.Find ("mc").gameObject;
    //										//				dfSprite tsp = tChild.GetComponent<dfSprite>();
    //										float tdis = Vector3.Distance (obja.transform.localPosition, objb.transform.localPosition);
    //										tChild.GetComponent<RectTransform> ().sizeDelta = new Vector2 (tdis + 14, 12);


    //										float teng = Mathf.Atan2 ((obja.transform.position.y - objb.transform.position.y), (obja.transform.position.x - objb.transform.position.x)) * Mathf.Rad2Deg;
    //										tlinkline.transform.Find ("mc").Rotate (new Vector3 (0, 0, teng));

    //										tlinkline.transform.SetAsFirstSibling ();
    //										//add reference for kill;
    //										nodes.Add (tlinkline);

    //								}
    //						}

    //				}
    //		}

    /// <summary>
    /// Clears the game.
    /// </summary>
    //	void clearGame(){
    //				foreach(GameObject tnode in nodes){
    ////			dfpanel_.RemoveControl(tnode);
    ////			dfpanel_.RemoveAllEventHandlers();
    //			DestroyImmediate(tnode.gameObject);
    //		}
    //		//nodes.Clear ();
    //		//EneryNode.currentNode = null;
    //		//		GameObject.Find ("btnTip").GetComponent<Button> ().interactable = true;
    //	}

    //handler event
    public void OnRetryClick()
    {
        GameObject.Find("linkdot").GetComponent<LinkDot>().init();
    }

    public GameObject panelWin;//win panel gameobject
    WinPanel winpanel;//winpanel controller
    /// <summary>
   /// when game wins.
   /// </summary>
   /// 
    public void gameWin()
    {
        if (GameData.getInstance().cLevel % 5 == 0 && GameData.getInstance().cLevel > 0)
        {
            //			musicScript.showCB();
            
        }

        GameData.getInstance().isWin = true;
        int threeStar = 10;// (int)(levelData [1].Count/2/1.2f)+2;//one line per second
        int twoStar = threeStar + 5;//
        int oneStar = threeStar + 20;//

        int starGet = 0;
        if ((120 - timeCount) <= threeStar)
        {
            starGet = 3;
        }
        else if ((120 - timeCount) > threeStar && (120 - timeCount) <= twoStar)
        {
            starGet = 2;
        }
        else if ((120 - timeCount) > twoStar && (120 - timeCount) <= oneStar)
        {
            starGet = 1;
        }
        else
        {
            starGet = 0;
        }

        panelWin.SetActive(true);

        winpanel = panelWin.GetComponent<WinPanel>();
        winpanel.showHidePanel(starGet);
        //		GameObject.Find ("btnTip").GetComponent<Button> ().interactable = false;

        //save as this game unlock all level automatically,not use
        //int saveLevel = 0;
        //if (GameData.getInstance().cLevel < GameData.totalLevel[GameData.difficulty] - 1)
        //{
        //    saveLevel = GameData.getInstance().cLevel + 1;
        //}

        //if (GameData.getInstance().levelPassed < saveLevel)
        //{
        //    PlayerPrefs.SetInt("levelPassed", saveLevel);
        //    GameData.getInstance().levelPassed = saveLevel;
        //}

        GameData.instance.levelStates[GameData.difficulty][GameData.instance.cLevel] = 1;
        PlayerPrefs.SetInt("linkdot_" + GameData.difficulty + "_" + GameData.instance.cLevel, 1);


        //save score
        int tallScore = 0;
        for(int i  = 0; i < GameData.totalLevel.Length; i++)
        {
            for(int j = 0; j < GameData.totalLevel[i]; j++)
            {
                if(GameData.instance.levelStates[i][j] == 1)
                {
                    tallScore++;
                }
            }
        }
        
        GameData.getInstance().bestScore = tallScore;

        //not use this mode
        /*
        int cLvScore = PlayerPrefs.GetInt("levelScore_" + GameData.getInstance().cLevel, 0);
        //		print (cLvScore + "_" + timeCount);
        if (cLvScore < timeCount)
        {
            PlayerPrefs.SetInt("levelScore_" + GameData.getInstance().cLevel, timeCount);
            PlayerPrefs.SetInt("levelStar_" + GameData.getInstance().cLevel, starGet);
            //save to GameData instantlly
            if (GameData.getInstance().lvStar.Count > GameData.getInstance().cLevel)
                GameData.getInstance().lvStar[GameData.getInstance().cLevel] = starGet;
            //			print ("save new score"+cLvScore+"_"+timeCount);


            //submitscore
            int tallScore = 0;
            for (int i = 0; i < GameData.totalLevel[GameData.difficulty]; i++)
            {
                int tScore = PlayerPrefs.GetInt("levelScore_" + i.ToString(), 0);
                tallScore += tScore;
            }
            GameData.getInstance().bestScore = tallScore;
            GameManager.getInstance().submitGameCenter();

        }
        */

    }



    /// <summary>
    /// deal Button clicks handler.
    /// </summary>
    /// <param name="control">Control.</param>
    public void buttonHandler(GameObject control)
    {
//        GameManager.getInstance().playSfx("click");
        switch (control.name)
        {
            case "btnMain":
                fadeIn("MainMenu");

                break;
            case "btnMenu":
                fadeIn("LevelMenu");
                break;
        }
    }

    public void loadMainScene()
    {
        GameObject.Find("particle").SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }

    public void loadLevelScene()
    {
//        GameManager.getInstance().playSfx("click");
        all_level = GameObject.Find("all_level");
        if (all_level != null)
        {
            //all_level.transform.parent.GetComponent<LevelMenu>().refreshLevel();//uncomment this if you want to choose level difficulty after cancel a game
            all_level.transform.parent.GetComponent<LevelMenu>().refreshView();
            all_level.transform.DOMoveX(all_level.transform.position.x + Screen.width, 1f).SetDelay(.4f).SetEase(Ease.OutBounce).OnComplete(() => { GameData.instance.currentScene = 1; });
            all_game = GameObject.Find("all_game");
            if (all_game != null)
            {
                float tx = GameObject.Find("Back_UI_Cam").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
              
                GameObject.Find("linkdot").SendMessage("clear");
            }
        }
        else
        {
            SceneManager.LoadScene("LevelMenu");
        }
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


    /// <summary>
    /// tween update event
    /// </summary>
    /// <param name="value">Value.</param>
    void OnUpdateTween(float value)
    {
        mask.color = new Color(0, 0, 0, value);
    }

}
