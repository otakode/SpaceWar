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
		Application.LoadLevel("Game");
	}

	void	Update()
	{
	}

	void	Awake()
	{
		DontDestroyOnLoad(this);
	}

	void	OnConnectedToServer()	// Appel� par le joueur JOIN apr�s s'�tre connect� au serveur avec succ�s.
	{
		Debug.LogError("in OnConnectedToServer");
		GameObject[]	spawners = GameObject.FindGameObjectsWithTag("Spawn");
		GameObject		camera = GameObject.FindGameObjectWithTag("Camera");
		int				rand = Random.Range(0, spawners.Length);
		GameObject		spawn = spawners[rand];

		this.playerInst = Network.Instantiate(this.player, spawn.transform.position, Quaternion.identity, 0) as GameObject;
		camera.GetComponent<CameraInitialiser>().Init(this.playerInst);
		Debug.LogError("out OnConnectedToServer");
	}

	void	OnServerInitialized()	// Appel� par le joueur HOST apr�s s'�tre connect� au serveur avec succ�s.
	{
		Debug.LogError("OnServerInitialized");
		GameObject[]	spawners = GameObject.FindGameObjectsWithTag("Spawn");
		GameObject		camera = GameObject.FindGameObjectWithTag("Camera");
		int				rand = Random.Range(0, spawners.Length);
		GameObject		spawn = spawners[rand];

		this.playerInst = Network.Instantiate(this.player, spawn.transform.position, Quaternion.identity, 0) as GameObject;
		camera.GetComponent<CameraInitialiser>().Init(this.playerInst);
		//this.playerInst.networkView.RPC("CheckPacMan", RPCMode.All);
	}

	void	OnDisconnectedFromServer(NetworkDisconnection info)	// Appel� par le joueur JOIN lorsque (la connexion a �t� perdue/il s'est d�connect� du serveur).
	{
		Application.LoadLevel("Menu");
	}

	void	OnPlayerDisconnected(NetworkPlayer player) // Appel� par le joueur HOST lorsque qu'un joueur s'est d�connect� du serveur).
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
			Debug.LogError("OnLevelWasLoaded JOIN");
		}

		if (Application.loadedLevelName == "Menu")
			Destroy(this.gameObject);

		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
	}
}
