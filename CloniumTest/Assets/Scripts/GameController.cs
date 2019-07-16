using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController {
	private Queue<Player> Players { get; set; }
	public Player Player { get; private set; }
	public Text AboutPlayers;
	public bool Stop { get; set; }
	private Field[] Fields { get; set; }

	public GameController(Field[] fields, Text text)
	{
		Players = new Queue<Player>();
		AboutPlayers = text;
		Fields = fields;
		var list = new List<Node>();
		foreach (var Field in Fields)
		{
			list.InsertRange(0, Field.GetNodesForSpawn());
		}
		var nodes = list.ToArray();
		for (int i = 0; i < nodes.Length; i++)
		{
			Player = new Player(i);
			nodes[i].Initialization(Player);
			Players.Enqueue(Player);
		}
		Player = Players.Dequeue();
		Stop = false;
		AboutPlayers.text = Player.Name;
		AboutPlayers.color = Player.Color;
	}

	public void UpdatePlayer()
	{
		//Debug.Log("UpdatePlayer");
		Players.Enqueue(Player);
		do
		{
			Player = Players.Dequeue();
		} while (Player.CountNodes <= 0);
		AboutPlayers.text = Player.Name;
		AboutPlayers.color = Player.Color;
		Stop = false;
	}
}
