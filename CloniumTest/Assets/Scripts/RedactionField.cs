using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedactionField : MonoBehaviour {
	public StartGameData data;
	public Field Field { get; private set; }
	public Slider sliderSize;
	public Text textSize;

	// Use this for initialization
	void Start () {
		//data = GameObject.FindGameObjectWithTag("startGameData").GetComponent<StartGameData>();
		data = new StartGameData { StepTime = 40, CountField = 3, CountPlayer = 6, SpawnPlayer = false, SpawnPort = false};

		var field = new GameObject();
		field.AddComponent<Field>();
		Field = field.GetComponent<Field>();
		Field.Init(1, 10, TypeField.square);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SizeFieldSlider()
	{
		int size = (int)sliderSize.value;
		//sliderSize.enabled = false; // for dont trigger on arrow
		//sliderSize.enabled = true; // for dont trigger on arrow
		sliderSize.interactable = false; // for dont trigger on arrow
		sliderSize.interactable = true; // for dont trigger on arrow
		Field.DestroyField();
		Field.Init(size);
		textSize.text = "size field " + size.ToString();
	}
}
