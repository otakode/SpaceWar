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

	private TextMesh[]		speeds;
	
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
		
		if (transform.parent.parent.rigidbody != null)
			_cacheParentRigidbody = transform.parent.parent.rigidbody;
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
		
		Transform cockpit = this.transform.parent.FindChild("Cockpit");
		this.speeds = new TextMesh[]{
			cockpit.FindChild("Max").GetComponent<TextMesh>(),
			cockpit.FindChild("Min").GetComponent<TextMesh>(),
			cockpit.FindChild("Stop").GetComponent<TextMesh>()
		};
		this.speeds[0].text = "Max";
		this.speeds[1].text = "Min";
		this.speeds[2].text = "Zero";
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

	void OnGUI()
	{
		if (this.percent < 0.2)
		{
			this.speeds[0].GetComponent<TextController>().SetColor(Color.white);
			this.speeds[1].GetComponent<TextController>().SetColor(Color.white);
			this.speeds[2].GetComponent<TextController>().SetColor(Color.green);
		}
		else if (this.percent < 0.6)
		{
			this.speeds[0].GetComponent<TextController>().SetColor(Color.white);
			this.speeds[1].GetComponent<TextController>().SetColor(Color.green);
			this.speeds[2].GetComponent<TextController>().SetColor(Color.white);
		}
		else
		{
			this.speeds[0].GetComponent<TextController>().SetColor(Color.green);
			this.speeds[1].GetComponent<TextController>().SetColor(Color.white);
			this.speeds[2].GetComponent<TextController>().SetColor(Color.white);
		}
	}
}
