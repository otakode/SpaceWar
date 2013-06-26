using	UnityEngine;
using	System.Collections;

public class	PlayMenu : MonoBehaviour
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
