using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchScript : MonoBehaviour
{
	public Camera[] cameras;
	public int startingCamera = 0;
	[Header("Idle Settings")]
	public float cameraSwitchTime = 2f;
	public float range = 1f;

	int activeCamIndex;
	bool demoMode;
	float idleTimer, targetTime;

	// Start is called before the first frame update
	private void Start()
	{
		activeCamIndex = startingCamera;

		for (int i = 0; i < cameras.Length; i++)
			cameras[i].gameObject.SetActive(false);

		if (startingCamera == -1)
		{
			demoMode = true;
			cameras[0].gameObject.SetActive(true);
		}
		else
		{
			demoMode = false;
			cameras[startingCamera].gameObject.SetActive(true);
		}

		idleTimer = 0f;
		targetTime = UnityEngine.Random.Range(cameraSwitchTime - range, cameraSwitchTime + range);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			changeCamera(0);
			ToggleCameraIdle(false);
			demoMode = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			changeCamera(1);
			ToggleCameraIdle(false);
			demoMode = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			changeCamera(2);
			ToggleCameraIdle(false);
			demoMode = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			changeCamera(3);
			ToggleCameraIdle(false);
			demoMode = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			ToggleCameraIdle(true);
			demoMode = true;
		}

		if (demoMode)
			RunIdle();
	}

	// randomly move camera and switch cameras
	private void RunIdle()
	{
		idleTimer += Time.deltaTime;

		if (idleTimer >= targetTime)
		{
			// change to random camera
			changeCamera(UnityEngine.Random.Range(0, cameras.Length));
			
			// make camera gen a new idle direciton
			cameras[activeCamIndex].GetComponent<CameraLookScript>().MakeNewIdleDir();

			// reset change timer
			targetTime = UnityEngine.Random.Range(cameraSwitchTime - range, cameraSwitchTime + range);
			idleTimer = 0f;
		}
	}

	// toggle idle state of all cameras
	void ToggleCameraIdle(bool state)
	{
		foreach(Camera c in cameras)
		{
			c.GetComponent<CameraLookScript>().SetIdle(state);
		}
	}

	public void changeCamera(int index)
	{
		if (index >= 0 && index < cameras.Length)
			switchCamera(index);
		else
			Debug.LogError("Index out of bounds of camera array");
	}

	private void switchCamera(int index)
	{
		cameras[activeCamIndex].gameObject.SetActive(false);

		activeCamIndex = index;
		cameras[activeCamIndex].gameObject.SetActive(true);
	}

	public GameObject GetCurrentCamera()
	{
		return cameras[activeCamIndex].gameObject;
	}
}
