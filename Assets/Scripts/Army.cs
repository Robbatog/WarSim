using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army {

	public string name;
	public Unit[,] units;
	public byte[] spriteBytesPNG;

	public Army(string armyName, ArmySave armySave)
	{
		this.name = armyName;
		units = new Unit[GS.ArmyRowAmount, GS.ArmyColumnAmount];
		var dataController = GameObject.Find("DataController").GetComponent<DataController>();
		for (int i = 0; i < units.GetLength(0); i++)
		{
			for (int j = 0; j < units.GetLength(1); j++)
			{
				var uName = armySave.unitNames[i, j];
				if (uName != "")
				{
					units[i, j] = new Unit(dataController.GetUnitData(uName));
				}
			}
		}
	}
}
