using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeField { triangle, square, hexagon, mixed, d3_triangle, d3_square, d3_hexagon, d3_mixed };


public class Field : MonoBehaviour {

	public int Index { get; private set; }
	public int Size { get; set; }
	public TypeField Type { get; set; }
	private Node[] nodes;
	public Vector3 Center;
	//public Vector3 Center { get; private set; }
	public CameraScr Camera { get; private set; }


	public void Init(int index, int size, TypeField type)
	{
		Index = index;
		Size = size;
		Type = type;
		if (Type == TypeField.square)
		{
			InitSquare();
		}
	}

	public void Init(int size)
	{
		Size = size;
		if (Type == TypeField.square)
		{
			InitSquare();
		}
	}

	private void InitSquare()
	{
		
		nodes = new Node[Size * Size];
		Center = new Vector3();
		// creating plane
		for (int i = 0; i < Size * Size; ++i)
		{
			var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

			plane.transform.position = new Vector3(12 * (i % Size), 0, 12 * (i / Size));
			Center += plane.transform.position;
			plane.transform.SetParent(transform);

			plane.AddComponent<Node>();
			nodes[i] = plane.GetComponent<Node>();
		}
		// addition neibors
		for (int i = 0; i < Size * Size; ++i)
		{
			Transform u = null;
			Transform d = null;
			Transform r = null;
			Transform l = null;
			if (i % Size != Size - 1)
			{
				u = nodes[i + 1].GetComponent<Transform>();
			}
			if (i % Size != 0)
			{
				d = nodes[i - 1].GetComponent<Transform>();
			}
			if (i > Size - 1)
			{
				l = nodes[i - Size].GetComponent<Transform>();
			}
			if (i < Size * (Size - 1))
			{
				r = nodes[i + Size].GetComponent<Transform>();
			}
			nodes[i].AddNeibors(u, r, d, l);
		}

		Center /= Size * Size;
		// camera transforming
		if (Camera == null)
		{
			Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScr>();
			Camera.Field = this;
			Camera.Fi = 0;
			Camera.Psi = Math.PI / 3;
		}

		Camera.Radius = Math.Sqrt(Center.sqrMagnitude) * 1.7;
		Camera.Transforming();
	}

	public void DestroyField()
	{
		foreach (Node node in nodes)
		{
			Destroy(node.gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
