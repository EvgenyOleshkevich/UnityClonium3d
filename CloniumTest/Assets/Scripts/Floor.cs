using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
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
		for (int i = 0; i < 4; ++i)
		{
			var Player = main.transform.GetChild(n + i).GetComponent<Player>();
			int childNumber = Random.Range(0, 99);
			while (transform.GetChild(childNumber).GetComponent<Node>().Player != null)
			{
				childNumber = Random.Range(0, 99);
			}
			transform.GetChild(childNumber).GetComponent<Node>().Initialization(Player);
		}
	}

	public IEnumerator DisclosureMain(Player player)
	{
		var order = new Queue<Transform>();
		order.Enqueue(this.transform);
		int sizeOrder = order.Count;
		while (sizeOrder != 0)
		{
			while (sizeOrder > 0)
			{
				Disclosure(order, player);
				--sizeOrder;
			}
			sizeOrder = order.Count;
			yield return new WaitForSeconds(1.3f);
		}
		main.GetComponent<Main>().UpdatePlayer();
	}

	private void Disclosure(Queue<Transform> order, Player player)
	{
		Transform node = order.Dequeue();
		var nodeObject = node.GetComponent<Node>();
		if (node.childCount < 4)
		{
			return;
		}

		player.CountNodes += 4;

		if (node.childCount == 4)
		{
			node.GetComponent<Node>().Player = null;
			node.GetComponent<Renderer>().material.color = defaultColor;
			node.GetComponent<Node>().Color = Color.white;
			--player.CountNodes;
		}

		int ChildIndex = node.childCount;
		// она нужна чтобы раскатывать с последнего шара, чтобы не оставалось шаров в центре

		for (int i = 0; i < 4; ++i)
		{
			--ChildIndex;
			var ball = node.GetChild(ChildIndex).gameObject;
			if (node.GetComponent<Node>().Neibors[i] == null)
			{
				Destroy(ball);
				--player.CountNodes;
			}
			else
			{
				var neiborNode = node.GetComponent<Node>().Neibors[i].transform;
				if (neiborNode.tag == "teleport")
				{
					ball.transform.SetParent(neiborNode);
					ball.transform.GetComponent<Ball>().AlignmentBall();
					neiborNode.GetComponent<Teleport>().Transport(ball, player);
					continue;
				}
				ball.transform.SetParent(neiborNode);
				ball.transform.GetComponent<Ball>().AlignmentBall();
				neiborNode.GetComponent<Renderer>().material.color = player.Color;
				neiborNode.GetComponent<Node>().Color = player.Color;

				if (neiborNode.GetComponent<Node>().Player != null)
				{
					--neiborNode.GetComponent<Node>().Player.CountNodes;
				}
				neiborNode.GetComponent<Node>().Player = player;

				if (neiborNode.childCount > 3)
				{
					order.Enqueue(neiborNode);
				}
			}
		}
	}
}