using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour 
{
	public float orbitSpeed = 0.0f;

	public float rotationSpeed = 0.0f;	
	

	private Transform cacheTransform;
	private Transform cacheMeshTransform;
	
	void Start ()
	{

		cacheTransform = transform;
		cacheMeshTransform = transform.Find("MoonObject");
	}
	
	void Update () 
	{		
		
		if (cacheTransform != null) 
		{
			cacheTransform.Rotate(Vector3.up * orbitSpeed * Time.deltaTime);
		}
		
		if (cacheMeshTransform != null) 
		{
			cacheMeshTransform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
		}
	}
}
