using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : ScriptableObject
{
    public int nLink = 0; //check in game.When nlink = 0.All the lines linked,so win.
    public int levelPassed = 0;//how much level you passed
    public int cLevel = 0;//currect level
    
    public static int totalLevel = 10;//total levels,currently,we make things easier,only use 50 levels the same for each difficulty
	public List<int> levelStates;
	public bool isWin = false;//check if win
	public List<int> lvStar = new List<int>();//level stars you got for each level
	public bool isfail = false;//whether the game failed

	private SimpleJSON.JSONNode levelData;

    public static GameData instance;
    public static GameData getInstance()
    {
        if (instance == null)
        {
            instance = ScriptableObject.CreateInstance<GameData>();
        }
        return instance;
    }


    public void ResetData()
    {
        isWin = false;
        isfail = false;

        //reset level pass states
        GameData.instance.levelStates = new List<int>();

		for (int j = 0; j < GameData.totalLevel; j++) 
		{
			int tState = PlayerPrefs.GetInt ("linkdot_" + 1 + "_" + j);
		
			GameData.instance.levelStates.Add (tState);
		}
    }
		

    public void InitializeGameData()
    {
        string tData = Datas.CreateInstance<Datas>().GetData()[GameData.instance.cLevel];//Getting the TextAsset data A/C level
        levelData = SimpleJSON.JSONArray.Parse(tData);

        bsize = int.Parse(levelData["r"]);

		//The required color array
       	colors = new Color[] { Color.clear, Color.red, Color.green,Color.blue, new Color(1f, 165f/255f , 0), new Color(128 / 255f, 0 , 128 / 255f) };

		ColorData = new int[bsize * bsize];
        DotColorData = new int[bsize * bsize];

        dotPoses = levelData["l"];//this is the  pathe between 2 dots

        GameData.instance.paths = new List<List<int>>();
        List<int> tpath0 = new List<int>();//the clear color path

        GameData.instance.paths.Add(tpath0);

        for (int i = 0; i < dotPoses.Count; i++)
        {
            List<int> tpath = new List<int>();
            GameData.instance.paths.Add(tpath);
        }
        winLinkCount = dotPoses.Count;

        linkedLines = new int[GameData.instance.paths.Count+1];
    }
		
    public bool isHolding = false;
    public int pickColor = -1;

//    [HideInInspector]
    public SimpleJSON.JSONNode dotPoses;
//    [HideInInspector]
    public int[] ColorData;
//    [HideInInspector]
    public int[] DotColorData;
//    [HideInInspector]
    public Color[] colors;
//    [HideInInspector]
    public int startId = -1;
//    [HideInInspector]
    public int lasttx = -1;
//    [HideInInspector]
    public int lastty = -1;
//    [HideInInspector]
    public List<List<int>> paths;
//    [HideInInspector]
    public int[] linkedLines;//check all colors whehter links
//    [HideInInspector]
    public static int bsize = 6;
//    [HideInInspector]
    public int winLinkCount;//how many linkage requires to win(commonly its half of the number of dots)


    public void ClearData()
    {
		Transform tcontainer = UIManager.Instance ().linkDotContainer.transform;
        foreach(Transform tobj in tcontainer)
        {
            Destroy(tobj.gameObject);
        }     
    }
}
