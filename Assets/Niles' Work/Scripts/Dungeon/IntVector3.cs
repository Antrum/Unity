using UnityEngine;
using System.Collections;

public class IntVector3 {

    public IntVector3(int setX, int setY, int setZ) {
		
        x = setX;
		y = setY;
        z = setZ;
    }
	
	public IntVector3(IntVector3 setIntVector3) {
		
        x = setIntVector3.x;
		y = setIntVector3.y;
        z = setIntVector3.z;
    }

    public int x;
	public int y;
    public int z;
}
