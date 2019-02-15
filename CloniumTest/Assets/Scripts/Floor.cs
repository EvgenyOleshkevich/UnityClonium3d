using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
	public GameObject originalTeleport;
	public Color color;
	public Color defaultColor;
	public GameObject main;

	void Start()
	{
		this.GetComponent<Renderer>().material.color = color;
		// adding neibors
		for (int i = 0; i < 100; ++i)
		{

			Transform u = null;
			Transform d = null;
			Transform r = null;
			Transform l = null;
			if (i % 10 != 9)
			{
				u = transform.GetChild(i + 1);
			}
			if (i % 10 != 0)
			{
				d = transform.GetChild(i - 1);
			}
			if (i > 9)
			{
				l = transform.GetChild(i - 10);
			}
			if (i < 90)
			{
				r = transform.GetChild(i + 10);
			}
			transform.GetChild(i).GetComponent<Node>().AddNeibors(u, r, d, l);
		}
	}

	public void Intialization(int n)
	{
		n *= 4;
		for (int i = 0; i < 2; ++i)
		{
			var Player = main.transform.GetChild(n + i).GetComponent<Player>();
			int childNumber = Random.Range(0, 2);
			while (transform.GetChild(childNumber).GetComponent<Node>().Player != null)
			{
				childNumber = Random.Range(0, 2);
			}
			transform.GetChild(childNumber).GetComponent<Node>().Initialization(Player);
		}
	}

	public void SpawnTeleport(Transform Floor)
	{
		int number = Random.Range(0, 100);
		int side = Random.Range(0, 4);
		float shift = 3f / 51;
		var position = this.transform.GetChild(number).position;
		var node = this.transform.GetChild(number).GetComponent<Node>();
		Quaternion quaternion = new Quaternion(0, 0, 0, 1);
		Transform neibor = node.Neibors[side];
		switch (side)
		{
			case 0:
				{
					position.z += shift;
					quaternion = new Quaternion(0, 90f, 0, 1);
					break;
				}
			case 1:
				{
					position.x += shift;
					quaternion = new Quaternion(90f, 0, 0, 1);
					break;
				}
			case 2:
				{
					position.z -= shift;
					quaternion = new Quaternion(0, 90f, 0, 1);
					break;
				}
			case 3:
				{
					position.x -= shift;
					quaternion = new Quaternion(90f, 0, 0, 1);
					break;
				}
		}
		var port = Instantiate(originalTeleport, position, quaternion, this.transform);
		if (neibor = null)
		{

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
		main.GetComponent<Main>().UpdatePlayer();
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
			node.GetComponent<Renderer>().material.color = defaultColor;
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
				
				if (node.Neibors[i].tag == "teleport")
				{
					ball.SetParent(node.Neibors[i]);
					ball.GetComponent<Ball>().AlignmentBall();
					if (node.Neibors[i].GetComponent<Teleport>().Node.childCount > 2)
					{
						order.Enqueue(node.Neibors[i].GetComponent<Teleport>().
							GetComponent<Node>());
					}
					node.Neibors[i].GetComponent<Teleport>().Transport(ball, player);
					continue;
				}

				var neiborNode = node.Neibors[i].GetComponent<Node>();
				ball.SetParent(neiborNode.transform);
				ball.GetComponent<Ball>().AlignmentBall();
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
}