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
		PlayerArmyDesignHandler adh = GameObject.Find("Player").GetComponent<PlayerArmyDesignHandler>();

		mArmyDesignPanel = Instantiate(prefabArmyDesignerPanel, this.transform);
		var cb = new ArmyDesignerPanel.ArmyDesignerPanelCallbackInterface(adh.GetArmies(), adh.GetArmy, adh.AddArmy, adh.ExistsArmy, adh.DeleteArmy, adh.GetAvailableUnits);
		mArmyDesignPanel.GetComponent<ArmyDesignerPanel>().Init(cb);
	}

	public void CloseArmyDesigner()
	{
		Destroy(mArmyDesignPanel);
	}
}
