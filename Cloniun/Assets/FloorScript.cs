using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
	public int IndexPlayers = 0;
	private int Width = 4;
	private GameObject Ball;
	private GameObject Player;
	public GameObject OldBall;
	public GameObject Main;
	public Transform[] Nodes;
	private Transform TempNode;
	private int[] OrderNodes = new int[40];
	public int IndexStartOrder = 0;
	public int IndexEndOrderFirst = -1;
	public int IndexEndOrderSecond = -1;
	public bool Stop = true;
	public int test = 0;

	public void Clicked(int Index)
	{
		int MasterOfNode = Nodes[Index].GetComponent<GameMaster>().Master;
		if ((MasterOfNode == IndexPlayers) && (Stop))
		{
			Stop = false;
			CreateAndAlignment(Index);
		}
		Stop = true;
		return;
	}

	void CreateAndAlignment(int Index)
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
		IndexEndOrderFirst = 0;
		IndexEndOrderSecond = 0;
		OrderNodes[IndexEndOrderFirst] = Index;

		while (IndexStartOrder <= IndexEndOrderSecond)
		{
			while (IndexStartOrder <= IndexEndOrderFirst)
			{
				Disclosure();
			}
			IndexEndOrderFirst = IndexEndOrderSecond;
			yield return new WaitForSeconds(1.4f);
		}
		IndexStartOrder = 0;
		for (int i = 0; i < IndexEndOrderSecond; ++i)
		{
			OrderNodes[i] = 0;
		}
		Stop = true;
		Main.GetComponent<Main>().UpdatePlayer();
	}

	void Disclosure()
	{
		int TempIndex = OrderNodes[IndexStartOrder];
		++IndexStartOrder;
		if (Nodes[TempIndex].childCount < 4)
		{
			return;
		}

		Player = Main.GetComponent<Main>().Players[IndexPlayers].gameObject;
		Player.GetComponent<PlayerScript>().CountNodes += 4;

		if (Nodes[TempIndex].childCount == 4)
		{
			Nodes[TempIndex].GetComponent<GameMaster>().Master = -1;
			Nodes[TempIndex].GetComponent<Renderer>().material.color = Color.white;
			Nodes[TempIndex].GetComponent<GameMaster>().MasterColor = Color.white;
			--Player.GetComponent<PlayerScript>().CountNodes;
			++test;
		}

		Color MasterColor = Player.GetComponent<PlayerScript>().ColorPlayer;
		Ball = Nodes[TempIndex].GetChild(3).gameObject;
		if (TempIndex + 4 > 15)
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
			++test;
		}
		else
		{
			Ball.transform.SetParent(Nodes[TempIndex + 4]);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempNode = this.transform.GetChild(TempIndex + 4);
			TempNode.GetComponent<Renderer>().material.color = MasterColor;
			TempNode.GetComponent<GameMaster>().MasterColor = MasterColor;
			int TempMaster = TempNode.GetComponent<GameMaster>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempNode.GetComponent<GameMaster>().Master = IndexPlayers;

			if (TempNode.childCount > 3)
			{
				++IndexEndOrderSecond;
				OrderNodes[IndexEndOrderSecond] = TempIndex + 4;
			}
		}

		////
		Ball = Nodes[TempIndex].GetChild(2).gameObject;
		if (TempIndex - 4 < 0)
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
			++test;
		}
		else
		{
			Ball.transform.SetParent(Nodes[TempIndex - 4]);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempNode = this.transform.GetChild(TempIndex - 4);
			TempNode.GetComponent<Renderer>().material.color = MasterColor;
			TempNode.GetComponent<GameMaster>().MasterColor = MasterColor;
			int TempMaster = TempNode.GetComponent<GameMaster>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempNode.GetComponent<GameMaster>().Master = IndexPlayers;

			if (Nodes[TempIndex - 4].childCount > 3)
			{
				++IndexEndOrderSecond;
				OrderNodes[IndexEndOrderSecond] = TempIndex - 4;
			}
		}

		////
		Ball = Nodes[TempIndex].GetChild(1).gameObject;
		if ((TempIndex + 1) % 4 == 0)
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
			++test;
		}
		else
		{
			Ball.transform.SetParent(Nodes[TempIndex + 1]);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempNode = this.transform.GetChild(TempIndex + 1);
			TempNode.GetComponent<Renderer>().material.color = MasterColor;
			TempNode.GetComponent<GameMaster>().MasterColor = MasterColor;
			int TempMaster = TempNode.GetComponent<GameMaster>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempNode.GetComponent<GameMaster>().Master = IndexPlayers;
			if (Nodes[TempIndex + 1].childCount > 3)
			{
				++IndexEndOrderSecond;
				OrderNodes[IndexEndOrderSecond] = TempIndex + 1;
			}
		}

		////
		Ball = Nodes[TempIndex].GetChild(0).gameObject;
		if (((TempIndex - 1) % 4 == 3) || (TempIndex - 1 < 0))
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
			++test;
		}
		else
		{
			Ball.transform.SetParent(Nodes[TempIndex - 1]);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempNode = this.transform.GetChild(TempIndex - 1);
			TempNode.GetComponent<Renderer>().material.color = MasterColor;
			TempNode.GetComponent<GameMaster>().MasterColor = MasterColor;
			int TempMaster = TempNode.GetComponent<GameMaster>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempNode.GetComponent<GameMaster>().Master = IndexPlayers;
			if (Nodes[TempIndex - 1].childCount > 3)
			{
				++IndexEndOrderSecond;
				OrderNodes[IndexEndOrderSecond] = TempIndex - 1;
			}
		}
	}

	public void Alignment(Transform Parent)
	{
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