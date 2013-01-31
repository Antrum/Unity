using UnityEngine;
using System.Collections;

public enum Rarity {
	
	Common,
	Uncommon,
	Rare,
	None
}

public class Item {
	
	public string name;
	public int price;
	public int sellFraction = 4;
	
	public Transform transform;
	
	public Rarity rarity = Rarity.Common;
	
	public int sellPrice () {
		
		return price / sellFraction;
	}
	
	public virtual void Start () {
	
	}
}
