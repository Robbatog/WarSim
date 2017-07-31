using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmySave
{
	public List<string> unitNames;

	public ArmySave()
	{
		unitNames = new List<string>();
	}
}
