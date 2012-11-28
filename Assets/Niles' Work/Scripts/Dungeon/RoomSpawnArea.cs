using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomSpawnArea {
		
	public RoomSpawnArea (CellLocation setLocation, IntVector3 setSize) {
		
		location = setLocation;
        size = setSize;
    }
	
	public CellLocation location;
	public IntVector3 size;
}