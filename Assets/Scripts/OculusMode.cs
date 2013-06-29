using UnityEngine;
using System.Collections;

public delegate void ChangeOculusMode();

public class OculusMode : MonoBehaviour
{
	public static ChangeOculusMode changeOculusMode;
	private static bool _on = true;
	public static bool on
	{
		get
		{
			return OculusMode._on;
		}
		set
		{
			OculusMode._on = value;
			OculusMode.changeOculusMode();
		}
	}

	public bool activatedInOculusMode;

	void Awake()
	{
		this.OnChangeOculusMode();
		OculusMode.changeOculusMode += this.OnChangeOculusMode;
	}

	void OnDestroy()
	{
		OculusMode.changeOculusMode -= this.OnChangeOculusMode;
	}

	private void OnChangeOculusMode()
	{
		this.gameObject.SetActive(this.activatedInOculusMode == OculusMode.on);
	}
}
