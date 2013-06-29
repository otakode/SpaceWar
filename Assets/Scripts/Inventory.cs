using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	private List<Weapon> inventory;
	private Weapon activeWeapon;
	private GameObject target;
	private float timeSeen;
	public float lockTime = 3f;
	public float angleLost = 15f;
	private GameObject targetLock;

	void Start()
	{
		this.inventory = new List<Weapon>();
		this.inventory.Add(Weapon.GetWeapon(Weapon.Type.Rifle));
		this.activeWeapon = this.inventory[0];
		this.target = null;
		this.targetLock = null;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			this.DropWeapon((Weapon.Type)Random.Range((int)Weapon.Type.Rocket, (int)Weapon.Type.Repair + 1));
		}
		RaycastHit hit;
		if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 400))
		{
			if (this.target != null && this.target != hit.collider.gameObject)
			{
				this.target = hit.collider.gameObject;
				this.timeSeen = Time.time;
				Debug.Log("Seen");
			}
		}
		else
		{
			if (this.targetLock != null)
			{
				if (Vector3.Angle(this.transform.forward, this.targetLock.transform.position - this.transform.position) > this.angleLost)
				{
					this.targetLock = null;
				}
			}
			if (this.targetLock == null && this.target != null)
			{
				if (Vector3.Angle(this.transform.forward, this.target.transform.position - this.transform.position) > this.angleLost)
				{
					this.target = null;
				}
				else if (Time.time - this.timeSeen > this.lockTime)
				{
					this.targetLock = this.target;
					this.target = null;
					Debug.Log("Locked " + this.targetLock.name);
				}
			}
		}
	}

	void OnGUI()
	{
		GUILayout.BeginVertical();
		foreach (Weapon weapon in this.inventory)
		{
			GUILayout.Label(new GUIContent(weapon.ToString()));
		}
		GUILayout.EndVertical();
	}

	public bool HasWeapon(Weapon.Type test)
	{
		foreach (Weapon weapon in this.inventory)
		{
			if (weapon.type == test)
				return true;
		}
		return false;
	}
	
	public void ChangeWeapon(string name)
	{
		Weapon.Type type = Weapon.StringToType(name);
		if (this.HasWeapon(type))
		{
			foreach (Weapon weapon in this.inventory)
			{
				if (weapon.type == type)
				{
					this.activeWeapon = weapon;
					Debug.Log(name + " ready.");
					break;
				}
			}
		}
		else
		{
			Debug.Log(name + " not found...");
		}
	}

	public void ChangeWeapon(int index)
	{
		if (index < this.inventory.Count)
		{
			this.activeWeapon = this.inventory[index];
			Debug.Log(this.activeWeapon.ToString() + " ready.");
		}
		else
		{
			Debug.Log("Weapon " + index + " not found...");
		}
	}

	public void Fire(GameObject target = null)
	{
		this.activeWeapon.Fire(this.transform.position, this.transform.rotation, this.gameObject, target);
	}

	void DropWeapon(Weapon.Type type)
	{
		Weapon weapon;
		if (type == Weapon.Type.Repair)
		{
			weapon = new Repair();
			weapon.Fire(this.transform.position, this.transform.rotation, this.gameObject, null);
		}
		else if (this.inventory.Count < 5)
		{
			weapon = Weapon.GetWeapon(type);
			Debug.Log(Weapon.TypeToString(type) + " dropped.");
			this.inventory.Add(weapon);
		}
		else
		{
			Debug.Log("Inventory full.");
		}
	}
}
