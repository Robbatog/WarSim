using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ArmyOverviewPanel : MonoBehaviour {

	public struct ArmyObverviewPanelCallbackInterface
	{
		public ArmyObverviewPanelCallbackInterface(
			List<string> currentArmies
		)
		{
			this.currentArmies = currentArmies;
		}

		public List<string> currentArmies;
	}

	// references to child objects
	GameObject closeButton;

	private void OnDestroy()
	{
		closeButton.GetComponent<Button>().onClick.RemoveAllListeners();
	}

	public void Init(ArmyObverviewPanelCallbackInterface cb)
	{
		// Fetch child GUI elements
		closeButton = this.transform.Find("CloseButton").gameObject;

		// Bind button functions
		closeButton.GetComponent<Button>().onClick.AddListener(() => { Destroy(this.gameObject); });
	}

}
