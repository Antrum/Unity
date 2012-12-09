using UnityEngine;
using System.Collections;

public enum ScrollType {
	
	Teleport,
	Protection,
	EnchantWeapon,
	EnchantArmour
}

public class Scroll : Item {
	
	public Scroll () {
	}
	
	public override void Start () {
	
		base.Start ();
	}
}
