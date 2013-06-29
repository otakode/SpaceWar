using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Spaceship : MonoBehaviour 
{
	public int	life = 15;
	public Thruster[] thrusters;
	public Vector3[] weaponMountPoints;	
	public Transform laserShotPrefab;
	public AudioClip soundEffectFire;
	public float timeToRespawn = 10.0f;
    public static bool boucle = false;
    public static int nb_fire = 5;
    float timer = 0.1f;
	
	public void set_life(int dif)
	{
		life += dif;
	}
	
	void Start ()
	{	
		
		foreach (Thruster thruster in thrusters)
		{
			if (thruster == null) 
			{
				Debug.LogError("Thruster array not properly configured. Attach thrusters to the game object and link them to the Thrusters array.");
			}			
		}
	}
	
	void Update () 
	{
        if (boucle && nb_fire > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                Fire();
                nb_fire--;
                timer = 0.1f;
                Debug.Log("Timer");
            }
        }
        else
            boucle = false;
		if (life <= 0)
		{
			this.RPCRespawn();
			this.Respawn();
		}
	}

	[RPC]
	IEnumerable	RPCRespawn()
	{
		GameObject[]	spawners = GameObject.FindGameObjectsWithTag("Spawn");
		int				rand = Random.Range(0, spawners.Length);
		GameObject		spawn = spawners[rand];

		this.GetComponent<BoxCollider>().enabled = false;
		this.GetComponent<SpaceChipsController>().enabled = false;
		this.GetComponent<VoiceHandler>().enabled = false;
		this.transform.FindChild("model3D noCockpit noThruster").gameObject.SetActive(false);
		this.transform.position = spawn.transform.position;

		yield return new WaitForSeconds(this.timeToRespawn);

		this.GetComponent<BoxCollider>().enabled = true;
		this.GetComponent<SpaceChipsController>().enabled = true;
		this.GetComponent<VoiceHandler>().enabled = true;
		this.transform.FindChild("model3D noCockpit noThruster").gameObject.SetActive(true);
	}

	private void	Respawn()
	{
		this.transform.FindChild("model3D noCockpit noThruster").gameObject.SetActive(false);

	}

	public void Fire()
	{
		foreach (Vector3 wmp in weaponMountPoints) 
		{
			Vector3 pos = transform.position + transform.right * wmp.x + transform.up * wmp.y + transform.forward * wmp.z;
			Transform laserShot = (Transform) Network.Instantiate(laserShotPrefab, pos, transform.rotation, 0);
			laserShot.GetComponent<LaserShot>().firedBy = transform;
		}
		if (soundEffectFire != null) 
		{
			audio.PlayOneShot(soundEffectFire);
		}
	}
}
