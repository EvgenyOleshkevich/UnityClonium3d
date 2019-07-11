using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeField { triangle, square, hexagon, mixed, d3_triangle, d3_square, d3_hexagon, d3_mixed };


public class Field : MonoBehaviour {

	public int Index { get; private set; }
	public int Size { get; private set; }
	public TypeField Type { get; private set; }
	private Node[] nodes;
	public Vector3 Center { get; private set; }
	public CameraScr camera;


	public void Init(int index, int size, TypeField type)
	{
		Index = index;
		Size = size;
		if (type == TypeField.square)
		{
			InitSquare();
		}
	}

	private void InitSquare()
	{
		nodes = new Node[Size];
		Center = new Vector3();
		for (int i = 0; i < Size * Size; ++i)
		{
			var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

			plane.transform.position = new Vector3(12 * (i % 10), 0, 12 * (i / 10));
			Center += plane.transform.position;
			//plane.transform.localScale = new Vector3(4, 1, 4);
			//plane.transform.parent.SetParent(this.transform);
		}
		Center /= Size * Size;
		if (camera == null)
		{
			camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().GetComponent<CameraScr>();
		}
		camera.Field = this;
		camera.Fi = 0;
		camera.Psi = Math.PI / 3;
		camera.Radius = Math.Sqrt(Center.sqrMagnitude) * 1.7;
		camera.Transforming();
	}

	// Use this for initialization
	void Start ()
	{
		//GameObject.CreatePrimitive(PrimitiveType.Capsule);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
