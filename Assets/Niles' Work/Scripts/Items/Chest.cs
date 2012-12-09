using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest {
	
	public Chest (CellLocation setLocation) {
		
		location = setLocation;
	}
	
	public CellLocation location;
	
	public Dir direction = Dir.South;
	public int rotation = 0;
	
	public bool opened = false;

	void Start () {
	
	}
}
