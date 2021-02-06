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

	private Coroutine lifeCoroutine = null;
	public bool IsLifeProcess => lifeCoroutine != null;


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
}
