using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour
{
	public float	LifeTime = 1;
	
	public GameObject[]	Spheres;

	private float deathTime = 0;

	void Start()
	{
		this.deathTime = Time.time + this.LifeTime;
	}

	void Update()
	{
		if (Time.time > this.deathTime)
		{
			Destroy(this.gameObject);
			return;
		}
		float alpha = 1 - ((this.deathTime - Time.time) / this.LifeTime);
		foreach (GameObject sphere in this.Spheres)
		{
			sphere.transform.localScale *= 1 + Time.deltaTime;
			sphere.renderer.material.SetFloat("_alpha", alpha);
		}
	}
}
