using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public GameObject Floor;
	private int PlayerCountNodes = 1;
	public int IndexPlayers = 0;
	public Text AboutPlayers;
	public Transform[] Players;
	public bool Stop = true;
	public Transform[] Floors = new Transform[3];
	private int ChangeIndex = 1;
	void Start()
	{
		CreatNewQueueAndUpdate();
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			ChangeIndex = 0;
		}
		if (Input.GetKey(KeyCode.Alpha2))
		{
			ChangeIndex = 1;
		}
		if (Input.GetKey(KeyCode.Alpha3))
		{
			ChangeIndex = 2;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 FloorVector = Floors[ChangeIndex].position;
			--FloorVector.y;
			Floors[ChangeIndex].position = FloorVector;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 FloorVector = Floors[ChangeIndex].position;
			++FloorVector.y;
			Floors[ChangeIndex].position = FloorVector;
		}
		if (Input.GetKey(KeyCode.W))
		{
			Vector3 FloorVector = Floors[ChangeIndex].position;
			++FloorVector.z;
			Floors[ChangeIndex].position = FloorVector;
		}
		if (Input.GetKey(KeyCode.S))
		{
			Vector3 FloorVector = Floors[ChangeIndex].position;
			--FloorVector.z;
			Floors[ChangeIndex].position = FloorVector;
		}
		if (Input.GetKey(KeyCode.A))
		{
			Vector3 FloorVector = Floors[ChangeIndex].position;
			--FloorVector.x;
			Floors[ChangeIndex].position = FloorVector;
		}
		if (Input.GetKey(KeyCode.D))
		{
			Vector3 FloorVector = Floors[ChangeIndex].position;
			++FloorVector.x;
			Floors[ChangeIndex].position = FloorVector;
		}


	}
	public void UpdatePlayer()
	{
		IndexPlayers = (IndexPlayers + 1);
		if (IndexPlayers >= this.transform.childCount)
		{
			IndexPlayers -= this.transform.childCount;
		}
		PlayerCountNodes = this.transform.GetChild(IndexPlayers).GetComponent<PlayerScript>().CountNodes;
		while (PlayerCountNodes <= 0)
		{
			IndexPlayers = (IndexPlayers + 1);
			if (IndexPlayers >= this.transform.childCount)
			{
				IndexPlayers -= this.transform.childCount;
			}
			PlayerCountNodes = this.transform.GetChild(IndexPlayers).GetComponent<PlayerScript>().CountNodes;
		}
		AboutPlayers.text = "Player "+ (IndexPlayers + 1).ToString();
		Stop = true;
	}

	void CreatNewQueueAndUpdate()
	{
		Players = new Transform[this.transform.childCount];
		for (int i = 0; i < this.transform.childCount; ++i)
		{
			Players[i] = this.transform.GetChild(i);
		}
	}
}
