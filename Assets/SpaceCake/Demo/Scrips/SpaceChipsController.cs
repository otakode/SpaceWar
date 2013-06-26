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
	
	public float nonPlayingSpeed = 5f;
	
    private PXCUPipeline pp;

    // Use this for initialization
    void Start()
    {
        pp = PerCPipeline.GetPipeline();
		if (this.pp == null)
			Debug.Log("Control Chips Init Failed");
        currentSpeedFactor = speed;
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
		if (pp == null)
			return;
		this.pp.Close();
		//this.pp.Dispose();
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
	
    // Update is called once per frame
    void Update()
    {
		if (pp == null)
		{
			pp = PerCPipeline.GetPipeline();
			return;
		}
        float speedFactor;
		PXCMGesture.GeoNode mainHand;
   		PXCMGesture.GeoNode secondaryHand;
		
		checkSpeedFactor(out speedFactor);
		//Compute the rotation with the hand position
		
        if (!pp.AcquireFrame(false)) 
			return;
			
        if (pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_HAND_MIDDLE, out mainHand) &&
            pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_HAND_MIDDLE, out secondaryHand))
		{
            checkHands(ref mainHand, ref secondaryHand);
						
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
		
		checkCollisions(speedFactor);
    }
	
	void calibrate(ref PXCMGesture.GeoNode mainHand){
		PXCMGesture.Gesture dataMain;
		PXCMGesture.Gesture dataSecondary;
		if(pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out dataMain)){
			if(dataMain.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP){
				calibrated = true;
		  			calibrationY = mainHand.positionWorld.y;
			}
		}
		else if(pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out dataSecondary)){
			if(dataSecondary.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP){
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