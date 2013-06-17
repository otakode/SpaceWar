using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForceRegistry
{
	static private ForceRegistry instance = null;
	private List<KeyValuePair<ForceGenerator, PhysicObject>> forces = new List<KeyValuePair<ForceGenerator, PhysicObject>>();

	static public ForceRegistry Instance()
	{
		if (ForceRegistry.instance == null)
			ForceRegistry.instance = new ForceRegistry();
		return (ForceRegistry.instance);
	}
	
	public void RegisterForce(ForceGenerator f, PhysicObject o)
	{
		forces.Add(new KeyValuePair<ForceGenerator, PhysicObject>(f, o));
	}
	
	public void UnRegisterForce(ForceGenerator f, PhysicObject o)
	{
		foreach (KeyValuePair<ForceGenerator, PhysicObject> p in forces)
		{
			if (p.Key == f && p.Value == o)
				forces.Remove(p);
		}
	}

	public void ComputeForces(float t)
	{
		foreach (KeyValuePair<ForceGenerator, PhysicObject> p in forces)
		{
			p.Key.UpdateForce(p.Value, t);
		}
	}
}