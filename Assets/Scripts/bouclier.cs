using UnityEngine;
using System.Collections;

public class bouclier : MonoBehaviour 
{
	float	timer = 4;
	public GameObject source;
	
	public void set_source(GameObject new_source)
	{
		source = new_source;
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer-= Time.deltaTime;
		if (timer <= 0)
			Destroy(this.gameObject);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.networkView.viewID != source.networkView.viewID)
			Destroy(other.gameObject);
    }
}
