using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	public string name;
	public int price;
	
	public int sellPrice () {
		
		return price / 4;
	}
	
	public virtual void Start () {
	
	}
}
