using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour {

	[HideInInspector] public List<Shop> allShops = new List<Shop> ();
	
	public FloorGenerator floorGenerator;
	public SceneryGenerator sceneryGenerator;
	public ItemManager itemManager;
	
	public Transform shopPrefab;
	
	public void InstantiateShop (Shop shop) {
		
	    sceneryGenerator.InstantiateShop (shop);
	}
	
	public void CreateShop () {
		
		List<Item> items = new List<Item> ();
		
		foreach (Item item in itemManager.allItems) {
			
			if (ItemStocked (item, 1)) {
				
				items.Add (item);
			}
		}
		
		Inventory inventory = new Inventory ();
		inventory.items = items;
		
		Shop shop = new Shop (inventory);
		AddShop (shop);
		
		shop.location = new CellLocation(9, 0, 9);
	}
	
	public bool ItemStocked (Item item, int storeNumber) {
		
		if (storeNumber == 1) {
			
			switch (item.name) {
				
			case "Bronze Helmet":
			case "Bronze Legs":
			case "Bronze Chestplate":
			case "Bronze Boots":
			case "Diluted Health Potion":
			case "Antidote":
				return true;
				
			default:
				return false;
			}
		}
		else if (storeNumber == 2) {
			
			switch (item.name) {
				
			case "Iron Helmet":
			case "Iron Legs":
			case "Iron Chestplate":
			case "Iron Boots":
			case "Diluted Health Potion":
			case "Antidote":
				return true;
				
			default:
				return false;
			}
		}
		return false;
	}
	
	public void AddShop (Shop shop) {
		
		allShops.Add (shop);
	}
	
	public void RemoveShop (Shop shop) {
		
		allShops.Remove (shop);
	}
	
	public void Start () {
		
		CreateShop ();
	}
}
