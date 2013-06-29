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
		commandList.Add(Weapon.TypeToString(Weapon.Type.Boost));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Alien));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Hack));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Shield));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Return));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Stealth));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Bomb));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Nova));
		commandList.Add(Weapon.TypeToString(Weapon.Type.Repair));
		commandList.Add("Fire");
		commandList.Add("Activate");
		commandList.Add("Launch");
		commandList.Add("Zero");
		commandList.Add("Min");
		commandList.Add("Max");
		this.commands = commandList.ToArray();

		//this.pp = PerCPipeline.GetPipeline();
		this.pp = new PXCUPipeline();
		if (this.pp != null && this.pp.Init(PXCUPipeline.Mode.VOICE_RECOGNITION))
		{
			this.pp.SetVoiceCommands(this.commands);
			Debug.Log("Voice Handler Init SUCCESS");
		}
		else
			Debug.Log("Voice Handler Init Failed");
		//PerCPipeline.pipelineUpdate += this.pipelineUpdate;
	}

	void OnDisable()
	{
		this.pp.Close();
		this.pp.Dispose();
		//PerCPipeline.pipelineUpdate -= this.pipelineUpdate;
	}

	//void pipelineUpdate(PerCPipeline.PipelineData data)
	void Update()
	{
		if (!this.pp.AcquireFrame(false))
			return;
		PerCPipeline.PipelineData data = new PerCPipeline.PipelineData();
		data.hasVoice = this.pp.QueryVoiceRecognized(out data.voice);

		if (data.hasVoice && data.voice.confidence > 20/* && data.voice.label < this.commands.Length*/)
		{
			this.command = this.commands[data.voice.label];
			Debug.Log(this.command);
			if (command == "Fire" || command == "Activate" || command == "Launch")
			{
				this.GetComponent<Inventory>().Fire();
			}
			else if (command == "Zero")
			{
				this.ChangeSpeed(0);
			}
			else if (command == "Min")
			{
				this.ChangeSpeed(50);
			}
			else if (command == "Max")
			{
				this.ChangeSpeed(100);
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
		this.pp.ReleaseFrame();
	}

	void ChangeSpeed(int percent)
	{
		this.GetComponent<Spaceship>().thrusters[0].SetThrusterPower(percent);
		Debug.Log("Speed at " + percent + " percent.");
	}
}
