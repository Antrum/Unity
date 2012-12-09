using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmourManager : MonoBehaviour {
	
	protected ItemManager itemManager;
	
	public List<Armour> allArmour = new List<Armour> ();
	
	public void PrintAllArmour () {
		
		foreach (Armour armour in allArmour) {
			
			PrintArmour (armour);
		}
	}
	
	public void PrintArmour (Armour armour) {
		
		print (armour.name 
			+ " - " 
			+ armour.defenceBoost 
			+ " Defence boost, Buy price: " 
			+ armour.price 
			+ ", Sell price: " 
			+ armour.sellPrice ());
	}
	
	public void CreateArmourSets () {
		
		CreateArmourSet ("Bronze", 1);
		CreateArmourSet ("Iron", 2);
		CreateArmourSet ("Steel", 3);
		CreateArmourSet ("Adamant", 4);
		CreateArmourSet ("Granite", 5);
		CreateArmourSet ("Crystal", 6);
		CreateArmourSet ("Legendary", 7);
	}
	
	protected void CreateArmourSet (string name, int level) {
		
		Armour armour;
		
		armour = new Armour (name + " Boots", ArmourType.Boots, level);
		AddArmour (armour);
		armour = new Armour (name + " Chest", ArmourType.Chest, level);
		AddArmour (armour);
		armour = new Armour (name + " Helmet", ArmourType.Helmet, level);
		AddArmour (armour);
		armour = new Armour (name + " Legs", ArmourType.Legs, level);
		AddArmour (armour);
	}
	
	protected void CalculatePrice (Armour armour) {
		
		armour.price = armour.defenceBoost * 20;
	}
	
	protected void AddArmour (Armour armour) {
		
		CalculatePrice (armour);
		itemManager.allItems.Add (armour);
		allArmour.Add (armour);
	}
	
	protected void RemoveArmour (Armour armour) {
		
		itemManager.allItems.Remove (armour);
		allArmour.Remove (armour);
	}
	
	public void Start () {
		
		itemManager = GetComponent<ItemManager> ();
	}
}
