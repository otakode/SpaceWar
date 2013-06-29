using UnityEngine;
using System.Collections;

public class Weapon
{
	public enum Type
	{
		None,
		Rifle,
		Rocket,
		Boost,
		Alien,
		Hack,
		Shield,
		Return,
		Stealth,
		Bomb,
		Nova,
		Repair
	}

	public static string TypeToString(Type weapon)
	{
		switch (weapon)
		{
			case Type.Rifle:
				return "Rifle";
			case Type.Rocket:
				return "Rocket";
			case Type.Boost:
				return "Boost";
			case Type.Alien:
				return "Alien";
			case Type.Hack:
				return "Hack";
			case Type.Shield:
				return "Shield";
			case Type.Return:
				return "Return";
			case Type.Stealth:
				return "Stealth";
			case Type.Bomb:
				return "Bomb";
			case Type.Nova:
				return "Nova";
			case Type.Repair:
				return "Repair";
		}
		return "None";
	}
	
	public static Type StringToType(string weapon)
	{
		switch (weapon)
		{
			case "Rifle":
				return Type.Rifle;
			case "Rocket":
				return Type.Rocket;
			case "Boost":
				return Type.Boost;
			case "Alien":
				return Type.Alien;
			case "Hack":
				return Type.Hack;
			case "Shield":
				return Type.Shield;
			case "Return":
				return Type.Return;
			case "Stealth":
				return Type.Stealth;
			case "Bomb":
				return Type.Bomb;
			case "Nova":
				return Type.Nova;
			case "Repair":
				return Type.Repair;
		}
		return Type.None;
	}

	public static Weapon GetWeapon(Weapon.Type type)
	{
		Weapon weapon;
		switch (type)
		{
			case Weapon.Type.Rifle:
				weapon = new Rifle();
				break;
			case Weapon.Type.Rocket:
				weapon = new Rocket();
				break;
			case Weapon.Type.Boost:
				weapon = new Boost();
				break;
			case Weapon.Type.Alien:
				weapon = new Alien();
				break;
			case Weapon.Type.Hack:
				weapon = new Hack();
				break;
			case Weapon.Type.Shield:
				weapon = new Shield();
				break;
			case Weapon.Type.Return:
				weapon = new Return();
				break;
			case Weapon.Type.Stealth:
				weapon = new Stealth();
				break;
			case Weapon.Type.Bomb:
				weapon = new Bomb();
				break;
			case Weapon.Type.Nova:
				weapon = new Nova();
				break;
			case Weapon.Type.Repair:
				weapon = new Repair();
				break;
			default:
				weapon = new Rifle();
				break;
		}
		return weapon;
	}

	public Type type { get; protected set; }
	public int ammo { get; protected set; }

	public Weapon(Type t, int a)
	{
		this.type = t;
		this.ammo = a;
	}

	public virtual bool Fire(Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		return false;
	}
}

public class Rifle : Weapon
{
	public Rifle() : base(Weapon.Type.Rifle, 0)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		Spaceship.nb_fire = 5;
		Spaceship.boucle = true;
		Debug.Log("pew pew pew pew pew");
		return true;
	}
}

public class Rocket : Weapon
{
	public static GameObject prefab;
	private GameObject	tmp;
	public Rocket() : base(Weapon.Type.Rocket, 3)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		tmp = GameObject.Instantiate(prefab, pos, rot) as GameObject;
		tmp.GetComponent<tete_chercheuse>().set_target(target.transform);
		this.ammo--;
		Debug.Log("bang... BOOM");
		return true;
	}
}

public class Boost : Weapon
{
	public static GameObject prefab;
	public Boost() : base(Weapon.Type.Boost, 1)
	{
		
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
//		prefab.GetComponent<SpaceChipsController>().set_thrusters(950);
	//	prefab.GetComponent<SpaceChipsController>().set_thrusters(100);
		Debug.Log("Boost");
		return true;
	}
}

public class Alien : Weapon
{
	public Alien() : base(Weapon.Type.Alien, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("Alien");
		return true;
	}
}

public class Hack : Weapon
{
	public Hack() : base(Weapon.Type.Hack, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;

		this.ammo--;
		Debug.Log("Hack");
		return true;
	}
}

public class Shield : Weapon
{
	static public GameObject prefab;
	private GameObject tmp;
	public Shield() : base(Weapon.Type.Shield, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
			tmp = GameObject.Instantiate(prefab, pos, rot) as GameObject;
		tmp.GetComponent<bouclier>().set_source(source);

		Debug.Log("shield");
		return true;
	}
}

public class Return : Weapon
{
	public Return() : base(Weapon.Type.Return, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("Return");
		return true;
	}
}

public class Stealth : Weapon
{
	public Stealth() : base(Weapon.Type.Stealth, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("stealth");
		return true;
	}
}

public class Bomb : Weapon
{
	static public GameObject prefab;
	private GameObject tmp;
	public Bomb() : base(Weapon.Type.Bomb, 5)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		
		if (this.ammo == 0)
			return false;
		tmp = GameObject.Instantiate(prefab, pos, rot)as GameObject;
		tmp.GetComponent<weapon_box>().set_source(source);
		this.ammo--;
		Debug.Log("bomb");
		return true;
	}
}

public class Nova : Weapon
{
	public Nova() : base(Weapon.Type.Nova, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("nova");
		return true;
	}
}

public class Repair : Weapon
{
	public Repair() : base(Weapon.Type.Repair, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject source, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("");
		return true;
	}
}