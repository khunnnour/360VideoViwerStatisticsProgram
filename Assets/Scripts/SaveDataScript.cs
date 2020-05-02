using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveDataScript : MonoBehaviour
{
	public Texture2D encodedTex;
	[Header("Save settings")]
	//public File dataFile;
	public string destination;
	public string filename;
	public float saveInterval = 1f;

	private RaycastOnSphereTest raycaster;
	//private FileStream dataFile;
	private Vector2 boxDim;
	private float rayWeight;
	private float timer;
	string fullDest;

	private void Awake()
	{
		timer = 0f;
	}

	// Start is called before the first frame update
	void Start()
	{
		raycaster = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RaycastOnSphereTest>();
		
		// Get ray weight from raycasting script
		rayWeight = raycaster.rayWeight;
		boxDim = raycaster.boxDim;

		// Create full path to data file
		fullDest = destination + filename;
		// Verify file exists
		if (!File.Exists(fullDest)) Debug.LogError("Invalid File/Path");
		// Clear out file
		StreamWriter sw = new StreamWriter(fullDest, false);
		sw.WriteLine("time in seconds,ray matrix");
		sw.Close();
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;

		if (timer >= saveInterval)
		{
			SaveDataToFile();
			timer = 0f;
		}
	}

	void SaveDataToFile()
	{
		// start data line
		string line = Time.time.ToString("F3") + ",[";

		// Get texture pixel colors
		Color[] cols = encodedTex.GetPixels();

		int numRays, pxIndex = 1;
		foreach (Color col in cols)
		{
			// unpack texture
			numRays = Mathf.FloorToInt(col.r / rayWeight);
			numRays += Mathf.FloorToInt(col.g / rayWeight);
			numRays += Mathf.FloorToInt(col.b / rayWeight);

			// add value to data line
			line += numRays.ToString();
			/* - add space or semicolon based on position in matrix - */
			// if % == 0, then start of new row (except skip first pixel)
			if (pxIndex % encodedTex.width == 0)
			{
				line += ";";
			}
			else
			{
				line += " ";
			}

			// increment the pixel index
			pxIndex++;
		}

		// cap off matrix
		line += "]";

		/* - Writing to File - */
		// create streamwriter and set append to true
		StreamWriter sw = new StreamWriter(fullDest, true);
		// append the new line
		sw.WriteLine(line);
		// close the stream
		sw.Close();
	}
}
