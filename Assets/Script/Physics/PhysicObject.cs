using UnityEngine;
using System.Collections;

public class PhysicObject : MonoBehaviour
{
	public float	Mass = 1;
	public Vector3	Speed = Vector3.zero;
	public float	damping = 0.95f;
	public float	MaxSpeed = 300;

	private Vector3	Force = new Vector3(0.0f, 0.0f, 0.0f);

	void Start()
	{
		PhysicEngine.Instance().RegisterObject(this);
	}

	void OnDestroy()
	{
		PhysicEngine.Instance().UnRegisterObject(this);
	}

	public void ComputePhysics(float t)
	{
		Vector3 a = 1.0f/Mass * Force;
		Vector3 oldSpeed = Speed;
		Speed = Vector3.ClampMagnitude(Speed + a * t * damping, MaxSpeed);
		transform.position += (Speed * 0.5f + oldSpeed * 0.5f) * t + 0.5f * a * t * t;
		ClearForce();
	}

	public void AddForce(Vector3 f)
	{
		Force += f;
	}

	public void ClearForce()
	{
		Force.Set(0.0f, 0.0f, 0.0f);
	}
	
	public void Impulse(Vector3 force)
	{
		
	}
}
