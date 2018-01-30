using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BAllScrit : MonoBehaviour
{
	Vector3 PositionParent;
	private int Speed = 3;
	private bool StopUpdate = true;

	void Update()
	{
		if (StopUpdate)
		{
			return;
		}
		Vector3 DirectionToParent = PositionParent - this.transform.position;
		if (DirectionToParent.magnitude < 0.05f)
		{
			StopUpdate = true;
			return;
		}
		this.transform.Translate(DirectionToParent * Speed * Time.deltaTime);
	}

	public void AlignmentBall()
	{
		PositionParent = this.transform.parent.position;
		PositionParent.y += 1f;
		int CountChild = this.transform.parent.childCount;
		if (CountChild == 1)
		{
			PositionParent.z -= 1f;
		}
		if (CountChild == 2)
		{
			PositionParent.z += 1f;
		}
		if (CountChild == 3)
		{
			PositionParent.x -= 1f;
		}
		if (CountChild == 4)
		{
			PositionParent.x += 1f;
		}
		StopUpdate = false;
		Update();
	}

	public void AlignmentTeleport(Transform Teleport)
	{
		PositionParent = Teleport.position;
		StopUpdate = false;
		Update();
	}

	void OnMouseEnter()
	{
		this.transform.GetComponentInParent<NodesScript>().OnMouseEnter();
	}

	void OnMouseExit()
	{
		this.transform.GetComponentInParent<NodesScript>().OnMouseExit();
	}
	void OnMouseDown()
	{
		this.transform.GetComponentInParent<NodesScript>().OnMouseDown();
	}
}