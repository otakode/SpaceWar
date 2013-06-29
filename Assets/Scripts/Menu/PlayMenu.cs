using	UnityEngine;
using	System.Collections;

public class	PlayMenu : MonoBehaviour
{
	public GameObject	host = null;
	public GameObject	join = null;
	public GameObject[]	rooms = null;
	
	public Color		normalColor = Color.white;
	public Color		roomSelectColor = Color.red;
	public Color		selectColor = Color.blue;

	public GameObject	networkMaster;
	public TextMesh		ip;
	public TextMesh		port;

	private GameObject		instantiatedMaster;
	private StartNetwork	scriptStartNet;
	private string			serverIp = "127.0.0.1";
	private string			strIp = "";
	private string			strPort = "";
	private int				serverPort = 4242;

	private TextMenu	actualText;
	private bool		scrollRoom;
	private int			actualRoom;

	private bool		ipReady;

	enum	TextMenu
	{
		HOST = 0,
		JOIN
	}

	enum TextRoom
	{
		NO_ROOM = 0,
		ROOM
	}

	void	Start()
	{
		this.host.GetComponent<TextController>().SetColor(this.selectColor);
		this.actualText = TextMenu.HOST;
		this.actualRoom = 0;
		this.ipReady = false;
	}
	
	void	Update()
	{
	
	}

	private void	JoinServer()
	{
		Debug.LogError("inJoinSever");
		this.instantiatedMaster = Instantiate(this.networkMaster, Vector3.zero, Quaternion.identity) as GameObject;
		this.scriptStartNet = this.instantiatedMaster.GetComponent<StartNetwork>();
		this.scriptStartNet.server = false;
		this.scriptStartNet.remoteIp = this.strIp;
		this.scriptStartNet.listenPort = int.Parse(this.strPort);
		this.scriptStartNet.Init();
	}

	private void	HostServer()
	{
		this.instantiatedMaster = Instantiate(this.networkMaster, Vector3.zero, Quaternion.identity) as GameObject;
		this.scriptStartNet = this.instantiatedMaster.GetComponent<StartNetwork>();
		this.scriptStartNet.server = true;
		this.scriptStartNet.remoteIp = this.serverIp;
		this.scriptStartNet.listenPort = this.serverPort;
		this.scriptStartNet.Init();
	}

	public void	SelectMenu(NavigatorMenu nav)
	{
		//print("this.actualText = " + this.actualText);
		if (this.actualText == TextMenu.HOST)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.host.GetComponent<TextController>().SetColor(this.normalColor);
				this.join.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.JOIN;
			}
			else if (Input.GetKeyDown(KeyCode.Return))
			{
				this.HostServer();
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
				nav.MoveToMain();
		}
		else if (this.actualText == TextMenu.JOIN)
		{
			if (this.scrollRoom == false)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
				{
					this.host.GetComponent<TextController>().SetColor(this.selectColor);
					this.join.GetComponent<TextController>().SetColor(this.normalColor);
					this.actualText = TextMenu.HOST;
				}
				else if (Input.GetKeyDown(KeyCode.Return))
				{
					this.actualRoom = 0;
					this.rooms[this.actualRoom].GetComponent<TextController>().SetColor(this.roomSelectColor);
					this.scrollRoom = true;
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.host.GetComponent<TextController>().SetColor(this.selectColor);
					this.join.GetComponent<TextController>().SetColor(this.normalColor);
					this.actualText = TextMenu.HOST;
					nav.MoveToMain();
				}
			}
			else
			{
				/*if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					this.rooms[this.actualRoom].GetComponent<TextController>().SetColor(this.normalColor);
					if (this.actualRoom == 0)
						this.actualRoom = this.rooms.Length - 1;
					else
						--this.actualRoom;
					this.rooms[this.actualRoom].GetComponent<TextController>().SetColor(this.roomSelectColor);
					//this.join.GetComponent<TextController>().SetColor(this.normalColor);
					//this.actualText = TextMenu.HOST;
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					this.rooms[this.actualRoom].GetComponent<TextController>().SetColor(this.normalColor);
					if (this.actualRoom == this.rooms.Length - 1)
						this.actualRoom = 0;
					else
						++this.actualRoom;
					this.rooms[this.actualRoom].GetComponent<TextController>().SetColor(this.roomSelectColor);
					//this.scrollRoom = false;
				}*/
				if (this.ipReady == true)
				{
					this.checkAreaKeyBoardPORT();
					if (Input.GetKeyDown(KeyCode.Escape))
					{
						this.strPort = "Tape Port";
						this.ipReady = false;
					}
					else if (Input.GetKeyDown(KeyCode.Return) && this.port.text != "")
						this.JoinServer();
					this.port.text = this.strPort;
				}
				else if (Input.GetKeyDown(KeyCode.Return) && this.ip.text != "")
				{
					this.ipReady = true;
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.rooms[this.actualRoom].GetComponent<TextController>().SetColor(this.normalColor);
					this.strIp = "Tape IP";
					this.scrollRoom = false;
				}
				if (this.ipReady != true)
					this.checkAreaKeyBoardIP();
				this.ip.text = this.strIp;
			}
		}
	}

	private void	checkAreaKeyBoardIP()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
			this.strIp = this.strIp + '0';
		else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
			this.strIp = this.strIp + '1';
		else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
			this.strIp = this.strIp + '2';
		else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
			this.strIp = this.strIp + '3';
		else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
			this.strIp = this.strIp + '4';
		else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
			this.strIp = this.strIp + '5';
		else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
			this.strIp = this.strIp + '6';
		else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
			this.strIp = this.strIp + '7';
		else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
			this.strIp = this.strIp + '8';
		else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
			this.strIp = this.strIp + '9';
		else if (Input.GetKeyDown(KeyCode.Period))
			this.strIp = this.strIp + '.';
		else if (Input.GetKeyDown(KeyCode.Backspace))
			this.strIp = this.strIp.Remove(this.strIp.Length - 1);
	}

	private void checkAreaKeyBoardPORT()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
			this.strPort = this.strPort + '0';
		else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
			this.strPort = this.strPort + '1';
		else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
			this.strPort = this.strPort + '2';
		else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
			this.strPort = this.strPort + '3';
		else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
			this.strPort = this.strPort + '4';
		else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
			this.strPort = this.strPort + '5';
		else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
			this.strPort = this.strPort + '6';
		else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
			this.strPort = this.strPort + '7';
		else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
			this.strPort = this.strPort + '8';
		else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
			this.strPort = this.strPort + '9';
		else if (Input.GetKeyDown(KeyCode.Backspace))
			this.strPort = this.strPort.Remove(this.strPort.Length - 1);
	}
}
