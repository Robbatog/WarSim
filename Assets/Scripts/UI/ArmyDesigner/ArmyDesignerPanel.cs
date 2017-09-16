using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDesignerPanel : MonoBehaviour {

	// references to child objects
	GameObject armyTileMap;
	GameObject armyAddUnitList;
	GameObject armyNameField;
	GameObject storedArmyScrollView;
	GameObject closeButton;
	GameObject clearButton;
	GameObject saveButton;
	GameObject loadButton;

	// callbacks to outside world that this ArmyDesignerPanel runs when buttons are pressed
	private Func<string, ArmySave> getArmyCB;
	private Func<ArmySave, bool> addArmyCB;
	private Func<string, bool> existsArmyCB;
	private Func<string, bool> deleteArmyCB;
	private Func<UnitBlueprint[]> getAvailableUnitsCB;

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

	private void OnDestroy()
	{
		closeButton.GetComponent<Button>().onClick.RemoveAllListeners();
		clearButton.GetComponent<Button>().onClick.RemoveAllListeners();
		saveButton.GetComponent<Button>().onClick.RemoveAllListeners();
		loadButton.GetComponent<Button>().onClick.RemoveAllListeners();
	}

	public void Init(Func<string, ArmySave> getArmy, Func<ArmySave, bool> addArmy, Func<string, bool> existsArmy, Func<string, bool> deleteArmy, Func<UnitBlueprint[]> getAvailableUnits)
	{
		this.getArmyCB = getArmy;
		this.addArmyCB = addArmy;
		this.existsArmyCB = existsArmy;
		this.deleteArmyCB = deleteArmy;
		this.getAvailableUnitsCB = getAvailableUnits;

		// Fetch child GUI elements
		armyTileMap = this.transform.Find("ArmyTileMap").gameObject;
		armyAddUnitList = this.transform
			.Find("AddUnitScrollView")
			.Find("Viewport")
			.Find("AddUnitList").gameObject;
		armyNameField = this.transform.Find("ArmyNameField").gameObject;
		storedArmyScrollView = this.transform.Find("StoredArmyScrollView").gameObject;
		closeButton = this.transform.Find("CloseButton").gameObject;
		clearButton = this.transform.Find("ClearButton").gameObject;
		saveButton = this.transform.Find("SaveButton").gameObject;
		loadButton = this.transform.Find("LoadButton").gameObject;

		// Bind button functions
		closeButton.GetComponent<Button>().onClick.AddListener(() => { Destroy(this.gameObject); });
		clearButton.GetComponent<Button>().onClick.AddListener(() => { ClearArmy(); });
		saveButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(SaveArmy()); });
		loadButton.GetComponent<Button>().onClick.AddListener(() => { /*TODO: remove this superfluous button*/ });

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

	int UnitNameToId(string name)
	{
		if (name == "")
		{
			return -1;
		}
		for (int i = 0; i < unitBlueprints.Length; i++)
		{
			if (unitBlueprints[i].name == name)
			{
				return i;
			}
		}
		Debug.LogError("Could not find id of unit with name: " + name);
		return -1;
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

	public void AddUnit(int id, int row, int col)
	{
		if (id < 0 || id >= unitBlueprints.Length)
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

	public void LoadArmy(string[,] unitNames)
	{
		int rows = unitNames.GetLength(0);
		int columns = unitNames.GetLength(1);
		if (unitNames == null)
		{
			Debug.LogError("Cannot load army! Array is null");
			return;
		}
		if (unitNames == null || rows != rowAmount || columns != columnAmount)
		{
			Debug.LogError("Cannot load army! Array dimension : [" + rows + "," + columns + "]. Expected: [" + rowAmount + "," + columnAmount + "]");
			return;
		}

		ClearArmy();
		for (int row = 0; row < rowAmount; row++)
		{
			for (int col = 0; col < columnAmount; col++)
			{
				AddUnit(UnitNameToId(unitNames[row, col]), row, col);
			}
		}
	}

	public void LoadArmy(string armyName)
	{
		if (!existsArmyCB(armyName))
		{
			Debug.Log("LoadArmy: Could not find army with name \"" + armyName + "\"");
		}

		string[,] unitNames = new string[tileUnits.GetLength(0), tileUnits.GetLength(1)];
		var e = getArmyCB(armyName).unitNames.GetEnumerator();
		for (int i = 0; i < tileUnits.GetLength(0); i++)
		{
			for (int j = 0; j < tileUnits.GetLength(1); j++)
			{
				e.MoveNext();
				unitNames[i, j] = e.Current;
			}
		}
		LoadArmy(unitNames);
		Debug.Log("Army loaded");
	}

	public void DeleteArmy(string armyName)
	{
		if (!existsArmyCB(armyName))
		{
			Debug.Log("DeleteArmy: Could not find army with name \"" + armyName + "\"");
		}

		GameObject armyButton = null;
		if (!storedArmyButtons.TryGetValue(armyName, out armyButton))
		{
			Debug.Log("DeleteArmy: Could not find army with name \"" + armyName + "\"");
		}

		deleteArmyCB(armyName);
		storedArmyButtons.Remove(armyName);
		GameObject.Destroy(armyButton);

		Debug.Log("Army deleted");
	}

	public IEnumerator SaveArmy()
	{
		yield return new WaitForEndOfFrame();
		InputField nameField = armyNameField.GetComponent<InputField>();
		if (nameField == null || nameField.text == "" || existsArmyCB(nameField.text))
		{
			yield break;
		}

		// make army icon
		var armyTileRect = armyTileMap.GetComponent<RectTransform>();
		Vector3[] corners = new Vector3[4];
		armyTileRect.GetWorldCorners(corners);

		var width = corners[2].x - corners[0].x;
		var height = corners[2].y - corners[0].y;
		var startX = corners[0].x;
		var startY = corners[0].y;

		var rect = new Rect(startX, startY, width, height);
		var tex = new Texture2D(System.Convert.ToInt32(width), System.Convert.ToInt32(height), TextureFormat.RGB24, false);
		tex.ReadPixels(rect, 0, 0);
		tex.Apply();

		//store army data
		ArmySave newArmy = new ArmySave()
		{
			armyName = nameField.text,
			spriteBytesPNG = tex.EncodeToPNG()
		};
		for (int i = 0; i < tileUnits.GetLength(0); i++)
		{
			for (int j = 0; j < tileUnits.GetLength(1); j++)
			{
				if (tileUnits[i, j] != null)
				{
					newArmy.unitNames.Add(tileUnits[i, j].name);
				}
				else
				{
					newArmy.unitNames.Add("");
				}
			}
		}

		//make a button
		MakeArmyButton(newArmy);

		//return callback
		addArmyCB(newArmy);
	}

	public void MakeArmyButton(ArmySave army)
	{
		//var armyTileMap = armyDesignerPanel.transform.Find("ArmyTileMap");
		var storedArmyList = storedArmyScrollView.transform.Find("Viewport").Find("StoredArmyList");

		GameObject newButton = Instantiate(storedArmyButton, storedArmyList);
		storedArmyButtons.Add(army.armyName, newButton);

		newButton.transform.Find("TextMsh").GetComponent<TMPro.TextMeshProUGUI>().text = army.armyName;

		string callbackString = army.armyName; // avoid capturing the wrong string (armyName) in lambda closure - add a local to capture instead
		newButton.GetComponent<Button>().onClick.AddListener(() => { LoadArmy(callbackString); });

		newButton.transform.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(() => { DeleteArmy(callbackString); });

		// set sprite
		var tex = new Texture2D(1, 1);
		tex.LoadImage(army.spriteBytesPNG);
		tex.Apply();

		Sprite s = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().sprite = s;
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().type = Image.Type.Simple;
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().preserveAspect = true;
	}
}
