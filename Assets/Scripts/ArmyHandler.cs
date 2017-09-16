using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the designed armies for a player, and the available units
/// </summary>
public class ArmyHandler : MonoBehaviour {

	// units available to use in armies
	UnitBlueprint[] availableUnitBlueprints;

	// settings and members for this object
	Dictionary<string, ArmySave> storedArmies;

	// Use this for initialization
	void Start () {
		availableUnitBlueprints = null;
		storedArmies = new Dictionary<string, ArmySave>();

		// temporarily, use all units as available units
		// later, the players tech etc should determine this
		availableUnitBlueprints = GameObject.Find("DataController").GetComponent<DataController>().GetUnitData();
	}
	
	void UpdateAvailableUnits(UnitBlueprint[] availableUnits)
	{
		throw new NotImplementedException();
	}

	public UnitBlueprint[] GetAvailableUnits()
	{
		return availableUnitBlueprints;
	}

	public ArmySave GetArmy(string armyName)
	{
		ArmySave army = null;
		if (!storedArmies.TryGetValue(armyName, out army))
		{
			Debug.Log("GetArmy: Could not find army with name \"" + armyName + "\"");
		}
		return army;
	}

	public Dictionary<string, ArmySave> GetArmies()
	{
		return storedArmies;
	}

	public bool AddArmy(ArmySave newArmy)
	{
		storedArmies.Add(newArmy.armyName, newArmy);

		Debug.Log("Army \"" + newArmy.armyName + "\" saved");
		return true;
	}

	public bool DeleteArmy(string armyName)
	{
		ArmySave army = null;
		if (!storedArmies.TryGetValue(armyName, out army))
		{
			Debug.Log("DeleteArmy: Could not find army with name \"" + armyName + "\"");
		}

		storedArmies.Remove(armyName);

		Debug.Log("Army \"" + armyName + "\" deleted");
		return true;
	}

	public bool ExistsArmy(string armyName)
	{
		ArmySave army = null;
		return storedArmies.TryGetValue(armyName, out army);
	}
}
