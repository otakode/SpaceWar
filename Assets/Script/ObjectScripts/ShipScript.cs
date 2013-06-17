using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour
{
	public GameObject	Boom;
	public FireScript	FireScript;

	public float Health = 100f;

	private float life = 0;
	private ShipEngine engine = null;
	private PhysicObject physic = null;
	
	void Start()
	{
		this.engine = new ShipEngine(this.gameObject);
		this.physic = this.GetComponent<PhysicObject>();
		this.life = this.Health;
	}
	
	void OnDestroy()
	{
		if (Network.isClient || Network.isServer)
			Network.Destroy(this.gameObject);
	}

	void Update()
	{
		if (this.life <= 0)
		{
			this.Die();
		}
	}

	public void Accelerate(float direction)
	{
		if (direction > 0)
			this.engine.activated = 1;
		else if (direction < 0)
			this.engine.activated = -1;
		else
			this.engine.activated = 0;
	}
	
	public void Rotate(float x, float y, float z)
	{
		this.transform.Rotate(x, y, z);
	}

	public void Fire(bool activate)
	{
		this.FireScript.Fire(activate);
	}
	
	public void Hit(float damage)
	{
		this.life -= damage;
	}

	public void Die()
	{
		Network.Instantiate(this.Boom, this.transform.position, this.transform.rotation, 0);
		this.life = this.Health;
		this.transform.position = Vector3.zero;
		this.transform.LookAt(Vector3.forward);
		this.physic.Speed = Vector3.zero;
	}
}
