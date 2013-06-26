using	UnityEngine;
using	System.Collections;

public class StartNetwork : MonoBehaviour
{
	public bool			server;
	public int			listenPort = 4141;
	public string		remoteIp;
	public GameObject	player;
	
	private GameObject	playerInst = null;

	public void	Init()
	{
		Application.LoadLevel("game");
	}

	void	Update()
	{
	}

	void	Awake()
	{
		DontDestroyOnLoad(this);
	}

	void	OnConnectedToServer()
	{
		GameObject[]	spawners = GameObject.FindGameObjectsWithTag("Spawn");
		int				rand = Random.Range(0, spawners.Length);
		GameObject		spawn = spawners[rand];

		this.playerInst = Network.Instantiate(this.player, spawn.transform.position, Quaternion.identity, 0) as GameObject;
		Camera.mainCamera.GetComponent<CameraController>().Init(playerInst);
	}

	void	OnServerInitialized()
	{
		GameObject[]	spawners = GameObject.FindGameObjectsWithTag("Spawn");
		int				rand = Random.Range(0, spawners.Length);
		GameObject		spawn = spawners[rand];

		this.playerInst = Network.Instantiate(this.player, spawn.transform.position, Quaternion.identity, 0) as GameObject;
		this.playerInst.networkView.RPC("CheckPacMan", RPCMode.All);
		Camera.mainCamera.GetComponent<CameraController>().Init(playerInst);
	}

	void	OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Application.LoadLevel("Menu");
	}

	void	OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	void	OnLevelWasLoaded()
	{
		if (this.server)
		{
			Network.InitializeSecurity();
			Network.InitializeServer(5, this.listenPort, false);

			foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
				go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			Network.Connect(this.remoteIp, this.listenPort);
		}

		if (Application.loadedLevelName == "mainMenu")
			Destroy(this.gameObject);

		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
	}
}
