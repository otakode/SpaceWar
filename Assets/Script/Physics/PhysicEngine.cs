using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicEngine : MonoBehaviour
{
	static private PhysicEngine instance = null;

	private ForceRegistry registry = null;
	private List<PhysicObject> world = new List<PhysicObject>();

	static public PhysicEngine Instance()
	{
		return (PhysicEngine.instance);
	}

	void Start()
	{
		PhysicEngine.instance = this;
		this.registry = ForceRegistry.Instance();
	}

	void Update()
	{
		float t = Time.deltaTime;
		this.registry.ComputeForces(t);
		foreach (PhysicObject o in this.world)
		{
			o.ComputePhysics(t);
		}
	}

	public void RegisterObject(PhysicObject o)
	{
		this.world.Add(o);
	}

	public void UnRegisterObject(PhysicObject o)
	{
		this.world.Remove(o);
	}
}
