using UnityEngine;
using System.Collections;

public class Stairs {
		
	public Stairs(CellLocation setLocation, int setRotation) {
		
		location = setLocation;
		rotation = setRotation;
	}
	
	public CellLocation location;
	public int rotation;
	
	public bool stairsUp = false;
	public bool stairsDown = false;
	public Dir direction = Dir.None;
	
}