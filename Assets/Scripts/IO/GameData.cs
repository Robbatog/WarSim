using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
	public JsonUnitBlueprint[] unitData;

	public UnitBlueprint[] GetUnitData()
	{
		return System.Array.ConvertAll<JsonUnitBlueprint, UnitBlueprint>(unitData, (p => new UnitBlueprint(p)));
	}
}
