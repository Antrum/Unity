using UnityEngine;
using System.Collections;

public class Void {
		
	public Void (CellLocation setLocation) {
		
        location = setLocation;
    }
	
	public CellLocation location;
	public bool clustered = false;
}