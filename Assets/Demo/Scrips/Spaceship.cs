using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Spaceship : MonoBehaviour 
{
	public int	life = 15;
	public Thruster[] thrusters;
	public Vector3[] weaponMountPoints;	
	public Transform laserShotPrefab;
	public AudioClip soundEffectFire;
	
	public void set_life(int dif)
	{
		life += dif;
	}
	
	void Start ()
	{	
		
		foreach (Thruster thruster in thrusters)
		{
			if (thruster == null) 
			{
				Debug.LogError("Thruster array not properly configured. Attach thrusters to the game object and link them to the Thrusters array.");
			}			
		}
	}
	
	void Update () 
	{
		
		if ( life <= 0)
			Network.Destroy(this.gameObject);
	}

	public void Fire()
	{
		foreach (Vector3 wmp in weaponMountPoints) 
		{
			Vector3 pos = transform.position + transform.right * wmp.x + transform.up * wmp.y + transform.forward * wmp.z;
			Transform laserShot = (Transform) Network.Instantiate(laserShotPrefab, pos, transform.rotation, 0);
			laserShot.GetComponent<LaserShot>().firedBy = transform;
		}
		if (soundEffectFire != null) 
		{
			audio.PlayOneShot(soundEffectFire);
		}
	}
}
