using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookScript : MonoBehaviour
{
	//public bool useClamp = false;
	//public Vector2 minTurn, maxTurn;
	[Range(0.1f, 1.0f)]
	public float sensitivty = 1.0f;
	[Range(0.1f, 1f)]
	public float zoomSensitivty = 0.7f;
	[Range(0.1f, 1.0f)]
	public float idleIntensity = 0.5f;

	private Camera cam;
	private bool dragging, demoMode;
	private Vector3 dragOrigin, idleDir;
	//private Vector2 minTurnSerial, maxTurnSerial;

	// Start is called before the first frame update
	void Start()
	{
		demoMode = false;
		dragging = false;
		dragOrigin = Vector3.zero;

		cam = GetComponent<Camera>();

		//minTurnSerial = minTurn;
		//maxTurnSerial = maxTurn;
		// turn min/max into 0-360 if needed
		//if (minTurnSerial.x < 0f)
		//	minTurnSerial.x = 360f + minTurnSerial.x;
		//if (minTurnSerial.y < 0f)
		//	minTurnSerial.y = 360f + minTurnSerial.y;
		//
		//if (maxTurnSerial.x < 0f)
		//	maxTurnSerial.x = 360f + maxTurnSerial.x;
		//if (maxTurnSerial.y < 0f)
		//	maxTurnSerial.y = 360f + maxTurnSerial.y;

		MakeNewIdleDir();
	}

	public void SetIdle(bool idle)
	{
		demoMode = idle;
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();

		if (demoMode)
		{
			Idle();
		}
		else if (dragging)
		{
			Drag();
		}
	}

	public void MakeNewIdleDir()
	{
		idleDir = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-0.5f, 0.5f), 0f);
		idleDir.Normalize();
	}

	// Essentially a drag with random drag values
	private void Idle()
	{
		Vector3 change = idleDir * idleIntensity * 0.1f;
		//Debug.Log(change.ToString("F2"));

		Vector3 newAngles = transform.rotation.eulerAngles;
		newAngles.x -= change.y;
		newAngles.y += change.x;
		newAngles.z = 0f;

		transform.rotation = Quaternion.Euler(newAngles);
	}

	private void Drag()
	{
		Vector3 currPos = Input.mousePosition;

		Vector3 change = (dragOrigin - currPos) * (sensitivty * 0.4f);

		Vector3 newAngles = transform.rotation.eulerAngles;
		newAngles.x -= change.y;
		newAngles.y += change.x;
		newAngles.z = 0f;

		//clampRotation(ref newAngles);

		if (newAngles.x < 0f)
			newAngles.x += 360f;
		if (newAngles.x > 360f)
			newAngles.x -= 360f;

		if (newAngles.y < 0f)
			newAngles.y += 360f;
		if (newAngles.y > 360f)
			newAngles.y -= 360f;

		transform.rotation = Quaternion.Euler(newAngles);

		dragOrigin = Input.mousePosition;
	}

	private void Zoom(float dir)
	{
		cam.fieldOfView += (zoomSensitivty * 50f * -dir);
	}

	// Max sure you haven't rotated outside of the bounds
	private void clampRotation(ref Vector3 angles)
	{
		Debug.Log(angles.ToString("F3"));


	}

	private void GetInput()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			dragging = true;
			dragOrigin = Input.mousePosition;
		}

		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			dragging = false;
		}

		Zoom(Input.GetAxis("Mouse ScrollWheel"));
	}
}