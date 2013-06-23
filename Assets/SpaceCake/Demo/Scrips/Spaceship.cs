using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour 
{
	public Thruster[] thrusters;
	public float rollRate = 100.0f;
	public float yawRate = 30.0f;
	public float pitchRate = 100.0f;
	public Vector3[] weaponMountPoints;	
	public Transform laserShotPrefab;
	public AudioClip soundEffectFire;
	private Rigidbody cacheRigidbody;

	void Start ()
	{		
		foreach (Thruster thruster in thrusters)
		{
			if (thruster == null) 
			{
				Debug.LogError("Thruster array not properly configured. Attach thrusters to the game object and link them to the Thrusters array.");
			}			
		}
		cacheRigidbody = rigidbody;
		if (cacheRigidbody == null) 
		{
			Debug.LogError("Spaceship has no rigidbody - the thruster scripts will fail. Add rigidbody component to the spaceship.");
		}
	}
	
	void Update () 
	{
		if (Input.GetButtonDown("Fire1")) 
		{		
			foreach (Thruster thruster in thrusters) 
			{
				thruster.StartThruster();
			}
		}
		if (Input.GetButtonUp("Fire1")) 
		{		
			foreach (Thruster thruster in thrusters) 
			{
				thruster.StopThruster();
			}
		}
		
		if (Input.GetButtonDown("Fire2")) 
		{
			this.Fire();
		}		
	}
	
	void FixedUpdate ()
	{
		cacheRigidbody.AddRelativeTorque(new Vector3(0,0,-Input.GetAxis("Horizontal")*rollRate*cacheRigidbody.mass));
		cacheRigidbody.AddRelativeTorque(new Vector3(0,Input.GetAxis("Horizontal")*yawRate*cacheRigidbody.mass,0));
		cacheRigidbody.AddRelativeTorque(new Vector3(Input.GetAxis("Vertical")*pitchRate*cacheRigidbody.mass,0,0));	
	}

	public void Fire()
	{
		foreach (Vector3 wmp in weaponMountPoints) 
		{
			Vector3 pos = transform.position + transform.right * wmp.x + transform.up * wmp.y + transform.forward * wmp.z;
			Transform laserShot = (Transform) Instantiate(laserShotPrefab, pos, transform.rotation);
			laserShot.GetComponent<LaserShot>().firedBy = transform;
		}
		if (soundEffectFire != null) 
		{
			audio.PlayOneShot(soundEffectFire);
		}
	}
}
