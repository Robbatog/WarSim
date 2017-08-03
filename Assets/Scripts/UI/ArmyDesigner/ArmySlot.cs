using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmySlot : MonoBehaviour, IUnitSlot, IDropHandler {

	// reference to grandparent
	public ArmyDesignerPanel armyDesignerPanel;

	public int TileRow { get; private set; }
	public int TileColumn { get; private set; }

	public void SetRowCol(int row, int col)
	{
		TileRow = row;
		TileColumn = col;
	}

	private void Start()
	{
		
	}

	public void OccupySlot(DraggableUnit unit)
	{
		if (unit != null)
		{
			unit.transform.SetParent(this.transform);
			unit.transform.position = this.transform.position;
			armyDesignerPanel.tileUnits[TileRow, TileColumn] = unit.unit;
		}
		else
		{
			armyDesignerPanel.tileUnits[TileRow, TileColumn] = null;
		}
	}

	public void OccupySlot(DraggableUnit unit, IUnitSlot other)
	{
		if (other != null)
		{
			// move our unit to other
			DraggableUnit ourUnit = null;
			if (this.transform.childCount > 0)
			{
				ourUnit = this.transform.GetChild(0).GetComponent<DraggableUnit>();
			}
			other.OccupySlot(ourUnit);

			// move input unit to us
			this.OccupySlot(unit);
		}
		else
		{
			Debug.LogError("ArmySlot::occupySlot() : other is == null!");
		}
	}

	public void BeginUnitDrag(DraggableUnit unit)
	{
		return; // ArmySlots do nothing when a unit is dragged from it
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
