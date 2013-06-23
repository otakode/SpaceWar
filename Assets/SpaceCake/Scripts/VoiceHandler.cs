using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class VoiceHandler : MonoBehaviour
{
	private PXCUPipeline pp;
	private string[] commands;
	private delegate void actions(string param);

	public enum Weapon
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
	private Weapon activeWeapon;
	private List<Weapon> inventory;

	private int speed;

	void Start()
	{
		this.activeWeapon = Weapon.Rifle;
		this.inventory = new List<Weapon>();
		this.inventory.Add(Weapon.Rifle);
		this.speed = 0;

		this.pp = new PXCUPipeline();

		if (this.pp.Init(PXCUPipeline.Mode.VOICE_RECOGNITION))
			Debug.Log("initialized Voice Recognition");
		else
			Debug.Log("initialize Voice Recognition FAILED");

		List<string> commandList = new List<string>();
		for (int i = 1; i <= 5; i++)
		{
			commandList.Add("Weapon " + i);
		}
		commandList.Add("Rifle");
		commandList.Add("Rocket");
	//	commandList.Add("");
	//	commandList.Add("");
	//	commandList.Add("");
		commandList.Add("Shield");
	//	commandList.Add("");
		commandList.Add("Stealth");
		commandList.Add("Bomb");
		commandList.Add("Nova");
		commandList.Add("Repair");
		commandList.Add("Fire");
		commandList.Add("Activate");
		for (int i = 0; i <= 100; i++)
		{
			commandList.Add("Speed at " + i + " percent");
		}
		commandList.Add("Maximum speed");
		this.commands = commandList.ToArray();
		this.pp.SetVoiceCommands(this.commands);
	}
	
	void OnDisable()
	{
		this.pp.Close();
		this.pp.Dispose();
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			this.DropWeapon((Weapon)Random.Range((int)Weapon.Rocket, (int)Weapon.Repair));
		}
		if (this.pp != null && this.pp.AcquireFrame(false))
		{
			PXCMVoiceRecognition.Recognition voice;
			if (this.pp.QueryVoiceRecognized(out voice) && voice.confidence > 30 && voice.label <= this.commands.Length)
			{
				string command = this.commands[voice.label];
				Debug.Log("Command: " + command);
				if (command == "Fire" || command == "Activate")
				{
					this.Fire();	
				}
				else if (command == "Maximum speed")
				{
					this.ChangeSpeed(100);
				}
				else if (command.StartsWith("Speed at "))
				{
					this.ChangeSpeed(int.Parse(command.Split(new char[]{' '})[2]));
				}
				else if (command.StartsWith("Weapon "))
				{
					this.ChangeWeaponIndex(int.Parse(command.Substring(command.IndexOf(" ") + 1)));
				}
				else
				{
					this.ChangeWeaponName(command);
				}
			}

			this.pp.ReleaseFrame();
		}
	}

	void OnGUI()
	{
		GUILayout.BeginVertical();
		foreach (Weapon weapon in this.inventory)
		{
			GUILayout.Label(new GUIContent(Name(weapon)));
		}
		GUILayout.EndVertical();
	}
	
	static string Name(Weapon weapon)
	{
		switch (weapon)
		{
			case Weapon.Rifle:
				return "Rifle";
			case Weapon.Rocket:
				return "Rocket";
	//		case Weapon.:
	//			return "";
	//		case Weapon.:
	//			return "";
	//		case Weapon.:
	//			return "";
			case Weapon.Shield:
				return "Shield";
	//		case Weapon.:
	//			return "";
			case Weapon.Stealth:
				return "Stealth";
			case Weapon.Bomb:
				return "Bomb";
			case Weapon.Nova:
				return "Nova";
			case Weapon.Repair:
				return "Repair";
		}
		return "None";
	}

	static Weapon Enum(string weapon)
	{
		switch (weapon)
		{
			case "Rifle":
				return Weapon.Rifle;
			case "Rocket":
				return Weapon.Rocket;
	//		case "":
	//			return Weapon.;
	//		case "":
	//			return Weapon.;
	//		case "":
	//			return Weapon.;
			case "Shield":
				return Weapon.Shield;
	//		case "":
	//			return Weapon.;
			case "Stealth":
				return Weapon.Stealth;
			case "Bomb":
				return Weapon.Bomb;
			case "Nova":
				return Weapon.Nova;
			case "Repair":
				return Weapon.Repair;
		}
		return Weapon.None;
	}

	bool HasWeapon(Weapon test)
	{
		foreach (Weapon weapon in this.inventory)
		{
			if (weapon == test)
				return true;
		}
		return false;
	}
	
	void ChangeWeaponName(string name)
	{
		Weapon weapon = Enum(name);
		if (this.HasWeapon(weapon))
		{
			this.activeWeapon = weapon;
			Debug.Log(name + " ready.");
		}
		else
		{
			Debug.Log(name + " not found...");
		}
	}

	void ChangeWeaponIndex(int index)
	{
		if (index < this.inventory.Count)
		{
			this.activeWeapon = this.inventory[index];
			Debug.Log(Name(this.activeWeapon) + " ready.");
		}
		else
		{
			Debug.Log("Weapon " + index + " not found...");
		}
	}

	void Fire()
	{
		switch (this.activeWeapon)
		{
			case Weapon.Rifle:
				Debug.Log("pew pew pew pew pew");
			this.GetComponent<Spaceship>().Fire();
				break;
			case Weapon.Rocket:
				Debug.Log("pew... BOOM");
				this.inventory.Remove(Weapon.Rocket);
				break;
		//	case Weapon.:
		//		Debug.Log("");
		//		break;
		//	case Weapon.:
		//		Debug.Log("");
		//		break;
		//	case Weapon.:
		//		Debug.Log("");
		//		break;
			case Weapon.Shield:
				Debug.Log("Bouclier anti-mourant, empechant la mort de passer");
				this.inventory.Remove(Weapon.Shield);
				break;
		//	case Weapon.:
		//		Debug.Log("");
		//		break;
			case Weapon.Stealth:
				Debug.Log("NINJA");
				this.inventory.Remove(Weapon.Stealth);
				break;
			case Weapon.Bomb:
				Debug.Log("bomb");
				this.inventory.Remove(Weapon.Bomb);
				break;
			case Weapon.Nova:
				Debug.Log("Nova");
				this.inventory.Remove(Weapon.Nova);
				break;
			case Weapon.Repair:
				Debug.Log("Repair");
				break;
		}
	}

	void DropWeapon(Weapon weapon)
	{
		if (this.inventory.Count < 5)
		{
			Debug.Log(Name(weapon) + " dropped.");
			this.inventory.Add(weapon);
		}
		else
		{
			Debug.Log("Inventory full.");
		}
	}

	void ChangeSpeed(int percent)
	{
		this.speed = percent;
		Debug.Log("Speed at " + percent + " percent.");
	}
}
