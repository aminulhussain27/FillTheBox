﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class ConnectionHandler : MonoBehaviour 
{

	public GameObject squarePrefab;
	public GameObject dotPrefab;
	public GameObject connectorPrefab;

  public void InitializeGrid()
	{
//		Debug.Log ("InitializeGrid");

		UIManager.Instance ().selectLevelButton.interactable = false;

		UIManager.Instance ().levelText.text = (GameData.getInstance ().currentLevel + 1).ToString ();

		UIManager.Instance ().linkDotContainer.SetActive (true);

		GameData.getInstance ().ClearData ();
         
		GameData.instance.ResetData ();

		GameData.getInstance ().InitializeGameData ();

		//Setting the dots to its position
		StartCoroutine (InitializeDots ());

		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.LEVEL_LOADED, 0.25f);
	}

	IEnumerator InitializeDots()
	{
		//Attached the prefabs require to creating the path and dots
		GameObject tBg = squarePrefab;
		GameObject tCircle = dotPrefab;
		GameObject tLink = connectorPrefab;

		float gridW = GetComponent<SpriteRenderer> ().sprite.bounds.size.x / GameData.bsize;
            
		float orginSize = tBg.GetComponent<SpriteRenderer> ().bounds.size.x;

		float tscale = gridW / tBg.GetComponent<SpriteRenderer> ().bounds.size.x;

		float gap = tBg.GetComponent<SpriteRenderer> ().bounds.size.x * (1f - tscale);

		float offsetx = -1 * gridW * GameData.bsize / 2 + gridW / 2;// 0;// gridW * 3f;

		float offsety = offsetx;
           
		List<GameObject> tbgs = new List<GameObject> ();
            
		yield return new WaitForSeconds (0.25f);

		for (int i = 0; i < GameData.bsize * GameData.bsize; i++) 
		{
			int tx = Mathf.FloorToInt (i % GameData.bsize);

			int ty = Mathf.FloorToInt (i / GameData.bsize);

			GameObject tbg = Instantiate (tBg, UIManager.Instance ().linkDotContainer.transform);

			tbg.transform.localScale *= tscale;
			tbg.transform.localPosition = new Vector2 (UIManager.Instance ().linkDotContainer.transform.localPosition.x + gridW * tx + offsetx, UIManager.Instance ().linkDotContainer.transform.localPosition.y + gridW * ty + offsety);
			tbg.GetComponent<SpriteRenderer> ().sortingOrder = 1;
			tbg.name = "Dot" + tx + "_" + ty;
			tbg.GetComponent<SpriteRenderer> ().color = Color.clear;
			tbgs.Add (tbg);

			//Adding collider to those squares
			tbg.gameObject.AddComponent<BoxCollider> ();
			//Adding the touch handler script to each of  this square prefab created
			tbg.gameObject.AddComponent<TouchHandler> ();

			tbg.gameObject.GetComponent<TouchHandler> ().tx = tx;
			tbg.gameObject.GetComponent<TouchHandler> ().ty = ty;

			GameData.instance.ColorData [i] = 0;//no color
			GameData.instance.DotColorData [i] = 0;//no color

			//In each side: Left,Right, Up, Down rotation array is created 
			int[] rotation = new int[] { 0, 90, 180, 270 };


			//Adding the 4 connector to each of the square for possible connection
			for (int j = 0; j < 4; j++) 
			{
				//Instantiating the square as connector
				GameObject tlink = Instantiate (tLink, UIManager.Instance ().linkDotContainer.transform);
                    
				tlink.transform.localPosition = tbg.transform.localPosition;
				tlink.transform.localScale = tbg.transform.localScale;
				tlink.transform.localEulerAngles = new Vector3 (0, 0, rotation [j]);

				tlink.GetComponent<SpriteRenderer> ().color = Color.clear;
				tlink.GetComponent<SpriteRenderer> ().sortingOrder = 2;
				switch (j) 
				{
					case 0://right
						tlink.name = "ConnectorRight" + tx + "_" + ty;
						break;
					case 1://up
						tlink.name = "ConnectorUp" + tx + "_" + ty;
						break;
					case 2://left
						tlink.name = "ConnectorLeft" + tx + "_" + ty;
						break;
					case 3://down
						tlink.name = "ConnectorDown" + tx + "_" + ty;
						break;
				}
			}
		}
			
         int n = 1;//because 0 is no color
         
		//Creating all dots here
		for (int i = 0; i < GameData.instance.dotPoses.Count; i++) 
		{ 

			string[] pos = new string[2];

			//Getting dot positions from game data
			pos [0] = GameData.instance.dotPoses [i] ["v"] [0] ["x"];
			pos [1] = GameData.instance.dotPoses [i] ["v"] [0] ["y"];

			if (pos [0] == null || pos [0] == "") {
				pos [0] = "0";
			}

			if (pos [1] == null || pos [1] == "") {
				pos [1] = "0";
			}

			int tx = int.Parse (pos [0]);
			int ty = int.Parse (pos [1]);

			int tindex = ty * GameData.bsize + tx;
   
			#region FIRST_DOT
			//Creating the dots here, In some particular place these will be present(2 of each color)
			GameObject tcircle = Instantiate (tCircle, tbgs [tindex].transform) as GameObject;

			//Rescaling for proper orientation
			tcircle.transform.localScale *= 0.9f;
			//adding all of the dots to sorting layer 3(Bringing the front)
			tcircle.GetComponent<SpriteRenderer> ().sortingOrder = 3;
			tcircle.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [i + 1];

			//Giving the name as dot
			tcircle.name = "dot";

			#endregion

			#region SECOND_DOT
			GameData.instance.DotColorData [tindex] = i + 1;

			int tcount = GameData.instance.dotPoses [i] ["v"].Count;
			pos [0] = GameData.instance.dotPoses [i] ["v"] [tcount - 1] ["x"];
			pos [1] = GameData.instance.dotPoses [i] ["v"] [tcount - 1] ["y"];

			if (pos [0] == null || pos [0] == "")
				pos [0] = "0";
			if (pos [1] == null || pos [1] == "")
				pos [1] = "0";

			tx = int.Parse (pos [0]);
			ty = int.Parse (pos [1]);

			tindex = ty * GameData.bsize + tx;
			//Instantiating another dot here
			tcircle = Instantiate (tCircle, tbgs [tindex].transform) as GameObject;

			tcircle.GetComponent<SpriteRenderer> ().sortingOrder = 3;
			tcircle.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [i + 1];
			tcircle.name = "dot";
			#endregion

			GameData.instance.DotColorData [tindex] = i + 1;
		}
			
		//Adding some offset for proper positioning
		transform.localScale = new Vector3 (1.65f, 1.6f, 1);

		UIManager.Instance ().selectLevelButton.interactable = true;
	}
}
