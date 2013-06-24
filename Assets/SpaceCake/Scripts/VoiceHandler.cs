using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class VoiceHandler : MonoBehaviour
{
	private PXCUPipeline pp;
	private string[] commands;
	private delegate void actions(string param);

	private Weapon.Type activeWeapon;
	private List<Weapon.Type> inventory;

	private int speed;

	void Start()
	{
		this.activeWeapon = Weapon.Type.Rifle;
		this.inventory = new List<Weapon.Type>();
		this.inventory.Add(Weapon.Type.Rifle);
		this.speed = 0;

		List<string> commandList = new List<string>();
		for (int i = 1; i <= 5; i++)
		{
			commandList.Add("Weapon " + i);
		}
		commandList.Add(Weapon.TypeToString(Weapon.Type.Rifle));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Rocket));
	//	commandList.Add(Weapon.TypeToString(Weapon.Type.);
	//	commandList.Add(Weapon.TypeToString(Weapon.Type.);
	//	commandList.Add(Weapon.TypeToString(Weapon.Type.);
		commandList.Add(Weapon.TypeToString(Weapon.Type.Shield));
	//	commandList.Add(Weapon.TypeToString(Weapon.Type.);
		commandList.Add(Weapon.TypeToString(Weapon.Type.Stealth));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Bomb));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Nova));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Repair));
		commandList.Add("Fire");
		commandList.Add("Activate");
		commandList.Add("Launch");
		for (int i = 0; i <= 100; i++)
		{
			commandList.Add("Speed at " + i + " percent");
		}
		commandList.Add("Maximum speed");
		this.commands = commandList.ToArray();

	//	this.pp = new PXCUPipeline();
		this.pp = PerCPipeline.GetPipeline();
		if (this.pp != null)
			this.pp.SetVoiceCommands(this.commands);
		else
			Debug.Log("Voice Handler Init Failed");
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			this.DropWeapon((Weapon.Type)Random.Range((int)Weapon.Type.Rocket, (int)Weapon.Type.Repair));
		}
		if (this.pp != null && this.pp.AcquireFrame(false))
		{
			PXCMVoiceRecognition.Recognition voice;
			if (this.pp.QueryVoiceRecognized(out voice) && voice.confidence > 20 && voice.label <= this.commands.Length)
			{
				string command = this.commands[voice.label];
				Debug.Log("Command: " + command);
				if (command == "Fire" || command == "Activate" || command == "Launch")
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
					this.ChangeWeapon(int.Parse(command.Substring(command.IndexOf(" ") + 1)));
				}
				else
				{
					this.ChangeWeapon(command);
				}
			}

			this.pp.ReleaseFrame();
		}
	}

	void OnGUI()
	{
		GUILayout.BeginVertical();
		foreach (Weapon.Type weapon in this.inventory)
		{
			GUILayout.Label(new GUIContent(Weapon.TypeToString(weapon)));
		}
		GUILayout.EndVertical();
	}

	bool HasWeapon(Weapon.Type test)
	{
		foreach (Weapon.Type weapon in this.inventory)
		{
			if (weapon == test)
				return true;
		}
		return false;
	}
	
	void ChangeWeapon(string name)
	{
		Weapon.Type weapon = Weapon.StringToType(name);
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

	void ChangeWeapon(int index)
	{
		if (index < this.inventory.Count)
		{
			this.activeWeapon = this.inventory[index];
			Debug.Log(Weapon.TypeToString(this.activeWeapon) + " ready.");
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
		}
	}

	void DropWeapon(Weapon.Type weapon)
	{
		if (this.inventory.Count < 5)
		{
			Debug.Log(Weapon.TypeToString(weapon) + " dropped.");
			this.inventory.Add(weapon);
		}
		else
		{
			if (weapon == Weapon.Type.Repair)
			{
				Debug.Log("Repair");
			}
			else
			{
				Debug.Log("Inventory full.");
			}
		}
	}

	void ChangeSpeed(int percent)
	{
		this.speed = percent;
		Debug.Log("Speed at " + percent + " percent.");
	}
}
