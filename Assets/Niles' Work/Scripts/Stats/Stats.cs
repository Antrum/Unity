using UnityEngine;
using System.Collections;

public class Stats {
	
	public int maxHealth = 100;
	[HideInInspector] public int health = 100;
	
	public int baseDefence = 0;
	public int baseStrength = 0;
	public int baseSpeed = 0;
	
	[HideInInspector] public int defenceBoost = 0;
	[HideInInspector] public int strengthBoost = 0;
	[HideInInspector] public int speedBoost = 0;
	
	[HideInInspector] public int defence = 0;
	[HideInInspector] public int strength = 0;
	[HideInInspector] public int speed = 0;
	
	public void Calculate (Equip equip) {
		
		defenceBoost = 0;
		strengthBoost = 0;
		speedBoost = 0;
		
		if (equip.boots != null) defenceBoost += equip.boots.defenceBoost;
		
		if (equip.helmet != null) defenceBoost += equip.helmet.defenceBoost;
		
		if (equip.chest != null) defenceBoost += equip.chest.defenceBoost;
		
		if (equip.legs != null) defenceBoost += equip.legs.defenceBoost;
		
		defence = baseDefence + defenceBoost;
		strength = baseStrength + strengthBoost;
		speed = baseSpeed + speedBoost;
	}
	
	public void TakeDamage (int amount) {
		
		health -= amount;
		
		if (health < 0) health = 0;
	}
	
	public void Heal (int amount) {
		
		health += amount;
		
		if (health > maxHealth) health = maxHealth;
	}
}
