using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGUI : MonoBehaviour {

	//members
	private ArmyDesignerToggler mArmyDesignerToggler;
	private ArmyOverviewToggler mArmyOverviewToggler;

	void Start()
	{
		mArmyDesignerToggler = GetComponent<ArmyDesignerToggler>();
		mArmyOverviewToggler = GetComponent<ArmyOverviewToggler>();
	}

	public void CloseAll()
	{
		mArmyDesignerToggler.CloseArmyDesigner();
		mArmyOverviewToggler.CloseArmyOverview();
	}

	public void ToggleArmyDesigner()
	{
		CloseAll();
		mArmyDesignerToggler.ToggleArmyDesigner();
	}

	public void ToggleArmyOverview()
	{
		CloseAll();
		mArmyOverviewToggler.ToggleArmyOverview();
	}
}
