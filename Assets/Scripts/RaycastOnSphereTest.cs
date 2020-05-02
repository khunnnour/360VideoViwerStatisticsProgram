using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastOnSphereTest : MonoBehaviour
{
	public Texture2D sphereTex;
	public float rayLength = 2.0f;
	public float rayWeight = 0.01f;
	//[Tooltip("Number of other rays casted to collect view data")]
	//public int addtlRays = 0;
	[Tooltip("Number rays along x/y axis in view box")]
	public Vector2 boxDim;

	private Camera cam;
	private RaycastHit hit;
	private Vector2 imageDim;
	private Vector2 screenDim;
	private Vector2 lowerCoord, upperCoord;
	private float hFOV;


	// Start is called before the first frame update
	void Start()
	{
		cam = GetComponent<Camera>();
		imageDim = new Vector2(sphereTex.width, sphereTex.height);
		screenDim = new Vector2(cam.pixelWidth, cam.pixelHeight);
		// constants are 16/9 and pi/180/2 for converting to radians and then the equation
		hFOV = (2f * Mathf.Atan(1.7777778f * Mathf.Tan(cam.fieldOfView * 0.0087266f))) * 57.2957795f;
	}

	// Update is called once per frame
	void Update()
	{
		hFOV = (2f * Mathf.Atan(1.7777778f * Mathf.Tan(cam.fieldOfView * 0.0087266f))) * 57.2957795f;

		// Reset texture
		Color[] colors = sphereTex.GetPixels();
		for (int i = 0; i < colors.Length; ++i)
		{
			colors[i] = Color.black;
		}
		sphereTex.SetPixels(0, 0, (int)imageDim.x, (int)imageDim.y, colors);

		// cast the rays
		CastAllRays();
	}

	private void CastAllRays()
	{
		//lowerCoord = new Vector2();
		//upperCoord = new Vector2();
		//
		////Ray ray = new Ray(transform.position, transform.forward);
		//Vector3 rot = Vector3.zero;
		//
		///* - Cast rays at corners based on FOV - */
		//// top left
		//Vector3 scrnPt = new Vector3(0f, screenDim.y);
		//
		//CastRay(cam.ScreenPointToRay(scrnPt));
		//
		//// top right
		//scrnPt.x = screenDim.x;
		//scrnPt.y = screenDim.y;
		//
		//CastRay(cam.ScreenPointToRay(scrnPt));
		//
		//// bottom right
		//scrnPt.x = screenDim.x;
		//scrnPt.y = 0f;
		//
		//CastRay(cam.ScreenPointToRay(scrnPt));
		//
		//// bottom left
		//scrnPt.x = 0f;
		//scrnPt.y = 0f;
		//
		//CastRay(cam.ScreenPointToRay(scrnPt));

		Vector3 scrnPt = Vector3.zero;
		float xDist = screenDim.x / (boxDim.x - 1f);
		float yDist = screenDim.y / (boxDim.y - 1f);

		for (int y = 0; y < boxDim.y; y++)
		{
			scrnPt.y = y * yDist;
			for (int x = 0; x < boxDim.x; x++)
			{
				scrnPt.x = x * xDist;
				CastRay(cam.ScreenPointToRay(scrnPt));
			}
		}
	}

	private void CastRay(Ray ray)
	{
		Debug.DrawRay(ray.origin, ray.direction * rayLength, new Color(ray.direction.x, ray.direction.y, ray.direction.z));
		Physics.Raycast(ray.origin, ray.direction * rayLength, out hit);
		//Debug.Log("Hit " + hit.collider.name + " at " + hit.textureCoord.ToString("F2"));

		DrawTexAuto();
	}

	private void DrawTexAuto()
	{
		// convert hit texcoord to pixel coord
		Vector2 pxLoc = new Vector2(
			hit.textureCoord.x * imageDim.x,
			hit.textureCoord.y * imageDim.y
			);

		// get the pixels current status
		Color color = FindNextPixelColor((int)pxLoc.x, (int)pxLoc.y);

		// set pixel
		sphereTex.SetPixel((int)pxLoc.x, (int)pxLoc.y, color);

		sphereTex.Apply();
	}

	private void DrawTexManual()
	{
		// unit vector towards sphere center from point on sphere
		Vector3 d = transform.up;

		// calculate uv coordinates
		float u = 0.5f + Mathf.Atan2(d.y, d.x) * 0.1591549f;
		float v = 1f - (0.5f - Mathf.Asin(d.z) * 0.3183099f);
		//float v = d.y * 0.5f + 0.5f;

		//Debug.Log(d.ToString("F2") + " -> (" + u.ToString("F2") + ", " + v.ToString("F2") + ")");

		// Reset texture
		Color[] colors = sphereTex.GetPixels();
		for (int i = 0; i < colors.Length; ++i)
		{
			colors[i] = Color.white;
		}
		Vector2 pxLoc = new Vector2(
			u * imageDim.x,
			v * imageDim.y
			);

		// set pixel
		sphereTex.SetPixels(0, 0, (int)imageDim.x, (int)imageDim.y, colors);
		sphereTex.SetPixel((int)pxLoc.x, (int)pxLoc.y, Color.red);
		sphereTex.Apply();
	}

	private Color FindNextPixelColor(int xPx, int yPx)
	{
		// get pixels current color
		Color col = sphereTex.GetPixel(xPx, yPx);

		// increment correct channel
		if (col.r < 1f)
		{
			col.r += rayWeight;
			if (col.r > 1f) col.r = 1f;
		}
		else if (col.g < 1f)
		{
			col.g += rayWeight;
			if (col.g > 1f) col.g = 1f;
		}
		else if (col.b < 1f)
		{
			col.b += rayWeight;
			if (col.b > 1f) col.b = 1f;
		}
		else
		{
			Debug.LogError("Texture Overloaded");
		}

		return col;
	}
}