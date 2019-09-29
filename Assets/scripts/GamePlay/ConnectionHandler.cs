﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
namespace linkDot{
	public class ConnectionHandler : MonoBehaviour {

        public void InitializeGrid() 
		{
			Debug.Log ("InitializeGrid");

			UIManager.Instance().levelText.text = (GameData.getInstance().currentLevel + 1).ToString();

			UIManager.Instance().linkDotContainer.SetActive (true);

            GameData.getInstance().ClearData();
         
			GameData.instance.ResetData();

			GameData.getInstance().InitializeGameData();


            GameObject tBg = Resources.Load("Prefabs/square") as GameObject;
            float gridW = GetComponent<SpriteRenderer>().sprite.bounds.size.x / GameData.bsize;
            
            float orginSize = tBg.GetComponent<SpriteRenderer>().bounds.size.x;
            float tscale = gridW / tBg.GetComponent<SpriteRenderer>().bounds.size.x;
            

            float gap = tBg.GetComponent<SpriteRenderer>().bounds.size.x * (1f - tscale);

            GameObject tCircle = Resources.Load("Prefabs/dot") as GameObject;
            GameObject tLink = Resources.Load("Prefabs/Connector") as GameObject;


            float offsetx = -1 * gridW * GameData.bsize / 2 + gridW / 2;// 0;// gridW * 3f;
            float offsety = offsetx;//0;// gridW * 2f;
           
			List<GameObject> tbgs = new List<GameObject>();
            
			for (int i = 0; i < GameData.bsize * GameData.bsize; i++)
            {
                int tx = Mathf.FloorToInt(i % GameData.bsize);
                int ty = Mathf.FloorToInt(i / GameData.bsize);
				GameObject tbg = Instantiate(tBg, UIManager.Instance().linkDotContainer.transform);
                tbg.transform.localScale *= tscale;
				tbg.transform.localPosition = new Vector2(UIManager.Instance().linkDotContainer.transform.localPosition.x + gridW * tx + offsetx, UIManager.Instance().linkDotContainer.transform.localPosition.y + gridW * ty + offsety);
                tbg.GetComponent<SpriteRenderer>().sortingOrder = 1;
                tbg.name = "Dot" + tx + "_" + ty;
                tbg.GetComponent<SpriteRenderer>().color = Color.clear;
                tbgs.Add(tbg);

                tbg.gameObject.AddComponent<BoxCollider>();
                tbg.gameObject.AddComponent<TouchHandler>();

                tbg.gameObject.GetComponent<TouchHandler>().tx = tx;
                tbg.gameObject.GetComponent<TouchHandler>().ty = ty;

                GameData.instance.ColorData[i] = 0;//no color
                GameData.instance.DotColorData[i] = 0;//no color

                int[] rotation = new int[] { 0, 90, 180, 270 };
                for (int j = 0; j < 4; j++)
                {//add 4 link lines to each square
					GameObject tlink = Instantiate(tLink, UIManager.Instance().linkDotContainer.transform);
                    
                    tlink.transform.localPosition = tbg.transform.localPosition;
                    tlink.transform.localScale = tbg.transform.localScale;
                    tlink.transform.localEulerAngles = new Vector3(0, 0, rotation[j]);

                    tlink.GetComponent<SpriteRenderer>().color = Color.clear;
                    tlink.GetComponent<SpriteRenderer>().sortingOrder = 2;
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
            for(int i = 0;i<GameData.instance.dotPoses.Count;i++) //(string tdotPoses in GameData.instance.dotPoses)
            {
                string[] pos = new string[2];
                pos[0] = GameData.instance.dotPoses[i]["v"][0]["x"];//
                pos[1] = GameData.instance.dotPoses[i]["v"][0]["y"];//

                if (pos[0] == null || pos[0] == "") pos[0] = "0";
                if (pos[1] == null || pos[1] == "") pos[1] = "0";

                int tx = int.Parse(pos[0]);
                int ty = int.Parse(pos[1]);

                int tindex = ty * GameData.bsize + tx;
   
                GameObject tcircle = Instantiate(tCircle, tbgs[tindex].transform) as GameObject;

				tcircle.transform.localScale *= 0.9f;
                tcircle.GetComponent<SpriteRenderer>().sortingOrder = 3;
                tcircle.GetComponent<SpriteRenderer>().color = GameData.instance.colors[i+1];

                tcircle.name = "dot";

                //start anim
                Vector3 tcScale = tcircle.transform.localScale;
                tcircle.transform.localScale = Vector3.zero;
                float tdelay = n * .1f;

                tcircle.transform.DOScale(tcScale, 1).SetDelay(tdelay).SetEase(Ease.OutBounce);

                GameData.instance.DotColorData[tindex] = i + 1;


                int tcount = GameData.instance.dotPoses[i]["v"].Count;
                pos[0] = GameData.instance.dotPoses[i]["v"][tcount-1]["x"];//
                pos[1] = GameData.instance.dotPoses[i]["v"][tcount-1]["y"];//

                if (pos[0] == null || pos[0] == "") pos[0] = "0";
                if (pos[1] == null || pos[1] == "") pos[1] = "0";

                tx = int.Parse(pos[0]);
                ty = int.Parse(pos[1]);

                tindex = ty * GameData.bsize + tx;
                tcircle = Instantiate(tCircle, tbgs[tindex].transform) as GameObject;


                tcircle.GetComponent<SpriteRenderer>().sortingOrder = 3;
                tcircle.GetComponent<SpriteRenderer>().color = GameData.instance.colors[i+1];

                tcircle.name = "dot";

                //start anim
                tcScale = tcircle.transform.localScale;
                tcircle.transform.localScale = Vector3.zero;
                tdelay = n * .1f;

                tcircle.transform.DOScale(tcScale, 1).SetDelay(tdelay).SetEase(Ease.OutBounce);

                GameData.instance.DotColorData[tindex] = i+1;
            }

//			transform.localScale = Vector3.one;
//			transform.localPosition = Vector3.zero;
			transform.localScale = new Vector3 (1.65f, 1.6f, 1);
//			container.transform.localPosition = new Vector3 (0, -1.25f, 3.584f);
		}
			

        public void clear()
        {
			if (UIManager.Instance().linkDotContainer != null)
            {
				foreach (Transform tobj in UIManager.Instance().linkDotContainer.transform)
                {
                    tobj.DOScale(Vector3.zero, .2f);
                }
            }
        }
	}
}