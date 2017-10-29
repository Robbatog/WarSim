using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyOverviewToggler : MonoBehaviour {

	// prefabs
	public GameObject prefabArmyOverviewPanel;

	// members
	GameObject mArmyOverviewPanel;

	public void ToggleArmyOverview()
	{
		if (mArmyOverviewPanel == null)
		{
			OpenArmyOverview();
		}
		else
		{
			CloseArmyOverview();
		}
	}

	private void OpenArmyOverview()
	{
		PlayerArmyDesignHandler adh = GameObject.Find("Player").GetComponent<PlayerArmyDesignHandler>();
		PlayerArmyHandler ah = GameObject.Find("Player").GetComponent<PlayerArmyHandler>();

		mArmyOverviewPanel = Instantiate(prefabArmyOverviewPanel, this.transform);
		var cb = new ArmyOverviewPanel.ArmyObverviewPanelCallbackInterface(adh.GetArmies());
		mArmyOverviewPanel.GetComponent<ArmyOverviewPanel>().Init(cb);
	}

	public void CloseArmyOverview()
	{
		Destroy(mArmyOverviewPanel);
	}
}
