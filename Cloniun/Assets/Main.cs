using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public GameObject Floor;
	private int PlayerCountNodes = 1;
	public int IndexPlayers = 0;
	public Text AboutPlayers;
	private int CountPlayers = 2;
	public Transform[] Players;

	void Start()
	{
		CreatNewQueueAndUpdate();
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
			Floor.GetComponent<FloorScript>().IndexPlayers = IndexPlayers;
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
