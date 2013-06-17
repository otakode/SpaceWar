using UnityEngine;
using System.Collections;

public class gravity : ForceGenerator
{
	float G;

	public gravity(float g)
	{
		G = g;
	}

	public void UpdateForce(PhysicObject o, float t)
	{
		o.AddForce(Vector3.down * G);
	}
}
