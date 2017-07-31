using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDesigner : MonoBehaviour {

	GameObject armyDesignerPanel;
	GameObject armyTileMap;
	GameObject armyAddUnitList;
	UnitBlueprint[] unitBlueprints;

	// prefabs to instantiate when adding a unit to a tile
	public GameObject armySlot;
	public GameObject addUnitSlot;
	public GameObject armyUnit;

	int columnAmount;
	int rowAmount;
	public Unit[,] tileUnits;
	public GameObject[,] tiles;

	public Unit[] addListTileUnits;
	public GameObject[] addListTiles;

	private void Start()
	{
		// Fetch unit database
		unitBlueprints = GameObject.Find("DataController").GetComponent<DataController>().GetUnitData();

		// Fetch GUI elements
		armyDesignerPanel = GameObject.Find("ArmyDesignerPanel");
		armyTileMap = armyDesignerPanel.transform.Find("ArmyTileMap").gameObject;
		armyAddUnitList = armyDesignerPanel.transform
			.Find("AddUnitScrollView")
			.Find("Viewport")
			.Find("AddUnitList").gameObject;

		// Setup ArmyTileMap
		columnAmount = 3;
		rowAmount = 5;
		tileUnits = new Unit[rowAmount, columnAmount];
		tiles = new GameObject[rowAmount, columnAmount];

		for (int row = 0; row < rowAmount; row++)
		{
			for (int col = 0; col < columnAmount; col++)
			{
				tiles[row, col] = Instantiate(armySlot);
				tiles[row, col].GetComponent<ArmySlot>().setRowCol(row, col);
				tiles[row, col].transform.SetParent(armyTileMap.transform, false);
			}
		}

		// Setup AddUnitList
		addListTileUnits = new Unit[unitBlueprints.Length];
		addListTiles = new GameObject[unitBlueprints.Length];

		for (int row = 0; row < unitBlueprints.Length; row++)
		{
			addListTiles[row] = Instantiate(addUnitSlot);
			addListTiles[row].GetComponent<AddUnitSlot>().setRow(row);
			addListTiles[row].transform.SetParent(armyAddUnitList.transform, false);

			AddUnitToAddList(row, row);
		}
	}

	public void AddUnit(int id, int row, int col)
	{
		if(id < 0 || id >= unitBlueprints.Length)
		{
			return;
		}

		RemoveUnit(row, col);

		Unit newUnit = new Unit(unitBlueprints[id]);
		tileUnits[row, col] = newUnit;

		GameObject unitObj = Instantiate(armyUnit);
		DraggableUnit unitData = unitObj.GetComponent<DraggableUnit>();
		unitData.unit = newUnit;

		unitObj.transform.SetParent(tiles[row, col].transform, false);
		unitObj.GetComponent<Image>().sprite = newUnit.sprite;
		unitObj.transform.position = unitObj.transform.parent.position;
	}

	public void RemoveUnit(int row, int col)
	{
		for (int i = 0; i < tiles[row, col].transform.childCount; i++)
		{
			Destroy(tiles[row, col].transform.GetChild(i).gameObject);
		}
		tileUnits[row, col] = null;
	}

	public void ClearArmy()
	{
		for (int i = 0; i < rowAmount; i++)
		{
			for (int j = 0; j < columnAmount; j++)
			{
				RemoveUnit(i, j);
			}
		}
	}

	public void AddUnitToAddList(int id, int row)
	{
		Unit newUnit = new Unit(unitBlueprints[row]);
		addListTileUnits[row] = newUnit;

		GameObject unitObj = Instantiate(armyUnit);
		DraggableUnit unitData = unitObj.GetComponent<DraggableUnit>();
		unitData.unit = newUnit;

		unitObj.transform.SetParent(addListTiles[row].transform, false);
		unitObj.GetComponent<Image>().sprite = newUnit.sprite;
		unitObj.transform.position = unitObj.transform.parent.position;
	}

	int UnitNameToId(string name)
	{
		if(name == "")
		{
			return -1;
		}
		for (int i = 0; i < unitBlueprints.Length; i++)
		{
			if(unitBlueprints[i].name == name)
			{
				return i;
			}
		}
		Debug.LogError("Could not find id of unit with name: " + name);
		return -1;
	}

	public void LoadArmy(string[,] unitNames)
	{
		int rows = unitNames.GetLength(0);
		int columns = unitNames.GetLength(1);
		if(unitNames == null)
		{
			Debug.LogError("Cannot load army! Array is null");
			return;
		}
		if (unitNames == null || rows != rowAmount || columns != columnAmount)
		{
			Debug.LogError("Cannot load army! Array dimension : [" + rows + "," + columns + "]. Expected: [" + rowAmount + "," + columnAmount + "]");
			return;
		}

		for (int row = 0; row < rowAmount; row++)
		{
			for (int col = 0; col < columnAmount; col++)
			{
				AddUnit(UnitNameToId(unitNames[row, col]), row, col);
			}
		}
	}
}
