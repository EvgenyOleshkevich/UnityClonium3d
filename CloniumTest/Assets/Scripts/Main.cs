using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {
	public GameObject originalPlayer;
	public int IndexPlayers { get; private set; }
	public Text AboutPlayers;
	public bool Stop { get; set; }
	public Transform[] Floors = new Transform[3];
	private int ChangeIndex = 1;

	private void Init()
	{
		IndexPlayers = 0;
		for (int i = 0; i < 2; ++i)
		{
			var player = Instantiate(originalPlayer, this.transform);
			player.GetComponent<Player>().CountNodes = 1;
			player.GetComponent<Player>().Number = i;
			player.GetComponent<Player>().Name = (i + 1).ToString();
		}
		transform.GetChild(0).GetComponent<Player>().Color = new Color(1, 0, 0);
		transform.GetChild(1).GetComponent<Player>().Color = new Color(0, 1, 0);
		//transform.GetChild(2).GetComponent<Player>().Color = new Color(0, 0, 1);
		//transform.GetChild(3).GetComponent<Player>().Color = new Color(0, 1, 1);
		//transform.GetChild(4).GetComponent<Player>().Color = new Color(1, 1, 0);
		//transform.GetChild(5).GetComponent<Player>().Color = new Color(1, 0, 1);
		//transform.GetChild(6).GetComponent<Player>().Color = new Color(0, 0, 0);
		//transform.GetChild(7).GetComponent<Player>().Color = new Color(0.5f, 1, 1);
		//transform.GetChild(8).GetComponent<Player>().Color = new Color(0.5f, 0, 1);
		//transform.GetChild(9).GetComponent<Player>().Color = new Color(0.5f, 1, 0);
		//transform.GetChild(10).GetComponent<Player>().Color = new Color(0.5f, 0.5f, 0.5f);
		//transform.GetChild(11).GetComponent<Player>().Color = new Color(1, 0.5f, 1);
		Floors[0].GetComponent<Floor>().Intialization(0);
		//Floors[1].GetComponent<Floor>().Intialization(1);
		//Floors[2].GetComponent<Floor>().Intialization(2);
		SpawnTeleport();
		Stop = false;
	}

	private void SpawnTeleport()
	{
		Floors[0].GetComponent<Floor>().SpawnTeleport(Floors[1]);
		Floors[0].GetComponent<Floor>().SpawnTeleport(Floors[2]);
		Floors[1].GetComponent<Floor>().SpawnTeleport(Floors[2]);
	}

	void Update()
	{
		if (Stop)
		{
			return;
		}
		if (Input.GetKey(KeyCode.Space) && this.transform.childCount == 0)
		{
			Init();
		}
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
		Debug.Log("UpdatePlayer");
		++IndexPlayers;
		if (IndexPlayers == this.transform.childCount)
		{
			IndexPlayers = 0;
		}
		int playerCountNodes = this.transform.GetChild(IndexPlayers).GetComponent<Player>().CountNodes;
		while (playerCountNodes <= 0)
		{
			++IndexPlayers;
			if (IndexPlayers == this.transform.childCount)
			{
				IndexPlayers = 0;
			}
			playerCountNodes = this.transform.GetChild(IndexPlayers).GetComponent<Player>().CountNodes;
		}
		AboutPlayers.text = "Player "+ this.transform.GetChild(IndexPlayers).GetComponent<Player>().Name;
		AboutPlayers.color = this.transform.GetChild(IndexPlayers).GetComponent<Player>().Color;
		Stop = false;
	}
}
