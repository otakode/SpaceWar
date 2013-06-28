using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class VoiceHandler : MonoBehaviour
{
	private PXCUPipeline pp;
	private string[] commands;

	private string command;

	void Start()
	{
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
		commandList.Add("Stop");
		commandList.Add("Slow");
		commandList.Add("Fast");
		this.commands = commandList.ToArray();

		//this.pp = PerCPipeline.GetPipeline();
		//this.pp = new PXCUPipeline();
	//	if (this.pp != null && this.pp.Init(PXCUPipeline.Mode.VOICE_RECOGNITION))
	//	{
	//		this.pp.SetVoiceCommands(this.commands);
	//		Debug.Log("Voice Handler Init SUCCESS");
	//	}
	//	else
	//		Debug.Log("Voice Handler Init Failed");
		//PerCPipeline.pipelineUpdate += this.pipelineUpdate;
	}

	void OnDisable()
	{
		//PerCPipeline.pipelineUpdate -= this.pipelineUpdate;
	}

	//void pipelineUpdate(PerCPipeline.PipelineData data)
	void Update2()
	{
		PerCPipeline.PipelineData data;
		data.hasVoice = this.pp.QueryVoiceRecognized(out data.voice);

		Debug.Log("a");
		if (data.hasVoice && data.voice.confidence > 30 && data.voice.label <= this.commands.Length)
		{
			this.command = this.commands[data.voice.label];
			if (command == "Fire" || command == "Activate" || command == "Launch")
			{
				this.GetComponent<Inventory>().Fire();	
			}
			else if (command == "Stop")
			{
				this.ChangeSpeed(0);
			}
			else if (command == "Half speed")
			{
				this.ChangeSpeed(50);
			}
			else if (command == "Maximum speed" || command == "Max speed")
			{
				this.ChangeSpeed(100);
			}
			else if (command == "Slower" || command == "Speed down")
			{
				this.Slower();
			}
			else if (command == "Faster" || command == "Speed up")
			{
				this.Faster();
			}
			else if (command.StartsWith("Speed at "))
			{
				this.ChangeSpeed(int.Parse(command.Split(new char[]{' '})[2]));
			}
			else if (command.StartsWith("Weapon "))
			{
				this.GetComponent<Inventory>().ChangeWeapon(int.Parse(command.Substring(command.IndexOf(" ") + 1)));
			}
			else
			{
				this.GetComponent<Inventory>().ChangeWeapon(command);
			}
		}
	}

/*	void FixedUpdate()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			this.DropWeapon((Weapon.Type)Random.Range((int)Weapon.Type.Rocket, (int)Weapon.Type.Repair));
		}
		if (this.pp != null && this.pp.AcquireFrame(false))
		{
			PXCMVoiceRecognition.Recognition voice;
			if (this.pp.QueryVoiceRecognized(out voice) && voice.confidence > 30 && voice.label <= this.commands.Length)
			{
				string command = this.commands[voice.label];
				Debug.Log("Command: " + command);
				if (command == "Fire" || command == "Activate" || command == "Launch")
				{
					this.Fire();	
				}
				else if (command == "Stop")
				{
					this.ChangeSpeed(0);
				}
				else if (command == "Half speed")
				{
					this.ChangeSpeed(50);
				}
				else if (command == "Maximum speed" || command == "Max speed")
				{
					this.ChangeSpeed(100);
				}
				else if (command == "Slower" || command == "Speed down")
				{
					this.Slower();
				}
				else if (command == "Faster" || command == "Speed up")
				{
					this.Faster();
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
	}*/


	void ChangeSpeed(int percent)
	{
		this.GetComponent<Spaceship>().thrusters[0].SetThrusterPower(percent);
		Debug.Log("Speed at " + percent + " percent.");
	}

	void Slower()
	{
		Thruster thruster = this.GetComponent<Spaceship>().thrusters[0];
		thruster.SetThrusterPower(Mathf.Max((int)0, (int)(thruster.percent * 100 - 10)));
		Debug.Log("Speed at " + thruster.percent * 100 + " percent.");
	}

	void Faster()
	{
		Thruster thruster = this.GetComponent<Spaceship>().thrusters[0];
		thruster.SetThrusterPower(Mathf.Min((int)100, (int)(thruster.percent * 100 + 10)));
		Debug.Log("Speed at " + thruster.percent * 100 + " percent.");
	}
}
