using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotionManager : MonoBehaviour {
	
	protected ItemManager itemManager;
	
	[HideInInspector] public List<Potion> allPotions = new List<Potion> ();
	
	public int sellFraction = 4;
	
	public void PrintAllPotions () {
		
		foreach (Potion potion in allPotions) {
			
			PrintPotion (potion);
		}
	}
	
	public void PrintPotion (Potion potion) {
		
		if (potion.type == PotionType.Health) {
			
			print (potion.name
			+ " - "
			+ potion.healPercentage
			+ "% Heal, Buy price: " 
			+ potion.price 
			+ ", Sell price: " 
			+ potion.sellPrice ());
		}
		else {
			
			print (potion.name
			+ " - Buy price: " 
			+ potion.price 
			+ ", Sell price: " 
			+ potion.sellPrice ());
		}
	}
	
	public void UsePotion (Stats stats, Potion potion) {
		
		if (potion.type == PotionType.Health) {
			
			int heal = (int)((float)stats.maxHealth * (float)potion.healPercentage / 100);
			
			stats.Heal (heal);
		}
	}
	
	public void CreatePotions () {
		
		Potion potion;
		
		potion = new Potion ("Diluted Health Potion", PotionType.Health);
		potion.healPercentage = 25;
		AddPotion (potion);
		potion = new Potion ("Health Potion", PotionType.Health);
		potion.healPercentage = 50;
		AddPotion (potion);
		potion = new Potion ("Full Health Potion", PotionType.Health);
		potion.healPercentage = 100;
		AddPotion (potion);
		potion = new Potion ("Antidote", PotionType.Antidote);
		AddPotion (potion);
	}
	
	protected void CalculatePrice (Potion potion) {
		
		if (potion.type == PotionType.Health) {
			
			potion.price = potion.healPercentage * 4;
		}
		
		else potion.price = 100;
	}
	
	public void AddPotion (Potion potion) {
		
		potion.sellFraction = sellFraction;
		CalculatePrice (potion);
		itemManager.allItems.Add (potion);
		allPotions.Add (potion);
	}
	
	public void RemovePotion (Potion potion) {
		
		itemManager.allItems.Remove (potion);
		allPotions.Remove (potion);
	}
	
	public void Start () {
	
		itemManager = GetComponent<ItemManager> ();
	}
}
