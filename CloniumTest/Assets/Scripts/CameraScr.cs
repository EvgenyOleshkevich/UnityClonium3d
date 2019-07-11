using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScr : MonoBehaviour {
	//public Field Field { get; set; }
	public Field Field;
	//public double Fi { get; set; }
	//public double Psi { get; set; }
	//public double Radius { get; set; }
	public double Fi;
	public double Psi;
	public double Radius;


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
			Radius = Math.Min(200, Radius + 4);
			Transforming();
		}
		if (mouseWheel < -0.1)
		{
			Radius = Math.Max(20, Radius - 4);
			Transforming();
		}
	}

	public void Transforming()
	{
		transform.position = new Vector3((float)(Radius * Math.Cos(Fi) * Math.Sin(Psi)),
			(float)(Radius * Math.Cos(Psi)), (float)(Radius * Math.Sin(Fi) * Math.Sin(Psi))) + Field.Center;
		Quaternion lookRatation = Quaternion.LookRotation(Field.Center - transform.position);
		transform.rotation = lookRatation;
	}
}
