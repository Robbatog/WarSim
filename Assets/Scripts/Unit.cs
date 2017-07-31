using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit {

	public string name;
	public UnitBlueprint.RaceType race;
	public UnitBlueprint.TierType tier;
	public UnitBlueprint.RoleType role; // Infantry, Cavalry, Archer, Flyer, Special

	public Sprite sprite;

	// Temporary effects that modifies the unit's stats
	public List<string> buffs;

	// Base stats of unit. Defined by the "blueprint" UnitBlueprint
	public BaseStats BaseStats { get; private set; }

	// Calculated stats of unit. Derived from base stats + buffs, abilities etc
	public CalculatedStats CalculatedStats { get; private set; }

	public Unit(UnitBlueprint baseUnit)
	{
		name = baseUnit.name;
		race = baseUnit.race;
		tier = baseUnit.tier;
		role = baseUnit.role;
		sprite = baseUnit.sprite;
		buffs = new List<string>();
		BaseStats = new BaseStats(baseUnit.baseStats);
		CalculatedStats = new CalculatedStats(BaseStats);
	}
	
}

public class CalculatedStats : BaseStats
{
	public CalculatedStats(BaseStats bs) : base(bs)
	{
		currentHP = maxHP;
	}

	public CalculatedStats(CalculatedStats cs) : base(cs)
	{
		currentHP = cs.currentHP;
	}

	public int currentHP;
}
