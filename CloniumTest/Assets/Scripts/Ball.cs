using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	Vector3 positionParent;
	private int speed = 3;
	private bool StopUpdate = true;

	void Update()
	{
		if (StopUpdate)
		{
			return;
		}
		Vector3 DirectionToParent = positionParent - this.transform.position;
		if (DirectionToParent.magnitude < 0.05f)
		{
			StopUpdate = true;
			return;
		}
		this.transform.Translate(DirectionToParent * speed * Time.deltaTime);
	}

	public void AlignmentBall()
	{
		positionParent = this.transform.parent.position;
		positionParent.y += 1f;
		int CountChild = this.transform.parent.childCount;
		if (CountChild == 1)
		{
			positionParent.z -= 1f;
		}
		if (CountChild == 2)
		{
			positionParent.z += 1f;
		}
		if (CountChild == 3)
		{
			positionParent.x -= 1f;
		}
		if (CountChild == 4)
		{
			positionParent.x += 1f;
		}
		StopUpdate = false;
		Update();
	}

	public void AlignmentBall(Vector3 postion)
	{
		positionParent = postion;
		StopUpdate = false;
		Update();
	}

	void OnMouseEnter()
	{
		this.transform.GetComponentInParent<Node>().OnMouseEnter();
	}

	void OnMouseExit()
	{
		this.transform.GetComponentInParent<Node>().OnMouseExit();
	}
	void OnMouseDown()
	{
		this.transform.GetComponentInParent<Node>().OnMouseDown();
	}
}