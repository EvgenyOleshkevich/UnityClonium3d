using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameData : MonoBehaviour {

	//public int CountPlayer { get; set; }
	//public int StepTime { get; set; }
	//public int CountField { get; set; }
	//public bool SpawnPlayer { get; set; }
	//public bool SpawnPort { get; set; }
	public int CountPlayer;
	public int StepTime;
	public int CountField;
	public bool SpawnPlayer;
	public bool SpawnPort;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
