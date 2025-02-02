﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class TouchHandler : MonoBehaviour 
	{
		public int tx,ty;
		bool colorPath = false;
		int id;
		void Start () 
		{
          
			id = ty * GameData.bsize + tx;
        }

		void OnMouseDown () 
		{
		//If game wins already No touches are taking
			if (GameData.instance.isWin) 
			{
				return;
			}
			if (GameData.instance.isHolding) 
			{
				return;
			}

			Transform dotChild = transform.Find ("dot");

			int tdotColor = GameData.getInstance().DotColorData [id];

			if (tdotColor != 0) 
			{
				//If i put the click to any dot then clearing any incomplete old path connected to this dot
				ClearOldPath(tdotColor);

				GameData.instance.pickColor = tdotColor;

				Color tcolor = GameData.instance.colors [tdotColor];

				tcolor.a = .5f;
                
				if (colorPath)
                {
                    transform.GetComponent<SpriteRenderer>().color = tcolor;
                }

				GameData.instance.ColorData [id] = tdotColor;

				GameData.instance.paths[tdotColor] = new List<int> ();//overwrite the old path

				AddPath(tdotColor,id);
			} 
			else 
			{
				int cBlockColor = GameData.instance.ColorData [id];

				if (cBlockColor == 0) 
				{
					//tap on a empty block,nothing should happen
					return;
				} 
				else 
				{
					//continue drawing on an already path block
					GameData.instance.pickColor = cBlockColor;
					//reopen the linkage
					GameData.instance.linkedLines[cBlockColor] = 0;

					for(int i = GameData.instance.paths[cBlockColor].Count - 1;i> 0;i--)
					{
						int oldid = GameData.instance.paths[cBlockColor] [i];

						if (oldid != id) 
						{
							//remove all paths until find the start one
							//and clear the useless path
							int oldx = Mathf.FloorToInt (oldid % GameData.bsize);

							int oldy = Mathf.FloorToInt (oldid / GameData.bsize);

							GameObject oldBg = GameObject.Find ("Dot" + oldx + "_" + oldy);

							Color tcolor = GameData.instance.colors [0];

							oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;

							GameData.instance.ColorData [oldid] = 0;

							RemovePath(cBlockColor,i);

							AddPath(cBlockColor,id);
						}
						else 
						{
							break;
						}
					}
				}
			}

			GameData.instance.isHolding = true;
			GameData.instance.startId = id;
			GameData.instance.lasttx = tx;
			GameData.instance.lastty = ty;

		//Checking for all the dots are conneted or not
            CheckForPossibleWinning();
        }


	//Clearing if any old paths are there 
		void ClearOldPath(int tcolorid)
		{
//			Debug.Log ("clearOldPath: " + tcolorid);

			int tlen = GameData.instance.paths[tcolorid].Count;

			while(GameData.instance.paths[tcolorid].Count > 0)
			{
				int oldid = GameData.instance.paths[tcolorid] [GameData.instance.paths[tcolorid].Count - 1];

					//and clear the old path
				int oldx = Mathf.FloorToInt (oldid % GameData.bsize);

				int oldy = Mathf.FloorToInt (oldid / GameData.bsize);

					GameObject oldBg = GameObject.Find ("Dot" + oldx + "_" + oldy);

					Color tcolor = GameData.instance.colors [0];

					oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;

					GameData.instance.ColorData [oldid] = 0;

			//Removing the path which was created
				RemovePath(tcolorid,GameData.instance.paths[tcolorid].Count-1);
				//reopen the Connection
				GameData.instance.linkedLines[tcolorid] = 0;
			}
		}


		void OnMouseOver()
		{
			if (GameData.instance.isWin)
				return;
			
			if (GameData.instance.isHolding) 
			{

				if (GameData.instance.pickColor != 0) 
				{
					//if dot here,get dot color
					Transform dotChild = transform.Find ("dot");

					int tdotColor = GameData.instance.DotColorData [id];

					//current block color;
					int tColorid = GameData.instance.ColorData [id];

				if ((tdotColor == 0 && tColorid == 0) || GameData.instance.pickColor == tdotColor && GameData.instance.paths [tdotColor] [0] != id) 
				{//all places which can be draw and have not draw on something
						
					//exclude not nearby blocks
					if ((Mathf.Abs (tx - GameData.instance.lasttx) == 1 && ty == GameData.instance.lastty) ||
					     (Mathf.Abs (ty - GameData.instance.lastty) == 1 && tx == GameData.instance.lasttx)) 
					{
						//Adding the color to the path
						AddColor ();
					}
				}
					else 
					{
						//draw on an already exist self color path
						if (tColorid != 0) 
						{
							int tlen = GameData.instance.paths[tColorid].Count;

							//exclude not nearby blocks
							if ((Mathf.Abs (tx - GameData.instance.lasttx) == 1 && ty == GameData.instance.lastty) ||
							    (Mathf.Abs (ty - GameData.instance.lastty) == 1 && tx == GameData.instance.lasttx)) 
							{
								if (tColorid == GameData.instance.pickColor) 
								{
									//draw on self old blocks
									//and clear the old path
									int oldId = GameData.instance.paths [tColorid] [GameData.instance.paths [tColorid].Count-1];

									while (oldId!= id) 
									{
										int oldx = Mathf.FloorToInt (oldId % GameData.bsize) ;

										int oldy = Mathf.FloorToInt (oldId / GameData.bsize);

										GameObject oldBg = GameObject.Find ("Dot" + oldx + "_" + oldy);

										Color tcolor = GameData.instance.colors [0];

										oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;

									   //Removing the path
										RemovePath(tColorid,GameData.instance.paths[tColorid].Count-1);

										GameData.instance.ColorData [oldId] = 0;

											//next prev id
										oldId = GameData.instance.paths [tColorid] [GameData.instance.paths [tColorid].Count - 1];

										GameData.instance.lasttx = tx;
										GameData.instance.lastty = ty;
									}
								} 
								else 
								{
									//draw on other block lines,cut them
									//clear the being cutted other color old paths
									int tOtherColorId = GameData.instance.ColorData[id];

									int oldId = GameData.instance.paths [tOtherColorId] [GameData.instance.paths [tOtherColorId].Count-1];
									
									if (GameData.instance.DotColorData[oldId] == 0 || tOtherColorId != GameData.instance.pickColor) 
									{
										//if this place doesnt have a dot or this place is a different color
										if (GameData.instance.DotColorData [id] == 0) 
										{	//make wont draw on other color dots

											while (oldId != id) 
											{
												int oldx = Mathf.FloorToInt (oldId % GameData.bsize);

												int oldy = Mathf.FloorToInt (oldId / GameData.bsize);

												GameObject oldBg = GameObject.Find ("Dot" + oldx + "_" + oldy);

												Color tcolor = GameData.instance.colors [0];

												oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;
											
												RemovePath(tOtherColorId,GameData.instance.paths[tOtherColorId].Count-1);

												GameData.instance.ColorData [oldId] = 0;

												//next prev id
												oldId = GameData.instance.paths [tOtherColorId] [GameData.instance.paths [tOtherColorId].Count - 1];
											}

											if (oldId == id) 
											{
												int oldx = Mathf.FloorToInt (oldId % GameData.bsize);

												int oldy = Mathf.FloorToInt (oldId / GameData.bsize);

												GameObject oldBg = GameObject.Find ("Dot" + oldx + "_" + oldy);

												Color tcolor = GameData.instance.colors [0];

												oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;

												RemovePath(tOtherColorId,GameData.instance.paths[tOtherColorId].Count-1);
											
												GameData.instance.ColorData [oldId] = 0;

											}
										}
									}
								}
							}
						}
					}

				}
				CheckForPossibleWinning ();
			}
		}

	//Creating color to the path which created
		private void AddColor()
		{
			int tdotColor = GameData.instance.DotColorData [id];

			if (tdotColor!= 0 && tdotColor != GameData.instance.pickColor)//draw on other color dots(not block)
			{
				GameData.instance.isHolding = false;

				GameData.instance.pickColor = 0;

				return;
			} 
				
			Color tcolor = GameData.instance.colors [GameData.instance.pickColor];
		
			tcolor.a = .5f;
           
			if (colorPath)
            {
                transform.GetComponent<SpriteRenderer>().color = tcolor;
            }

			AddPath(GameData.instance.pickColor,id);

			GameData.instance.ColorData [id] = GameData.instance.pickColor;//write color to data

			if (tdotColor != 0 && tdotColor == GameData.instance.pickColor && GameData.instance.paths [tdotColor].Count > 1) 
			{
				GameData.instance.linkedLines [tdotColor] = 1;

				GameData.instance.pickColor = 0;
			}
				
			GameData.instance.lasttx = tx;

			GameData.instance.lastty = ty;
		}

	//Mouse up So no more path creating or removing
		private	void OnMouseUp()
		{
			if (GameData.instance.isWin)
				
				return;
			
			GameData.instance.isHolding = false;
		}
		

		void AddPath(int colorId,int placeId)
		{
           
		GameData.instance.paths [colorId].Add (placeId);

		int tlen = GameData.instance.paths [colorId].Count;

			if (tlen > 1) 
			{
				int tlastId1 = GameData.instance.paths [colorId] [tlen - 2];

				int tlastId2 = GameData.instance.paths [colorId] [tlen - 1];

				int tx = Mathf.FloorToInt (tlastId1 % GameData.bsize);

				int ty = Mathf.FloorToInt (tlastId1 / GameData.bsize);

				int placeOffset = tlastId2 - tlastId1;

				int tRight = 1;

				int tLeft = -1;

				int tUp = GameData.bsize;//paths go up

				int tDown = -GameData.bsize;

				GameObject tlink = null;

				if (placeOffset == 1) 
				{
					tlink = GameObject.Find("ConnectorRight"+tx+"_"+ty);
				} 
				else if (placeOffset == -1) 
				{
					tlink = GameObject.Find("ConnectorLeft"+tx+"_"+ty);
				} 
				else if (placeOffset == tUp) 
				{
					tlink = GameObject.Find("ConnectorUp"+tx+"_"+ty);
				} 
				else if (placeOffset == tDown) 
				{
					tlink = GameObject.Find("ConnectorDown"+tx+"_"+ty);
				}
			if (tlink != null) 
			{
				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [colorId];
				SoundManager.Instance ().playSound (SoundManager.SOUND_ID.TOUCH);
			}
			}
		}

		private void RemovePath(int colorId,int index)
		{
		
			//clear all the connections on this node
			int tlastId = GameData.instance.paths [colorId] [index];

			GameObject tlink = null;

			int tx = Mathf.FloorToInt (tlastId % GameData.bsize);

			int ty = Mathf.FloorToInt (tlastId / GameData.bsize);

			tlink = GameObject.Find ("ConnectorDown" + tx + "_" + ty);

			tlink = GameObject.Find ("ConnectorLeft" + tx + "_" + ty);

			tlink = GameObject.Find ("ConnectorRight" + tx + "_" + ty);

			tlink = GameObject.Find ("ConnectorUp" + tx + "_" + ty);

			tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];

			tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];

			tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];

			tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];

			GameData.instance.paths[colorId].RemoveAt (index);

			if (index > 0) 
			{
				tlastId = GameData.instance.paths [colorId] [index - 1];
			
				tx = Mathf.FloorToInt (tlastId % GameData.bsize);

				ty = Mathf.FloorToInt (tlastId / GameData.bsize);
	
				tlink = GameObject.Find ("ConnectorRight" + tx + "_" + ty);

				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];

				tlink = GameObject.Find ("ConnectorLeft" + tx + "_" + ty);

				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];

				tlink = GameObject.Find ("ConnectorUp" + tx + "_" + ty);

				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];

				tlink = GameObject.Find ("ConnectorDown" + tx + "_" + ty);

				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];
			}

            GameData.instance.linkedLines[colorId] = 0;//reopen the linkage
        }
			
		private void CheckForPossibleWinning()
		{

			int nwin = 0;

			for (int k = 0; k < GameData.instance.linkedLines.Length; k++) 
			{
				if (GameData.instance.linkedLines [k] == 1) 
				{
				//Number of connection increased
					nwin++;
				}
			}

			if(nwin >= GameData.instance.winLinkCount)
			{
				//All connection is done
				GameData.instance.isHolding = false;

				GameData.instance.isWin = true;

			//Winning popup showing here
				UIManager.Instance ().GameWin ();
			}
		}
	}
