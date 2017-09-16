using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmySave
{
	public string armyName;
	public List<string> unitNames;
	public byte[] spriteBytesPNG;

	public ArmySave()
	{
		unitNames = new List<string>();
	}
}
