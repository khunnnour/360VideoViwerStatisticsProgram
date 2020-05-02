using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Holds relevant data for a heatmap frame
public class HeatMapFrame
{
	public float timeStamp;
	public Color[] colors;

	public HeatMapFrame()
	{
		timeStamp = 0f;
		colors = new Color[1] { Color.white };
	}

	public HeatMapFrame(float t, Color[] c)
	{
		timeStamp = t;
		colors = c;
	}
}

public class LoadDataScript : MonoBehaviour
{
	public Texture2D heatMapTex;
	public string destination;
	public string filename;
	//public int numFrames = 4;
	public float heatMaxVal = 6f;

	private DrawHeatMap drawer;
	private string fullDest;
	private StreamReader reader;
	private bool loading, finished;

	void Start()
	{
		drawer = GetComponent<DrawHeatMap>();

		fullDest = destination + filename;
		reader = File.OpenText(fullDest);

		// skip header
		reader.ReadLine();

		// start loading framings
		LoadFrames();
		finished = false;
	}

	void Update()
	{
		if (!loading && !finished)
			LoadFrames();
	}

	private void LoadFrames()
	{
		loading = true;
		float startTime = Time.time;
		int framesLoaded = 0; // TODO: remove debug var
		string line;

		line = reader.ReadLine();

		if (line == null)
		{
			finished = true;
			return;
		}

		// get rid of open matrix bracket
		line = line.Trim('[');
		while (line != null)
		{
			// pull info from line
			int delimLoc = line.IndexOf(',');
			float frameTime = float.Parse(line.Substring(0, delimLoc));

			// Get colors
			Color[] colors = new Color[heatMapTex.width * heatMapTex.height];
			MatrixToHeatMap(line.Substring(delimLoc + 1), ref colors);

			// Add frame to list
			HeatMapFrame frame = new HeatMapFrame(frameTime, colors);
			drawer.AddFrame(frame);

			// check if times up
			if (Time.time - startTime >= Time.deltaTime * 0.75f)
				break;

			framesLoaded++;

			// Get next line
			line = reader.ReadLine();
		}

		DebugStopLoading(framesLoaded);
	}

	void MatrixToHeatMap(string map, ref Color[] cols)
	{
		Color color = new Color();
		int delim, rowNum = 0;
		string row;

		// trim the opening bracket
		map = map.Trim('[');
		while (map.Length > 0)
		{
			// Get the matrix row
			delim = map.IndexOf(';');
			if (delim == -1) break;

			row = map.Substring(0, delim);

			// Cycle thru the row
			int col = 0, delim2 = 1;
			while (delim2 != -1)
			{
				// find value end
				delim2 = row.IndexOf(' ');

				// break when you hit the end of the row
				//if (delim2 == -1)
				//	break;

				// find point on gradient if >0
				float val;
				if (delim2 != -1)
					val = float.Parse(row.Substring(0, delim2)) / heatMaxVal;
				else
					val = float.Parse(row.Substring(0)) / heatMaxVal;
				//Debug.Log(val.ToString("F3"));

				if (val > 0)
				{
					if (val > 1f) val = 1f;
					color = Color.Lerp(Color.blue, Color.red, val);
				}
				else
				{
					color = Color.clear;
				}

				//Debug.Log(rowNum * heatMapTex.width + col + " | " + row);
				cols[rowNum * heatMapTex.width + col] = color;

				row = row.Remove(0, delim2 + 1);
				col++;
			}

			rowNum++;
			map = map.Remove(0, delim + 1);
		}
	}

	// TODO: Remove funciton, and move relevant runtime functionality to main funciton
	// Stops loading, and outputs progress
	private void DebugStopLoading(int num)
	{
		Debug.Log("Processed " + num + " frames");

		loading = false;
	}
}