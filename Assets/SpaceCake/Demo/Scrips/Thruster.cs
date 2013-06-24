using UnityEngine;
using System.Collections;

public class Thruster : MonoBehaviour 
{
	public float			thrusterForce = 30000;
	public float			percent = 0;
	public bool				addForceAtPosition = false;
	public float			soundEffectVolume = 1.0f;
	
	private bool			_isActive = false;	
	private Transform		_cacheTransform;
	private Rigidbody		_cacheParentRigidbody;
	private Light			_cacheLight;
	private ParticleSystem	_cacheParticleSystem;
	
	public void StartThruster() 
	{
		_isActive = true; 
	}
	
	public void StopThruster() 
	{		
		_isActive = false; 
	}

	public void SetThrusterPower(int power)
	{
		this.percent = power / 100.0f;
		if (power == 0)
			StopThruster();
		else
			StartThruster();
	}
	
	void Start () 
	{
		_cacheTransform = transform;
		
		if (transform.parent.rigidbody != null)
			_cacheParentRigidbody = transform.parent.rigidbody;
		else 
			Debug.LogError("Thruster has no parent with rigidbody that it can apply the force to.");
		_cacheLight = transform.GetComponent<Light>().light;
		if (_cacheLight == null) 
			Debug.LogError("Thruster prefab has lost its child light. Recreate the thruster using the original prefab.");
		_cacheParticleSystem = particleSystem;
		if (_cacheParticleSystem == null) 
			Debug.LogError("Thruster has no particle system. Recreate the thruster using the original prefab.");
		audio.loop = true;
		audio.volume = soundEffectVolume;
		audio.mute = true;
		audio.Play();		
	}	
	
	void Update () 
	{
		if (_cacheLight != null)
			_cacheLight.intensity = _cacheParticleSystem.particleCount / 20;
		if (_isActive) 
		{
			if (audio.mute) 
				audio.mute=false;
			if (audio.volume < soundEffectVolume) 
				audio.volume += 5f * Time.deltaTime;
			if (_cacheParticleSystem != null) 
				_cacheParticleSystem.enableEmission = true;	
		}
		else 
		{
			if (audio.volume > 0.01f) 
				audio.volume -= 5f * Time.deltaTime;	
			else 
				audio.mute = true;
			if (_cacheParticleSystem != null) 
				_cacheParticleSystem.enableEmission = false;				
		}
	}
	
	void FixedUpdate() 
	{
		if (_isActive) 
		{
			if (addForceAtPosition)
				_cacheParentRigidbody.AddForceAtPosition (_cacheTransform.up * thrusterForce * percent, _cacheTransform.position);
			else 
				_cacheParentRigidbody.AddRelativeForce (Vector3.forward * thrusterForce * percent);				
		}		
	}
}
