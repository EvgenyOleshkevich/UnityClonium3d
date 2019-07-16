using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public Text CountPlayer;
	public Text StepTime;
	public Text CountField;
	public Toggle AutoPlayer;
	public Toggle AutoPort;

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
		int count = int.Parse(CountPlayer.text.Split()[2]);
		count = Math.Min(12, count + 1);
		CountPlayer.text = "count players " + count.ToString();
	}

	public void DownCount()
	{

		int count = int.Parse(CountPlayer.text.Split()[2]);
		count = Math.Max(2, count - 1);
		CountPlayer.text = "count players " + count.ToString();
	}

	public void UpTime()
	{
		int count = int.Parse(StepTime.text.Split()[2]);
		count = Math.Min(60, count + 10);
		StepTime.text = "step time " + count.ToString() + " sec";
	}

	public void DownTime()
	{

		int count = int.Parse(StepTime.text.Split()[2]);
		count = Math.Max(20, count - 10);
		StepTime.text = "step time " + count.ToString() + " sec";
	}

	public void UpField()
	{
		int count = int.Parse(CountField.text.Split()[2]);
		//if (count == 1)
		//{
		//	AutoPort.interactable = true;
		//}
		count = Math.Min(3, count + 1);
		CountField.text = "count field " + count.ToString();
	}

	public void DownField()
	{

		int count = int.Parse(CountField.text.Split()[2]);
		count = Math.Max(1, count - 1);
		CountField.text = "count field " + count.ToString();
		//if (count == 1)
		//{
		//	AutoPort.interactable = false;
		//}
	}

	public void Next()
	{
		//Application.LoadLevel("game");
		var data = GameObject.FindGameObjectWithTag("startGameData").GetComponent<StartGameData>();
		data.CountPlayer = int.Parse(CountPlayer.text.Split()[2]);
		data.StepTime = int.Parse(StepTime.text.Split()[2]);
		data.CountField = int.Parse(CountField.text.Split()[2]);
		data.SpawnPlayer = AutoPlayer.isOn;
		data.SpawnPort = AutoPort.isOn;
		SceneManager.LoadScene("redaction", LoadSceneMode.Single);
		//Destroy(this);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
