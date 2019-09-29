using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using linkDot;
public class TestMouseTouch : MonoBehaviour {

    public GameObject linkdotgame;
	void OnMouseDown()
    {
       
        if (linkdotgame != null)
        {
            //init the level
            GameData.getInstance().cLevel = 0;//0-49;


            linkdotgame.SetActive(true);
            linkdotgame.GetComponent<LinkDot>().init();
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    void linkDotWin()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
