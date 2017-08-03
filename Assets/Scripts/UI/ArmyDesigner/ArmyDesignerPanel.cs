using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDesignerPanel : MonoBehaviour {

	// references to child objects
	GameObject armyTileMap;
	GameObject armyAddUnitList;

	// callbacks to outside world that this ArmyDesignerPanel runs when buttons are pressed
	private Action<ArmySave> addArmy;
	private Action<string> deleteArmy;
	private Func<UnitBlueprint[]> getAvailableUnits;

	// prefabs to instantiate when adding a unit to a tile
	public GameObject armySlot;
	public GameObject addUnitSlot;
	public GameObject armyUnit;
	public GameObject storedArmyButton;

	// settings and members for this object
	int columnAmount;
	int rowAmount;
	public Unit[,] tileUnits;
	public GameObject[,] tiles;
	public Unit[] addListTileUnits;
	public GameObject[] addListTiles;
	UnitBlueprint[] unitBlueprints;
	Dictionary<string, GameObject> storedArmyButtons;

	public void Init(Action<ArmySave> addArmy, Action<string> deleteArmy, Func<UnitBlueprint[]> getAvailableUnits)
	{
		this.addArmy = addArmy;
		this.deleteArmy = deleteArmy;
		this.getAvailableUnits = getAvailableUnits;

		// Fetch child GUI elements
		armyTileMap = this.transform.Find("ArmyTileMap").gameObject;
		armyAddUnitList = this.transform
			.Find("AddUnitScrollView")
			.Find("Viewport")
			.Find("AddUnitList").gameObject;

		// Setup ArmyTileMap
		columnAmount = 3;
		rowAmount = 5;
		tileUnits = new Unit[rowAmount, columnAmount];
		tiles = new GameObject[rowAmount, columnAmount];
		storedArmyButtons = new Dictionary<string, GameObject>();

		for (int row = 0; row < rowAmount; row++)
		{
			for (int col = 0; col < columnAmount; col++)
			{
				tiles[row, col] = Instantiate(armySlot);
				tiles[row, col].transform.SetParent(armyTileMap.transform, false);
				ArmySlot slot = tiles[row, col].GetComponent<ArmySlot>();
				slot.SetRowCol(row, col);
				slot.armyDesignerPanel = this;
			}
		}

		// Setup AddUnitList
		unitBlueprints = getAvailableUnits();
		addListTileUnits = new Unit[unitBlueprints.Length];
		addListTiles = new GameObject[unitBlueprints.Length];

		for (int row = 0; row < unitBlueprints.Length; row++)
		{
			addListTiles[row] = Instantiate(addUnitSlot);
			addListTiles[row].transform.SetParent(armyAddUnitList.transform, false);
			AddUnitSlot slot = addListTiles[row].GetComponent<AddUnitSlot>();
			slot.SetRow(row);
			slot.armyDesignerPanel = this;

			AddUnitToAddList(row, row);
		}
	}

	public void AddUnitToAddList(int id, int row)
	{
		Unit newUnit = new Unit(unitBlueprints[id]);
		addListTileUnits[row] = newUnit;

		GameObject unitObj = Instantiate(armyUnit);
		DraggableUnit unitData = unitObj.GetComponent<DraggableUnit>();
		unitData.unit = newUnit;
		unitData.armyDesignerPanel = this.gameObject;

		unitObj.transform.SetParent(addListTiles[row].transform, false);
		unitObj.GetComponent<Image>().sprite = newUnit.sprite;
		unitObj.transform.position = unitObj.transform.parent.position;
	}

}
