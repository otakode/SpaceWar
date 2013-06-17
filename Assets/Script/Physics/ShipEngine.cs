using UnityEngine;
using System.Collections;

public class ShipEngine : ForceGenerator
{
	public int			activated = 0;
	
	private GameObject	ship;

	public ShipEngine(GameObject g)
	{
		ForceRegistry.Instance().RegisterForce(this, g.GetComponent<PhysicObject>());
		this.ship = g;
	}

	public void UpdateForce(PhysicObject o, float t)
	{
		if (this.activated != 0)
		{
			o.AddForce(this.ship.transform.forward * this.activated * 50);
		}
	}
}
