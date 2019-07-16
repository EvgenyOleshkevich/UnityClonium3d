using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeField { triangle, square, hexagon, mixed, d3_triangle, d3_square, d3_hexagon, d3_mixed };

public enum MethodCreating { newField, playerField, librariesField };

public enum Mode { none, cutting, spawnPlayer, spawnPort1, spawnPort2, spawnPort3, spawnPort4, playing };

public class Field : MonoBehaviour {

	//public int Index { get; private set; }
	public int Size { get; private set; }
	public TypeField Type { get; set; }
	public MethodCreating Method { get; set; }
	public Node[] Nodes { get; private set; }
	public Vector3 Center { get; private set; }
	//public CameraScr Camera { get; private set; }
	public RedactionField Fields { get; set; }
	private readonly int minimalNodes = 24;
	private Teleport[] ports;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init(int size, TypeField type, MethodCreating method, RedactionField redactionField)
	{
		Size = size;
		Type = type;
		Method = method;
		Fields = redactionField;

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

	public void Init(TypeField type)
	{
		Type = type;
		if (Type == TypeField.square)
		{
			InitSquare();
		}
	}

	//public void Init()
	//{
	//	if (Type == TypeField.square)
	//	{
	//		InitSquare();
	//	}
	//}

	private void InitSquare()
	{
		
		Nodes = new Node[Size * Size];
		Center = new Vector3();
		// creating plane
		for (int i = 0; i < Size * Size; ++i)
		{
			var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
			plane.tag = "node";

			plane.transform.position = new Vector3(12 * (i % Size), 0, 12 * (i / Size)) + transform.position;
			Center += plane.transform.position;
			plane.transform.SetParent(transform);

			plane.AddComponent<Node>();
			Nodes[i] = plane.GetComponent<Node>();
		}
		// addition neibors
		for (int i = 0; i < Size * Size; ++i)
		{
			GameObject u = null;
			GameObject d = null;
			GameObject r = null;
			GameObject l = null;
			if (i % Size != Size - 1)
			{
				u = Nodes[i + 1].gameObject;
			}
			if (i % Size != 0)
			{
				d = Nodes[i - 1].gameObject;
			}
			if (i > Size - 1)
			{
				l = Nodes[i - Size].gameObject;
			}
			if (i < Size * (Size - 1))
			{
				r = Nodes[i + Size].gameObject;
			}
			Nodes[i].AddNeibors(u, r, d, l);
		}

		Center /= Size * Size;
		Fields.Camera.Field = this;
		Fields.Camera.Radius = Math.Sqrt((Center - transform.position).sqrMagnitude) * 1.7;
		Fields.Camera.Transforming();
	}

	public void DestroyField()
	{
		if (Nodes == null)
		{
			return;
		}
		int countRedNodes = 0;
		foreach (Node node in Nodes)
		{
			if (node.Color == Color.red)
			{
				++countRedNodes;
			}
			Destroy(node.gameObject);
		}
		if (countRedNodes != 0)
		{
			Fields.RemainSpawnPlayer += countRedNodes;
			Fields.next.interactable = false;
		}
		Nodes = null;
	}

	public void CutNode(Node node)
	{
		if (Nodes.Length == minimalNodes)
		{
			return;
		}
		int i = 0;
		for (; i < node.Neibors.Length; i++)
		{
			if (node.Neibors[i] != null
				&& node.Neibors[i].tag == "node")
			{
				for (int j = 0; j < node.Neibors[i].GetComponent<Node>().Neibors.Length; j++)
				{
					if (node.Neibors[i].GetComponent<Node>().Neibors[j] == node)
					{
						node.Neibors[i].GetComponent<Node>().Neibors[j] = null;
					}
				}
			}
		}
		i = 0;
		int k = 0;
		var newNodes = new Node[Nodes.Length - 1];
		for (; i < Nodes.Length; i++)
		{
			if (Nodes[i] != node)
			{
				newNodes[k] = Nodes[i];
				++k;
			}
		}
		Nodes = newNodes;
		var color = node.Color; 
		Destroy(node.gameObject);
		if (color == Color.red)
		{
			++Fields.RemainSpawnPlayer;
			UpdateColorNodes();
			Fields.next.interactable = false;
		}
	}

	public void SpawnPlayer(Node node)
	{
		if (node.Color == Color.red)
		{
			node.SetColor(Color.white);
			++Fields.RemainSpawnPlayer;
			UpdateColorNodes();
			Fields.next.interactable = false;
			return;
		}
		if (node.Color == Color.blue
			|| node.Color == Color.green
			|| Fields.RemainSpawnPlayer == 0)
		{
			return;
		}

		PrintColorAroundRed(node);

		--Fields.RemainSpawnPlayer;
		if (Fields.RemainSpawnPlayer == 0)
		{
			Fields.next.interactable = true;
		}
	}

	private void UpdateColorNodes()
	{
		foreach (var node in Nodes)
		{
			if (node.Color == Color.blue
				|| node.Color == Color.green)
			{
				node.SetColor(Color.white);
			}
		}
		foreach (var node in Nodes)
		{
			if (node.Color == Color.red)
			{
				PrintColorAroundRed(node);
			}
		}
	}

	public void WhitePrintingNodes()
	{
		foreach (var node in Nodes)
		{
			if (node.Color == Color.blue
				|| node.Color == Color.green)
			{
				node.SetColor(Color.white);
			}
		}
	}

	private void PrintColorAroundRed(Node node)
	{
		//for (int i = 0; i < node.Neibors.Length; i++)
		//{
		//	if (node.Neibors[i] != null && node.Neibors[i].tag == "node")
		//	{
		//		for (int j = 0; j < node.Neibors[i].GetComponent<Node>().Neibors.Length; j++)
		//		{
		//			if (node.Neibors[i].GetComponent<Node>().Neibors[j] != null
		//				&& node.Neibors[i].GetComponent<Node>().Neibors[j].tag == "node"
		//				&& node.Neibors[i].GetComponent<Node>().Neibors[j].GetComponent<Node>().Color == Color.white)
		//			{
		//				node.Neibors[i].GetComponent<Node>().Neibors[j].GetComponent<Node>().SetColor(Color.green);
		//			}
		//		}
		//	}
		//}
		for (int i = 0; i < node.Neibors.Length; i++)
		{
			if (node.Neibors[i] != null && node.Neibors[i].tag == "node")
			{
				PrintColorAroundBlue(node.Neibors[i].GetComponent<Node>()); 
				//.SetColor(Color.blue);
			}
		}
		node.SetColor(Color.red);
	}

	private void PrintColorAroundBlue(Node node)
	{
		for (int i = 0; i < node.Neibors.Length; i++)
		{
			if (node.Neibors[i] != null && node.Neibors[i].tag == "node")
			{
				node.Neibors[i].GetComponent<Node>().SetColor(Color.green);
			}
		}
		node.SetColor(Color.blue);
	}

	public void SpawnPort(Node node)
	{
		Fields.next.interactable = false;
		PrintColorAroundBlue(node);

		ports = new Teleport[node.Neibors.Length];
		for (int i = 0; i < node.Neibors.Length; i++)
		{
			var port = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			port.AddComponent<Teleport>();
			ports[i] = port.GetComponent<Teleport>();
			ports[i].Node = node.gameObject;
			ports[i].Field = this;
			ports[i].tag = "teleport";
		}
		if (Type == TypeField.square)
		{
			ports[0].transform.position = node.transform.position + new Vector3(6, 3, 0);
			ports[0].NodeBack = node.Neibors[0];
			ports[1].transform.position = node.transform.position + new Vector3(0, 3, 6);
			ports[1].NodeBack = node.Neibors[1];
			ports[2].transform.position = node.transform.position + new Vector3(-6, 3, 0);
			ports[2].NodeBack = node.Neibors[2];
			ports[3].transform.position = node.transform.position + new Vector3(0, 3, -6);
			ports[3].NodeBack = node.Neibors[3];
		}
		for (int i = 0; i < node.Neibors.Length; i++)
		{
			Quaternion lookRatation = Quaternion.LookRotation(node.transform.position
				- ports[i].transform.position + new Vector3(0, 90, 0));
			//Quaternion lookRatation = Quaternion.LookRotation(new Vector3(0, 90, 0));
			ports[i].transform.rotation = lookRatation;
			ports[i].transform.localScale = new Vector3(8, 1, 8);
		}
		if (Fields.Mode == Mode.spawnPort1)
		{
			Fields.Mode = Mode.spawnPort2;
		}
		else
		{
			Fields.Mode = Mode.spawnPort4;
		}
	}

	public void ChoosePort(Teleport port)
	{
		if (Array.Find(ports, i => i == port) == null)
		{
			return;
		}
		for (int i = 0; i < ports.Length; i++)
		{
			if (ports[i] != port)
			{
				Destroy(ports[i].gameObject);
			}
			else
			{
				port.Node.GetComponent<Node>().Neibors[i] = port.gameObject;
				if (port.NodeBack != null)
				{
					port.NodeBack.GetComponent<Node>().Neibors[(i + 2) % 4] = null;
				}
			}
		} 
		if (Fields.Mode == Mode.spawnPort2)
		{
			Fields.Port = port;
			Fields.Mode = Mode.spawnPort3;
			return;
		}
		port.ExitTeleport = Fields.Port;
		Fields.Port.ExitTeleport = port;
		Fields.Mode = Mode.spawnPort1;
		Fields.Ports.Add(new KeyValuePair<Teleport, Teleport>(Fields.Port, port));
		Fields.Port = null;
		if (Fields.CheckingConnection())
		{
			Fields.next.interactable = true;
		}
	}

	public IEnumerator DisclosureMain(Node node, Player player)
	{
		var order = new Queue<Node>();
		order.Enqueue(node);
		int sizeOrder = order.Count;
		while (order.Count > 0)
		{
			while (sizeOrder > 0)
			{
				Disclosure(order, player);
				--sizeOrder;
			}
			sizeOrder = order.Count;
			yield return new WaitForSeconds(1.2f);
		}
		Fields.Main.UpdatePlayer();
	}

	private void Disclosure(Queue<Node> order, Player player)
	{
		var node = order.Dequeue();
		if (node.transform.childCount < 4)
		{
			return;
		}

		player.CountNodes += 4;

		if (node.transform.childCount == 4)
		{
			node.Player = null;
			node.GetComponent<Renderer>().material.color = Color.white;
			node.Color = Color.white;
			--player.CountNodes;
		}

		int ChildIndex = node.transform.childCount;
		// она нужна чтобы раскатывать с последнего шара, чтобы не оставалось шаров в центре

		for (int i = 0; i < 4; ++i)
		{

			--ChildIndex;
			var ball = node.transform.GetChild(ChildIndex);
			if (node.Neibors[i] == null)
			{
				Destroy(ball.gameObject);
				--player.CountNodes;
			}
			else
			{
				Node neiborNode;
				if (node.Neibors[i].tag == "teleport")
				{
					ball.SetParent(node.Neibors[i].transform);
					ball.GetComponent<Ball>().AlignmentBall();
					var port = node.Neibors[i].GetComponent<Teleport>();
					port.Transport(ball, player);
					neiborNode = port.ExitTeleport.Node.GetComponent<Node>();
				}
				else
				{
					neiborNode = node.Neibors[i].GetComponent<Node>();
					ball.SetParent(neiborNode.transform);
					ball.GetComponent<Ball>().AlignmentBall();
				}
				neiborNode.GetComponent<Renderer>().material.color = player.Color;
				neiborNode.Color = player.Color;

				if (neiborNode.Player != null)
				{
					--neiborNode.Player.CountNodes;
				}
				neiborNode.Player = player;

				if (neiborNode.transform.childCount > 3)
				{
					order.Enqueue(neiborNode);
				}
			}
		}
	}

	public List<Node> GetNodesForSpawn()
	{
		var list = new List<Node>();
		foreach (var node in Nodes)
		{
			if (node.Color == Color.red)
			{
				list.Add(node);
			}
		}
		return list;
	}
}
