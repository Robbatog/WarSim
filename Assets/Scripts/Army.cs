using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army {

	public Unit[,] units;
	public byte[] spriteBytesPNG;

	public Army(ArmySave armySave)
	{
		units = new Unit[GS.armyRowAmount, GS.armyColumnAmount];
		var dataController = GameObject.Find("DataController").GetComponent<DataController>();
		for (int i = 0; i < units.GetLength(0); i++)
		{
			for (int j = 0; j < units.GetLength(1); j++)
			{
				var name = armySave.unitNames[i, j];
				if (name != "")
				{
					units[i, j] = new Unit(dataController.GetUnitData(name));
				}
			}
		}
	}
}
