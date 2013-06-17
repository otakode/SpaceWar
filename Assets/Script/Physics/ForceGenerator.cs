using UnityEngine;
using System.Collections;

public interface ForceGenerator
{
	void UpdateForce(PhysicObject o, float t);
}