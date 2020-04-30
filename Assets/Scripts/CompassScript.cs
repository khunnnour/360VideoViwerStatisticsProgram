using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour
{
	public GameObject xArrow, yArrow;
	public Text camName;

	private CameraSwitchScript camManager;
	private Vector3 defaultPos;
	// Start is called before the first frame update
	void Start()
	{
		camManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraSwitchScript>();

		defaultPos = new Vector3(0f, 0.25f, -0.1f);
	}

	// Update is called once per frame
	void Update()
	{
		GameObject cam = camManager.GetCurrentCamera();

		// Get name
		camName.text = "Current Camera: " + cam.name;

		// Get rotations
		Vector2 degs = cam.transform.rotation.eulerAngles;
		Vector2 rads = degs / 180f * Mathf.PI;
		//Debug.Log(degs.ToString("F3"));

		// Do math for x-axis
		Vector3 newXCompPos = defaultPos;

		newXCompPos.x = defaultPos.x * Mathf.Cos(rads.x) - defaultPos.y * Mathf.Sin(rads.x);
		newXCompPos.y = defaultPos.y * Mathf.Cos(rads.x) + defaultPos.x * Mathf.Sin(rads.x);
		//Debug.Log(newXCompPos.ToString("F3"));

		// Do math for y-axis
		Vector3 newYCompPos = defaultPos;

		newYCompPos.x = defaultPos.x * Mathf.Cos(rads.y) - defaultPos.y * Mathf.Sin(rads.y);
		newYCompPos.y = defaultPos.y * Mathf.Cos(rads.y) + defaultPos.x * Mathf.Sin(rads.y);

		// Set values
		xArrow.transform.localPosition = newXCompPos;
		xArrow.transform.localRotation = Quaternion.Euler(0f, 0f, degs.x);

		yArrow.transform.localPosition = newYCompPos;
		yArrow.transform.localRotation = Quaternion.Euler(0f, 0f, degs.y);
	}
}