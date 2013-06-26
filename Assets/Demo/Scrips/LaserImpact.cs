using UnityEngine;
using System.Collections;

public class LaserImpact : MonoBehaviour 
{
	public Transform	_cacheLight;

	void Start () 
	{
		if (transform.Find("light") != null) 
		{
			_cacheLight = transform.Find("light");
			_cacheLight.light.intensity = 1.0f;
			_cacheLight.transform.Translate(Vector3.up*5, Space.Self);

		} 
		else 
			Debug.LogWarning("Missing required child light. Impact light effect won't be visible");
		
	}
		
	void Update () 
	{
		if (_cacheLight != null)
			_cacheLight.light.intensity = (float) (transform.particleEmitter.particleCount / 50.0f);
	}
}
