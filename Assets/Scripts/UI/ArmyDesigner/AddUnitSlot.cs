using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddUnitSlot : MonoBehaviour, IUnitSlot, IDropHandler {
	private ArmyDesigner armyDesigner;

	public int TileRow { get; private set; }

	public void SetRow(int row)
	{
		TileRow = row;
	}

	private void Start()
	{
		armyDesigner = GameObject.Find("ArmyDesigner").GetComponent<ArmyDesigner>();
	}

	public void OccupySlot(DraggableUnit unit)
	{
		if (unit != null)
		{
			// Units cannot move to an AddUnitSlot. If they do, they are destroyed
			Destroy(unit.gameObject);
			Debug.Log("Unit destroyed?");
		}
	}

	public void OccupySlot(DraggableUnit unit, IUnitSlot other)
	{
		if (unit != null)
		{
			// Units cannot move to an AddUnitSlot. If they do, they are destroyed
			Destroy(unit.gameObject);
			Debug.Log("Unit from other slot destroyed?");
		}
	}

	public void BeginUnitDrag(DraggableUnit unit)
	{
		// When a unit leaves an AddUnitSlot, we want to immediately create a new one to replace it
		armyDesigner.AddUnitToAddList(TileRow, TileRow);
	}

	public void OnDrop(PointerEventData eventData)
	{
		DraggableUnit unit = eventData.pointerDrag.GetComponent<DraggableUnit>();
		if (unit != null)
		{
			unit.parentSlotAfterDrop = this;
		}
	}
}
