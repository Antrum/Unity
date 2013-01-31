using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (ArmourManager))]
[RequireComponent (typeof (PotionManager))]
[RequireComponent (typeof (ScrollManager))]

public class ItemManager : MonoBehaviour {
	
	protected ArmourManager armourManager;
	protected PotionManager potionManager;
	protected ScrollManager scrollManager;
	
	[HideInInspector] public List<Item> allItems = new List<Item> ();
	[HideInInspector] public List<Item> allCommonItems = new List<Item> ();
	[HideInInspector] public List<Item> allUncommonItems = new List<Item> ();
	[HideInInspector] public List<Item> allRareItems = new List<Item> ();
	
	public void RefreshRarity (int floorNumber) {
		
		foreach (Item item in allCommonItems) allCommonItems.Remove (item);
		foreach (Item item in allUncommonItems) allUncommonItems.Remove (item);
		foreach (Item item in allRareItems) allRareItems.Remove (item);
		
		int commonArmourLevel = 1;
		
		if (floorNumber < 11) commonArmourLevel = 1;
		else if (floorNumber < 21) commonArmourLevel = 2;
		else if (floorNumber < 31) commonArmourLevel = 3;
		else if (floorNumber < 41) commonArmourLevel = 4;
		else commonArmourLevel = 5;
		
		foreach (Armour armour in allItems) {
			
			if (armour.level == commonArmourLevel) armour.rarity = Rarity.Common;
			else if (armour.level == commonArmourLevel + 1) armour.rarity = Rarity.Uncommon;
			else if (armour.level == commonArmourLevel + 2) armour.rarity = Rarity.Rare;
			else armour.rarity = Rarity.None;
		}
		
		foreach (Item item in allItems) {
			
			if (item.rarity == Rarity.Common) allCommonItems.Add (item);
			else if (item.rarity == Rarity.Uncommon) allUncommonItems.Add (item);
			else if (item.rarity == Rarity.Rare) allRareItems.Add (item);
		}
	}
	
	public void PrintAllItems () {
		
		//foreach (Item item in allItems) PrintItem (item);
		armourManager.PrintAllArmour ();
	}
	
	public void PrintItem (Item item) {
		
		if (item is Armour) armourManager.PrintArmour (item as Armour);
		else if (item is Potion) potionManager.PrintPotion (item as Potion);
		//else if (item is Scroll) scrollManager.PrintScroll (item as Scroll);
		else print ("Error");
	}
	
	public void CreateItems () {
		
		armourManager.CreateArmourSets ();
		potionManager.CreatePotions ();
	}
	
	public void AddItem (Item item) {
		
		if (item is Armour) armourManager.AddArmour (item as Armour);
		else if (item is Potion) potionManager.AddPotion (item as Potion);
		else if (item is Scroll) scrollManager.AddScroll (item as Scroll);
		else print ("Error");
	}
	
	public void RemoveItem (Item item) {
		
		if (item is Armour) armourManager.RemoveArmour (item as Armour);
		else if (item is Potion) potionManager.RemovePotion (item as Potion);
		else if (item is Scroll) scrollManager.RemoveScroll (item as Scroll);
	}
	
	void Start () {
	
		armourManager = GetComponent<ArmourManager> ();
		potionManager = GetComponent<PotionManager> ();
		scrollManager = GetComponent<ScrollManager> ();
		
		armourManager.Start ();
		potionManager.Start ();
		scrollManager.Start ();
		
		CreateItems ();
		PrintAllItems ();
	}
}