using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastOnSphereTest : MonoBehaviour
{
	public Texture2D sphereTex;
	public float rayLength = 2.0f;
	[Tooltip("Number of other rays casted to collect view data")]
	public int addtlRays = 0;

	private Camera cam;
	private RaycastHit hit;
	private Vector2 imageDim;
	private float hFOV;
	//private Vector2 screenDim;

	// Start is called before the first frame update
	void Start()
	{
		cam = GetComponent<Camera>();
		imageDim = new Vector2(sphereTex.width, sphereTex.height);
		//screenDim = new Vector2(cam.pixelWidth, cam.pixelHeight);
		// constants are 16/9 and pi/180/2 for converting to radians and then the equation
		hFOV = (2f * Mathf.Atan(1.7777778f * Mathf.Tan(cam.fieldOfView * 0.0087266f)))*57.2957795f;
	}

	// Update is called once per frame
	void Update()
	{
		hFOV = (2f * Mathf.Atan(1.7777778f * Mathf.Tan(cam.fieldOfView * 0.0087266f))) * 57.2957795f;

		// Reset texture
		Color[] colors = sphereTex.GetPixels();
		for (int i = 0; i < colors.Length; ++i)
		{
			colors[i] = Color.white;
		}
		sphereTex.SetPixels(0, 0, (int)imageDim.x, (int)imageDim.y, colors);

		// cast the rays
		CastAllRays();
	}

	private void CastAllRays()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		Vector3 rot = Vector3.zero;
		/* - decide how rays will look - */
		// if no additional rays
		if (addtlRays == 0)
		{
			// just use forward ray
			CastRay(ray);
		}
		else if (addtlRays == 1)
		{
			// if 2 rays: eyes
			rot.x = 0f;
			rot.y = hFOV * 0.25f;
			rot.z = 0f;

			ray.direction = Matrix4x4.Rotate(Quaternion.Euler(rot)) * transform.forward;
			CastRay(ray);

			rot.y *= -1f;

			ray.direction = Matrix4x4.Rotate(Quaternion.Euler(rot)) * transform.forward;
			CastRay(ray);
		}
		else if(addtlRays>1)
		{
			// otherwise, do some funky shizz

		}
	}

	private void CastRay(Ray ray)
	{
		Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green);
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

		// set pixel
		sphereTex.SetPixel((int)pxLoc.x, (int)pxLoc.y, Color.red);
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
}