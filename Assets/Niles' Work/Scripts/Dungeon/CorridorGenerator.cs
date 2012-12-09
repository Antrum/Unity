using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to generate corridors.
/// </summary>
public class CorridorGenerator : MonoBehaviour {
	
	public List<Corridor> allCorridors = new List<Corridor> ();
	
	protected DungeonGenerator dungeonGenerator;
	
	protected bool CorridorCanReplace (CellType type) {
		
		if (type == CellType.Empty || type == CellType.RockFloor || type == CellType.Rock) {
			
			return true;
		}
		return false;
	}
	
	protected List<CellLocation> PotentialCorridorLocations () {
		
		List<CellLocation> potentialCorridorLocations = new List<CellLocation> ();
		
		for (var i = 1; i < dungeonGenerator.numCellsX - 1; i+= 2) {
			
			for (var j = 1; j < dungeonGenerator.numCellsZ - 1; j += 2) {
				
				CellLocation location = new CellLocation (i, dungeonGenerator.floorNumber, j);
				
				potentialCorridorLocations.Add (location);
			}
		}
		
		return potentialCorridorLocations;
	}
	
	protected List<CellLocation> PossibleCorridorLocations () {
		
		List<CellLocation> possibleCorridorLocations = new List<CellLocation> ();
		
		foreach (CellLocation location in PotentialCorridorLocations ()) {
			
			CellType testType = dungeonGenerator.cellTypeGrid[location.x, location.y, location.z];
			
			if (CorridorCanReplace(testType)) {
				
				possibleCorridorLocations.Add (location);
			}
		}
		
		return possibleCorridorLocations;
	}
	
	protected bool CorridorCanGo (Corridor corridor, Dir direction) {
		
		CellType testType = CellType.Blocked;
		
		if (direction == Dir.North) {
			
			if (corridor.location.z < dungeonGenerator.numCellsZ - 3) {
				
				testType = dungeonGenerator.cellTypeGrid[corridor.location.x, corridor.location.y, corridor.location.z + 2];
			}
		}
		else if (direction == Dir.South) {
			
			if (corridor.location.z > 2) {
			
				testType = dungeonGenerator.cellTypeGrid[corridor.location.x, corridor.location.y, corridor.location.z - 2];
			}
		}
		else if (direction == Dir.East) {
			
			if (corridor.location.x < dungeonGenerator.numCellsX - 3) {
				
				testType = dungeonGenerator.cellTypeGrid[corridor.location.x + 2, corridor.location.y, corridor.location.z];
			}
		}
		else if (direction == Dir.West) {
			
			if (corridor.location.x > 2) {
				
				testType = dungeonGenerator.cellTypeGrid[corridor.location.x - 2, corridor.location.y, corridor.location.z];
			}
		}
		
		if (CorridorCanReplace (testType)) {
			
			return true;
		}
		
		return false;
	}
	
	public void ConnectEntrances (Entrance entranceA, Entrance entranceB) {
		
		if (entranceA.location == entranceB.location) return;
		
		CellLocation startLocation;
		if (entranceA.direction == Dir.North) startLocation = entranceA.location.getNorth ();
		else if (entranceA.direction == Dir.South) startLocation = entranceA.location.getSouth ();
		else if (entranceA.direction == Dir.East) startLocation = entranceA.location.getEast ();
		else startLocation = entranceA.location.getWest ();
		
		CellLocation goalLocation;
		if (entranceB.direction == Dir.North) goalLocation = entranceB.location.getNorth ();
		else if (entranceB.direction == Dir.South) goalLocation = entranceB.location.getSouth ();
		else if (entranceB.direction == Dir.East) goalLocation = entranceB.location.getEast ();
		else goalLocation = entranceB.location.getWest ();
		
		
		Corridor corridorStart = new Corridor (startLocation);
		AddCorridor (corridorStart);
		
		Corridor corridorGoal = new Corridor (goalLocation);
		AddCorridor (corridorGoal);
	}
	
	public void StartNewCorridor () {
		
		if (PossibleCorridorLocations ().Count > 0) {
			
			CellLocation startLocation = PossibleCorridorLocations ()[0];
			GenerateCorridor (startLocation);
		}
	}
	
	protected void GenerateCorridor (CellLocation location) {
		
		Corridor corridor = new Corridor (location);
		
		AddCorridor (corridor);
		
		Dir wayToGo = Dir.None;
		List<Dir> possibleDirections = new List<Dir> ();
		
		if (CorridorCanGo (corridor, Dir.North)) possibleDirections.Add (Dir.North);
		if (CorridorCanGo (corridor, Dir.South)) possibleDirections.Add (Dir.South);
		if (CorridorCanGo (corridor, Dir.East)) possibleDirections.Add (Dir.East);
		if (CorridorCanGo (corridor, Dir.West)) possibleDirections.Add (Dir.West);
		
		if (possibleDirections.Count > 0) {
			
			int randomIndex = Random.Range (1, possibleDirections.Count) - 1;
			wayToGo = possibleDirections[randomIndex];
		}
		
		if (wayToGo != Dir.None) {
			
			CellLocation newLocation;
			CellLocation middleLocation;
			Corridor middleCorridor;
			
			if (wayToGo == Dir.North) {
				
				newLocation = new CellLocation(location.x, location.y, location.z + 2);
				middleLocation = new CellLocation(location.x, location.y, location.z + 1);
			}
			else if (wayToGo == Dir.South) {
				
				newLocation = new CellLocation(location.x, location.y, location.z - 2);
				middleLocation = new CellLocation(location.x, location.y, location.z - 1);
			}
			else if (wayToGo == Dir.East) {
				
				newLocation = new CellLocation(location.x + 2, location.y, location.z);
				middleLocation = new CellLocation(location.x + 1, location.y, location.z);
			}
			else {
				
				newLocation = new CellLocation(location.x - 2, location.y, location.z);
				middleLocation = new CellLocation(location.x - 1, location.y, location.z);
			}
			
			middleCorridor = new Corridor (middleLocation);
			AddCorridor (middleCorridor);
			
			GenerateCorridor (newLocation);
			
		} else {
			
			StartNewCorridor ();
		}
	}
	
	public void CleanDeadEnds () {
		
		List<Corridor> corridorsToRemove = new List<Corridor> ();
		
		bool finished = false;
		
		while (!finished) {
		
			foreach (Corridor corridor in allCorridors) {
				
				int numberOfConnections = 0;
				
				if (dungeonGenerator.HasDir(corridor.location, Dir.North)) numberOfConnections++;
				if (dungeonGenerator.HasDir(corridor.location, Dir.South)) numberOfConnections++;
				if (dungeonGenerator.HasDir(corridor.location, Dir.East)) numberOfConnections++;
				if (dungeonGenerator.HasDir(corridor.location, Dir.West)) numberOfConnections++;
				
				if (numberOfConnections < 2) corridorsToRemove.Add (corridor);
			}
			
			if (corridorsToRemove.Count == 0) finished = true;
			
			foreach (Corridor corridor in corridorsToRemove) {
				
				RemoveCorridor (corridor);
			}
			
			corridorsToRemove.Clear ();
		}
	}
	
	protected void AddCorridor (Corridor corridor) {
		
		allCorridors.Add (corridor);
		dungeonGenerator.cellTypeGrid[corridor.location.x, corridor.location.y, corridor.location.z] = CellType.Corridor;
	}
	
	protected void RemoveCorridor (Corridor corridor) {
		
		allCorridors.Remove (corridor);
		dungeonGenerator.cellTypeGrid[corridor.location.x, corridor.location.y, corridor.location.z] = CellType.Empty;
	}
	
	/// <summary>
	/// Used for initialization.
	/// </summary>
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
	}
}
