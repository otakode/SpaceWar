using	UnityEngine;
using	System.Collections;

public class	CameraInitialiser : MonoBehaviour
{

	void	Start()
	{
	}
	
	void	Update()
	{
	}

	public void Init(GameObject target)
	{
		this.transform.parent = target.transform;
		this.transform.position = target.transform.position;
	}
}
