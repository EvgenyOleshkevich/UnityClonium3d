using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScr : MonoBehaviour {
	public Field Field { get; set; }
	// public Field Field;
	public double Fi { get; set; }
	public double Psi { get; set; }
	public double Radius { get; set; }
	//public double Fi;
	//public double Psi;
	//public double Radius;
	private double zoom = 1;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.DownArrow))
		{
			Psi = Math.Min(Math.PI / 2 - 0.01, Psi + 0.01);
			Transforming();
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			Psi = Math.Max(0.01, Psi - 0.01);
			Transforming();
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			Fi += 0.01;
			Transforming();
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Fi -= 0.01;
			Transforming();
		}
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if (mouseWheel > 0.1)
		{
			zoom = Math.Min(1.5, zoom + 0.1);
			Transforming();
		}
		if (mouseWheel < -0.1)
		{
			zoom = Math.Max(0.3, zoom - 0.1);
			Transforming();
		}
	}

	public void Transforming()
	{
		transform.position = new Vector3((float)(zoom * Radius * Math.Cos(Fi) * Math.Sin(Psi)),
			(float)(zoom * Radius * Math.Cos(Psi)), (float)(zoom * Radius * Math.Sin(Fi) * Math.Sin(Psi))) + Field.Center;
		Quaternion lookRatation = Quaternion.LookRotation(Field.Center - transform.position);
		transform.rotation = lookRatation;
	}
}
