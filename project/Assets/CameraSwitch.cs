using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour {
	
	public GameObject cameraOne;
	public GameObject cameraTwo;
	
	AudioListener cameraOneAudioLis;
	AudioListener cameraTwoAudioLis;
	
	// Use this for initialization
	void Start()
	{
		cameraOneAudioLis = cameraOne.GetComponent<AudioListener>();
		cameraTwoAudioLis = cameraTwo.GetComponent<AudioListener>();

		cameraPositionChange(PlayerPrefs.GetInt("CameraPosition"));
	}
	
	// Update is called once per frame
	void Update()
	{
		switchCamera();
	}
	
	void switchCamera()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			cameraChangeCounter();
		}
	}

	void cameraChangeCounter()
	{
		int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
		cameraPositionCounter++;
		cameraPositionChange(cameraPositionCounter);
	}

	void cameraPositionChange(int camPosition)
	{
		if (camPosition > 1)
		{
			camPosition = 0;
		}

		PlayerPrefs.SetInt("CameraPosition", camPosition);

		if (camPosition == 0)
		{
			cameraOne.SetActive(true);
			cameraOneAudioLis.enabled = true;
			
			cameraTwoAudioLis.enabled = false;
			cameraTwo.SetActive(false);
		}

		if (camPosition == 1)
		{
			cameraTwo.SetActive(true);
			cameraTwoAudioLis.enabled = true;
			
			cameraOneAudioLis.enabled = false;
			cameraOne.SetActive(false);
		}
		
	}
}