using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
	
	[HideInInspector]
	List<Inventory> allInventories = new List<Inventory> ();
	
	public void ShiftToInventory (Item item, Inventory inventoryA, Inventory inventoryB) {
		
		inventoryB.RemoveItem (item);
		inventoryA.AddItem (item);
	}
	
	void Start () {
	
	}
}
