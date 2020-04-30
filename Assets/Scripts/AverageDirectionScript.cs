using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageDirectionScript : MonoBehaviour
{
	public GameObject line;
	[Tooltip("How many other views to simulate")]
	public int otherLines = 5; // number of fake lines to use
	public Color avgLineColor; // Color of the average line

	private Vector3 defaultPos;
	private List<float> lineRots; // Holds all z rotations
	private float avgRot; // Holds average z rotation
	private float stdDev; // Holds standard deviation

	// Start is called before the first frame update
	void Start()
	{
		defaultPos = new Vector3(0f, 0.25f, -0.1f);

		avgRot = 0f;
		stdDev = 0f;

		// init lineRots
		lineRots = new List<float>();
		lineRots.Add(line.transform.rotation.eulerAngles.z);
		for (int i = 0; i < otherLines; i++)
		{
			lineRots.Add(0f);
		}
	}

	// Update is called once per frame
	void Update()
	{
		UpdateLineRots();

		CalcAverage();

		CalcStdDev();

		UpdateAvgLine();
		
		//Debug.Log(avgRot.ToString("F3") + " +/- " + stdDev.ToString("F3"));
	}

	private void UpdateAvgLine()
	{
		/* * *
		 * Update rotation/position of line
		 * * */

		// Get rotations
		float rads = avgRot / 180f * Mathf.PI;
		//Debug.Log(degs.ToString("F3"));

		// Do math for x-axis
		Vector3 newXCompPos = defaultPos;

		newXCompPos.x = defaultPos.x * Mathf.Cos(rads) - defaultPos.y * Mathf.Sin(rads);
		newXCompPos.y = defaultPos.y * Mathf.Cos(rads) + defaultPos.x * Mathf.Sin(rads);
		//Debug.Log(newXCompPos.ToString("F3"));

		// Set values
		transform.localPosition = newXCompPos;
		transform.localRotation = Quaternion.Euler(0f, 0f, avgRot);

		/* * *
		 * Update color of line
		 * * */

		float diff = stdDev / 360f;

		Color newCol = new Color();

		newCol.r = diff + avgLineColor.r * (1 - diff);
		newCol.g = diff + avgLineColor.g * (1 - diff);
		newCol.b = diff + avgLineColor.b * (1 - diff);

		gameObject.GetComponent<Renderer>().material.color = newCol;
	}

	private void UpdateLineRots()
	{
		lineRots[0] = line.transform.rotation.eulerAngles.z;
		for (int i = 1; i < lineRots.Count; i++)
		{
			lineRots[i] = ClampRotation(lineRots[i] + UnityEngine.Random.Range(-1f, 1f));
		}
	}

	private float ClampRotation(float deg)
	{
		if (deg < 0f)
			return deg + 360f;

		if (deg > 360f)
			return deg - 360f;

		return deg;
	}

	private void CalcAverage()
	{
		avgRot = 0f;
		for (int i = 0; i < lineRots.Count; i++)
		{
			avgRot += lineRots[i];
		}
		avgRot /= lineRots.Count;
	}

	private void CalcStdDev()
	{
		stdDev = 0f;
		for (int i = 0; i < lineRots.Count; i++)
		{
			stdDev += ((lineRots[i]- avgRot)* (lineRots[i] - avgRot));
		}
		stdDev /= (lineRots.Count-1);
		stdDev = Mathf.Sqrt(stdDev);
	}
}
