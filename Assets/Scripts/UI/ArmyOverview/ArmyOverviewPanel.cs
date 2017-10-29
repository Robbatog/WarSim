using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ArmyOverviewPanel : MonoBehaviour {

	public struct ArmyObverviewPanelCallbackInterface
	{
		public ArmyObverviewPanelCallbackInterface(
			Dictionary<string, ArmySave> availableArmyTypes
		)
		{
			this.availableArmyTypes = availableArmyTypes;
		}

		public Dictionary<string, ArmySave> availableArmyTypes;
	}

	// references to child objects
	GameObject closeButton;
	GameObject recruitArmyScrollView; 
	GameObject ownedArmyScrollView;

	// callbacks to outside world that this ArmyOverviewPanel runs when buttons are pressed
	private ArmyObverviewPanelCallbackInterface cb;

	// prefabs to instantiate
	public GameObject recruitableArmyButton;
	public GameObject ownedArmyButton;

	// settings and members for this object
	Dictionary<string, ArmySave> availableArmyTypes;
	Dictionary<string, GameObject> recruitArmyButtons;
	Dictionary<string, GameObject> ownedArmyButtons;

	private void OnDestroy()
	{
		closeButton.GetComponent<Button>().onClick.RemoveAllListeners();
	}

	public void Init(ArmyObverviewPanelCallbackInterface callbacks)
	{
		cb = callbacks;

		// Fetch child GUI elements
		closeButton = this.transform.Find("CloseButton").gameObject;
		recruitArmyScrollView = this.transform.Find("RecruitArmyScrollView").gameObject;
		ownedArmyScrollView = this.transform.Find("OwnedArmyScrollView").gameObject;

		// Bind button functions
		closeButton.GetComponent<Button>().onClick.AddListener(() => { Destroy(this.gameObject); });

		// Setup button storage
		recruitArmyButtons = new Dictionary<string, GameObject>();
		ownedArmyButtons = new Dictionary<string, GameObject>();

		// Make buttons for recruiting each army design
		availableArmyTypes = cb.availableArmyTypes;
		foreach (var armyKV in availableArmyTypes)
		{
			MakeArmyRecruitButton(armyKV.Value);
		}
	}

	public void RecruitArmy(string armyTypeName)
	{
		string tmpArmyName = "temp army name"; //TODO: get next name from army handler
		Army army = new Army(tmpArmyName, availableArmyTypes[armyTypeName]);
		if (army == null)
		{
			Debug.LogError("RecruitArmy: Could not create army of type \"" + armyTypeName + "\"");
		}

		MakeOwnedArmyButton(tmpArmyName, army);
		Debug.Log("RecruitArmy: created army of type \"" + armyTypeName + "\"");
	}

	public void MakeOwnedArmyButton(string armyName, Army army)
	{
		var ownedArmyList = ownedArmyScrollView.transform.Find("Viewport").Find("OwnedArmyList");

		GameObject newButton = Instantiate(ownedArmyButton, ownedArmyList);
		ownedArmyButtons.Add(armyName, newButton);

		newButton.transform.Find("TextMsh").GetComponent<TMPro.TextMeshProUGUI>().text = armyName;

		string callbackString = armyName; // avoid capturing the wrong string (armyName) in lambda closure - add a local to capture instead
		newButton.GetComponent<Button>().onClick.AddListener(() => { RecruitArmy(callbackString); });

		// set sprite
		var tex = new Texture2D(1, 1);
		tex.LoadImage(army.spriteBytesPNG);
		tex.Apply();

		Sprite s = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().sprite = s;
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().type = Image.Type.Simple;
		newButton.transform.Find("ArmyScreenshot").GetComponent<Image>().preserveAspect = true;
	}

	public void MakeArmyRecruitButton(ArmySave army)
	{
		var recruitableArmyList = recruitArmyScrollView.transform.Find("Viewport").Find("RecruitableArmyList");

		GameObject newButton = Instantiate(recruitableArmyButton, recruitableArmyList);
		recruitArmyButtons.Add(army.armyName, newButton);

		newButton.transform.Find("TextMsh").GetComponent<TMPro.TextMeshProUGUI>().text = army.armyName;

		string callbackString = army.armyName; // avoid capturing the wrong string (armyName) in lambda closure - add a local to capture instead
		newButton.GetComponent<Button>().onClick.AddListener(() => { RecruitArmy(callbackString); });

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
