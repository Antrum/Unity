using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop {
	
	public Shop (Inventory setInventory) {
		
		inventory = setInventory;
	}
	
	public Transform transform;
	public CellLocation location;
	
	public Inventory inventory;
	
	public void BuyItem (Item item, Unit unit) {
		
		if (unit.inventory.coins < item.price) return;
		
		inventory.items.Remove (item);
		unit.inventory.items.Add (item);
		unit.inventory.coins -= item.price;
	}
	
	public void SellItem (Item item, Unit unit) {
		
		unit.inventory.items.Remove (item);
		inventory.items.Add (item);
		unit.inventory.coins += item.sellPrice ();
	}
	
	public void SellJunk (Unit unit) {
		
		foreach (Item item in unit.inventory.items) {
			
			if (Junk (item, unit)) SellItem (item, unit);
		}
	}
	
	public bool Junk (Item item, Unit unit) {
		
		if (item is Armour) {
			
			Armour armour = item as Armour;
			
			if (armour.type == ArmourType.Boots && armour.defenceBoost < unit.equip.boots.defenceBoost) 
				return true;
			
			else if (armour.type == ArmourType.Chest && armour.defenceBoost < unit.equip.chest.defenceBoost) 
				return true;
			
			else if (armour.type == ArmourType.Helmet && armour.defenceBoost < unit.equip.helmet.defenceBoost) 
				return true;
			
			else if (armour.type == ArmourType.Legs && armour.defenceBoost < unit.equip.legs.defenceBoost) 
				return true;
		}
		return false;
	}
	
	void Start () {
	
	}
}
