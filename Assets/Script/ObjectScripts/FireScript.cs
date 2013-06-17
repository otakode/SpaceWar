using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour
{
	public ShipScript	Ship;
	public GameObject	Bullet;
	public GameObject	Fire1;
	public GameObject	Fire2;
	[Range (0, 0.5f)]
	public float		coolDown = 3f;

	private bool		fireOn = false;
	private bool		fireSwap = false;
	private float		nextFire = 0;
	
	void Update()
	{
		if (this.fireOn && Time.time > this.nextFire)
		{
			GameObject fire = (this.fireSwap ? this.Fire1 : this.Fire2);
			GameObject bullet = Network.Instantiate(this.Bullet, fire.transform.position + fire.transform.forward * 10, fire.transform.rotation, 0) as GameObject;
			bullet.GetComponent<BulletScript>().SetVelocity(Vector3.zero);
			this.fireSwap = !this.fireSwap;
			this.nextFire = Time.time + this.coolDown;
			this.audio.Play();
		}
	}

	public void Fire(bool activate)
	{
		this.fireOn = activate;
	}
}
