using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollManager : MonoBehaviour {
	
	protected ItemManager itemManager;
	
	[HideInInspector]
	public List<Scroll> allScrolls = new List<Scroll> ();
	
	public int sellFraction = 4;
	
	protected void CalculatePrice (Scroll scroll) {
		
		scroll.price = 100;
	}
	
	public void UseScroll (Scroll scroll) {
		
	}
	
	public void AddScroll (Scroll scroll) {
		
		scroll.sellFraction = 4;
		itemManager.allItems.Add (scroll);
		allScrolls.Add (scroll);
	}
	
	public void RemoveScroll (Scroll scroll) {
		
		itemManager.allItems.Remove (scroll);
		allScrolls.Remove (scroll);
	}
	
	public void Start () {
	
		itemManager = GetComponent<ItemManager> ();
	}
}
