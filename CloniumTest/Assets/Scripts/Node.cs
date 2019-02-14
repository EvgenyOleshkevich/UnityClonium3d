using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
	public GameObject originalBall;
	public GameObject Main;
	public Player Player;
	public Transform[] Neibors { get; set; }
	public Color Color;

	void Start()
	{
		Color = Color.white;
		this.GetComponent<Renderer>().material.color = Color;
	}

	public void Initialization(Player _player)
	{
		Player = _player;
		Color = Player.Color;
		this.GetComponent<Renderer>().material.color = Color;
		Instantiate(originalBall, this.transform);
		Instantiate(originalBall, this.transform);
		Instantiate(originalBall, this.transform);
		Alignment();
	}

	public void AddNeibors(Transform u, Transform r, Transform d, Transform l)
	{
		Neibors = new Transform[4];
		Neibors[0] = u;
		Neibors[1] = r;
		Neibors[2] = d;
		Neibors[3] = l;
	}

	public void OnMouseEnter()
	{
		this.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color, 0.8f);
	}

	public void OnMouseExit()
	{
		this.GetComponent<Renderer>().material.color = Color;
	}

	public void OnMouseDown()
	{
		int numberPlayer = Player.Number;
		if ((numberPlayer == Main.GetComponent<Main>().IndexPlayers) 
			&& (!Main.GetComponent<Main>().Stop))
		{
			// creating new ball
			Main.GetComponent<Main>().Stop = true;
			Instantiate(originalBall, this.transform);
			Alignment();
			if (this.transform.childCount > 3)
			{
				StartCoroutine(this.transform.parent.GetComponent<Floor>().DisclosureMain(Player));
			}
			else
			{
				Main.GetComponent<Main>().UpdatePlayer();
			}
		}
	}

	void Alignment()
	{
		Vector3 PositionBall = this.transform.position;
		PositionBall.y += 1f;
		if (this.transform.childCount > 0)
		{
			PositionBall.z -= 1f;
			this.transform.GetChild(0).position = PositionBall;
			PositionBall.z += 1f;
		}
		if (this.transform.childCount > 1)
		{
			PositionBall.z += 1f;
			this.transform.GetChild(1).position = PositionBall;
			PositionBall.z -= 1f;
		}
		if (this.transform.childCount > 2)
		{
			PositionBall.x -= 1f;
			this.transform.GetChild(2).position = PositionBall;
			PositionBall.x += 1f;
		}
		if (this.transform.childCount > 3)
		{
			PositionBall.x += 1f;
			this.transform.GetChild(3).position = PositionBall;
			PositionBall.x -= 1f;
		}
	}
}
