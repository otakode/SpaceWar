using	UnityEngine;
using	System.Collections;

public class	AboutMenu : MonoBehaviour
{
	void	Start()
	{
	}

	void	Update()
	{
	}

	public void	SelectMenu(NavigatorMenu nav)
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
			nav.MoveToMain();			
	}
}
