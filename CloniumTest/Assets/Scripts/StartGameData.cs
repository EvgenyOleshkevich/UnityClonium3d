using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameData : MonoBehaviour {

	public int CountPlayer { get; set; }
	public int StepTime { get; set; }
	public int CountField { get; set; }
	public bool SpawnPlayer { get; set; }
	public bool SpawnPort { get; set; }
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
