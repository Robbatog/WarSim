using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUnit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public Unit unit;
	//public int TileRow { get; private set; }
	//public int TileColumn { get; private set; }

	//public void setRowCol(int row, int col)
	//{
	//	TileRow = row;
	//	TileColumn = col;
	//}

	GameObject armyDesignerPanel;

	public IUnitSlot parentSlotBeforeDrop = null;
	public IUnitSlot parentSlotAfterDrop = null;

	private void Start()
	{
		armyDesignerPanel = GameObject.Find("ArmyDesignerPanel");
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if(unit != null)
		{
			parentSlotBeforeDrop = this.transform.parent.GetComponent<IUnitSlot>();
			parentSlotAfterDrop = parentSlotBeforeDrop;

			this.transform.SetParent(armyDesignerPanel.transform);
			this.transform.position = eventData.position;
			GetComponent<CanvasGroup>().blocksRaycasts = false;

			parentSlotBeforeDrop.BeginUnitDrag(this);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (unit != null)
		{
			this.transform.position = eventData.position;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().blocksRaycasts = true;

		if (parentSlotAfterDrop != null)
		{
			parentSlotAfterDrop.OccupySlot(this, parentSlotBeforeDrop);
		}
		parentSlotBeforeDrop = null;
		parentSlotAfterDrop = null;
	}
}
