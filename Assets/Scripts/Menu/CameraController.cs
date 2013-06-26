using	UnityEngine;
using	System.Collections;

public class	CameraController : MonoBehaviour
{
	public float		speed = 0.1f;	// Vitesse de déplacement de la caméra.

	private Transform	targetCamera = null;
	
	void	Update()
	{
		//Vector3 to;
		if (this.targetCamera != null)
			this.transform.position = Vector3.Lerp(this.transform.position, this.targetCamera.position, this.speed);
		//to = this.player.position - this.transform.position;
		//this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(to), 0.1f);
	}

	public void Init(GameObject target)
	{
		if (target != null)
			this.targetCamera = target.transform;
	}
}
