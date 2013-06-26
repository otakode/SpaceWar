using	UnityEngine;
using	System.Collections;

public class	NavigatorMenu : MonoBehaviour
{
	public float		speed = 25.0f;		// Definition du temps mis par l'insecte pour parcourir son chemin.
	public string		namePath = null;	// Definition du nom du chemin emprunté par l'insecte.
	public GameObject	cam = null;
	public GameObject	mainMenu = null;
	public GameObject	aboutMenu = null;
	public GameObject	playMenu = null;


	private int			pointValue;			// Valeur de l'insecte (voir FoodGenerator.cs).
	private Menu		actualMenu;

	protected Hashtable	hashTab;			// Tableau contenant les paramètres necessaires à Itween pour suivre le chemin.

	public enum	Menu
	{
		MAIN_MENU = 0,
		PLAY_MENU = 1,
		OPTIONS_MENU = 2,
		ABOUT_MENU = 3
	}

	public virtual void	Start()
	{
		this.actualMenu = Menu.MAIN_MENU;
		SetPath(this.actualMenu);
		iTween.MoveTo(this.gameObject, this.hashTab);
		this.cam.GetComponent<CameraController>().Init(this.gameObject);
	}
	
	void	Update()
	{
		if (this.actualMenu == Menu.MAIN_MENU)
			this.mainMenu.GetComponent<MainMenu>().SelectMenu(this);
		else if (this.actualMenu == Menu.PLAY_MENU)
			this.playMenu.GetComponent<PlayMenu>().SelectMenu(this);
		/*else if (this.actualMenu == Menu.OPTIONS_MENU)
			this.optionsMenu.GetComponent<OtionsMenu>().SelectMenu();*/
		else if (this.actualMenu == Menu.ABOUT_MENU)
			this.aboutMenu.GetComponent<AboutMenu>().SelectMenu(this);
	}

	private void SetPath(Menu menu)
	{
		if (this.namePath != "null")
		{
			Vector3[]	path;

			if (menu == Menu.ABOUT_MENU)
				path = iTweenPath.GetPath("Path_About");
			/*else if (menu == Menu.OPTIONS_MENU)
				path = iTweenPath.GetPath("Path_Options");*/
			else if (menu == Menu.PLAY_MENU)
				path = iTweenPath.GetPath("Path_Play");
			else
				path = iTweenPath.GetPath("Path_Main");

			transform.position = path[0];
			this.hashTab = iTween.Hash("path", path,
									   "orienttopath", true,
									   "easetype", "linear",
									   "time", this.speed);
		}
	}

	public void	MoveToPlay()
	{
		this.actualMenu = Menu.PLAY_MENU;
		SetPath(this.actualMenu);
		iTween.MoveTo(this.gameObject, this.hashTab);
	}

	public void MoveToAbout()
	{
		this.actualMenu = Menu.ABOUT_MENU;
		SetPath(this.actualMenu);
		iTween.MoveTo(this.gameObject, this.hashTab);
	}

	public void MoveToMain()
	{
		//Vector3[]	path = iTweenPath.GetPath(this.namePath);

		this.actualMenu = Menu.MAIN_MENU;
		SetPath(this.actualMenu);
		iTween.MoveTo(this.gameObject, this.hashTab);
		//this.transform.position = new Vector3(path[0].x, path[0].y, path[0].z);
	}
}
