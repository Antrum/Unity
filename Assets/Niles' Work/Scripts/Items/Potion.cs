using UnityEngine;
using System.Collections;

public enum PotionType {
	
	Health,
	Antidote
}

public class Potion : Item {
	
	public Potion (string setName, PotionType setType) {
	
		name = setName;
		type = setType;
	}
	
	public PotionType type;
	public int duration;
	public int healPercentage;
	
	public override void Start () {
		
		base.Start ();
	}
}
