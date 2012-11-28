using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntranceSpawnArea {
		
	public EntranceSpawnArea (CellLocation setLocation, IntVector3 setSize) {
		
		location = setLocation;
        size = setSize;
    }
	
	public CellLocation location;
	public IntVector3 size;
}