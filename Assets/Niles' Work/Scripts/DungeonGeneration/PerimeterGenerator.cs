using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to generate perimeters.
/// </summary>
public class PerimeterGenerator : MonoBehaviour {
	
	protected DungeonGenerator dungeonGenerator;
	protected RoomGenerator roomGenerator;
	
	protected bool PerimeterCanReplace (CellType type) {
		
		if (type == CellType.Blocked || type == CellType.StairsPerimeter || type == CellType.Perimeter || type == CellType.Entrance) {
			
			return false;
		}
		return true;
	}
	
	protected List<CellLocation> PotentialPerimeterLocations (Room room) {
		
		List<CellLocation> potentialPerimeterLocations = new List<CellLocation> ();
		
		for (var i = room.westMost - 1; i < room.eastMost + 2; i++) {
			
			potentialPerimeterLocations.Add (new CellLocation(i, room.location.y, room.southMost - 1));
			potentialPerimeterLocations.Add (new CellLocation(i, room.location.y, room.northMost + 1));
		}
		
		for (var j = room.southMost; j < room.northMost + 1; j ++) {
			
			potentialPerimeterLocations.Add (new CellLocation(room.westMost - 1, room.location.y, j));
			potentialPerimeterLocations.Add (new CellLocation(room.eastMost + 1, room.location.y, j));
		}
		
		return potentialPerimeterLocations;
	}
	
	protected List<CellLocation> PossiblePerimeterLocations (Room room) {
		
		List<CellLocation> possiblePerimeterLocations = new List<CellLocation> ();
		
		foreach (CellLocation location in PotentialPerimeterLocations(room)) {
			
			CellType testType = dungeonGenerator.cellTypeGrid[location.x, location.y, location.z];
			
			if (PerimeterCanReplace(testType)) {
				
				possiblePerimeterLocations.Add (location);
			}
		}
		
		return possiblePerimeterLocations;
	}
	
	public void GeneratePerimeter (Room room) {
		
		foreach (CellLocation location in PossiblePerimeterLocations (room)) {
			
			AddPerimeter (location);
		}
	}
	
	protected void AddPerimeter (CellLocation location) {
		
		dungeonGenerator.cellTypeGrid[location.x, location.y, location.z] = CellType.Perimeter;
	}
	
	protected bool StairsPerimeterCanReplace (CellType type) {
		
		if (type == CellType.Blocked || type == CellType.StairsPerimeter || type == CellType.Entrance) {
			
			return false;
		}
		return true;
	}
	
	protected List<CellLocation> PotentialStairsPerimeterLocations (Stairs stairs) {
		
		List<CellLocation> potentialStairsPerimeterLocations = new List<CellLocation> ();
		
		for (var i = stairs.location.x - 1; i < stairs.location.x + 2; i++) {
			
			potentialStairsPerimeterLocations.Add (new CellLocation(i, stairs.location.y, stairs.location.z - 1));
			potentialStairsPerimeterLocations.Add (new CellLocation(i, stairs.location.y, stairs.location.z + 1));
		}
		
		for (var j = stairs.location.z; j < stairs.location.z + 1; j ++) {
			
			potentialStairsPerimeterLocations.Add (new CellLocation(stairs.location.x - 1, stairs.location.y, j));
			potentialStairsPerimeterLocations.Add (new CellLocation(stairs.location.x + 1, stairs.location.y, j));
		}
		
		return potentialStairsPerimeterLocations;
	}
	
	protected List<CellLocation> PossibleStairsPerimeterLocations (Stairs stairs) {
		
		List<CellLocation> possibleStairsPerimeterLocations = new List<CellLocation> ();
		
		foreach (CellLocation location in PotentialStairsPerimeterLocations(stairs)) {
			
			CellType testType = dungeonGenerator.cellTypeGrid[location.x, location.y, location.z];
			
			if (StairsPerimeterCanReplace(testType)) {
				
				possibleStairsPerimeterLocations.Add (location);
			}
		}
		
		return possibleStairsPerimeterLocations;
	}
	
	public void GenerateStairsPerimeter (Stairs stairs) {
		
		foreach (CellLocation location in PossibleStairsPerimeterLocations (stairs)) {
			
			AddStairsPerimeter (location);
		}
	}
	
	protected void AddStairsPerimeter (CellLocation location) {
		
		dungeonGenerator.cellTypeGrid[location.x, location.y, location.z] = CellType.StairsPerimeter;
	}
	
	public void ConvertToDungeonPerimeter () {
		
		for (var i = 0; i < dungeonGenerator.numCellsX; i++) {
			
			for (var j = 0; j < dungeonGenerator.numCellsZ; j++) {
				
				CellType testType = dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j];
				
				if (testType == CellType.Blocked || testType == CellType.Empty || testType == CellType.Perimeter || testType == CellType.StairsPerimeter) {
					
					CellLocation location = new CellLocation(i, dungeonGenerator.floorNumber, j);
					
					if (dungeonGenerator.HasDir (location, Dir.North)
					|| dungeonGenerator.HasDir (location, Dir.South)
					|| dungeonGenerator.HasDir (location, Dir.East)
					|| dungeonGenerator.HasDir (location, Dir.West)
					|| dungeonGenerator.HasDir (location, Dir.NorthEast)
					|| dungeonGenerator.HasDir (location, Dir.NorthWest)
					|| dungeonGenerator.HasDir (location, Dir.SouthEast)
					|| dungeonGenerator.HasDir (location, Dir.SouthWest)
					) {
				
						dungeonGenerator.cellTypeGrid[location.x, location.y, location.z] = CellType.DungeonPerimeter;
					}	
				}
			}
		}
	}
	
	/// <summary>
	/// Used for initialization.
	/// </summary>
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		roomGenerator = GetComponent<RoomGenerator> ();
	}
}
