using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
	public Player Player { get; set; }
	public GameObject[] Neibors { get; set; }
	public Color Color { get; set; }
	public Field Field { get; private set; }

	void Start()
	{
		Color = Color.white;
		Field = transform.parent.GetComponent<Field>();
		GetComponent<Renderer>().material.color = Color;
		//main = transform.parent.GetComponent<Floor>().main;
	}

	public void Initialization(Player player)
	{
		Player = player;
		SetColor(Player.Color);
		Instantiate(Field.Fields.OriginalBall, transform);
		Instantiate(Field.Fields.OriginalBall, transform);
		Instantiate(Field.Fields.OriginalBall, transform);
		Alignment();
	}

	public void AddNeibors(GameObject u, GameObject r, GameObject d, GameObject l)
	{
		Neibors = new GameObject[4];
		Neibors[0] = u;
		Neibors[1] = r;
		Neibors[2] = d;
		Neibors[3] = l;
	}

	public void OnMouseEnter()
	{
		//if (Field.RedactionField.Mode == Mode.spawnPort)
		//{
		//	var t = Input.mousePosition + Field.transform.position;
		//	GameObject.CreatePrimitive(PrimitiveType.Capsule).transform.position = t;
		//	var y = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//	GameObject.CreatePrimitive(PrimitiveType.Capsule).transform.position = y;
		//}
		GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color, 0.8f);
	}

	public void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = Color;
	}

	public void OnMouseDown()
	{
		switch (Field.Fields.Mode)
		{
			case Mode.none:
				return;
			case Mode.cutting:
				Field.CutNode(this);
				return;
			case Mode.spawnPlayer:
				Field.SpawnPlayer(this);
				return;
			case Mode.spawnPort1:
				if (Color != Color.red
					&& Color != Color.blue)
				{
					Field.SpawnPort(this);
				}
				return;
			case Mode.spawnPort3:
				if (Color != Color.red
					&& Color != Color.blue)
				{
					Field.SpawnPort(this);
				}
				return;
			case Mode.playing:
				ClickInGame();
				return;
		}
	}

	public void ClickInGame()
	{
		if (Player == null)
		{
			return;
		}
		if ((Player == Field.Fields.Main.Player)
			&& (!Field.Fields.Main.Stop))
		{
			// creating new ball
			Field.Fields.Main.Stop = true;
			Instantiate(Field.Fields.OriginalBall, transform);
			Alignment();
			if (transform.childCount > 3)
			{
				StartCoroutine(Field.DisclosureMain(this, Player));
			}
			else
			{
				Field.Fields.Main.UpdatePlayer();
			}
		}
	}

	private void Alignment()
	{
		Vector3 PositionBall = transform.position;
		PositionBall.y += 1f;
		if (transform.childCount > 0)
		{
			PositionBall.z -= 3f;
			transform.GetChild(0).position = PositionBall;
			PositionBall.z += 3f;
		}
		if (transform.childCount > 1)
		{
			PositionBall.z += 3f;
			transform.GetChild(1).position = PositionBall;
			PositionBall.z -= 3f;
		}
		if (transform.childCount > 2)
		{
			PositionBall.x -= 3f;
			transform.GetChild(2).position = PositionBall;
			PositionBall.x += 3f;
		}
		if (transform.childCount > 3)
		{
			PositionBall.x += 3f;
			transform.GetChild(3).position = PositionBall;
			PositionBall.x -= 3f;
		}
	}

	public void SetColor(Color color)
	{
		Color = color;
		GetComponent<Renderer>().material.color = color;
	}
}
