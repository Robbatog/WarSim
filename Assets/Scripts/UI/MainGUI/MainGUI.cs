using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGUI : MonoBehaviour {

	// prefabs
	public GameObject prefabArmyDesignerPanel;

	// members
	GameObject mArmyDesignPanel;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleArmyDesigner()
	{
		if(mArmyDesignPanel == null)
		{
			OpenArmyDesigner();
		}
		else
		{
			CloseArmyDesigner();
		}
	}

	public void OpenArmyDesigner()
	{
		PlayerArmyDesignHandler ah = GameObject.Find("Player").GetComponent<PlayerArmyDesignHandler>();

		mArmyDesignPanel = Instantiate(prefabArmyDesignerPanel, this.transform);
		mArmyDesignPanel.GetComponent<ArmyDesignerPanel>().Init(ah.GetArmies(), ah.GetArmy, ah.AddArmy, ah.ExistsArmy, ah.DeleteArmy, ah.GetAvailableUnits);
	}

	public void CloseArmyDesigner()
	{
		Destroy(mArmyDesignPanel);
	}
}
