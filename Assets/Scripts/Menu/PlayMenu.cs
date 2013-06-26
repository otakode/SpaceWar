using	UnityEngine;
using	System.Collections;

public class	PlayMenu : MonoBehaviour
{
	public GameObject	host = null;
	public GameObject	join = null;
	public GameObject[]	rooms = null;
	/*public GameObject	about = null;
	public GameObject	quit = null;*/
	public Color		normalColor = Color.white;
	public Color		roomSelectColor = Color.red;
	public Color		selectColor = Color.blue;

	private TextMenu	actualText;
	private bool		scrollRoom;
	private int			actualRoom;

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
	}
	
	void	Update()
	{
	
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
				if (Input.GetKeyDown(KeyCode.UpArrow))
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
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.rooms[this.actualRoom].GetComponent<TextController>().SetColor(this.normalColor);
					this.scrollRoom = false;
				}
			}
		}
	}
}
