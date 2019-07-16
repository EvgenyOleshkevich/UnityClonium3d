using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Player {
	public string Name { get; set; }
	public int CountNodes { get; set; }
	public int CountBalls { get; set; }
	public int Index { get; set; }
	public Color Color;

	public Player(int index)
	{
		Name = "plyer " + index.ToString();
		Index = index;
		CountBalls = 3;
		CountNodes = 1;
		var rand = new Random();
		//var rand = new Random((int)(Index * DateTime.Now.ToBinary()));
		Color = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1f);
	}
}
