using UnityEngine;
using System.Collections;

public class get_new_weapon : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.gameObject.GetComponent<Inventory>().DropWeapon((Weapon.Type)Random.Range((int)Weapon.Type.Rocket, (int)Weapon.Type.Repair + 1)); ;
			Destroy(this.gameObject);
		}
    }
	
}
