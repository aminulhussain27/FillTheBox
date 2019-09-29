using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{
	public GameObject levelButton;//the level button template instance

	private GameObject listItemg;

	private List<GameObject> gContainer;//each icon group for per page

	private List<GameObject> pageDots;//all page dots

	private GameObject controlbar;

	private GameObject btnDiff;

	private Vector3 middlestart;

	private int page = 0;//current page

	private int pages = 1;//how many page

	private float gap = Screen.width / 8.5f;//the gap for each page

	public int perpage = 8;//icons per page

	public Image mask;//the fade in/out mask


    void Start()
    {
		GameData.getInstance().ResetData();

        InitializeLevels();
	}


	private void InitializeLevels()
    {
		Transform container = levelButton.transform.parent;
		container.transform.localScale = Vector3.one;

		//Instantiating the level selection buttons
		for (int i = 0; i < GameData.totalLevel; i++)//Till max number of of levels
		{
			GameObject levelBtn = Instantiate(levelButton,container.transform) as GameObject;

			levelBtn.SetActive(true);//By default I kept them deactivated

			//Updating the level tag
			levelBtn.GetComponentInChildren<Text>().text = (i + 1).ToString();

			Text ttext = levelBtn.GetComponentInChildren<Text>();

			//Setting the name as level count
			levelBtn.name = "level" + (i + 1);

			//Assigning the button action to each level button
			levelBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
				ClickLevel(levelBtn)
			);
		}

		GameObject btnConfirm = GameObject.Find ("confirm");
		if (btnConfirm != null) 
		{
			btnConfirm.GetComponentInChildren<Text> ().text = "continue";
		}
    }


    //refresh button states
    public void RefreshView()
    {

        for (int i = 0; i < GameData.totalLevel; i++)
        {
            GameObject tbtn = GameObject.Find("level" + (i+1));
            if (tbtn != null)
            {
				int tlevelButtonState = GameData.instance.levelStates[i];

				if (tlevelButtonState == 1)
                {
                    tbtn.GetComponent<Image>().color = new Color(1, 111f/225f, 0);
                }
                else 
				{
                    tbtn.GetComponent<Image>().color = Color.grey;
                }
            }
        }
    }
		

    // Clicks the level button.
    void ClickLevel(GameObject tbtn)
    {
//		Debug.Log ("clickLevel  tbtn: " + tbtn);

		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.CLICK);

        GameData.getInstance().currentLevel = int.Parse(tbtn.GetComponentInChildren<Text>().text) - 1;

		//Initializing the Arena where all the dots and Grid will be there
		UIManager.Instance ().linkDotGO.GetComponent<ConnectionHandler> ().InitializeGrid ();
			
		if (UIManager.Instance ().gamePanel != null) 
		{
			//Putting the main gameplay panel to centre position
			UIManager.Instance ().gamePanel.transform.position = Vector2.zero;
		}

		//Gameplay starts And all menu panel deactivated
		UIManager.Instance ().mainMenuPanel.SetActive (false);
    }


    // Set dots for pages.not used

    void SetPageDot()
    {
        for (int i = 0; i < pageDots.Count; i++)
        {
            pageDots[i].GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        }

        pageDots[page].GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }                    
}
