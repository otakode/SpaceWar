using UnityEngine;
using System.Collections;

public class tete_chercheuse : MonoBehaviour
{

	public Transform	target;
	public int			moveSpeed;
	public int			rotationSpeed;
	public int 			maxDistance;
	private Transform	myTransform;
	public	float		timer = 8;
	private	float		p_timer;
	
	void	Awake()
	{
		myTransform = transform;
	}
	// Use this for initialization
	void Start ()
	{
		p_timer = timer;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer-= Time.deltaTime;
		if (timer > 0)
		{	
			if (target != null)
			{
				//Debug.DrawLine(target.transform.position, myTransform.position, Color.red);
				myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion .LookRotation(target.position - myTransform.position),rotationSpeed * Time.deltaTime);
				if (Vector3.Distance(target.position,myTransform.position) > maxDistance)
				{
					myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
				}
			}
		}
		else
			Destroy(gameObject);
	}
	/*
	private void FindTarget()
	{
		if(gameObject.tag != "bulletPlayer")	
		{
			GameObject go = GameObject.FindGameObjectWithTag("Player");
			target = go.transform;
		}
		else
		{
			// On focus l'ennemi le plus proche
			GameObject[] ennemies = GameObject.FindGameObjectsWithTag("enemy");
			float distMin = 9999;
			GameObject enemyTarget;
			foreach (GameObject enemy in ennemies)
			{
				float dist = Vector3.Distance(gameObject.transform.position,enemy.transform.position);
				if (dist < distMin)
				{
					enemyTarget = enemy;
					distMin = dist;	
					target = enemyTarget.transform;
				}					
			}
			
			if (distMin == 9999)
			{
				// Pas de cible
				target = null;
			}
			
		}
	}*/
}
