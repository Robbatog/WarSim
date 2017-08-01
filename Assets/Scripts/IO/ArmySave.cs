using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmySave
{
	public string armyName;
	public List<string> unitNames;

	public ArmySave()
	{
		unitNames = new List<string>();
	}
}
