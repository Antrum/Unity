using UnityEngine;
using System.Collections;

public class Corridor {
		
	public Corridor (CellLocation setLocation) {
		
        location = setLocation;
    }
	
	public CellLocation location;
	
	public bool reflectableInX = false;
	public bool reflectableInZ = false;
}