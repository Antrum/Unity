using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RoomType {
	
	None,
	Central,
	Platform,
	SingleEntrance,
	Boss
}

public class Room {
		
	public Room (CellLocation setLocation, IntVector3 setSize) {
		
		location = setLocation;
        size = setSize;
		
		northMost = location.z + (size.z - 1) / 2;
		southMost = location.z - (size.z - 1) / 2;
		eastMost = location.x + (size.x - 1) / 2;
		westMost = location.x - (size.x - 1) / 2;
		
		for (int i = westMost; i < eastMost + 1; i++) {
			
			for (int k = southMost; k < northMost + 1; k++) {
				
				CellLocation cellLocation = new CellLocation (i, location.y, k);
				
				cellLocations.Add (cellLocation);
			}
		}
    }
	
	public CellLocation location;
	public IntVector3 size;
	
	public int northMost;
	public int southMost;
	public int eastMost;
	public int westMost;
	
	public List<CellLocation> cellLocations = new List<CellLocation> ();
		
	
	public List<Entrance> entrances = new List<Entrance> ();
		
	public int reasonableNumberOfEntrances;
	
	public bool reflectableInX = true;
	public bool reflectableInZ = true;
	
	public bool reflectedRoomX = false;
	public bool reflectedRoomZ = false;
	
	public RoomType type = RoomType.None;
}