﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public int Master = -1;
	public Color MasterColor = Color.white;
	public int IndexNodes;
	void Start()
	{
		this.GetComponent<Renderer>().material.color = MasterColor;
		for (int i = 0; i < 16; ++i)
		{
			if (this.transform == this.transform.parent.GetChild(i).transform)
			{
				IndexNodes = i;
			}
		}
	}


	void OnMouseEnter()
	{
		this.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, MasterColor, 0.8f);
	}

	void OnMouseExit()
	{
		this.GetComponent<Renderer>().material.color = MasterColor;
	}

	void OnMouseDown()
	{
		this.transform.parent.GetComponent<FloorScript>().Clicked(IndexNodes);
		//this.GetComponentInParent<FloorScript>().Index = Index;
		//this.GetComponentInParent<FloorScript>().StopUpdate = 0;
	}

}