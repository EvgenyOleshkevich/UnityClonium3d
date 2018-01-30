using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesScript : MonoBehaviour {

	public int Master = -1;
	public Color MasterColor = Color.white;
	private int IndexNodes;
	public int IndexPlayers = 0;
	private int Width = 10;
	private int Size = 99;
	private GameObject Ball;
	private GameObject Player;
	public GameObject OldBall;
	public GameObject Main;
	private Queue<Transform> Order = new Queue<Transform>();
	private int SizeOrder = -1;
	private Transform Right = null;
	private Transform Left = null;
	private Transform Down = null;
	private Transform Up = null;
	public Transform Port;
	public int TeleportStatus = -1;


	void Start()
	{
		this.GetComponent<Renderer>().material.color = MasterColor;
		for (int i = 0; i < 100; ++i)
		{
			if (this.transform == this.transform.parent.GetChild(i).transform)
			{
				IndexNodes = i;
			}
		}
		if (IndexNodes + Width <= Size)
		{
			Down = this.transform.parent.GetChild(IndexNodes + Width).transform;
		}
		if (IndexNodes - Width >= 0)
		{
			Up = this.transform.parent.GetChild(IndexNodes - Width).transform;
		}
		if ((IndexNodes + 1) % Width != 0)
		{
			Right = this.transform.parent.GetChild(IndexNodes + 1).transform;
		}
		if (((IndexNodes - 1) % Width != Width - 1) && (IndexNodes - 1 >= 0))
		{
			Left = this.transform.parent.GetChild(IndexNodes - 1).transform;
		}
	}


	public void OnMouseEnter()
	{
		this.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, MasterColor, 0.8f);
	}

	public void OnMouseExit()
	{
		this.GetComponent<Renderer>().material.color = MasterColor;
	}

	public void OnMouseDown()
	{
		IndexPlayers = Main.GetComponent<Main>().IndexPlayers;
		Clicked(IndexNodes);
	}


	public void Clicked(int Index)
	{
		if ((Master == IndexPlayers) && (Main.GetComponent<Main>().Stop))
		{
			Main.GetComponent<Main>().Stop = false;
			CreateBall(Index);
		}
		return;
	}

	void CreateBall(int Index)
	{
		Ball = Instantiate(OldBall, this.transform);
		Alignment(this.transform);
		if (this.transform.childCount > 3)
		{
			StartCoroutine(DisclosureMain());
		}
		else
		{
			Main.GetComponent<Main>().UpdatePlayer();
		}
		return;
	}

	IEnumerator DisclosureMain()
	{
		Order.Enqueue(this.transform);
		SizeOrder = Order.Count;
		while (Order.Count != 0)
		{
			while (SizeOrder > 0)
			{
				Disclosure();
				--SizeOrder;
			}
			SizeOrder = Order.Count;
			yield return new WaitForSeconds(1.3f);
		}
		Main.GetComponent<Main>().UpdatePlayer();
	}

	void Disclosure()
	{
		Transform TempNode = Order.Dequeue();
		if (TempNode.childCount < 4)
		{
			//Debug.Log(TempNode.childCount);
			//Alignment(TempNode);
			return;
		}
		if (TempNode.GetComponent<NodesScript>().TeleportStatus != -1)
		{
			DisclosureWithTeleport(TempNode);
			return;
		}

		Player = Main.GetComponent<Main>().Players[IndexPlayers].gameObject;
		//Player = Main.GetComponent<Main>().transform.GetChild(IndexPlayers).gameObject;
		Player.GetComponent<PlayerScript>().CountNodes += 4;

		if (TempNode.childCount == 4)
		{
			TempNode.GetComponent<NodesScript>().Master = -1;
			TempNode.GetComponent<Renderer>().material.color = Color.white;
			TempNode.GetComponent<NodesScript>().MasterColor = Color.white;
			--Player.GetComponent<PlayerScript>().CountNodes;
		}

		Color MasterColor = Player.GetComponent<PlayerScript>().ColorPlayer;
		int ChildIndex = TempNode.childCount - 1;
		// она нужна чтобы раскатывать с последнего шара, чтобы не оставалось шаров в центре

		Ball = TempNode.GetChild(ChildIndex).gameObject;
		if (TempNode.GetComponent<NodesScript>().Down == null)
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
		}
		else
		{
			Transform TempDown = TempNode.GetComponent<NodesScript>().Down;
			Ball.transform.SetParent(TempDown);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempDown.GetComponent<Renderer>().material.color = MasterColor;
			TempDown.GetComponent<NodesScript>().MasterColor = MasterColor;
			int TempMaster = TempDown.GetComponent<NodesScript>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempDown.GetComponent<NodesScript>().Master = IndexPlayers;

			if (TempDown.childCount > 3)
			{
				Order.Enqueue(TempDown);
			}
		}

		////
		--ChildIndex;
		Ball = TempNode.GetChild(ChildIndex).gameObject;
		if (TempNode.GetComponent<NodesScript>().Up == null)
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
		}
		else
		{
			Transform TempUp = TempNode.GetComponent<NodesScript>().Up;
			Ball.transform.SetParent(TempUp);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempUp.GetComponent<Renderer>().material.color = MasterColor;
			TempUp.GetComponent<NodesScript>().MasterColor = MasterColor;
			int TempMaster = TempUp.GetComponent<NodesScript>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempUp.GetComponent<NodesScript>().Master = IndexPlayers;

			if (TempUp.childCount > 3)
			{
				Order.Enqueue(TempUp);
			}
		}

		////
		--ChildIndex;
		Ball = TempNode.GetChild(ChildIndex).gameObject;
		if (TempNode.GetComponent<NodesScript>().Right == null)
		{
			// дестрой очень медленная
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
		}
		else
		{
			Transform TempRight = TempNode.GetComponent<NodesScript>().Right;
			Ball.transform.SetParent(TempRight);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempRight.GetComponent<Renderer>().material.color = MasterColor;
			TempRight.GetComponent<NodesScript>().MasterColor = MasterColor;
			int TempMaster = TempRight.GetComponent<NodesScript>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempRight.GetComponent<NodesScript>().Master = IndexPlayers;

			if (TempRight.childCount > 3)
			{
				Order.Enqueue(TempRight);
			}
		}

		////
		--ChildIndex;
		Ball = TempNode.GetChild(ChildIndex).gameObject;
		if (TempNode.GetComponent<NodesScript>().Left == null)
		{
			Destroy(Ball);
			--Player.GetComponent<PlayerScript>().CountNodes;
		}
		else
		{
			Transform TempLeft = TempNode.GetComponent<NodesScript>().Left;
			Ball.transform.SetParent(TempLeft);
			Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
			TempLeft.GetComponent<Renderer>().material.color = MasterColor;
			TempLeft.GetComponent<NodesScript>().MasterColor = MasterColor;
			int TempMaster = TempLeft.GetComponent<NodesScript>().Master;
			if (TempMaster != -1)
			{
				--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
			}
			TempLeft.GetComponent<NodesScript>().Master = IndexPlayers;
			if (TempLeft.childCount > 3)
			{
				Order.Enqueue(TempLeft);
			}
		}
	}

	void DisclosureWithTeleport(Transform TempNode)
	{
		if (TempNode.childCount < 4)
		{
			//Debug.Log(TempNode.childCount);
			//Alignment(TempNode);
			return;
		}

		Player = Main.GetComponent<Main>().Players[IndexPlayers].gameObject;
		//Player = Main.GetComponent<Main>().transform.GetChild(IndexPlayers).gameObject;
		Player.GetComponent<PlayerScript>().CountNodes += 4;

		if (TempNode.childCount == 4)
		{
			TempNode.GetComponent<NodesScript>().Master = -1;
			TempNode.GetComponent<Renderer>().material.color = Color.white;
			TempNode.GetComponent<NodesScript>().MasterColor = Color.white;
			--Player.GetComponent<PlayerScript>().CountNodes;
		}

		Transform TempPotr = TempNode.GetComponent<NodesScript>().Port;
		Color MasterColor = Player.GetComponent<PlayerScript>().ColorPlayer;
		int ChildIndex = TempNode.childCount - 1;
		// она нужна чтобы раскатывать с последнего шара, чтобы не оставалось шаров в центре
		Ball = TempNode.GetChild(ChildIndex).gameObject;
		if (TempNode.GetComponent<NodesScript>().TeleportStatus % 4 == 0)
		{
			if (TempNode.GetComponent<NodesScript>().TeleportStatus == 0)
			{
				Transform TempTeleportNode = TempPotr.GetComponent<Teleport>().ExitTeleport.GetComponent<Teleport>().FrontNode;
				Ball.transform.SetParent(TempTeleportNode);
				Ball.transform.GetComponent<BAllScrit>().AlignmentTeleport(TempPotr);
				TempPotr.GetComponent<Teleport>().Transport(Ball);
				TempTeleportNode.GetComponent<Renderer>().material.color = MasterColor;
				TempTeleportNode.GetComponent<NodesScript>().MasterColor = MasterColor;
				int TempMaster = TempTeleportNode.GetComponent<NodesScript>().Master;
				if (TempMaster != -1)
				{
					--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
				}
				TempTeleportNode.GetComponent<NodesScript>().Master = IndexPlayers;

				if (TempTeleportNode.childCount > 3)
				{
					Order.Enqueue(TempTeleportNode);
				}
			}
			else
			{
				Destroy(Ball);
				--Player.GetComponent<PlayerScript>().CountNodes;
			}
		}
		else
		{
			if (TempNode.GetComponent<NodesScript>().Down == null)
			{
				Destroy(Ball);
				--Player.GetComponent<PlayerScript>().CountNodes;
			}
			else
			{
				Transform TempDown = TempNode.GetComponent<NodesScript>().Down;
				Ball.transform.SetParent(TempDown);
				Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
				TempDown.GetComponent<Renderer>().material.color = MasterColor;
				TempDown.GetComponent<NodesScript>().MasterColor = MasterColor;
				int TempMaster = TempDown.GetComponent<NodesScript>().Master;
				if (TempMaster != -1)
				{
					--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
				}
				TempDown.GetComponent<NodesScript>().Master = IndexPlayers;

				if (TempDown.childCount > 3)
				{
					Order.Enqueue(TempDown);
				}
			}
		}

		////
		--ChildIndex;
		Ball = TempNode.GetChild(ChildIndex).gameObject;
		if (TempNode.GetComponent<NodesScript>().TeleportStatus % 4 == 1)
		{
			if (TempNode.GetComponent<NodesScript>().TeleportStatus == 1)
			{
				Transform TempTeleportNode = TempPotr.GetComponent<Teleport>().ExitTeleport.GetComponent<Teleport>().FrontNode;
				Ball.transform.SetParent(TempTeleportNode);
				Ball.transform.GetComponent<BAllScrit>().AlignmentTeleport(TempPotr);
				TempPotr.GetComponent<Teleport>().Transport(Ball);
				TempTeleportNode.GetComponent<Renderer>().material.color = MasterColor;
				TempTeleportNode.GetComponent<NodesScript>().MasterColor = MasterColor;
				int TempMaster = TempTeleportNode.GetComponent<NodesScript>().Master;
				if (TempMaster != -1)
				{
					--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
				}
				TempTeleportNode.GetComponent<NodesScript>().Master = IndexPlayers;

				if (TempTeleportNode.childCount > 3)
				{
					Order.Enqueue(TempTeleportNode);
				}
			}
			else
			{
				Destroy(Ball);
				--Player.GetComponent<PlayerScript>().CountNodes;
			}
		}
		else
		{
			if (TempNode.GetComponent<NodesScript>().Up == null)
			{
				Destroy(Ball);
				--Player.GetComponent<PlayerScript>().CountNodes;
			}
			else
			{
				Transform TempUp = TempNode.GetComponent<NodesScript>().Up;
				Ball.transform.SetParent(TempUp);
				Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
				TempUp.GetComponent<Renderer>().material.color = MasterColor;
				TempUp.GetComponent<NodesScript>().MasterColor = MasterColor;
				int TempMaster = TempUp.GetComponent<NodesScript>().Master;
				if (TempMaster != -1)
				{
					--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
				}
				TempUp.GetComponent<NodesScript>().Master = IndexPlayers;

				if (TempUp.childCount > 3)
				{
					Order.Enqueue(TempUp);
				}
			}
		}

		////
		--ChildIndex;
		Ball = TempNode.GetChild(ChildIndex).gameObject;
		if (TempNode.GetComponent<NodesScript>().TeleportStatus % 4 == 2)
		{
			if (TempNode.GetComponent<NodesScript>().TeleportStatus == 2)
			{
				Transform TempTeleportNode = TempPotr.GetComponent<Teleport>().ExitTeleport.GetComponent<Teleport>().FrontNode;
				Ball.transform.SetParent(TempTeleportNode);
				Ball.transform.GetComponent<BAllScrit>().AlignmentTeleport(TempPotr);
				TempPotr.GetComponent<Teleport>().Transport(Ball);
				TempTeleportNode.GetComponent<Renderer>().material.color = MasterColor;
				TempTeleportNode.GetComponent<NodesScript>().MasterColor = MasterColor;
				int TempMaster = TempTeleportNode.GetComponent<NodesScript>().Master;
				if (TempMaster != -1)
				{
					--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
				}
				TempTeleportNode.GetComponent<NodesScript>().Master = IndexPlayers;

				if (TempTeleportNode.childCount > 3)
				{
					Order.Enqueue(TempTeleportNode);
				}
			}
			else
			{
				Destroy(Ball);
				--Player.GetComponent<PlayerScript>().CountNodes;
			}
		}
		else
		{
			if (TempNode.GetComponent<NodesScript>().Right == null)
			{
				// дестрой очень медленная
				Destroy(Ball);
				--Player.GetComponent<PlayerScript>().CountNodes;
			}
			else
			{
				Transform TempRight = TempNode.GetComponent<NodesScript>().Right;
				Ball.transform.SetParent(TempRight);
				Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
				TempRight.GetComponent<Renderer>().material.color = MasterColor;
				TempRight.GetComponent<NodesScript>().MasterColor = MasterColor;
				int TempMaster = TempRight.GetComponent<NodesScript>().Master;
				if (TempMaster != -1)
				{
					--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
				}
				TempRight.GetComponent<NodesScript>().Master = IndexPlayers;

				if (TempRight.childCount > 3)
				{
					Order.Enqueue(TempRight);
				}
			}
		}

		////
		--ChildIndex;
		Ball = TempNode.GetChild(ChildIndex).gameObject;
		if (TempNode.GetComponent<NodesScript>().TeleportStatus % 4 == 3)
		{
			if (TempNode.GetComponent<NodesScript>().TeleportStatus == 3)
			{
				Transform TempTeleportNode = TempPotr.GetComponent<Teleport>().ExitTeleport.GetComponent<Teleport>().FrontNode;
				Ball.transform.SetParent(TempTeleportNode);
				Ball.transform.GetComponent<BAllScrit>().AlignmentTeleport(TempPotr);
				TempPotr.GetComponent<Teleport>().Transport(Ball);
				TempTeleportNode.GetComponent<Renderer>().material.color = MasterColor;
				TempTeleportNode.GetComponent<NodesScript>().MasterColor = MasterColor;
				int TempMaster = TempTeleportNode.GetComponent<NodesScript>().Master;
				if (TempMaster != -1)
				{
					--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
				}
				TempTeleportNode.GetComponent<NodesScript>().Master = IndexPlayers;

				if (TempTeleportNode.childCount > 3)
				{
					Order.Enqueue(TempTeleportNode);
				}
			}
			else
			{
				Destroy(Ball);
				--Player.GetComponent<PlayerScript>().CountNodes;
			}
		}
		else
		{
			if (TempNode.GetComponent<NodesScript>().Left == null)
			{
				Destroy(Ball);
				--Player.GetComponent<PlayerScript>().CountNodes;
			}
			else
			{
				Transform TempLeft = TempNode.GetComponent<NodesScript>().Left;
				Ball.transform.SetParent(TempLeft);
				Ball.transform.GetComponent<BAllScrit>().AlignmentBall();
				TempLeft.GetComponent<Renderer>().material.color = MasterColor;
				TempLeft.GetComponent<NodesScript>().MasterColor = MasterColor;
				int TempMaster = TempLeft.GetComponent<NodesScript>().Master;
				if (TempMaster != -1)
				{
					--Main.GetComponent<Main>().Players[TempMaster].GetComponent<PlayerScript>().CountNodes;
				}
				TempLeft.GetComponent<NodesScript>().Master = IndexPlayers;
				if (TempLeft.childCount > 3)
				{
					Order.Enqueue(TempLeft);
				}
			}
		}
	}

	void Alignment(Transform Parent)
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
