using	UnityEngine;
using	System.Collections;

public class	NavigatorMenu : MonoBehaviour
{

	public float		speed = 25.0f;		// Definition du temps mis par l'insecte pour parcourir son chemin.
	public string		namePath = null;	// Definition du nom du chemin emprunté par l'insecte.
	public GameObject	cam = null;
	public GameObject	mainMenu = null;
	public GameObject	aboutMenu = null;

	private int			pointValue;			// Valeur de l'insecte (voir FoodGenerator.cs).
	private Menu		actualMenu;

	protected bool		direction;			// Direction par laquelle l'insecte arrive (gauche ou droite).
	protected Hashtable	hashTab;			// Tableau contenant les paramètres necessaires à Itween pour suivre le chemin.

	public enum Menu
	{
		MAIN_MENU = 0,
		PLAY_MENU = 1,
		OPTIONS_MENU = 2,
		ABOUT_MENU = 3
	}

	public virtual void	Start()
	{
		//this.direction = false;
		this.actualMenu = Menu.MAIN_MENU;

		if (this.namePath != "null")
		{
			Vector3[] path = iTweenPath.GetPath(this.namePath);

			if (this.direction == true)
			{
				Vector3[] pathTemp = path;
				int i = 0;

				while (i < pathTemp.Length)
				{
					path[i] = pathTemp[pathTemp.Length - 1 - i];
					++i;
				}
				this.gameObject.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
			}

			transform.position = path[0];
			//transform.parent = GameObject.Find("Game").transform;
			this.hashTab = iTween.Hash("path", path,
									   "orienttopath", true,
									   //"looptype", "pingPong",
									   //"onComplete", "ChangeDirection",
									   "easetype", "linear",
									   "time", this.speed);
		}
		this.cam.GetComponent<CameraController>().Init(this.gameObject);
	}
	
	void	Update()
	{
		if (this.actualMenu == Menu.MAIN_MENU)
			this.mainMenu.GetComponent<MainMenu>().SelectMenu(this);
		/*else if (this.actualMenu == Menu.PLAY_MENU)
			this.mainMenu.GetComponent<MainMenu>().SelectMenu();
		else if (this.actualMenu == Menu.OPTIONS_MENU)
			this.mainMenu.GetComponent<MainMenu>().SelectMenu();
		*/else if (this.actualMenu == Menu.ABOUT_MENU)
			this.aboutMenu.GetComponent<AboutMenu>().SelectMenu(this);
	}

	public void	MoveToAbout()
	{
		this.actualMenu = Menu.ABOUT_MENU;
		iTween.MoveTo(this.gameObject, this.hashTab);
	}

	public void MoveToMain()
	{
		this.actualMenu = Menu.MAIN_MENU;

		Vector3[] path = iTweenPath.GetPath(this.namePath);

		this.transform.position = new Vector3(path[0].x, path[0].y, path[0].z);

		/*Vector3[] pathTemp = path;
		int i;

		i = 0;
		while (i < pathTemp.Length)
		{
			path[i] = pathTemp[pathTemp.Length - 1 - i];
			++i;
		}
		//this.gameObject.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
		/*this.hashTab = iTween.Hash("path", path,
								   "orienttopath", true,
			//"looptype", "pingPong",
			//"onComplete", "ChangeDirection",
								   "easetype", "linear",
								   "time", this.speed);

		iTween.MoveTo(this.gameObject, this.hashTab);*/
	}
}
