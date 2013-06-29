using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class SpaceChipsController : MonoBehaviour
{
	//The default speed of the aircraft
    public float speed = 2f;
	public float sensibilityFactor = 3f;
	//Is the player currently playing ?
    public bool isPlaying = true;
	public float firstAccelerationRate = 0.15f;
	public float secondAccelerationRate = 0.001f;
		
	private float DEFAULT_COLLISION_DISTANCE = 25f;
	private float FORWARD_COLLISION_DISTANCE = 40f;
	
    private float currentSpeedFactor;
    private bool calibrated = false;
	private bool gogoGadgeto = false;
    
    private float calibrationY;

	public float rollRate = 100.0f;
	public float yawRate = 30.0f;
	public float pitchRate = 100.0f;
	private Rigidbody cacheRigidbody;
	public GameObject	pilotHands;
	private Transform	handRight;
	private Transform	handLeft;
	
	public GameObject	manche;

	public float nonPlayingSpeed = 5f;
	public float	cooldown = 0.15f;
	
    private PXCUPipeline.Mode mode = PXCUPipeline.Mode.GESTURE;
    private PXCUPipeline pp;

    // Use this for initialization
    void Start()
    {
        pp = new PXCUPipeline();
        if (this.pp != null && this.pp.Init(mode))
		{
			Debug.Log("Space Chips Init SUCCESS");
		}
		else
			Debug.Log("Space Chips Init Failed");
        currentSpeedFactor = speed;

		cacheRigidbody = rigidbody;
		if (cacheRigidbody == null)
		{
			Debug.LogError("Spaceship has no rigidbody - the thruster scripts will fail. Add rigidbody component to the spaceship.");
		}
    }
	public void set_thrusters(int power)
	{
		Thruster[] thrusters = this.transform.GetComponent<Spaceship>().thrusters;
		foreach (Thruster thruster in thrusters)
		{
			thruster.SetThrusterPower(power);
		}

	}
	public float getSpeed(){
		return currentSpeedFactor;	
	}

    public void increaseSpeed(float maxvalue)
    {
        if (currentSpeedFactor < maxvalue)
            currentSpeedFactor += (currentSpeedFactor * firstAccelerationRate);
        else
            currentSpeedFactor += (currentSpeedFactor * secondAccelerationRate);
    }

    void OnDisable()
    {
        pp.Close();
    }

    public void resetSpeed()
    {
        currentSpeedFactor = speed;
    }

    public float getSpeedFactor()
    {
        return currentSpeedFactor;
    }

    void checkSpeedFactor(out float speedFactor){
		if (isPlaying)
            speedFactor = currentSpeedFactor;
        else
            speedFactor = nonPlayingSpeed;
	}
	
	public void increaseAllSpeeds(){
		currentSpeedFactor += 2f;
		nonPlayingSpeed += 2f;
		speed += 2f;
	}
	
	public void decreaseAllSpeeds(){
		currentSpeedFactor -= 2f;
		nonPlayingSpeed -= 2f;
		
		if(currentSpeedFactor <=1f)
			currentSpeedFactor = 1f;
		if(nonPlayingSpeed <=1f)
			nonPlayingSpeed = 1f;
		if(speed <=1f)
			speed = 1f;
	}

	void FixedUpdate()
	{
		cacheRigidbody.AddRelativeTorque(new Vector3(0, 0, -Input.GetAxis("Horizontal") * rollRate * cacheRigidbody.mass));
		cacheRigidbody.AddRelativeTorque(new Vector3(0, Input.GetAxis("Horizontal") * yawRate * cacheRigidbody.mass, 0));
		cacheRigidbody.AddRelativeTorque(new Vector3(Input.GetAxis("Vertical") * pitchRate * cacheRigidbody.mass, 0, 0));
	}
	
    // Update is called once per frame
    void Update()
    {
        float speedFactor;
		PXCMGesture.GeoNode mainHand;
   		PXCMGesture.GeoNode secondaryHand;
		checkSpeedFactor(out speedFactor);
		//Compute the rotation with the hand position
        if (!pp.AcquireFrame(false)) return;

		Thruster[] thrusters = this.transform.GetComponent<Spaceship>().thrusters;
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
		PXCMGesture.Gesture dataMain;
		if(pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out dataMain)){
			if (dataMain.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP)
			{
				cooldown -= Time.deltaTime;
				
				if (cooldown <= 0)
				{
					cooldown = 0.15f;
					this.transform.GetComponent<Spaceship>().Fire();
				}
			}
		}

		if (Input.GetButtonDown("Fire2"))
		{
			this.transform.GetComponent<Spaceship>().Fire();
		}

		
		
        if (pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out mainHand) &&
            pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY, out secondaryHand))
		{
            checkHands(ref mainHand, ref secondaryHand);
			handRight = pilotHands.transform.GetChild(0).transform;
			handLeft = pilotHands.transform.GetChild(1).transform;
			handRight.localPosition = new Vector3(-secondaryHand.positionWorld.x-0.15f,secondaryHand.positionWorld.z+0.2f,-secondaryHand.positionWorld.y+0.65f);
			handLeft.localPosition = new Vector3(-mainHand.positionWorld.x+0.15f,mainHand.positionWorld.z+0.2f,-mainHand.positionWorld.y+0.65f);
			if (!calibrated)
			{
				//bien degueu mais bon...
				calibrate(ref mainHand);
				handLeft.GetChild(1).gameObject.renderer.enabled = true;
				handLeft.GetChild(2).gameObject.renderer.enabled = true;
				handLeft.GetChild(4).gameObject.renderer.enabled = true;
				handRight.GetChild(1).gameObject.renderer.enabled = true;
				handRight.GetChild(2).gameObject.renderer.enabled = true;
				handRight.GetChild(3).gameObject.renderer.enabled = true;
				pp.ReleaseFrame();
				return;
			}else{
				calibrate(ref mainHand);
				if (handLeft.GetChild(1).gameObject.renderer.enabled)
				{
					handLeft.GetChild(1).gameObject.renderer.enabled = false;
					handLeft.GetChild(2).gameObject.renderer.enabled = false;
					handLeft.GetChild(4).gameObject.renderer.enabled = false;
					handRight.GetChild(1).gameObject.renderer.enabled = false;
					handRight.GetChild(2).gameObject.renderer.enabled = false;
					handRight.GetChild(3).gameObject.renderer.enabled = false;
					
				}
			}
			if (gogoGadgeto)
			{
				float mainHandY = mainHand.positionWorld.y;
				float mainHandZ = mainHand.positionWorld.z;
			
				float secondaryHandY = secondaryHand.positionWorld.y;
				float secondaryHandZ = secondaryHand.positionWorld.z;
			
				controlRoll(mainHandZ, secondaryHandZ);
				controlYaw(mainHandY, secondaryHandY);
				controlPitch(mainHandY, secondaryHandY);
				
			}

        }else{ calibrated = false; }
		
        pp.ReleaseFrame();

        if (!calibrated) return;
		
    }
	
	void calibrate(ref PXCMGesture.GeoNode mainHand){
		PXCMGesture.Gesture dataMain;
		PXCMGesture.Gesture dataSecondary;
		if(pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out dataMain)){
			if(dataMain.label == PXCMGesture.Gesture.Label.LABEL_POSE_BIG5){
				calibrated = true;
		  			calibrationY = mainHand.positionWorld.y;
				gogoGadgeto = false;
			}
		}
		else if(pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out dataSecondary)){
			if(dataSecondary.label == PXCMGesture.Gesture.Label.LABEL_POSE_BIG5){
				calibrated = true;
		  			calibrationY = mainHand.positionWorld.y;
				gogoGadgeto = false;
			}
		}
		else if (calibrated)
		{
			gogoGadgeto = true;
		}
	}

	void checkHands(ref PXCMGesture.GeoNode mainHand, ref PXCMGesture.GeoNode secondaryHand){
		if(mainHand.positionWorld.x > secondaryHand.positionWorld.x){
			PXCMGesture.GeoNode temp = mainHand;
			mainHand = secondaryHand;
			secondaryHand = temp;
		}
	}
	
    void controlRoll(float mainHandZ, float secondaryHandZ){
		float speedFactor;
		checkSpeedFactor(out speedFactor);
		float roll = mainHandZ - secondaryHandZ;
		//manche.transform.rotation = new Quaternion(0.0f,0.0f,roll,0.0f);
		//Here is the trick to smooth the moves
		roll *= Mathf.Abs (roll);
		roll *= sensibilityFactor* 2f;
		
		transform.RotateAroundLocal(transform.forward, roll);
	}
	
	void controlYaw(float mainHandY, float secondaryHandY){
		float speedFactor;
		checkSpeedFactor(out speedFactor);
		float yaw = mainHandY - secondaryHandY;
		//Here is the trick to smooth the moves
		yaw *= Mathf.Abs (yaw);
		yaw *= sensibilityFactor;
		transform.RotateAroundLocal(transform.up, yaw);
	}
	
	void controlPitch(float mainHandY, float secondaryHandY){
		float speedFactor;
		checkSpeedFactor(out speedFactor);
		float positionY = (mainHandY<secondaryHandY) ? mainHandY : secondaryHandY;
		float pitch = calibrationY - positionY;
		//Here is the trick to smooth the moves
		pitch *= Mathf.Abs (pitch);
		pitch *= sensibilityFactor * 2f;
		transform.RotateAroundLocal(transform.right, pitch);
	}
    
}