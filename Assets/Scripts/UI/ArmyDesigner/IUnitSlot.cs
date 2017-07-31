using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitSlot {

	// call when a unit wants to move to this slot. Must handle null units
	void OccupySlot(DraggableUnit unit);

	// call when a unit wants to move to this slot from another slot. Must handle null units
	void OccupySlot(DraggableUnit unit, IUnitSlot other);

	// call when a unit is dragged from this slot
	void BeginUnitDrag(DraggableUnit unit);
}
