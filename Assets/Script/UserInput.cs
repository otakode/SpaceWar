using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour
{
	public Camera		Eye;
	[Range (1, 500)]
	public float		Sensibility = 1;

	public float		range = 5000;
	public GameObject	star;
	public int 			number;

	private GameLauncher	launcher;

//	private GameObject	Ship;
	private ShipScript	shipScript = null;

	void Start()
	{
		GameObject connector = GameObject.Find("Connector") as GameObject;
		this.launcher = connector.GetComponent<GameLauncher>();
		if (this.launcher.Host)
		{
			Network.InitializeSecurity();
			Network.InitializeServer(50, int.Parse(this.launcher.localPort), false);
		}
		else
		{
			Network.Connect(this.launcher.remoteIP, int.Parse(this.launcher.remotePort));
		}
		Screen.showCursor = false;
		Screen.lockCursor = true;
	}

	void Init()
	{
		this.SetShip(this.launcher.SelectedShip);
	}

	void Update()
	{
		if (this.shipScript == null)
			return;
		this.shipScript.Accelerate(Input.GetAxis("Vertical"));
		this.shipScript.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * this.Sensibility,
								Input.GetAxis("Mouse X") * Time.deltaTime * this.Sensibility,
								Input.GetAxis("Horizontal") * Time.deltaTime * this.Sensibility);
		this.shipScript.Fire((Input.GetAxis("Jump") != 0 || Input.GetAxis("Fire1") != 0));
	}

	public void SetShip(GameObject prefab)
	{
		GameObject ship = Network.Instantiate(prefab, Vector3.zero, Quaternion.identity, 0) as GameObject;
		this.shipScript = ship.GetComponent<ShipScript>();
		ship.transform.parent = this.transform;
		ship.transform.localPosition = Vector3.zero;
		ship.transform.localRotation = Quaternion.identity;
		this.Eye.transform.parent = ship.transform.Find("Eye").transform;
		this.Eye.transform.localPosition = Vector3.zero;
		this.Eye.transform.localRotation = Quaternion.identity;
	}

	void OnConnectedToServer()
	{
		this.Init();
	}

	void OnServerInitialized()
	{
		for (int i = 0 ; i < number ; ++i)
		{
			Network.Instantiate(this.star, new Vector3(Random.Range(-this.range, this.range), Random.Range(-this.range, this.range), Random.Range(-this.range, this.range)), Quaternion.identity, 0);
		}
		this.Init();
	}
}

