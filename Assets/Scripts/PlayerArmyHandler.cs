using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmyHandler : MonoBehaviour {

	private int nextArmyID = 1;

	public int getNextArmyID()
	{
		return nextArmyID++;
	}
}
