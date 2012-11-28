using UnityEngine;
using System.Collections;

public enum CellType {
		
	None,
	Empty,
	Blocked,
	Room,
	Perimeter,
	Entrance,
	Corridor,
	StairsUp,
	StairsDown,
	StairsPerimeter,
	Rock,
	RockFloor,
	DungeonPerimeter
}

public enum CellWalkable {
		
	Yes,
	No
}
	
public enum Dir {
	
	North,
	South,
	East,
	West,
	NorthEast,
	NorthWest,
	SouthEast,
	SouthWest,
	None
}

public class Common
{
}
