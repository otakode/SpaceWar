using	UnityEngine;
using	System.Collections;

public class	NavigatorMenu : MonoBehaviour
{

	public float		speed = 25.0f;		// Definition du temps mis par l'insecte pour parcourir son chemin.
	public string		namePath = null;	// Definition du nom du chemin emprunté par l'insecte.
	public GameObject	cam = null;

	private int			pointValue;			// Valeur de l'insecte (voir FoodGenerator.cs).

	protected bool		direction;			// Direction par laquelle l'insecte arrive (gauche ou droite).
	protected Hashtable	hashTab;			// Tableau contenant les paramètres necessaires à Itween pour suivre le chemin.

	public virtual void	Start()
	{
		//this.direction = false;

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
									   "onComplete", "ChangeDirection",
									   "easetype", "linear",
									   "time", this.speed);
		}
	}
	
	void	Update()
	{
		this.cam.GetComponent<CameraController>().Init(this.gameObject);
		if (Input.GetKey(KeyCode.Space))
		{
			iTween.MoveTo(this.gameObject, this.hashTab);
		}
	
	}
}
