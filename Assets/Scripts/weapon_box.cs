using UnityEngine;
using System.Collections;

public class weapon_box : MonoBehaviour
{
	public int dmg;
	public GameObject source;
	
	public void set_source(GameObject n_source)
	{
		source = n_source;
	}
	
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
		if (other.networkView.viewID != source.networkView.viewID)
		{
			other.gameObject.GetComponent<Spaceship>().set_life(-dmg);
			Destroy(this.gameObject);
		}
    }
}
