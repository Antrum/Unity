using UnityEngine;
using System.Collections;

public class Equip {
	
	[HideInInspector] public Armour helmet = null;
	[HideInInspector] public Armour chest = null;
	[HideInInspector] public Armour legs = null;
	[HideInInspector] public Armour boots = null;
	
	public void MoveItemFromInventory (Item item, Inventory inventory) {
		
		inventory.RemoveItem (item);
		AddItem (item);
	}
	
	public void MoveItemToInventory (Item item, Inventory inventory) {
		
		RemoveItem (item);
		inventory.AddItem (item);
	}
	
	public void AddItem (Item item) {
		
		if (item is Armour) {
			
			Armour armour = item as Armour;
			
			if (armour.type == ArmourType.Helmet) helmet = armour;
			else if (armour.type == ArmourType.Chest) chest = armour;
			else if (armour.type == ArmourType.Legs) legs = armour;
			else if (armour.type == ArmourType.Boots) boots = armour;
		}
	}
	
	public void RemoveItem (Item item) {
		
		if (item is Armour) {
			
			Armour armour = item as Armour;
			
			if (armour.type == ArmourType.Helmet) helmet = null;
			else if (armour.type == ArmourType.Chest) chest = null;
			else if (armour.type == ArmourType.Legs) legs = null;
			else if (armour.type == ArmourType.Boots) boots = null;
		}
	}
	
	void Start () {
	
	}
}
