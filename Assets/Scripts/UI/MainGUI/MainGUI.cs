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
		mArmyDesignPanel = Instantiate(prefabArmyDesignerPanel, this.transform);
	}

	public void CloseArmyDesigner()
	{
		Destroy(mArmyDesignPanel);
	}
}
