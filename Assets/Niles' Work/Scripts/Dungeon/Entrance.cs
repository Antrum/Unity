using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entrance {
		
	public Entrance (CellLocation setLocation) {
		
        location = setLocation;
    }
	
	public CellLocation location;
	
	public List<Room> rooms = new List<Room> ();
	
	public bool reflectableInX = false;
	public bool reflectableInZ = false;
	
	public bool entranceToStairs = false;
	
	public Dir direction = Dir.None;
}