using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
	
	//GESTURE TRACK
	private PXCUPipeline		pp;
//	private int[] 				size=new int[2]{0,0};
//	private PXCUPipeline.Mode 	mode=PXCUPipeline.Mode.GESTURE;

	void Start ()
	{
		pp = PerCPipeline.GetPipeline();
//		pp=new PXCUPipeline();
//		Debug.Log(pp);
//		if (!pp.Init(mode)) {
//			print("Unable to initialize the PXCUPipeline");
//		}
		//peut etre pas utile
		/*if (pp.QueryLabelMapSize(size))
	        print("LabelMap: width=" + size[0] + ", height=" + size[1]);
		else if (pp.QueryRGBSize(size))
			print("RGB: width="+size[0]+", height="+size[1]);*/
		
		
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
	
	void OnDisable()
	{
		this.pp.Close();
		this.pp.Dispose();
	}
	
	void Update () 
	{
		
 		if (!pp.AcquireFrame(false))
 		{
 		}
 		else
 		{
			PXCMGesture.Gesture gdata;
			if (pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_ANY, out gdata))
			{
				if (gdata.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP)
					print ("gesture (label="+gdata.label+")");
			}
 			
 			PXCMGesture.GeoNode ndata;
			if (pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY,out ndata))
 			{
 				if (ndata.side == PXCMGesture.GeoNode.Side.LABEL_LEFT)
 					print ("geonode handLEFT (x="+ndata.positionWorld.x+", y="+ndata.positionWorld.y+") z="+ndata.positionWorld.z+")");
 				if (ndata.side == PXCMGesture.GeoNode.Side.LABEL_RIGHT)
 					print ("geonode handRIGHT (x="+ndata.positionWorld.x+", y="+ndata.positionWorld.y+") z="+ndata.positionWorld.z+")");
 			}
			if (pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY,out ndata))
 			{
 				if (ndata.side == PXCMGesture.GeoNode.Side.LABEL_LEFT)
 					print ("geonode handLEFT (x="+ndata.positionWorld.x+", y="+ndata.positionWorld.y+") z="+ndata.positionWorld.z+")");
 				if (ndata.side == PXCMGesture.GeoNode.Side.LABEL_RIGHT)
 					print ("geonode handRIGHT (x="+ndata.positionWorld.x+", y="+ndata.positionWorld.y+") z="+ndata.positionWorld.z+")");
 			}
	
			pp.ReleaseFrame();
		}
		
		if (Input.GetButtonDown("Fire1")) 
		{		
			foreach (Thruster thruster in thrusters) 
			{
				thruster.SetThrusterPower(50);
			}
		}
		
		if (Input.GetButtonUp("Fire1")) 
		{		
			foreach (Thruster thruster in thrusters) 
			{
				thruster.SetThrusterPower(0);
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
