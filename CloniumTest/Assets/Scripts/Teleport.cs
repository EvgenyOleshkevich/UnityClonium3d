using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
	public GameObject Node { get; set; }
	public GameObject NodeBack { get; set; }
	public Teleport ExitTeleport;
	//public GameObject Node;
	//public GameObject NodeBack;
	//public GameObject ExitTeleport;
	public Field Field { get; set; }
	//public Color Color { get; set; }

	private void Start()
	{
		GetComponent<Renderer>().material.color = Color.black;
	}

	public void Transport(Transform Ball, Player player)
	{
		StartCoroutine(Transporter(Ball));
	}

	public void OnMouseDown()
	{
		if (Field.Fields.Mode == Mode.spawnPort2 ||
			Field.Fields.Mode == Mode.spawnPort4)
		Field.ChoosePort(this);
	}

	IEnumerator Transporter(Transform Ball)
	{
		yield return new WaitForSeconds(0.75f);
		Ball.SetParent(ExitTeleport.GetComponent<Teleport>().Node.transform);
		Ball.position = ExitTeleport.transform.position;
		Ball.GetComponent<Ball>().AlignmentBall();
	}
}
