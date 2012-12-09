using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollManager : MonoBehaviour {
	
	protected ItemManager itemManager;
	
	public List<Scroll> allScrolls = new List<Scroll> ();
	
	protected void CalculatePrice (Scroll scroll) {
		
		scroll.price = 100;
	}
	
	protected void AddScroll (Scroll scroll) {
		
		itemManager.allItems.Add (scroll);
		allScrolls.Add (scroll);
	}
	
	protected void RemoveScroll (Scroll scroll) {
		
		itemManager.allItems.Remove (scroll);
		allScrolls.Remove (scroll);
	}
	
	public void Start () {
	
		itemManager = GetComponent<ItemManager> ();
	}
}
