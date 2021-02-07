using System.Collections;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    private Orbit orbit;
    public Orbit Orbit
	{
		get
		{
			if(orbit == null)
			{
				orbit = GetComponent<Orbit>();
			}
			return orbit;
		}
	}

	private Rigidbody2D rigidbody;
	public Rigidbody2D Rigidbody
	{
		get
		{
			if(rigidbody == null)
			{
				rigidbody = GetComponent<Rigidbody2D>();
			}
			return rigidbody;
		}
	}


	private Coroutine lifeCoroutine = null;
	public bool IsLifeProcess => lifeCoroutine != null;

	public Transform target;
	public float h = 25;
	public float gravity = -18;


	public bool isAlive = true;


	private void Awake()
	{
		StartLife();
	}

	private void StartLife()
	{
		if(!IsLifeProcess)
		{
			lifeCoroutine = StartCoroutine(LifeCycle());
		}
	}
	private IEnumerator LifeCycle()
	{
		while(isAlive)
		{
			Orbit.UpdateAngle();

			if(Input.GetKeyDown(KeyCode.Space))
			{
				Launch();
			}

			DrawPath();

			yield return null;
		}

		StopLife();
	}
	private void StopLife()//death
	{
		if(IsLifeProcess)
		{
			StopCoroutine(lifeCoroutine);
			lifeCoroutine = null;
		}
	}


	private void Launch()
	{
		Physics.gravity = gravity * Vector3.up;
		Rigidbody.gravityScale = 1;
		Rigidbody.velocity = CalculateLaunchVelocity().initialVelocity;
	}

	private LaunchData CalculateLaunchVelocity()
	{
		float diffY = target.position.y - transform.position.y;
		Vector3 diffXZ = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);

		float time = (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (diffY - h) / gravity));

		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
		Vector3 velocityXZ = diffXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
	}
	private void DrawPath()
	{
		LaunchData launchData = CalculateLaunchVelocity();
		Vector3 previousDrawPoint = transform.position;

		int resolution = 30;
		for(int i = 1; i <= resolution; i++)
		{
			float simulationTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = transform.position + displacement;
			Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);

			previousDrawPoint = drawPoint;
		}
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawSphere(target.position, 0.2f);
	}


	private struct LaunchData
	{
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;
		public LaunchData(Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}
	}
}
