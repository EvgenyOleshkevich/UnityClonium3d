using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RedactionField : MonoBehaviour {
	public StartGameData data;
	public Field[] Fields { get; private set; }
	public Field CurrentField { get; private set; }
	public Slider sliderSize;
	public Text textSize;
	private CameraScr Camera { get; set; }

	// Use this for initialization
	void Start () {
		//data = GameObject.FindGameObjectWithTag("startGameData").GetComponent<StartGameData>();
		Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScr>();
		data = new StartGameData { StepTime = 40, CountField = 3, CountPlayer = 6, SpawnPlayer = false, SpawnPort = false};

		Fields = new Field[data.CountField];
		for (int i = 0; i < data.CountField; ++i)
		{
			var field = new GameObject();
			field.transform.position = new Vector3(-10000 + i * 10000, 0, 0);
			field.AddComponent<Field>();
			Fields[i] = field.GetComponent<Field>();
			Fields[i].Init(i, 10, TypeField.square, MethodCreating.newField);
		}
		CurrentField = Fields[0];
		CurrentField.Camera.Field = CurrentField;
		CurrentField.Camera.Transforming();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Alpha1))
		{
			CurrentField = Fields[0];
		}
		else if (Input.GetKey(KeyCode.Alpha2) && Fields.Length > 1)
		{
			CurrentField = Fields[1];
		}
		else if (Input.GetKey(KeyCode.Alpha3) && Fields.Length > 2)
		{
			CurrentField = Fields[2];
		}
		else
		{
			return;
		}

		Camera.Field = CurrentField;
		Camera.Radius = Math.Sqrt((CurrentField.Center - CurrentField.transform.position).sqrMagnitude) * 1.7;
		Camera.Transforming();
		sliderSize.value = CurrentField.Size;

		var dropdown = transform.GetChild(2).GetComponent<Dropdown>();
		if (dropdown == null)
		{
			throw new Exception("incorrect child index");
		}
		var toggle = dropdown.transform.GetChild(0).GetComponent<Toggle>();
		if (toggle == null)
		{
			throw new Exception("incorrect child index");
		}
		int type = (int)CurrentField.Type;
		dropdown.value = type % 4;
		toggle.isOn = type >= 4;

		var methodCreating = transform.GetChild(3).GetComponent<Dropdown>();
		if (methodCreating == null)
		{
			throw new Exception("incorrect child index");
		}
		methodCreating.value = (int)CurrentField.Method;
	}

	public void SizeFieldSlider()
	{
		int size = (int)sliderSize.value;
		//sliderSize.enabled = false; // for dont trigger on arrow
		//sliderSize.enabled = true; // for dont trigger on arrow
		sliderSize.interactable = false; // for dont trigger on arrow
		sliderSize.interactable = true; // for dont trigger on arrow
		CurrentField.DestroyField();
		CurrentField.Init(size);
		textSize.text = "size field " + size.ToString();
	}

	public void SetTypeField()
	{
		var item = transform.GetChild(2).GetComponent<Dropdown>();
		if (item == null)
		{
			throw new Exception("incorrect child index");
		}
		var itemToggle = item.transform.GetChild(0).GetComponent<Toggle>();
		if (itemToggle == null)
		{
			throw new Exception("incorrect child index");
		}
		CurrentField.DestroyField();
		int type = item.value;
		if (itemToggle.isOn)
		{
			type += 4;
		}
		CurrentField.Init((TypeField)type);
	}

	public void SetMethodCreatingField()
	{
		var item = transform.GetChild(3).GetComponent<Dropdown>();
		if (item == null)
		{
			throw new Exception("incorrect child index");
		}

	}

	public void BackToMenu()
	{
		Destroy(data.transform);
		SceneManager.LoadScene("menu", LoadSceneMode.Single);
	}
}
