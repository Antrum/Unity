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
		
		defenceBoost = baseDefenceBoost ()* (int)Mathf.Pow(2.0f, (float)level);
	}
	
	public ArmourType type;
	public int level;
	public int defenceBoost;
	
	public int baseDefenceBoost () {
		
		if (type == ArmourType.Boots) return 1;
		else if (type == ArmourType.Chest) return 5;
		else if (type == ArmourType.Helmet) return 2;
		else return 3;
	}
	
	public override void Start () {
		
		base.Start ();
	}
}
