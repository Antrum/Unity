using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InventoryType {
	
}

public class Inventory : MonoBehaviour {
	
	public Inventory () {
		
		
	}
	
	public void AddCoins (int amount) {
		
		coins += amount;
	}
	
	public void RemoveCoins (int amount) {
		
		coins -= amount;
	}
	
	public void AddItem (Item item) {
		
		items.Add (item);
	}
	
	public void RemoveItem (Item item) {
		
		items.Remove (item);
	}
	
	public List<Item> items = new List<Item> ();
	
	public int coins = 0;
	
	public InventoryType type;
	public int capacity;
	
	public virtual void Start () {
	
	}
}
