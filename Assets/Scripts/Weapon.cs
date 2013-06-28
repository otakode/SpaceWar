using UnityEngine;
using System.Collections;

public class Weapon
{
	public enum Type
	{
		None,
		Rifle,
		Rocket,
	//	,
	//	,
	//	,
		Shield,
	//	,
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
	//		case Type.:
	//			return "";
	//		case Type.:
	//			return "";
	//		case Type.:
	//			return "";
			case Type.Shield:
				return "Shield";
	//		case Type.:
	//			return "";
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
	//		case "":
	//			return Type.;
	//		case "":
	//			return Type.;
	//		case "":
	//			return Type.;
			case "Shield":
				return Type.Shield;
	//		case "":
	//			return Type.;
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
/*			case Weapon.Type.:
				weapon = new ();
				break;*/
/*			case Weapon.Type.:
				weapon = new ();
				break;*/
/*			case Weapon.Type.:
				weapon = new ();
				break;*/
			case Weapon.Type.Shield:
				weapon = new Shield();
				break;
/*			case Weapon.Type.:
				weapon = new ();
				break;*/
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

	public virtual bool Fire(Vector3 pos, Quaternion rot, GameObject target)
	{
		return false;
	}
}

public class Rifle : Weapon
{
	public Rifle() : base(Weapon.Type.Rifle, 0)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		Debug.Log("pew pew pew pew pew");
		return true;
	}
}

public class Rocket : Weapon
{
	public Rocket() : base(Weapon.Type.Rocket, 3)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("bang... BOOM");
		return true;
	}
}
/*
public class  : Weapon
{
	public () : base(Weapon.Type.)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("");
		return true;
	}
}*/
/*
public class  : Weapon
{
	public () : base(Weapon.Type.)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("");
		return true;
	}
}*/
/*
public class  : Weapon
{
	public () : base(Weapon.Type.)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("");
		return true;
	}
}*/

public class Shield : Weapon
{
	public Shield() : base(Weapon.Type.Shield, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("shield");
		return true;
	}
}
/*
public class  : Weapon
{
	public () : base(Weapon.Type.)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("");
		return true;
	}
}*/

public class Stealth : Weapon
{
	public Stealth() : base(Weapon.Type.Stealth, 1)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
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
	public Bomb() : base(Weapon.Type.Bomb, 5)
	{
	}

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		if (this.ammo == 0)
			return false;
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

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
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

	public override bool Fire (Vector3 pos, Quaternion rot, GameObject target)
	{
		if (this.ammo == 0)
			return false;
		this.ammo--;
		Debug.Log("");
		return true;
	}
}