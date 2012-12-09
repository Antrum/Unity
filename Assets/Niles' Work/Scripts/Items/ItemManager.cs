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
	
	public List<Item> allItems = new List<Item> ();
	
	public void PrintAllItems () {
		
		//armourManager.PrintArmour ();
		
		foreach (Item item in allItems) {
			
			PrintItem (item);
		}
	}
	
	public void PrintItem (Item item) {
		
		//if ((item as Armour) != null) {
		if (item is Armour) {
			
			armourManager.PrintArmour (item as Armour);
		}
		
		else if (item is Potion) {
			
			potionManager.PrintPotion (item as Potion);
		}
		
		else if (item is Scroll) {
			
			//scrollManager.PrintScroll (item as Scroll);
		}
		
		else print ("Error");
	}
	
	public void CreateItems () {
		
		armourManager.CreateArmourSets ();
		potionManager.CreatePotions ();
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