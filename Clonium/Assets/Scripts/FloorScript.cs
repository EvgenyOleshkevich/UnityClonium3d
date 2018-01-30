using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
	public int IndexPlayers = 0;
	private int Width = 10;
	private int Size = 99;
	private GameObject Ball;
	private GameObject Player;
	public GameObject OldBall;
	public GameObject Main;
	public Transform[] Nodes;
	private Transform TempNode;
	private int[] OrderNodes = new int[160];
	private Queue<int> OrderTest = new Queue<int>();
	public int IndexEndOrderFirst = -1;
	private bool Stop = true;
	// public int test = 0;


	void Start()
	{
		Nodes = new Transform[this.transform.childCount];
		for (int i = 0; i < this.transform.childCount; ++i)
		{
			Nodes[i] = this.transform.GetChild(i);
		}
	}
	public void Clicked(int Index)
	{
		int MasterOfNode = Nodes[Index].GetComponent<NodesScript>().Master;
		if ((MasterOfNode == IndexPlayers) && (Stop))
		{
			Stop = false;
			CreateBall(Index);
		}
		return;
	}

	void CreateBall(int Index)
	{
		Ball = Instantiate(OldBall, Nodes[Index]);
		Alignment(Nodes[Index]);
		if (Nodes[Index].childCount > 3)
		{
			StartCoroutine(DisclosureMain(Index));
		}
		else
		{
			Stop = true;
			Main.GetComponent<Main>().UpdatePlayer();
		}
		return;
	}

	IEnumerator DisclosureMain(int Index)
	{
		OrderTest.Enqueue(Index);
		IndexEndOrderFirst = OrderTest.Count;
		while (OrderTest.Count != 0)
		{
			while (IndexEndOrderFirst > 0)
			{
				Disclosure();
				--IndexEndOrderFirst;
			}
			IndexEndOrderFirst = OrderTest.Count;
			yield return new WaitForSeconds(1.4f);
		}
		Stop = true;
		Main.GetComponent<Main>().UpdatePlayer();
	}

	void Disclosure()
	{
		int TempIndex = OrderTest.Dequeue();
		if (Nodes[TempIndex].childCount < 4)
		{
			return;
		}

		Player = Main.GetComponent<Main>().Players[IndexPlayers].gameObject;
		Player.GetComponent<PlayerScript>().CountNodes += 4;

		if (Nodes[TempIndex].childCount == 4)
		{
			Nodes[TempIndex].GetComponent<NodesScript>().Master = -1;
			Nodes[TempIndex].GetComponent<Renderer>().material.color = Color.white;
			Nodes[TempIndex].GetComponent<NodesScript>().MasterColor = Color.white;
			--Player.GetComponent<PlayerScript>().CountNodes;
		}

		Color MasterColor = Player.GetComponent<PlayerScript>().ColorPlayer;
		Ball = Nodes[TempIndex].GetChild(3).gameObject;
		if (TempIndex + Width > Size)
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
		}
		else
		{
			Ball.transform.SetParent(Nodes[TempIndex + Width]);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempNode = this.transform.GetChild(TempIndex + Width);
			TempNode.GetComponent<Renderer>().material.color = MasterColor;
			TempNode.GetComponent<NodesScript>().MasterColor = MasterColor;
			int TempMaster = TempNode.GetComponent<NodesScript>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempNode.GetComponent<NodesScript>().Master = IndexPlayers;

			if (TempNode.childCount > 3)
			{
				OrderTest.Enqueue(TempIndex + Width);
			}
		}

		////
		Ball = Nodes[TempIndex].GetChild(2).gameObject;
		if (TempIndex - Width < 0)
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
		}
		else
		{
			Ball.transform.SetParent(Nodes[TempIndex - Width]);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempNode = this.transform.GetChild(TempIndex - Width);
			TempNode.GetComponent<Renderer>().material.color = MasterColor;
			TempNode.GetComponent<NodesScript>().MasterColor = MasterColor;
			int TempMaster = TempNode.GetComponent<NodesScript>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempNode.GetComponent<NodesScript>().Master = IndexPlayers;

			if (Nodes[TempIndex - Width].childCount > 3)
			{
				OrderTest.Enqueue(TempIndex - Width);
			}
		}

		////
		Ball = Nodes[TempIndex].GetChild(1).gameObject;
		if ((TempIndex + 1) % Width == 0)
		{
			// дестрой очень медленная
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
		}
		else
		{
			Ball.transform.SetParent(Nodes[TempIndex + 1]);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempNode = this.transform.GetChild(TempIndex + 1);
			TempNode.GetComponent<Renderer>().material.color = MasterColor;
			TempNode.GetComponent<NodesScript>().MasterColor = MasterColor;
			int TempMaster = TempNode.GetComponent<NodesScript>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempNode.GetComponent<NodesScript>().Master = IndexPlayers;
			if (Nodes[TempIndex + 1].childCount > 3)
			{
				OrderTest.Enqueue(TempIndex + 1);
			}
		}

		////
		Ball = Nodes[TempIndex].GetChild(0).gameObject;
		if (((TempIndex - 1) % Width == Width - 1) || (TempIndex - 1 < 0))
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
		}
		else
		{
			Ball.transform.SetParent(Nodes[TempIndex - 1]);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempNode = this.transform.GetChild(TempIndex - 1);
			TempNode.GetComponent<Renderer>().material.color = MasterColor;
			TempNode.GetComponent<NodesScript>().MasterColor = MasterColor;
			int TempMaster = TempNode.GetComponent<NodesScript>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempNode.GetComponent<NodesScript>().Master = IndexPlayers;
			if (Nodes[TempIndex - 1].childCount > 3)
			{
				OrderTest.Enqueue(TempIndex - 1);
			}
		}
		Alignment(Nodes[TempIndex]);
	}

	void Alignment(Transform Parent)
	{
		Debug.Log(Parent.childCount);
		Vector3 PositionBall = Parent.position;
		PositionBall.y += 1f;
		if (Parent.childCount > 0)
		{
			PositionBall.z -= 1f;
			Parent.GetChild(0).position = PositionBall;
			PositionBall.z += 1f;
		}
		if (Parent.childCount > 1)
		{
			PositionBall.z += 1f;
			Parent.GetChild(1).position = PositionBall;
			PositionBall.z -= 1f;
		}
		if (Parent.childCount > 2)
		{
			PositionBall.x -= 1f;
			Parent.GetChild(2).position = PositionBall;
			PositionBall.x += 1f;
		}
		if (Parent.childCount > 3)
		{
			PositionBall.x += 1f;
			Parent.GetChild(3).position = PositionBall;
			PositionBall.x -= 1f;
		}
	}
}