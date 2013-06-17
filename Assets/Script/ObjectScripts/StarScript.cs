using UnityEngine;
using System.Collections;

public class StarScript : MonoBehaviour
{
	void Start() 
	{
	
	}

	void Update()
	{
		this.renderer.material.SetFloat("_coef", Mathf.Sin(Time.time * 3) * 0.1f);
	}

	void OnTriggerEnter(Collider c)
	{
		ShipScript script = c.transform.parent.GetComponent<ShipScript>() as ShipScript;
		if (script != null)
		{
			script.Die();
		}
	}
}
