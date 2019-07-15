using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	Vector3 positionParent;
	private int speed = 3;
	private bool StopUpdate = true;

	private void Start()
	{}

	void Update()
	{
		if (StopUpdate)
		{
			return;
		}
		Vector3 DirectionToParent = positionParent - transform.position;
		if (DirectionToParent.magnitude < 0.05f)
		{
			StopUpdate = true;
			return;
		}
		transform.Translate(DirectionToParent * speed * Time.deltaTime);
	}

	public void AlignmentBall()
	{
		positionParent = transform.parent.position;
		positionParent.y += 1f;
		int CountChild = transform.parent.childCount;
		if (CountChild == 1)
		{
			positionParent.z -= 3f;
		}
		if (CountChild == 2)
		{
			positionParent.z += 3f;
		}
		if (CountChild == 3)
		{
			positionParent.x -= 3f;
		}
		if (CountChild == 4)
		{
			positionParent.x += 3f;
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
		var node = transform.GetComponentInParent<Node>();
		if (node)
		{
			node.OnMouseEnter();
		}
	}

	void OnMouseExit()
	{
		var node = transform.GetComponentInParent<Node>();
		if (node)
		{
			node.OnMouseExit();
		}
	}

	void OnMouseDown()
	{
		var node = transform.GetComponentInParent<Node>();
		if (node)
		{
			node.OnMouseDown();
		}
	}
}