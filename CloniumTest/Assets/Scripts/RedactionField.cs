using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RedactionField : MonoBehaviour {
	public Button next;
	public GameObject OriginalBall;

	private StartGameData data;
	public Field[] Fields { get; private set; }
	public Field CurrentField { get; private set; }
	private CameraScr Camera { get; set; }
	public int RemainSpawnPlayer { get; set; }
	public Mode Mode;
	public Teleport Port { get; set; }
	public List<KeyValuePair<Teleport, Teleport>> Ports;
	public Main Main { get; private set; }
	

	// Use this for initialization
	void Start() {
		//data = GameObject.FindGameObjectWithTag("startGameData").GetComponent<StartGameData>();
		Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScr>();
		data = GameObject.FindGameObjectWithTag("startGameData").GetComponent<StartGameData>();
		Destroy(GameObject.FindGameObjectWithTag("startGameData"));
		//data = new StartGameData { StepTime = 40, CountField = 2, CountPlayer = 1, SpawnPlayer = false, SpawnPort = false };

		Ports = new List<KeyValuePair<Teleport, Teleport>>();
		Mode = Mode.none;
		RemainSpawnPlayer = data.CountPlayer;

		Fields = new Field[data.CountField];
		for (int i = 0; i < data.CountField; ++i)
		{
			var field = new GameObject();
			field.transform.position = new Vector3(-10000 + i * 10000, 0, 0);
			field.AddComponent<Field>();
			Fields[i] = field.GetComponent<Field>();
			Fields[i].Init(6, TypeField.square, MethodCreating.newField, this);
		}
		CurrentField = Fields[0];
		CurrentField.Camera.Field = CurrentField;
		CurrentField.Camera.Transforming();

		// preparing dropdown for cutting, spawn
		var panel = transform.GetChild(4).GetComponent<Dropdown>();
		if (panel == null)
		{
			throw new Exception("incorrect child index");
		}
		var listOption = new List<Dropdown.OptionData>()
		{
			new Dropdown.OptionData("none"),
			new Dropdown.OptionData("cutting field")
		};
		if (!data.SpawnPlayer)
		{
			listOption.Add(new Dropdown.OptionData("spawn player"));
		}
		else
		{
			RemainSpawnPlayer = 0;
			next.interactable = true;
		}
		
		panel.AddOptions(listOption);
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

		if ((int)Mode > 2)
		{
			return;
		}

		var sliderSize = transform.GetChild(1).GetComponent<Slider>();
		if (sliderSize == null)
		{
			throw new Exception("incorrect child index");
		}
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
		var sliderSize = transform.GetChild(1).GetComponent<Slider>();
		if (sliderSize == null)
		{
			throw new Exception("incorrect child index");
		}
		int size = (int)sliderSize.value;
		//sliderSize.enabled = false; // for dont trigger on arrow
		//sliderSize.enabled = true; // for dont trigger on arrow
		sliderSize.interactable = false; // for dont trigger on arrow
		sliderSize.interactable = true; // for dont trigger on arrow
		CurrentField.DestroyField();
		CurrentField.Init(size);
		sliderSize.transform.GetChild(0).GetComponent<Text>().text = "size field " + size.ToString();
		UpdateMode();
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
		UpdateMode();
	}

	public void SetMethodCreatingField()
	{
		var item = transform.GetChild(3).GetComponent<Dropdown>();
		if (item == null)
		{
			throw new Exception("incorrect child index");
		}
		UpdateMode();
	}

	public void BackToMenu()
	{
		//Destroy(data.transform);
		SceneManager.LoadScene("menu", LoadSceneMode.Single);
	}

	public void Next()
	{
		if (transform.childCount > 4)
		{
			Destroy(transform.GetChild(4).gameObject);
			Destroy(transform.GetChild(3).gameObject);
			Destroy(transform.GetChild(2).gameObject);
			Destroy(transform.GetChild(1).gameObject);
		}
		if (data.SpawnPort)
		{
			Destroy(transform.GetChild(5).gameObject);
			AutoSpawnPort();
		}
		else
		{
			if (Mode != Mode.spawnPort)
			{
				Mode = Mode.spawnPort;
				next.interactable = CheckingConnection();
				return;
			}
		}
		if (data.SpawnPlayer)
		{
			AutoSpawnPlayer();
		}
		var text = transform.GetChild(2).GetComponent<Text>();
		Destroy(transform.GetChild(1).gameObject);
		Mode = Mode.playing;
		next = null;
		Port = null;
		Ports = null;
		data = null;

		foreach (var item in Fields)
		{
			item.WhitePrintingNodes();
		}
		
		text.transform.position -= new Vector3(0, 70, 0);
		Main = new Main(Fields, text);
		
	}

	public bool CheckingConnection()
	{
		var set = new HashSet<Field>();
		set.Add(Fields[0]); // firts field is every time available(dostupno)
		foreach (var item in Ports)
		{
			if (item.Key.Field != item.Value.Field)
			{
				set.Add(item.Key.Field);
				set.Add(item.Value.Field);
			}
		}
		return set.Count == Fields.Length;
	}

	private void AutoSpawnPort() {

		if (data.SpawnPlayer)
		{
			AutoSpawnPlayer();
		}
	}

	private void AutoSpawnPlayer()
	{

	}

	public void SetMode()
	{
		// cheking panel
		var panel = transform.GetChild(4).GetComponent<Dropdown>();
		if (panel == null)
		{
			throw new Exception("incorrect child index");
		}
		// 0 - none, 1 - cutting, 2 - player, 3 - port
		Mode = (Mode)panel.value;
	}

	private void UpdateMode()
	{
		var panel = transform.GetChild(4).GetComponent<Dropdown>();
		if (panel == null)
		{
			throw new Exception("incorrect child index");
		}
		panel.value = 0;
	}
}
