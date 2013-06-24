using	UnityEngine;
using	System.Collections;

public class	TextController : MonoBehaviour
{
	//public Color	color = Color.black;

	void Start()
	{
	
	}
	
	void Update()
	{
		//TextMesh	myTextMesh = this.GetComponent<TextMesh>();

		//renderer.material.color = this.color;
		//myTextMesh.text = "name";
	}

	public void SetColor(Color color)
	{
		renderer.material.color = color;
	}
}
