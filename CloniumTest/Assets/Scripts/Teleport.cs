using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
	public Transform Node;
	public Transform exitTeleport;
	public Color Color { get; set; }

	public void Transport(Transform Ball, Player player)
	{
		StartCoroutine(Transporter(Ball));
	}
	IEnumerator Transporter(Transform Ball)
	{
		yield return new WaitForSeconds(0.75f);
		Ball.position = exitTeleport.position;
		Ball.GetComponent<Ball>().AlignmentBall();
	}
}
