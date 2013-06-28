using UnityEngine;
using System.Collections;

public class weapon_box : MonoBehaviour
{
	public int dmg;
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
		other.gameObject.GetComponent<Spaceship>().set_life(-dmg);
		Destroy(this.gameObject);
    }
}
