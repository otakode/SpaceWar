using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	private List<Weapon>	inventory;
	private TextMesh[]		weaponsText;
	private Weapon			activeWeapon;
	private GameObject		target;
	private float			timeSeen;
	public float			lockTime = 3f;
	public float			angleLost = 15f;
	public GameObject		targetLock;
	public GameObject		rocket_prefab;
	public GameObject		caisse_prefab;

	public GameObject		bouclier_prefab;

	public Texture			locker;
	private float			lockAngle=0.0f;


	void Start()
	{
		this.inventory = new List<Weapon>();
		this.inventory.Add(Weapon.GetWeapon(Weapon.Type.Rifle));
		Transform cockpit = this.transform.FindChild("Model3D").FindChild("Cockpit");
		this.weaponsText = new TextMesh[]{
			cockpit.FindChild("Weapon1").GetComponent<TextMesh>(),
			cockpit.FindChild("Weapon2").GetComponent<TextMesh>(),
			cockpit.FindChild("Weapon3").GetComponent<TextMesh>(),
			cockpit.FindChild("Weapon4").GetComponent<TextMesh>(),
			cockpit.FindChild("Weapon5").GetComponent<TextMesh>()
		};
		this.activeWeapon = this.inventory[0];
		this.target = null;
		this.targetLock = null;
		Rocket.prefab = rocket_prefab;
		Bomb.prefab = caisse_prefab;
		Boost.prefab = this.gameObject;
		Shield.prefab = bouclier_prefab;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			this.DropWeapon((Weapon.Type)Random.Range((int)Weapon.Type.Rocket, (int)Weapon.Type.Repair + 1));
		}
		Ray ray = new Ray(this.transform.position, this.transform.forward);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit, 50000))
		{

			if (this.target == null)
			{
				this.target = hit.collider.gameObject;
				this.timeSeen = Time.time;
			}
		}
		if (this.targetLock != null)
		{
			if (Vector3.Angle(this.transform.forward, this.targetLock.transform.position - this.transform.position) > this.angleLost)
			{
				this.targetLock = null;
			}
		}
		if (this.targetLock == null && this.target != null)
		{
			if (Vector3.Angle(this.transform.forward, this.target.transform.position - this.transform.position) > this.angleLost)
			{
				this.target = null;
			}
			else if (Time.time - this.timeSeen > this.lockTime)
			{
				this.targetLock = this.target;
				this.target = null;
			}
		}
	}

	void OnGUI()
	{
		if (this.targetLock != null)
		{
			if (!OculusMode.on)
			{
				Vector3 test = this.transform.FindChild("Camera").transform.FindChild("Main Camera").camera.WorldToScreenPoint(this.targetLock.transform.position);
				GUI.DrawTexture(new Rect(test.x - (locker.width * 0.5f), (Screen.height - test.y) - (locker.height * 0.5f), locker.width, locker.height), locker);
			}
			else
			{
				Vector3 test = this.transform.FindChild("Camera").transform.FindChild("OVRCameraController").FindChild("CameraLeft").camera.WorldToScreenPoint(this.targetLock.transform.position);
				GUI.DrawTexture(new Rect(test.x - (locker.width * 0.5f), (Screen.height - test.y) - (locker.height * 0.5f), locker.width, locker.height), locker);
				Vector3 test2 = this.transform.FindChild("Camera").transform.FindChild("OVRCameraController").FindChild("CameraRight").camera.WorldToScreenPoint(this.targetLock.transform.position);
				GUI.DrawTexture(new Rect(test2.x - (locker.width * 0.5f), (Screen.height - test2.y) - (locker.height * 0.5f), locker.width, locker.height), locker);
			}
		}
		GUILayout.BeginVertical();
		int i = 0;
		foreach (Weapon weapon in this.inventory)
		{
			//GUILayout.Label(new GUIContent(weapon.ToString()));
			this.weaponsText[i].text = weapon.ToString();
			if (weapon == this.activeWeapon)
				this.weaponsText[i].GetComponent<TextController>().SetColor(Color.green);
			else
				this.weaponsText[i].GetComponent<TextController>().SetColor(Color.white);
			i++;
		}
		while (i < 5)
		{
			this.weaponsText[i].text = "";
			i++;
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
	{
		this.activeWeapon.Fire(this.transform.position, this.transform.rotation, this.gameObject, this.targetLock);
		if (this.activeWeapon.ammo == 0)
		{
			this.inventory.Remove(this.activeWeapon);
			this.activeWeapon = this.inventory[0];
		}
	}

	public void DropWeapon(Weapon.Type type)
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
