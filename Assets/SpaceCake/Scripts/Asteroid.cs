using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
	public enum 		PolyCount { HIGH, MEDIUM, LOW }
	public PolyCount	polyCount = PolyCount.HIGH;
	public PolyCount	polyCountCollider = PolyCount.LOW;
	
	public Transform	meshLowPoly;
	public Transform	meshMediumPoly;
	public Transform	meshHighPoly;
	
	public float		rotationSpeed = 0.0f;
	public Vector3		rotationalAxis = Vector3.up;	
	public float		driftSpeed = 0.0f;
	public Vector3		driftAxis = Vector3.up;
	
	private Transform	_cacheTransform;
	
	void Start () 
	{
		_cacheTransform = transform;
		SetPolyCount(polyCount);
	}
	
	void Update ()
	{						
		if (_cacheTransform != null)
		{
			_cacheTransform.Rotate(rotationalAxis * rotationSpeed * Time.deltaTime);
			_cacheTransform.Translate(driftAxis * driftSpeed * Time.deltaTime, Space.World);
		}
	}
	
	public void SetPolyCount(PolyCount _newPolyCount)
	{ 
		SetPolyCount(_newPolyCount, false);
	}
	
	public void SetPolyCount(PolyCount _newPolyCount, bool _collider) 
	{
		if (!_collider) 
		{
			polyCount = _newPolyCount;
			switch (_newPolyCount) 
			{
				case PolyCount.LOW:
					transform.GetComponent<MeshFilter>().sharedMesh = meshLowPoly.GetComponent<MeshFilter>().sharedMesh;				
				break;
				case PolyCount.MEDIUM:
					transform.GetComponent<MeshFilter>().sharedMesh = meshMediumPoly.GetComponent<MeshFilter>().sharedMesh;
				break;
				case PolyCount.HIGH:
					transform.GetComponent<MeshFilter>().sharedMesh = meshHighPoly.GetComponent<MeshFilter>().sharedMesh;			
				break;
			}
		} 
		else
		{
			polyCountCollider = _newPolyCount;
			switch (_newPolyCount) 
			{
				case PolyCount.LOW:
					transform.GetComponent<MeshCollider>().sharedMesh = meshLowPoly.GetComponent<MeshFilter>().sharedMesh;				
				break;
				case PolyCount.MEDIUM:
					transform.GetComponent<MeshCollider>().sharedMesh = meshMediumPoly.GetComponent<MeshFilter>().sharedMesh;
				break;
				case PolyCount.HIGH:
					transform.GetComponent<MeshCollider>().sharedMesh = meshHighPoly.GetComponent<MeshFilter>().sharedMesh;			
				break;
			}			
		}
	}
			
}
