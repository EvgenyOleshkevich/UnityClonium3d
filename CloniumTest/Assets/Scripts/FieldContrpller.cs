using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController
{
	public GameObject originalTeleport;
	public Color defaultColor;
	public GameObject main;

	public Field[] Fields { get; private set; }
	public Field CurrentField { get; private set; }



}