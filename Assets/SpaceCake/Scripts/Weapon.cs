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

	public Type type { get; private set; }
	public int ammo { get; private set; }

	Weapon(Type t, int a)
	{
		this.type = t;
		this.ammo = a;
	}

	public virtual void Fire(Vector3 pos, Quaternion rot)
	{
	}
}
/*
public class Rifle : Weapon
{
	public GameObject bullet;

	Rifle() : base(Type.Rifle)
	{
	}

	public override void Fire (Vector3 pos, Quaternion rot)
	{
		Transform laserShot = (Transform) Instantiate(bullet, pos, transform.rotation);
		laserShot.GetComponent<LaserShot>().firedBy = transform;
	}
}

public class Rocket : Weapon
{
	Rocket() : base(Type.Rocket)
	{
	}
}*/
/*
public class  : Weapon
{
	() : base(Type.)
	{
	}
}*/
/*
public class  : Weapon
{
	() : base(Type.)
	{
	}
}*/
/*
public class  : Weapon
{
	() : base(Type.)
	{
	}
}*/
/*
public class Shield : Weapon
{
	Shield() : base(Type.Shield)
	{
	}
}*/
/*
public class  : Weapon
{
	() : base(Type.)
	{
	}
}*/
/*
public class Stealth : Weapon
{
	Stealth() : base(Type.Stealth)
	{
	}
}

public class Bomb : Weapon
{
	Bomb() : base(Type.Bomb)
	{
	}
}

public class Nova : Weapon
{
	Nova() : base(Type.Nova)
	{
	}
}

public class Repair : Weapon
{
	Repair() : base(Type.Repair)
	{
	}
}*/
