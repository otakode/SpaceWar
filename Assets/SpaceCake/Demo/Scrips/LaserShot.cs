using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour 
{	
	public float		life = 2.0f;
	public float		velocity = 1000.0f;
	public Transform	impactEffect;
	public Transform	explosionEffect;
	public Transform	firedBy {get; set;}
	
	private Vector3		_velocity;
	private Vector3		_newPos;
	private Vector3		_oldPos;	
	
	void Start () 
	{
		_newPos = transform.position;
		_oldPos = _newPos;			
		_velocity = velocity * transform.forward;
		Destroy(gameObject, life);
	}
	
	void Update () 
	{
		_newPos += transform.forward * _velocity.magnitude * Time.deltaTime;
		Vector3 _direction = _newPos - _oldPos;
		float _distance = _direction.magnitude;
		if (_distance > 0) 
		{
			RaycastHit _hit;
			if (Physics.Raycast(_oldPos, _direction, out _hit, _distance)) 
			{
				if (_hit.transform != firedBy && !_hit.collider.isTrigger)
				{		
					Quaternion _rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);
					Instantiate(impactEffect, _hit.point, _rotation);
					if (Random.Range(0, 20) < 2)
					{
						Instantiate(explosionEffect, _hit.transform.position, _rotation);
						Destroy(_hit.transform.gameObject);
					}
					Destroy(gameObject);
				}
			}
		}
		_oldPos = transform.position;
		transform.position = _newPos;		
	}
}
