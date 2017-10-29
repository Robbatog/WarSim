using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyDesignerToggler : MonoBehaviour {

	// prefabs
	public GameObject prefabArmyDesignerPanel;

	// members
	GameObject mArmyDesignPanel;

	public void ToggleArmyDesigner()
	{
		if (mArmyDesignPanel == null)
		{
			OpenArmyDesigner();
		}
		else
		{
			CloseArmyDesigner();
		}
	}

	private void OpenArmyDesigner()
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
