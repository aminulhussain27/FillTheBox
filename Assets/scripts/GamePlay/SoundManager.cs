using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour
{
	static GameObject soundObj = null;

	private static SoundManager soundManager = null;
	//Creating singleton for use in different positions and for making sure only one instace is created
	public static SoundManager Instance ()
	{		
		if (soundManager == null)
		{
			if (GameObject.Find ("SoundManager") == null)
			{
				soundObj = new GameObject ("Sounds");
			}
			soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
		}
		return soundManager;
	}

	GameObject soundObject = null;
	public AudioClip[] musicClips;//In sce attaching the sound clips


	public void playSound (SoundManager.SOUND_ID id, float volume =1f, bool isLoop = false)
	{
		//A new gameobject is creating and attaching audioSource here
		soundObject = new  GameObject ("Sound");

		soundObject.transform.SetParent (transform);

		soundObject.AddComponent<AudioSource> ();
		AudioSource audioSource = soundObject.GetComponent<AudioSource> ();
		audioSource.clip = musicClips [(int)id];
		audioSource.Play ();

		//If in some place i need custom volume
		if(volume != 1f)
		{
			audioSource.volume = volume;
		}
		if (isLoop) 
		{
			audioSource.loop = true;
		} 
		else {
			//Destroying the sound object after fully playing
			Destroy (soundObject, audioSource.clip.length);
		}
	}

	public void StopallSound()
	{

		for (int i = 1; i <= transform.childCount; i++) 
		{
			Destroy (transform.GetChild(i-1).gameObject);
		}

	}


	//These are the Clip Id
	public enum SOUND_ID
	{
		NONE = -1,
		LOOP_BACKGROUND = 0,
		CLICK = 1,
		WIN = 2,
		LEVEL_LOADED = 3,
		TOUCH = 4,
	}
}