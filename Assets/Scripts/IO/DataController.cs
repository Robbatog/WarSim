using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataController : MonoBehaviour {

	private UnitBlueprint[] unitBlueprints;
	private string gameDataFileName = "data.json";

	//private ArmyDesigner armyDesigner;
	//private GameObject armyDesignerPanel;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);

		LoadGameData();

		//armyDesigner = GameObject.Find("ArmyDesigner").GetComponent<ArmyDesigner>();
		//armyDesignerPanel = GameObject.Find("ArmyDesignerPanel");
		// Set initial scene here
		//initScene();
	}
	
	private void LoadGameData()
	{
		string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

		if (File.Exists(filePath))
		{
			string dataAsJson = File.ReadAllText(filePath);

			GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
			unitBlueprints = loadedData.GetUnitData();
		}
		else
		{
			Debug.LogError("Cannot load game data");
		}
	}

	//public void SaveArmyData(GameObject nameFieldObj)
	//{
	//	InputField nameField = nameFieldObj.GetComponent<InputField>();

	//	if (nameField == null || nameField.text == "")
	//	{
	//		return;
	//	}

	//	string fileName = nameField.text + ".army";
	//	string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

	//	ArmySave newArmy = new ArmySave()
	//	{
	//		armyName = nameField.text
	//	};
	//	for (int i = 0; i < armyDesigner.tileUnits.GetLength(0); i++)
	//	{
	//		for (int j = 0; j < armyDesigner.tileUnits.GetLength(1); j++)
	//		{
	//			if (armyDesigner.tileUnits[i, j] != null)
	//			{
	//				newArmy.unitNames.Add(armyDesigner.tileUnits[i, j].name);
	//			}
	//			else
	//			{
	//				newArmy.unitNames.Add("");
	//			}
	//		}
	//	}
	//	string dataAsJson = JsonUtility.ToJson(newArmy);

	//	File.WriteAllText(filePath, dataAsJson);

	//	// make stored army button
	//	StartCoroutine(MakeStoredArmyButton(nameField.text));

	//	Debug.Log("Army saved");
	//}

	//public IEnumerator MakeStoredArmyButton(string armyName)
	//{
	//	yield return new WaitForEndOfFrame(); // it must be a coroutine 

	//	var armyTileMap = armyDesignerPanel.transform.Find("ArmyTileMap");
	//	var storedArmyList = armyDesignerPanel.transform.Find("StoredArmyScrollView").Find("Viewport").Find("StoredArmyList");
	//	var armyTileRect = armyTileMap.GetComponent<RectTransform>();

	//	GameObject newButton = Instantiate(storedArmyButton, storedArmyList);
	//	newButton.transform.Find("Text").GetComponent<Text>().text = armyName;

	//	// set sprite
	//	Vector3[] corners = new Vector3[4];
	//	armyTileRect.GetWorldCorners(corners);

	//	var width = corners[2].x - corners[0].x;
	//	var height = corners[2].y - corners[0].y;
	//	var startX = corners[0].x;
	//	var startY = corners[0].y;

	//	//var rect = RectTransformUtility.PixelAdjustRect(armyTileRect, canvas);
	//	var rect = new Rect(startX, startY, width, height);
	//	var tex = new Texture2D(System.Convert.ToInt32(width), System.Convert.ToInt32(height), TextureFormat.RGB24, false);
	//	tex.ReadPixels(rect, 0, 0);
	//	tex.Apply();

	//	Sprite s = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
	//	newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().sprite = s;
	//	newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().type = Image.Type.Simple;
	//	newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().preserveAspect = true;

	//	var bytes = tex.EncodeToPNG();

	//	string fileName = armyName + ".png";
	//	string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
	//	File.WriteAllBytes(filePath, bytes);

	//	Debug.Log("Saved army's button added");
	//}

	//public void LoadArmyData(GameObject nameFieldObj)
	//{
	//	InputField nameField = nameFieldObj.GetComponent<InputField>();

	//	if (nameField == null || nameField.text == "")
	//	{
	//		return;
	//	}

	//	string fileName = nameField.text + ".army";
	//	string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

	//	ArmySave newArmy;
	//	if (File.Exists(filePath))
	//	{
	//		string dataAsJson = File.ReadAllText(filePath);

	//		newArmy = JsonUtility.FromJson<ArmySave>(dataAsJson);
	//	}
	//	else
	//	{
	//		Debug.LogError("Cannot load army: " + filePath);
	//		return;
	//	}

	//	string[,] armyNames = new string[armyDesigner.tileUnits.GetLength(0), armyDesigner.tileUnits.GetLength(1)];
	//	for (int i = 0; i < armyDesigner.tileUnits.GetLength(0); i++)
	//	{
	//		for (int j = 0; j < armyDesigner.tileUnits.GetLength(1); j++)
	//		{
	//			armyNames[i, j] = newArmy.unitNames[0];
	//			newArmy.unitNames.RemoveAt(0);
	//		}
	//	}
	//	armyDesigner.LoadArmy(armyNames);
	//	Debug.Log("Army loaded");
	//}

	public UnitBlueprint[] GetUnitData()
	{
		return unitBlueprints;
	}
}
