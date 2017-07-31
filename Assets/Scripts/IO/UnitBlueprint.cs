using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitData
{
	public enum RaceType
	{
		Human,
		Orc,
		Elf,
		Goblin,
		Necromancer
	}

	public enum TierType
	{
		T1,
		T2,
		T3
	}

	public enum RoleType
	{
		Infantry,
		Cavalry,
		Archer,
		Flyer,
		Special
	}

	public string name;
	public RaceType race;
	public TierType tier;
	public RoleType role; // Infantry, Cavalry, Archer, Flyer, Special

	// Base stats of unit
	public BaseStats baseStats;

	public UnitData(UnitData other)
	{
		name = other.name;
		race = other.race;
		tier = other.tier;
		role = other.role;
		baseStats = new BaseStats(other.baseStats);
	}

	public UnitData()
	{

	}
}

[System.Serializable]
public class BaseStats
{
	public BaseStats(BaseStats other)
	{
		abilities = (string[])other.abilities.Clone();
		upkeep = other.upkeep;
		restingHP = other.restingHP;
		maxHP = other.maxHP;
		initiative = other.initiative;
		morale = other.morale;
		attack = other.attack;
		defence = other.defence;
		damage = other.damage;
		armor = other.armor;
	}

	public string[] abilities;
	public int upkeep;
	public int restingHP; // when resting, how much hp per turn
	public int maxHP;
	public int initiative;
	public int morale;
	public int attack;
	public int defence;
	public int damage;
	public int armor;
}

// Raw data representation of a type of unit. Read from json file
[System.Serializable]
public class JsonUnitBlueprint : UnitData
{

	//public Sprite sprite;
	public string spritePath;
}

// In-game representation of a type of unit. Has a Sprite instead of a spritePath etc.
public class UnitBlueprint : UnitData
{
	public Sprite sprite;
	//public string spritePath;
	public UnitBlueprint(JsonUnitBlueprint jsonUnit) : base(jsonUnit)
	{
		sprite = Resources.Load<Sprite>(jsonUnit.spritePath);
	}
}
