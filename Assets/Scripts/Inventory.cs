using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	private List<Weapon> inventory;
	private Weapon activeWeapon;

	void Start()
	{
		this.inventory = new List<Weapon>();
		this.inventory.Add(Weapon.GetWeapon(Weapon.Type.Rifle));
		this.activeWeapon = this.inventory[0];
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			this.DropWeapon((Weapon.Type)Random.Range((int)Weapon.Type.Rocket, (int)Weapon.Type.Repair + 1));
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
	{/*
		switch (this.activeWeapon)
		{
			case Weapon.Type.Rifle:
				Debug.Log("pew pew pew pew pew");
				this.GetComponent<Spaceship>().Fire();
				break;
			case Weapon.Type.Rocket:
				Debug.Log("pew... BOOM");
				this.inventory.Remove(Weapon.Type.Rocket);
				break;
		//	case Weapon.Type.:
		//		Debug.Log("");
		//		break;
		//	case Weapon.Type.:
		//		Debug.Log("");
		//		break;
		//	case Weapon.Type.:
		//		Debug.Log("");
		//		break;
			case Weapon.Type.Shield:
				Debug.Log("Bouclier anti-mourant, empechant la mort de passer");
				this.inventory.Remove(Weapon.Type.Shield);
				break;
		//	case Weapon.Type.:
		//		Debug.Log("");
		//		break;
			case Weapon.Type.Stealth:
				Debug.Log("NINJA");
				this.inventory.Remove(Weapon.Type.Stealth);
				break;
			case Weapon.Type.Bomb:
				Debug.Log("bomb");
				this.inventory.Remove(Weapon.Type.Bomb);
				break;
			case Weapon.Type.Nova:
				Debug.Log("Nova");
				this.inventory.Remove(Weapon.Type.Nova);
				break;
			case Weapon.Type.Repair:
				Debug.Log("Repair");
				break;
		}*/
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
