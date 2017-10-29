using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class DataController : MonoBehaviour {

	private UnitBlueprint[] unitBlueprints;
	private Dictionary<string, UnitBlueprint> unitBlueprintMap;
	private string gameDataFileName = "data.json";

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);

		LoadGameData();
	}
	
	private void LoadGameData()
	{
		string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

		if (File.Exists(filePath))
		{
			string dataAsJson = File.ReadAllText(filePath);

			GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
			unitBlueprints = loadedData.GetUnitData();
			unitBlueprintMap = new Dictionary<string, UnitBlueprint>();
			foreach (var unit in unitBlueprints)
			{
				unitBlueprintMap.Add(unit.name, unit);
			}
		}
		else
		{
			Debug.LogError("Cannot load game data");
		}
	}

	public UnitBlueprint[] GetUnitData()
	{
		return unitBlueprints;
	}

	public UnitBlueprint GetUnitData(string unitName)
	{
		return unitBlueprintMap[unitName];
	}
}
