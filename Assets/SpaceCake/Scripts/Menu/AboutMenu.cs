using	UnityEngine;
using	System.Collections;

public class	AboutMenu : MonoBehaviour
{
	public GameObject	name1 = null;
	public GameObject	name2 = null;
	public GameObject	name3 = null;
	public GameObject	name4 = null;
	public Color		normalColor = Color.white;
	public Color		noColor = Color.blue;

	void	Start()
	{
		this.name1.GetComponent<TextController>().SetColor(this.noColor);
		this.name2.GetComponent<TextController>().SetColor(this.noColor);
		this.name3.GetComponent<TextController>().SetColor(this.noColor);
		this.name4.GetComponent<TextController>().SetColor(this.noColor);
	}

	void	Update()
	{
	}

	public void	SelectMenu(NavigatorMenu nav)
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
		{
			this.name1.GetComponent<TextController>().SetColor(this.noColor);
			this.name2.GetComponent<TextController>().SetColor(this.noColor);
			this.name3.GetComponent<TextController>().SetColor(this.noColor);
			this.name4.GetComponent<TextController>().SetColor(this.noColor);
			nav.MoveToMain();
		}
		else
		{
			this.name1.GetComponent<TextController>().SetColor(this.normalColor);
			this.name2.GetComponent<TextController>().SetColor(this.normalColor);
			this.name3.GetComponent<TextController>().SetColor(this.normalColor);
			this.name4.GetComponent<TextController>().SetColor(this.normalColor);
		}
	}
}
