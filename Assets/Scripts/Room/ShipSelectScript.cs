using UnityEngine;
using System.Collections;

public class ShipSelectScript : MonoBehaviour
{
	public GameObject[] ShipsPrefab;

	public float	RotationSpeed = 50;
	public float	RepeatKey = 0.3f;
	
	[HideInInspector]
	public int				SelectedShipType = 0;
	private GameObject[]	Ships = null;
	private Quaternion[]	ShipsRotation = null;
	private GameObject		SelectedShip = null;
	private int				LastSelectedShip = 0;

	private float	Radius = 50;
	private float	TurnStep = 1;

	private float	nextKey = 0;

	private float		beginRotation = 0;
	private Quaternion	targetRotation = Quaternion.identity;

	void Start()
	{
		this.Ships = new GameObject[this.ShipsPrefab.Length];
		this.ShipsRotation = new Quaternion[this.ShipsPrefab.Length];
		this.TurnStep = 360f / this.Ships.Length;
		for (int i = 0 ; i < this.Ships.Length ; ++i)
		{
			float angle = i * TurnStep;
			float radAngle = angle * Mathf.PI / 180;
			this.Ships[i] = Instantiate(this.ShipsPrefab[i]) as GameObject;
			GameObject reflect = Instantiate(this.ShipsPrefab[i]) as GameObject;
			reflect.transform.parent = this.Ships[i].transform;
			reflect.transform.localPosition = new Vector3(0, -10, 0);
			reflect.transform.localRotation = Quaternion.identity;
			reflect.transform.localScale = new Vector3(1, -1, 1);
			this.Ships[i].transform.parent = this.transform;
			this.Ships[i].transform.localPosition = new Vector3(Mathf.Sin(radAngle), 0, Mathf.Cos(radAngle)) * this.Radius;
			this.ShipsRotation[i] = Quaternion.Euler(0, angle, 0);
			this.Ships[i].transform.localRotation = this.ShipsRotation[i];
		}
		this.SelectedShip = this.Ships[0];
		this.LastSelectedShip = 0;
		this.targetRotation = Quaternion.LookRotation(this.transform.forward);
	}
	
	void Update()
	{
		float change = Input.GetAxisRaw("Horizontal");
		if (change != 0 && Time.time > nextKey)
		{
			if (change > 0)
				this.TurnSelection(true);
			else
				this.TurnSelection(false);
			this.nextKey = Time.time + this.RepeatKey;
		}
		float rotationTime = Time.time - this.beginRotation;
		if (this.LastSelectedShip != this.SelectedShipType)
			this.Ships[this.LastSelectedShip].transform.localRotation = Quaternion.Lerp(this.Ships[this.LastSelectedShip].transform.localRotation, this.ShipsRotation[this.LastSelectedShip], rotationTime);
		this.SelectedShip.transform.Rotate(this.SelectedShip.transform.up, Time.deltaTime * this.RotationSpeed);
		this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, this.targetRotation, rotationTime);
	}

	void TurnSelection(bool clockwise)
	{
		this.LastSelectedShip = this.SelectedShipType;
		this.SelectedShipType = (this.SelectedShipType + this.Ships.Length + (clockwise ? -1 : 1)) % this.Ships.Length;
		this.SelectedShip = this.Ships[this.SelectedShipType];
		this.targetRotation = Quaternion.Euler(0, 180 - this.SelectedShipType * this.TurnStep, 0);
		this.beginRotation = Time.time;
	}
	
}
