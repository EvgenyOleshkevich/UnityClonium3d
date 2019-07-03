using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public Text CountPlayers;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void UpCount()
	{
		int count = int.Parse(CountPlayers.text.Split()[2]);
		count = Math.Min(12, count + 1);
		CountPlayers.text = "count players " + count.ToString();
	}

	public void DownCount()
	{

		int count = int.Parse(CountPlayers.text.Split()[2]);
		count = Math.Max(2, count - 1);
		CountPlayers.text = "count players " + count.ToString();
	}
}
