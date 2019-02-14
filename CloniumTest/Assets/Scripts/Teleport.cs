using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
	public Transform FrontNode;
	public Transform BackNode;
	public Transform ExitTeleport;
	public Color color;

	public void Transport(GameObject Ball, Player player)
	{
		StartCoroutine(Transporter(Ball));
	}
	IEnumerator Transporter(GameObject Ball)
	{
		yield return new WaitForSeconds(0.75f);
		Ball.transform.position = ExitTeleport.position;
		Ball.GetComponent<Ball>().AlignmentBall();
	}
}
