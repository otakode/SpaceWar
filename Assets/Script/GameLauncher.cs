using UnityEngine;
using System.Collections;

public class GameLauncher : MonoBehaviour
{
	public ShipSelectScript selector;
	//public GameObject		userControl;
	
	public string	remoteIP = "127.0.0.1";
	public string	remotePort = "6666";
	public string	localPort = "6666";

	[HideInInspector]
	public GameObject	SelectedShip = null;
	[HideInInspector]
	public bool			Host = true;

	private bool		gameStarted = false;

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

	void OnGUI()
	{
		if (!this.gameStarted)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("IP: ");
			this.remoteIP = GUILayout.TextField(this.remoteIP);
			GUILayout.Label("Port: ");
			this.remotePort = GUILayout.TextField(this.remotePort);
			if (GUILayout.Button("Join"))
			{
				this.Host = false;
				this.StartGame();
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Port: ");
			this.localPort = GUILayout.TextField(this.localPort);
			if (GUILayout.Button("Host"))
			{
				this.Host = true;
				this.StartGame();
			}
			GUILayout.EndHorizontal();
		}
	}

	void StartGame()
	{
		this.SelectedShip = this.selector.ShipsPrefab[this.selector.SelectedShipType];
		DontDestroyOnLoad(this.gameObject);
		Application.LoadLevelAsync("Game");
		this.gameStarted = true;
	}
}
