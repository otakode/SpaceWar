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

	public static string TypeToString(Weapon.Type weapon)
	{
		switch (weapon)
		{
			case Weapon.Type.Rifle:
				return "Rifle";
			case Weapon.Type.Rocket:
				return "Rocket";
	//		case Weapon.Type.:
	//			return "";
	//		case Weapon.Type.:
	//			return "";
	//		case Weapon.Type.:
	//			return "";
			case Weapon.Type.Shield:
				return "Shield";
	//		case Weapon.Type.:
	//			return "";
			case Weapon.Type.Stealth:
				return "Stealth";
			case Weapon.Type.Bomb:
				return "Bomb";
			case Weapon.Type.Nova:
				return "Nova";
			case Weapon.Type.Repair:
				return "Repair";
		}
		return "None";
	}
	
	public static Weapon.Type StringToType(string weapon)
	{
		switch (weapon)
		{
			case "Rifle":
				return Weapon.Type.Rifle;
			case "Rocket":
				return Weapon.Type.Rocket;
	//		case "":
	//			return Weapon.Type.;
	//		case "":
	//			return Weapon.Type.;
	//		case "":
	//			return Weapon.Type.;
			case "Shield":
				return Weapon.Type.Shield;
	//		case "":
	//			return Weapon.Type.;
			case "Stealth":
				return Weapon.Type.Stealth;
			case "Bomb":
				return Weapon.Type.Bomb;
			case "Nova":
				return Weapon.Type.Nova;
			case "Repair":
				return Weapon.Type.Repair;
		}
		return Weapon.Type.None;
	}

}
