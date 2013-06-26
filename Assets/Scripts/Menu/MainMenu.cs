using	UnityEngine;
using	System.Collections;

public class	MainMenu : MonoBehaviour
{
	public GameObject	play = null;
	public GameObject	options = null;
	public GameObject	about = null;
	public GameObject	quit = null;
	public Color		normalColor = Color.white;
	public Color		selectColor = Color.blue;

	private TextMenu	actualText;

	enum TextMenu
	{
		PLAY = 0,
		OPTIONS,
		ABOUT,
		QUIT
	}

	void	Start()
	{
		this.play.GetComponent<TextController>().SetColor(this.selectColor);
		this.actualText = TextMenu.PLAY;
	}
	
	void	Update()
	{
	}

	public void SelectMenu(NavigatorMenu nav)
	{
		if (this.actualText == TextMenu.PLAY)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.play.GetComponent<TextController>().SetColor(this.normalColor);
				this.quit.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.QUIT;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.play.GetComponent<TextController>().SetColor(this.normalColor);
				this.options.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.OPTIONS;
			}
			else if (Input.GetKeyDown(KeyCode.Return))
				nav.MoveToPlay();
		}
		else if (this.actualText == TextMenu.OPTIONS)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.options.GetComponent<TextController>().SetColor(this.normalColor);
				this.play.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.PLAY;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.options.GetComponent<TextController>().SetColor(this.normalColor);
				this.about.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.ABOUT;
			}
		}
		else if (this.actualText == TextMenu.ABOUT)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.about.GetComponent<TextController>().SetColor(this.normalColor);
				this.options.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.OPTIONS;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.about.GetComponent<TextController>().SetColor(this.normalColor);
				this.quit.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.QUIT;
			}
			else if (Input.GetKeyDown(KeyCode.Return))
				nav.MoveToAbout();
		}
		else if (this.actualText == TextMenu.QUIT)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.quit.GetComponent<TextController>().SetColor(this.normalColor);
				this.about.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.ABOUT;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.quit.GetComponent<TextController>().SetColor(this.normalColor);
				this.play.GetComponent<TextController>().SetColor(this.selectColor);
				this.actualText = TextMenu.PLAY;
			}
			else if (Input.GetKeyDown(KeyCode.Return))
				Application.Quit();
		}
	}
}
