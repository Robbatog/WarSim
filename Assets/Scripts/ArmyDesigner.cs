using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDesigner : MonoBehaviour {
	
	// references to other objects
	GameObject armyDesignerPanel;
	GameObject armyTileMap;
	GameObject armyAddUnitList;
	UnitBlueprint[] unitBlueprints;

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
	Dictionary<string, ArmySave> storedArmies;
	Dictionary<string, GameObject> storedArmyButtons;

	//private void Start()
	//{
	//	// Fetch unit database
	//	unitBlueprints = GameObject.Find("DataController").GetComponent<DataController>().GetUnitData();

	//	// Fetch GUI elements
	//	armyDesignerPanel = GameObject.Find("ArmyDesignerPanel");
	//	armyTileMap = armyDesignerPanel.transform.Find("ArmyTileMap").gameObject;
	//	armyAddUnitList = armyDesignerPanel.transform
	//		.Find("AddUnitScrollView")
	//		.Find("Viewport")
	//		.Find("AddUnitList").gameObject;

	//	// Setup ArmyTileMap
	//	columnAmount = 3;
	//	rowAmount = 5;
	//	tileUnits = new Unit[rowAmount, columnAmount];
	//	tiles = new GameObject[rowAmount, columnAmount];
	//	storedArmies = new Dictionary<string, ArmySave>();
	//	storedArmyButtons = new Dictionary<string, GameObject>();

	//	for (int row = 0; row < rowAmount; row++)
	//	{
	//		for (int col = 0; col < columnAmount; col++)
	//		{
	//			tiles[row, col] = Instantiate(armySlot);
	//			tiles[row, col].GetComponent<ArmySlot>().setRowCol(row, col);
	//			tiles[row, col].transform.SetParent(armyTileMap.transform, false);
	//		}
	//	}

	//	// Setup AddUnitList
	//	addListTileUnits = new Unit[unitBlueprints.Length];
	//	addListTiles = new GameObject[unitBlueprints.Length];

	//	for (int row = 0; row < unitBlueprints.Length; row++)
	//	{
	//		addListTiles[row] = Instantiate(addUnitSlot);
	//		addListTiles[row].GetComponent<AddUnitSlot>().SetRow(row);
	//		addListTiles[row].transform.SetParent(armyAddUnitList.transform, false);

	//		AddUnitToAddList(row, row);
	//	}
	//}

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
		ArmySave army = null;
		if(!storedArmies.TryGetValue(armyName, out army))
		{
			Debug.Log("LoadArmy: Could not find army with name \"" + armyName + "\"");
		}

		string[,] unitNames = new string[tileUnits.GetLength(0), tileUnits.GetLength(1)];
		var e = army.unitNames.GetEnumerator();
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
		ArmySave army = null;
		if (!storedArmies.TryGetValue(armyName, out army))
		{
			Debug.Log("DeleteArmy: Could not find army with name \"" + armyName + "\"");
		}

		GameObject armyButton = null;
		if (!storedArmyButtons.TryGetValue(armyName, out armyButton))
		{
			Debug.Log("DeleteArmy: Could not find army with name \"" + armyName + "\"");
		}

		storedArmies.Remove(armyName);
		storedArmyButtons.Remove(armyName);
		GameObject.Destroy(armyButton);

		Debug.Log("Army deleted");
	}

	public void SaveArmy(GameObject nameFieldObj)
	{
		InputField nameField = nameFieldObj.GetComponent<InputField>();

		if (nameField == null || nameField.text == "")
		{
			return;
		}

		if (storedArmies.ContainsKey(nameField.text))
		{
			Debug.Log("Army \"" + nameField.text + "\" already exists");
			return;
		}

		ArmySave newArmy = new ArmySave()
		{
			armyName = nameField.text
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
		//string dataAsJson = JsonUtility.ToJson(newArmy);
		//File.WriteAllText(filePath, dataAsJson);

		storedArmies.Add(newArmy.armyName, newArmy);

		// make stored army button
		StartCoroutine(MakeStoredArmyButton(nameField.text));

		Debug.Log("Army \"" + nameField.text + "\" saved");
	}

	public IEnumerator MakeStoredArmyButton(string armyName)
	{
		yield return new WaitForEndOfFrame(); // it must be a coroutine 

		var armyTileMap = armyDesignerPanel.transform.Find("ArmyTileMap");
		var storedArmyList = armyDesignerPanel.transform.Find("StoredArmyScrollView").Find("Viewport").Find("StoredArmyList");
		var armyTileRect = armyTileMap.GetComponent<RectTransform>();

		GameObject newButton = Instantiate(storedArmyButton, storedArmyList);
		storedArmyButtons.Add(armyName, newButton);

		newButton.transform.Find("TextMsh").GetComponent<TMPro.TextMeshProUGUI>().text = armyName;

		string callbackString = armyName; // avoid capturing the wrong string (armyName) in lambda closure - add a local to capture instead
		newButton.GetComponent<Button>().onClick.AddListener(() => { LoadArmy(callbackString); });

		newButton.transform.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(() => { DeleteArmy(callbackString); });

		// set sprite
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

		Sprite s = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().sprite = s;
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().type = Image.Type.Simple;
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().preserveAspect = true;

		//var bytes = tex.EncodeToPNG();
		//string fileName = armyName + ".png";
		//string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
		//File.WriteAllBytes(filePath, bytes);

		Debug.Log("Saved army's button added");
	}
}
