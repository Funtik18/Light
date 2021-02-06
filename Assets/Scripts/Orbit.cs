using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
	[Header("Info")]
	public Transform character;

	[Header("Orbit")]
	public Transform pivotMain;
	public Transform pivotAdd;

	public float radiusMain = 1f;
	public float offsetMain = 0.5f;
	[Header("Angle")]
	public float alphaDeg = 0;
	public Vector2 angleClampBtw = new Vector2(180, 0);

	public AnimationCurve curveSpeed;
	public float speed = 1f;
	private int angleDir = 1;

	public Vector2 GetDirection()
	{
		return (pivotMain.position - character.position).normalized;
	}


	public void UpdateAngle()
	{
		if(!(angleClampBtw.x >= alphaDeg && alphaDeg >= angleClampBtw.y))
		{
			angleDir *= -1;
		}

		//speed = curveSpeed.Evaluate(Time.time);

		alphaDeg += speed * angleDir;

		UpdateOrbit();
	}

	public void SetPivotAngle(float bethaDeg)
	{
		alphaDeg = bethaDeg;
		UpdateOrbit();
	}

	private void UpdateOrbit()
	{
		Vector2 direction = transform.right;

		Vector2 pivotPos = Quaternion.Euler(0, 0, alphaDeg) * (direction * radiusMain);
		pivotMain.position = (Vector2)transform.position + pivotPos;

		pivotPos = Quaternion.Euler(0, 0, alphaDeg) * (direction * (radiusMain - offsetMain));
		pivotAdd.position = (Vector2)transform.position + pivotPos;
	}

	//private List<Vector2> positions = new List<Vector2>();

	private int vertexCount = 40;
	private void OnDrawGizmos()
	{
		Vector2 direction = transform.right;

		//angle clamp btw
		Gizmos.color = Color.green;
		for(float i = angleClampBtw.y; i <= angleClampBtw.x; i+=3)
		{
			Vector2 pos = Quaternion.Euler(0, 0, i) * (direction * radiusMain);
			Gizmos.DrawLine(transform.position, (Vector2)transform.position + pos);
		}

		//circle
		Gizmos.color = Color.red;
		float deltaTheta = (2f * Mathf.PI) / vertexCount;
		float theta = 0f;

		Vector2 oldPos = (Vector2)transform.position + (direction * radiusMain);

		for(int i = 0; i < vertexCount + 1; i++)
		{
			Vector2 pos = new Vector2(radiusMain * Mathf.Cos(theta), radiusMain * Mathf.Sin(theta));
			Vector2 newPos = (Vector2)transform.position + pos;

			Gizmos.DrawLine(oldPos, newPos);

			oldPos = newPos;

			theta += deltaTheta;
		}

		UpdateOrbit();

		//line to pivot
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, pivotMain.position);
	}
}
