using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class LevelMenu : MonoBehaviour
{

    // Use this for initialization
	public GameObject mainMenuPanel;
    GameObject listItemg;

    List<GameObject> groups;

	public GameObject all_mainMenu;
	public GameObject all_game;

	GameObject controlbar;
	GameObject btnDiff;

	Vector3 middlestart;


    void Start()
    {
//        GameManager.getInstance().init();
		UIManager.Instance ().Init ();
		GameData.getInstance().resetData();

        initLevels();
	}


    void initLevels()
    {
		Debug.Log ("initLevels");

        initView();
        GameData.getInstance().currentScene = 1;//1 is levelmenu
        groups = new List<GameObject>();

    }

    public void refreshLevel()
    {
        GameData.instance.currentScene = 1;
//        difficultPanel.transform.position = new Vector2(difficultPanel.transform.position.x, middlestart.y);
    }
		

    bool isMoving = false;
    public void move(float dis)
    {
        if (canmove)
        {
//            foreach (Transform m in mainContainer.transform)
//            {
//                m.transform.Translate(dis, 0, 0);
//            }
            isMoving = true;
        }
    }

    /// <summary>
    /// simulate Swipes the page to is right position
    /// </summary>
    /// <param name="force">Force.</param>
    public void swipePage(float force)
    {


        if (Mathf.Abs(force) < 1f)
        {//user not do a quick swipe
            if (groups[page].transform.position.x < Screen.width / 4)
            {
                if (page >= 0 && page < pages)
                {
                    GoRight();
                }
                else
                {
                    returnPage();
                }
            }
            else if (groups[page].transform.position.x > Screen.width)
            {
                if (page <= pages && page > 0)
                {
                    GoLeft();
                }
                else
                {
                    returnPage();
                }
            }
            else
            {
                returnPage();
            }
        }
        else
        {
            if (groups[page].transform.position.x < Screen.width / 2)
            {
                if (page >= 0 && page < pages)
                {
                    GoRight();
                }
                else
                {
                    returnPage();
                }

            }
            else if (groups[page].transform.position.x > Screen.width / 2)
            {
                if (page <= pages && page > 0)
                {
                    GoLeft();
                }
                else
                {
                    returnPage();
                }
            }
            else
            {
                returnPage();
            }
        }

        //not allow level buttons active while moving the menu
        StopCoroutine("swiped");
        StartCoroutine("swiped");

    }

    //lock the game while page is auto moving.Unlock when finished
    IEnumerator swiped()
    {
        yield return new WaitForEndOfFrame();
        isMoving = false;
    }

    public GameObject levelButton;//the level button template instance
    public GameObject dot;//the page dot for turn page

    int page = 0;//current page
    int pages = 1;//how many page
    public int perpage = 8;//icons per page
    List<GameObject> gContainer;//each icon group for per page
    List<GameObject> pageDots;//all page dots
    float gap = Screen.width / 8.5f;//the gap for each page
    public Image mask;//the fade in/out mask
    void initView()
    {

        //pageDots = new List<GameObject>();


        //pages = Mathf.FloorToInt((GameData.totalLevel[GameData.difficulty] - 1) / perpage);
        //for (int i = 0; i <= pages; i++)
        //{
        //    GameObject tdot = Instantiate(dot, dot.transform.parent) as GameObject;
        //    tdot.SetActive(true);
        //    pageDots.Add(tdot);
        //    tdot.name = "dot_" + i;

        //}

        //setpageDot();
        //fadeOut ();

        //gContainer = new List<GameObject>();
        //gContainer.Add(levelButton.transform.parent.gameObject);

        Transform container = levelButton.transform.parent;
        container.transform.localScale = Vector3.one;

        //for (int i = perpage; i < GameData.totalLevel[GameData.difficulty]; i += perpage)
        //{
        //    GameObject tgroup = Instantiate(levelButton.transform.parent.gameObject, levelButton.transform.parent.position, Quaternion.identity) as GameObject;
        //    tgroup.transform.Translate(gap * (i + 1), 0, 0);
        //    gContainer.Add(tgroup);

        //    tgroup.transform.parent = levelButton.transform.parent.gameObject.transform.parent;
        //}


        for (int i = 0; i < GameData.totalLevel[GameData.difficulty]; i++)
        {
            GameObject tbtn = Instantiate(levelButton,container.transform) as GameObject;

            //int tContainerNo = Mathf.FloorToInt(i / perpage);
            //tbtn.transform.parent = gContainer[tContainerNo].transform;

            tbtn.SetActive(true);


            tbtn.GetComponentInChildren<Text>().text = (i + 1).ToString();

            Text ttext = tbtn.GetComponentInChildren<Text>();


            //star not used for this game
            //if (GameData.getInstance().lvStar.Count > i)
            //{

            //    int starCount = GameData.getInstance().lvStar[i];


            //    if (GameData.getInstance().lvStar.Count > i + 1)
            //    {
            //        for (int j = 1; j <= starCount; j++)
            //        {
            //            ttext.transform.parent.Find("star" + j).GetComponent<Image>().enabled = true;
            //        }
            //    }
            //}


            //if (i > GameData.getInstance().levelPassed && i > 0)
            //{

            //    ttext.enabled = false;

            //}
            //else
            //{


            tbtn.name = "level" + (i + 1);
            tbtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => clickLevel(tbtn));
            ttext.gameObject.transform.parent.Find("lock").GetComponent<Image>().enabled = false;

            //}
        }


        //GameObject.Find("txtScore").GetComponent<Text>().text = Localization.Instance.GetString("totalScore") + GameData.getInstance().bestScore;
        GameObject btnConfirm = GameObject.Find("confirm");//
        if (btnConfirm != null) btnConfirm.GetComponentInChildren<Text>().text = "continue";


    }

    //refresh button states
    public void refreshView()
    {
        //print(GameData.difficulty);
        for (int i = 0; i < GameData.totalLevel[GameData.difficulty]; i++)
        {
            GameObject tbtn = GameObject.Find("level" + (i+1));
            if (tbtn != null)
            {
                int tlevelButtonState = GameData.instance.levelStates[GameData.difficulty][i];
                if (tlevelButtonState == 1)
                {
                    tbtn.GetComponent<Image>().color = new Color(1, 111f/225f, 0);
                }
                else {
                    tbtn.GetComponent<Image>().color = Color.grey;
                }
            }
        }
    }


    /// <summary>
    /// Clicks the dot. For turn page,not used
    /// </summary>
    /// <param name="tdot">Tdot.</param>
    public void clickDot(GameObject tdot)
    {
        int tdotIndex = int.Parse(tdot.transform.parent.name.Substring(4, tdot.transform.parent.name.Length - 4));
        page = tdotIndex;
        canmove = false;
        ATween.MoveTo(gContainer[0].transform.parent.gameObject, ATween.Hash("ignoretimescale", true, "islocal", true, "x", -gContainer[page].transform.localPosition.x, "time", .3f, "easeType", "easeOutExpo", "oncomplete", "dotclicked", "oncompletetarget", this.gameObject));
    }

    /// <summary>
    /// page turned,not used
    /// </summary>
    void dotclicked()
    {
        canmove = true;
        setpageDot();

    }


    public static bool islock = false;
    /// <summary>
    /// Clicks the level button.
    /// </summary>
    /// <param name="tbtn">Tbtn.</param>
    void clickLevel(GameObject tbtn)
    {
		Debug.Log ("clickLevel  tbtn: " + tbtn);

        GameData.getInstance().cLevel = int.Parse(tbtn.GetComponentInChildren<Text>().text) - 1;
        if (GameData.instance.mode == 1)//the fade transition for test only
        {
            if (!isMoving)
            {
               
                fadeIn("game");
            }
        }
        else//load level
        {
            all_game = GameObject.Find("all_game");

           // all_game.transform.parent.GetComponent<MainScript>().refreshView();//some ui must active before anim finishes;
			//all_game.transform.parent.GetComponent<MainScript>().init();
			UIManager.Instance().linkDotGO.GetComponent<linkDot.LinkDot>().init();
            if (all_game != null)
            {
				all_game.transform.position = Vector2.zero;
               // all_game.transform.DOMoveX(0, 1f).SetEase(Ease.OutBounce);
            }
				
			mainMenuPanel.SetActive (false);
        }
    }


    /// <summary>
    /// Set dots for pages.not used
    /// </summary>
    void setpageDot()
    {
        for (int i = 0; i < pageDots.Count; i++)
        {
            pageDots[i].GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        }
        pageDots[page].GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }


    /// <summary>
    /// touch the continue to Continues your last level.not used
    /// </summary>
    public void continueLevel()
    {

        int tLastLevel = GameData.getInstance().levelPassed;

        if (tLastLevel < GameData.totalLevel[GameData.difficulty])
        {
            GameData.getInstance().cLevel = tLastLevel;
        }
        else
        {
            GameData.getInstance().cLevel = GameData.totalLevel[GameData.difficulty];
        }

        string tstr = "game";// + GameData.getInstance ().cLevel;


        if (GameData.instance.mode == 1)//this is always for test because you may not start from the initiate window.
        {
            fadeIn(tstr);

        }
        else
        {
			//all_game.transform.parent.GetComponent<MainScript> ().init ();
			UIManager.Instance ().linkDotGO.GetComponent<linkDot.LinkDot> ().init ();
            all_game = GameObject.Find("all_game");

        }
    }

    /// <summary>
    /// Backs the main scene.
    /// </summary>
    public void backMain()
    {
        if (all_mainMenu == null || GameData.instance.mode == 1)//this is always for test because you may not start from the initiate window.
        {
            fadeIn("MainMenu");
        }
        else
        {
            all_mainMenu.transform.DOMoveX(all_mainMenu.transform.position.x + Screen.width, 1f).SetEase(Ease.OutBounce).OnComplete(() => { 
				GameData.instance.currentScene = 0; 
			});
          //  all_level.transform.DOMoveX(all_level.transform.position.x + Screen.width, 1f).SetEase(Ease.OutBounce);
        }



    }

    bool canmove = true;//can not enter a level and can not move when moving
                        /// <summary>
                        /// page Goes right.not used
                        /// </summary>
    public void GoRight()
    {
        if (!canmove)
            return;
        if (page < pages)
        {
            page++;
            canmove = false;

            ATween.MoveTo(gContainer[0].transform.parent.gameObject, ATween.Hash("ignoretimescale", true, "islocal", true, "x", -gContainer[page].transform.localPosition.x, "time", .3f, "easeType", "easeOutExpo", "oncomplete", "dotclicked", "oncompletetarget", this.gameObject));

        }
    }
    /// <summary>
    /// page goes left.
    /// </summary>
    public void GoLeft()
    {
        if (!canmove)
            return;
        if (page > 0)
        {
            page--;
            canmove = false;
            ATween.MoveTo(gContainer[0].transform.parent.gameObject, ATween.Hash("ignoretimescale", true, "islocal", true, "x", -gContainer[page].transform.localPosition.x, "time", .3f, "easeType", "easeOutExpo", "oncomplete", "dotclicked", "oncompletetarget", this.gameObject));
        }
    }


    void fadeOut()
    {
        mask.gameObject.SetActive(true);
        mask.color = Color.black;

        ATween.ValueTo(mask.gameObject, ATween.Hash("ignoretimescale", true, "from", 1, "to", 0, "time", 1, "onupdate", "OnUpdateTween", "onupdatetarget", this.gameObject, "oncomplete", "fadeOutOver", "oncompletetarget", this.gameObject));

    }

    void fadeIn(string sceneName)
    {
        if (mask.IsActive())
            return;
        mask.gameObject.SetActive(true);
        mask.color = new Color(0, 0, 0, 0);

        ATween.ValueTo(mask.gameObject, ATween.Hash("ignoretimescale", true, "from", 0, "to", 1, "time", 1, "onupdate", "OnUpdateTween", "onupdatetarget", this.gameObject, "oncomplete", "fadeInOver", "oncompleteparams", sceneName, "oncompletetarget", this.gameObject));
    }

    void fadeInOver(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// when fade out over
    /// </summary>
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




    /// <summary>
    /// Returns the page to its origin place.
    /// </summary>
    void returnPage()
    {
        canmove = false;
        ATween.MoveTo(gContainer[page].transform.parent.gameObject, ATween.Hash("ignoretimescale", true, "islocal", true, "x", -gContainer[page].transform.localPosition.x, "time", .3f, "easeType", "easeOutExpo", "oncomplete", "dotclicked", "oncompletetarget", this.gameObject));

    }

}
