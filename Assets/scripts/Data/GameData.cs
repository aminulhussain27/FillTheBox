using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : ScriptableObject
{
    public int nLink = 0; //check in game.When nlink = 0.All the lines linked,so win.
    public int levelCompleted = 0;//how much level you passed
    public int currentLevel = 0;//currect level
	public int startId = -1;
	public int lasttx = -1;
	public int lastty = -1;
	public int winLinkCount;//how many linkage requires to win(commonly its half of the number of dots)
	public int pickColor = -1;

	public int[] ColorData;
	public int[] DotColorData;
	public int[] linkedLines;//check all colors whehter connected or not

	public List<int> levelStates;
	public List<int> lvStar = new List<int>();//level stars you got for each level
	public List<List<int>> paths;
    
	public static int totalLevel = 10;//total levels,currently,we make things easier,only use 50 levels the same for each difficulty
	public static int bsize = 6;


	public bool isWin = false;//check if win
	public bool isfail = false;//whether the game failed
	public bool isHolding = false;

	public Color[] colors;

	public SimpleJSON.JSONNode dotPoses;

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
        string tData = Datas.CreateInstance<Datas>().GetData()[GameData.instance.currentLevel];//Getting the TextAsset data A/C level
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
		
    public void ClearData()
    {
		foreach(Transform tobj in UIManager.Instance ().linkDotContainer.transform)
        {
            Destroy(tobj.gameObject);
        }     
    }
}
