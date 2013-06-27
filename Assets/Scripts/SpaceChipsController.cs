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
		if (this.pp != null)
		{
			Debug.Log("Control Chips Init Success");
		}
		else
		{
			Debug.Log("Control Chips Init Failed");
		}
        currentSpeedFactor = speed;
		PerCPipeline.pipelineUpdate += this.pipelineUpdate;
    }
	
	public float getSpeed()
	{
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
		PerCPipeline.pipelineUpdate -= this.pipelineUpdate;
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
    void pipelineUpdate(PerCPipeline.PipelineData data)
    {
		Debug.Log("b");
        float speedFactor;
		PXCMGesture.GeoNode mainHand;
   		PXCMGesture.GeoNode secondaryHand;
		
		checkSpeedFactor(out speedFactor);
		//Compute the rotation with the hand position
		
    //    if (!pp.AcquireFrame(false)) 
	//		return;
			
        if (data.hasMainHand && data.hasSecondaryHand)
		{
            checkHands(ref data.mainHand, ref data.secondaryHand);
						
			if (!calibrated)
			{
				calibrate(ref data);
				return;
			}
			else
			{
				calibrate(ref data);
			}

			float mainHandY = data.mainHand.positionWorld.y;
			float mainHandZ = data.mainHand.positionWorld.z;
			
			float secondaryHandY = data.secondaryHand.positionWorld.y;
			float secondaryHandZ = data.secondaryHand.positionWorld.z;
			
			controlRoll(mainHandZ, secondaryHandZ);
			controlYaw(mainHandY, secondaryHandY);
			controlPitch(mainHandY, secondaryHandY);

        }
		else
		{
			calibrated = false;
		}

        if (!calibrated) return;
		
		//checkCollisions(speedFactor);
    }
	
	void calibrate(ref PerCPipeline.PipelineData data)
	{
		if(data.hasMainGesture)
		{
			if(data.mainGesture.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP)
			{
				calibrated = true;
		  			calibrationY = data.mainHand.positionWorld.y;
			}
		}
		else if(data.hasSecondaryGesture)
		{
			if(data.secondaryGesture.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP)
			{
				calibrated = true;
		  		calibrationY = data.mainHand.positionWorld.y;
			}
		}	
	}
	
	bool checkCollide(ref Ray ray, out RaycastHit hit, float distance)
	{
		if (Physics.Raycast(ray, out hit, distance))
        {
            return hit.collider.gameObject.tag != "Ring";
        }
		return false;
	}
	
	bool checkCollisionDown(float speedFactor, out RaycastHit hit)
	{
		Ray rayDown = new Ray(transform.position, -transform.up);
		return checkCollide(ref rayDown, out hit, DEFAULT_COLLISION_DISTANCE);
	}
	
	bool checkCollisionForward(float speedFactor, out RaycastHit hit)
	{
		Ray rayForward = new Ray(transform.position, transform.forward);
		return checkCollide(ref rayForward, out hit, FORWARD_COLLISION_DISTANCE);
	}
	
	bool checkCollisionUp(float speedFactor, out RaycastHit hit)
	{
		Ray rayUp = new Ray(transform.position, transform.up);
		return checkCollide(ref rayUp, out hit, DEFAULT_COLLISION_DISTANCE);
	}
	
	void checkCollisions(float speedFactor)
	{
		RaycastHit hit;
		if(checkCollisionForward(speedFactor, out hit))
		{
			Quaternion target = Quaternion.LookRotation((transform.position + transform.up*3 + transform.forward) - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, target, speedFactor * (FORWARD_COLLISION_DISTANCE/hit.distance) * Time.deltaTime);
			transform.position = transform.position + transform.forward * speedFactor/3;
		}
		else if(checkCollisionDown(speedFactor, out hit))
		{
            transform.position = transform.position + transform.forward * speedFactor + Vector3.up * (DEFAULT_COLLISION_DISTANCE - hit.distance) * 0.5f;
		}
		else if(checkCollisionUp(speedFactor, out hit))
		{
            transform.position = transform.position + transform.forward * speedFactor + Vector3.down * (DEFAULT_COLLISION_DISTANCE - hit.distance) * 0.5f;
		}
		else
		{
			transform.position = transform.position + transform.forward * speedFactor;
		}
	}

	void checkHands(ref PXCMGesture.GeoNode mainHand, ref PXCMGesture.GeoNode secondaryHand)
	{
		if(mainHand.positionWorld.x > secondaryHand.positionWorld.x)
		{
			PXCMGesture.GeoNode temp = mainHand;
			mainHand = secondaryHand;
			secondaryHand = temp;
		}
	}
	
    void controlRoll(float mainHandZ, float secondaryHandZ)
	{
		float speedFactor;
		checkSpeedFactor(out speedFactor);
		float roll = mainHandZ - secondaryHandZ;
		//Here is the trick to smooth the moves
		roll *= Mathf.Abs (roll);
		roll *= sensibilityFactor* 2f;
		transform.RotateAroundLocal(transform.forward, roll);
	}
	
	void controlYaw(float mainHandY, float secondaryHandY)
	{
		float speedFactor;
		checkSpeedFactor(out speedFactor);
		float yaw = mainHandY - secondaryHandY;
		//Here is the trick to smooth the moves
		yaw *= Mathf.Abs (yaw);
		yaw *= sensibilityFactor;
		transform.RotateAroundLocal(transform.up, yaw);
	}
	
	void controlPitch(float mainHandY, float secondaryHandY)
	{
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