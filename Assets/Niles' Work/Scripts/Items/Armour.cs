using UnityEngine;
using System.Collections;

public enum ArmourType {
	
	Boots,
	Chest,
	Helmet,
	Legs
}

public class Armour : Item {
	
	public Armour (string setName, ArmourType setType, int setLevel) {
		
		name = setName;
		type = setType;
		level = setLevel;
	}
	
	public ArmourType type;
	public int level;
	public int defenceBoost;
	public float ramp = 2.0f;
	
	public void CalculateDefenceBoost () {
		
		defenceBoost = (int)((float)baseDefenceBoost ()* Mathf.Pow (ramp, (float)level));
	}
	
	public int baseStatBoots = 10;
	public int baseStatChest = 50;
	public int baseStatHelmet = 20;
	public int baseStatLegs = 30;
	
	public int baseDefenceBoost () {
		
		if (type == ArmourType.Boots) return baseStatBoots;
		else if (type == ArmourType.Chest) return baseStatChest;
		else if (type == ArmourType.Helmet) return baseStatHelmet;
		else return baseStatLegs;
	}
	
	public override void Start () {
		
		base.Start ();
	}
}
