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
    private float calibrationY;

	public float rollRate = 100.0f;
	public float yawRate = 30.0f;
	public float pitchRate = 100.0f;
	private Rigidbody cacheRigidbody;
	public GameObject	pilotHands;

	public float nonPlayingSpeed = 5f;
	
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
			if(dataMain.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP){
				this.transform.GetComponent<Spaceship>().Fire();
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
			Transform	handRight = pilotHands.transform.GetChild(0).transform;
			Transform	handLeft = pilotHands.transform.GetChild(1).transform;
			handRight.localPosition = new Vector3(-secondaryHand.positionWorld.x,secondaryHand.positionWorld.z,secondaryHand.positionWorld.y);
			handLeft.localPosition = new Vector3(-mainHand.positionWorld.x,mainHand.positionWorld.z,mainHand.positionWorld.y);
			if (!calibrated)
			{
				calibrate(ref mainHand);
				pp.ReleaseFrame();
				return;
			}else{
				calibrate(ref mainHand);
			}

			float mainHandY = mainHand.positionWorld.y;
			float mainHandZ = mainHand.positionWorld.z;
			
			float secondaryHandY = secondaryHand.positionWorld.y;
			float secondaryHandZ = secondaryHand.positionWorld.z;
			
			controlRoll(mainHandZ, secondaryHandZ);
			controlYaw(mainHandY, secondaryHandY);
			controlPitch(mainHandY, secondaryHandY);

        }else{ calibrated = false; }
		
        pp.ReleaseFrame();

        if (!calibrated) return;
		
		//checkCollisions(speedFactor);
    }
	
	void calibrate(ref PXCMGesture.GeoNode mainHand){
		PXCMGesture.Gesture dataMain;
		PXCMGesture.Gesture dataSecondary;
		if(pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out dataMain)){
			if(dataMain.label == PXCMGesture.Gesture.Label.LABEL_POSE_BIG5){
				calibrated = true;
		  			calibrationY = mainHand.positionWorld.y;
			}
		}
		else if(pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out dataSecondary)){
			if(dataSecondary.label == PXCMGesture.Gesture.Label.LABEL_POSE_BIG5){
				calibrated = true;
		  			calibrationY = mainHand.positionWorld.y;
			}
		}	
	}
	
	bool checkCollide(ref Ray ray, out RaycastHit hit, float distance){
		if (Physics.Raycast(ray, out hit, distance))
        {
            return hit.collider.gameObject.tag != "Ring";
        }
		return false;
	}
	
	bool checkCollisionDown(float speedFactor, out RaycastHit hit){
		Ray rayDown = new Ray(transform.position, -transform.up);
		return checkCollide(ref rayDown, out hit, DEFAULT_COLLISION_DISTANCE);
	}
	
	bool checkCollisionForward(float speedFactor, out RaycastHit hit){
		Ray rayForward = new Ray(transform.position, transform.forward);
		return checkCollide(ref rayForward, out hit, FORWARD_COLLISION_DISTANCE);
	}
	
	bool checkCollisionUp(float speedFactor, out RaycastHit hit){
		Ray rayUp = new Ray(transform.position, transform.up);
		return checkCollide(ref rayUp, out hit, DEFAULT_COLLISION_DISTANCE);
	}
	
	void checkCollisions(float speedFactor){
		RaycastHit hit;
		if(checkCollisionForward(speedFactor, out hit)){
			Quaternion target = Quaternion.LookRotation((transform.position + transform.up*3 + transform.forward) - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, target, speedFactor * (FORWARD_COLLISION_DISTANCE/hit.distance) * Time.deltaTime);
			transform.position = transform.position + transform.forward * speedFactor/3;
		}else if(checkCollisionDown(speedFactor, out hit)){
            transform.position = transform.position + transform.forward * speedFactor + Vector3.up * (DEFAULT_COLLISION_DISTANCE - hit.distance) * 0.5f;
		}else if(checkCollisionUp(speedFactor, out hit)){
            transform.position = transform.position + transform.forward * speedFactor + Vector3.down * (DEFAULT_COLLISION_DISTANCE - hit.distance) * 0.5f;
		}else{
			transform.position = transform.position + transform.forward * speedFactor;
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