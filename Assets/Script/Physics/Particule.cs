using UnityEngine;
using System.Collections;

public class Particule : MonoBehaviour
{
	public float		g = 10;
	public float		O = 90;
	public float		v0 = 10;
	public float		duration = 5;
	public float		disparition = 1;

	private Vector3	p0;
	private float	t0;

	void Start()
	{
		p0 = transform.position;
		t0 = Time.time;
	}
	
	void Update()
	{
		float t = Time.time - t0;
		if (t > duration)
		{
			float t2 = t - duration;
			if (t2 >= disparition)
			{
				DestroyImmediate(gameObject);
				return;
			}
			float coef = (disparition - t2) / disparition;
			transform.GetComponent<Light>().color *= coef;
		}
		transform.position = new Vector3(v0*Mathf.Cos(Mathf.Deg2Rad*O)*t + p0.x, -0.5f*g*t*t + v0*Mathf.Sin(Mathf.Deg2Rad*O)*t + p0.y, 0f);
	}
}
