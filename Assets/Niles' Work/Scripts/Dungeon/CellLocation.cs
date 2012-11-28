using UnityEngine;
using System.Collections;

public class CellLocation {
		
    public CellLocation (int setX, int setY, int setZ) {
		
        x = setX;
		y = setY;
        z = setZ;
    }

    public int x;
	public int y;
    public int z;
	
	public CellLocation getNorth () {
		
		return new CellLocation (x, y, z + 1);
	}
	
	public CellLocation getSouth () {
		
		return new CellLocation (x, y, z - 1);
	}
	
	public CellLocation getEast () {
		
		return new CellLocation (x + 1, y, z);
	}
	
	public CellLocation getWest () {
		
		return new CellLocation (x - 1, y, z);
	}
	
    public override bool Equals (object obj) {
		
        if (obj is CellLocation) {
			
            CellLocation testObj = (CellLocation)obj;
            if (testObj.x == x && testObj.y == y && testObj.z == z) {
				
                return true;
            }
        }

        return false;
    }

    public override int GetHashCode () {
		
        return base.GetHashCode ();
    }
}
