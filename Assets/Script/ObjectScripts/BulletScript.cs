using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	[Range (1, 1000)]
	public float Speed = 1;
	[Range (1, 10)]
	public float Power = 1;
	[Range (1, 50)]
	public float LifeTime = 10;

	private float deathTime = 0;
	private Vector3 velocity = Vector3.zero;

	void Start()
	{
		this.deathTime = Time.time + this.LifeTime;
	}

	public void SetVelocity(Vector3 vel)
	{
		this.velocity = vel + this.transform.forward * this.Speed;
	}

	void Update()
	{
		if (Time.time > this.deathTime)
		{
			Destroy(this.gameObject);
			return;
		}
		this.transform.position += this.velocity * Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider c)
	{
		ShipScript script = c.transform.parent.GetComponent<ShipScript>() as ShipScript;
		if (script != null)
		{
			script.Hit(this.Power);
		}
	}
}
