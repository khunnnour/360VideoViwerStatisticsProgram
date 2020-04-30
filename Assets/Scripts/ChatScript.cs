using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class ChatScript : MonoBehaviour
{
	public Text chatWindow;

	[Header("Resource Files")]
	public string userFilename;
	public string messFilename;
	//public TextAsset usernameFile;
	//public TextAsset messageFile;

	[Header("Chat settings")]
	public float avgMessageTime = 0.5f;
	public float rngMessageRange = 0.1f;


	private string path = "Assets/Resources/InfoFiles/";
	private List<string> usernames;
	private List<string> messages;
	private float timer, nextTime;
	private int numLines, maxLines;

	// Start is called before the first frame update
	void Start()
	{
		// initialize lists
		usernames = new List<string>();
		messages = new List<string>();
		// populate lists
		GetContents(userFilename, ref usernames);
		GetContents(messFilename, ref messages);

		numLines = 0;
		// calculate max lines in text box
		maxLines = (int)(chatWindow.canvas.pixelRect.size.y / ((float)chatWindow.fontSize*chatWindow.lineSpacing+4f));
		//Debug.Log("height = "+ chatWindow.canvas.pixelRect.size.y.ToString("F2") + ", so maxLines = " + maxLines);

		// start timer
		timer = 0f;

		// get time to next message
		nextTime = GetRandomTime();
	}

	// Update is called once per frame
	void Update()
	{
		// update timer
		timer += Time.deltaTime;

		// check if time to post message
		if (timer >= nextTime)
		{
			PostMessage();
			nextTime = GetRandomTime();
			timer = 0f;
		}
	}

	private void PostMessage()
	{
		string user, message, totMessage;

		// Get rng user
		user = usernames[UnityEngine.Random.Range(0, usernames.Count)];
		// Get rng message
		message = messages[UnityEngine.Random.Range(0, messages.Count)];

		// create message
		totMessage = "\n<color=#000080ff><b>" + user + "</b></color>  " + message;

		chatWindow.text += totMessage;
		numLines++;
		//Debug.Log(numLines + " / " + maxLines);

		// Remove oldest message if too many lines
		if (numLines >= maxLines)
		{
			int stopIndex = chatWindow.text.IndexOf("\n", 1);

			chatWindow.text = chatWindow.text.Remove(0, stopIndex);

			//Debug.Log("maxLines reached (" + numLines + ")\nCut from 0 to " + stopIndex);
		}
	}

	void GetContents(string file, ref List<string> container)
	{
		// make sure container is clean
		container.Clear();

		// Create stream reader
		StreamReader reader = new StreamReader(path + file);

		// Read until the end of file
		string line;
		while ((line = reader.ReadLine()) != null)
		{
			container.Add(line);
			//Debug.Log(line);
		}
	}

	private float GetRandomTime()
	{
		return UnityEngine.Random.Range(avgMessageTime - rngMessageRange, avgMessageTime + rngMessageRange);
	}
}