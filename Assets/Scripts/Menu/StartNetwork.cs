using	UnityEngine;
using	System.Collections;

public class StartNetwork : MonoBehaviour
{
	public bool			server;
	public int			listenPort = 4141;
	public string		remoteIp;
	public GameObject	player;
	public GameObject	meteor;
	public Texture	lockerPNG;

	private GameObject	playerInst = null;
	private GameObject	meteorInst = null;

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

	void	OnConnectedToServer()	// Appelé par le joueur JOIN après s'être connecté au serveur avec succès.
	{
		Debug.Log("in OnConnectedToServer");

		this.InitPlayer();
		Debug.Log("out OnConnectedToServer");
	}

	private void	InitPlayer()
	{
		Debug.Log("init player");
		GameObject[]	spawners = GameObject.FindGameObjectsWithTag("Spawn");
		GameObject		camera = GameObject.FindGameObjectWithTag("Camera");
		int				rand = Random.Range(0, spawners.Length);
		GameObject		spawn = spawners[rand];

		this.playerInst = Network.Instantiate(this.player, spawn.transform.position, Quaternion.identity, 0) as GameObject;

		this.playerInst.transform.GetComponent<SpaceChipsController>().enabled = true;
		this.playerInst.transform.GetComponent<VoiceHandler>().enabled = true;
		this.playerInst.transform.GetComponent<Inventory>().enabled = true;
		this.playerInst.transform.GetComponent<Inventory>().locker = lockerPNG;
		this.playerInst.transform.FindChild("Camera").gameObject.SetActive(true);
		Debug.Log("2");
		this.playerInst.transform.FindChild("Model3D").FindChild("Cockpit").gameObject.SetActive(true);
		this.playerInst.transform.FindChild("Model3D").FindChild("External").gameObject.SetActive(false);
		Debug.Log("3");
		GameObject.Find("SpaceCamera").GetComponent<SU_SpaceSceneCamera>().Init(this.playerInst.transform.FindChild("Camera").transform);
		Debug.Log(" out init player");
	}

	void	OnServerInitialized()	// Appelé par le joueur HOST après s'être connecté au serveur avec succès.
	{
		Debug.Log(" in OnServerInitialized");

		this.InitPlayer();
		this.meteorInst = Instantiate(this.meteor) as GameObject; 

		//this.playerInst = Network.Instantiate(this.player, spawn.transform.position, Quaternion.identity, 0) as GameObject;
		//this.playerInst.networkView.RPC("CheckPacMan", RPCMode.All);

		Debug.Log(" out OnServerInitialized");
	}

	void	OnDisconnectedFromServer(NetworkDisconnection info)	// Appelé par le joueur JOIN lorsque (la connexion a été perdue/il s'est déconnecté du serveur).
	{
		Application.LoadLevel("Menu");
	}

	void	OnPlayerDisconnected(NetworkPlayer player) // Appelé par le joueur HOST lorsque qu'un joueur s'est déconnecté du serveur).
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
			Debug.Log("OnLevelWasLoaded JOIN");
		}

		if (Application.loadedLevelName == "Menu")
			Destroy(this.gameObject);
		Debug.Log("destroy Menu");

		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
		Debug.Log("send message ok");
	}
}
