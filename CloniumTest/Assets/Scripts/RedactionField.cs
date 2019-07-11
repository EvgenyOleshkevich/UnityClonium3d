using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedactionField : MonoBehaviour {
	public StartGameData data;

	// Use this for initialization
	void Start () {
		//data = GameObject.FindGameObjectWithTag("startGameData").GetComponent<StartGameData>();
		data = new StartGameData { StepTime = 40, CountField = 3, CountPlayer = 6, SpawnPlayer = false, SpawnPort = false};

		var n = new GameObject();
		n.AddComponent<Field>();
		n.GetComponent<Field>().Init(1, 10, TypeField.square);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SizeFieldSlider()
	{

	}
}
