using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FloorType {
	
	Dungeon,
	Cave
}

public enum FloorTheme {
	
	Sand,
	None
}

public class Floor {
		
	public Floor (int setFloorNumber) {
		
		floorNumber = setFloorNumber;
	}
	
	public FloorType type = FloorType.Dungeon;
	public FloorTheme theme = FloorTheme.Sand;
	
	public int floorNumber;
	
	public Material floorMaterial;
	public Material wallMaterial;
	public Mesh wallMesh;
	
	public bool generated = false;
	
	public Transform PrefabCellParent;
	public Transform OtherStuffParent;
	
	public List<Stairs> allStairsUp = new List<Stairs> ();
	public List<Stairs> allStairsDown = new List<Stairs> ();
	public List<Room> allRooms = new List<Room> ();
	public List<Entrance> allEntrances = new List<Entrance> ();
	public List<Corridor> allCorridors = new List<Corridor> ();
	public List<Chest> allChests = new List<Chest> ();
	
	public bool reflectRoomsX = false;
	public bool reflectRoomsZ = false;
	public bool reflectEntrancesX = false;
	public bool reflectEntrancesZ = false;
}