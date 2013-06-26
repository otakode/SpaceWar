using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
	public Vector3 planetRotation;
	private Transform cacheTransform;
	
	void Start () 
	{
		cacheTransform = transform;	
	}
	
	void Update () {
		// Rotate the planet based on the rotational vector
		if (cacheTransform != null) 
		{			
			cacheTransform.Rotate(planetRotation * Time.deltaTime);
		}
	}

}
