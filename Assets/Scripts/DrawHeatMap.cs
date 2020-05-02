using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawHeatMap : MonoBehaviour
{
	public Texture2D heatMap;

	private List<HeatMapFrame> frames;
	private int currFrame;
	private bool finished;

	// Start is called before the first frame update
	void Awake()
	{
		frames = new List<HeatMapFrame>();
		finished = false;

		currFrame = 0;
		//ChangeFrame(); // switch to first frame
	}

	// Update is called once per frame
	void Update()
	{
		CheckFrameStatus();

		if (finished)
			CheckForNewFrames();
	}

	private void CheckForNewFrames()
	{
		if (currFrame < frames.Count)
			finished = false;
	}

	private void CheckFrameStatus()
	{
		// check if passed current frame
		if (Time.time >= frames[currFrame].timeStamp && !finished)
			ChangeFrame();
	}

	private void ChangeFrame()
	{
		if (currFrame + 1 >= frames.Count)
		{
			finished = true;
		}
		else
		{
			currFrame++;
			heatMap.SetPixels(frames[currFrame].colors);
			heatMap.Apply();
			Debug.Log("Flipped to frame " + currFrame);
		}
	}

	public void AddFrame(HeatMapFrame frm)
	{
		frames.Add(frm);
	}
}