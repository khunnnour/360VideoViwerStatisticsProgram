using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Holds relevant data for a heatmap frame
public class HeatMapFrame
{
	float timeStamp;
	Texture2D frame;
}

public class LoadDataScript : MonoBehaviour
{
	public string destination;
	public string filename;

	private string fullDest;
	private HeatMapFrame[] frames;

	// Start is called before the first frame update
	void Start()
    {
		fullDest = destination + filename;
	}

    void FixedUpdate()
    {
        
    }
}
