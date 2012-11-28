using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveGenerator : MonoBehaviour {
	
	protected DungeonGenerator dungeonGenerator;
	
	public CellType[,,] caveCellGrid;
	
	public void GenerateCave (Floor floor) {
		
		FillWithRock (floor);
//		RandomlyPlaceFloor (floor, 50);
		
		RandomFloor (floor, 50);
		
		Generate (floor);
//		
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		RandomlyDistrabute (floor, 50);
		Generate (floor);
		Generate (floor);
		Generate (floor);
		Generate (floor);
		Generate (floor);
	}
	
	protected void FillWithRock (Floor floor) {
		
		for (var i = 0; i < dungeonGenerator.numCellsX; i++) {
			
			for (var j = 0; j < dungeonGenerator.numCellsZ; j++) {
			
				caveCellGrid[i, floor.floorNumber, j] = CellType.Rock;
			}
		}
	}
	
	protected void RandomFloor(Floor floor, int percentage) {
		
		List<CellLocation> locationsChosen = new List<CellLocation> ();
		
		List<CellLocation> possibleLocations = new List<CellLocation> ();
		
		for (var i = 1; i < dungeonGenerator.numCellsX - 1; i++) {
			
			for (var j = 1; j < dungeonGenerator.numCellsZ - 1; j++) {
			
				CellLocation possibleLocation = new CellLocation(i, floor.floorNumber, j);
				
				possibleLocations.Add (possibleLocation);
			}
		}
		
		int locationsNeeded = (int)((float)possibleLocations.Count * (float)(percentage) / 100);
		int possibleLocationsLeft = possibleLocations.Count;
		
		
		foreach (CellLocation location in possibleLocations) {
			
			int probability = (int)((float)locationsNeeded * 100 / (float)possibleLocationsLeft);
			
			int rnd = Random.Range (0, 100);
				
			if (rnd < probability) {
					
				locationsChosen.Add (location);
				locationsNeeded--;
			}
			
			possibleLocationsLeft--;
			
			if (locationsNeeded == 0) break;
		}
		
		foreach (CellLocation location in locationsChosen) {
			
			caveCellGrid[location.x, location.y, location.z] = CellType.RockFloor;
		}
	}
	
	protected void RandomlyPlaceFloor (Floor floor, int percentage) {
		
		for (var i = 1; i < dungeonGenerator.numCellsX - 1; i++) {
			
			for (var j = 1; j < dungeonGenerator.numCellsZ - 1; j++) {
			
				int rnd = Random.Range (1, 100);
				
				if (rnd < percentage) {
					
					caveCellGrid[i, floor.floorNumber, j] = CellType.RockFloor;
				}
			}
		}
	}
	
	protected void RandomlyDistrabute (Floor floor, int percentage) {
		
		List<CellLocation> locationsToModify = new List<CellLocation> ();
		
		for (var i = 1; i < dungeonGenerator.numCellsX - 1; i++) {
			
			for (var j = 1; j < dungeonGenerator.numCellsZ - 1; j++) {
				
				CellLocation location = new CellLocation(i, floor.floorNumber, j);
				
				int numRocks = 0;
				if (caveCellGrid[location.x, location.y, location.z] == CellType.Rock) numRocks++;
			
				if (!dungeonGenerator.HasDir (location, Dir.North)) numRocks++;
				if (!dungeonGenerator.HasDir (location, Dir.South)) numRocks++;
				if (!dungeonGenerator.HasDir (location, Dir.East)) numRocks++;
				if (!dungeonGenerator.HasDir (location, Dir.West)) numRocks++;
				if (!dungeonGenerator.HasDir (location, Dir.NorthEast)) numRocks++;
				if (!dungeonGenerator.HasDir (location, Dir.NorthWest)) numRocks++;
				if (!dungeonGenerator.HasDir (location, Dir.SouthEast)) numRocks++;
				if (!dungeonGenerator.HasDir (location, Dir.SouthWest)) numRocks++;
				
				if (numRocks == 0 || numRocks == 9) {
					
					locationsToModify.Add (location);
				}
			}
		}
		
		foreach (CellLocation location in locationsToModify) {
			
			int rnd = Random.Range (1, 100);
				
				if (rnd < percentage) {
					
					if (caveCellGrid[location.x, location.y, location.z] == CellType.RockFloor)
					caveCellGrid[location.x, location.y, location.z] = CellType.Rock;
					
					else if (caveCellGrid[location.x, location.y, location.z] == CellType.Rock)
					caveCellGrid[location.x, location.y, location.z] = CellType.RockFloor;
				}
		}
	}
	
	protected void Generate (Floor floor) {
		
		for (var i = 1; i < dungeonGenerator.numCellsX - 1; i++) {
			
			for (var j = 1; j < dungeonGenerator.numCellsZ - 1; j++) {
				
				CellLocation location = new CellLocation (i, floor.floorNumber, j);
				
				int numRocks = 0;
				if (caveCellGrid[location.x, location.y, location.z] == CellType.Rock) numRocks++;
				
				if (!HasDir (location, Dir.North)) numRocks++;
				if (!HasDir (location, Dir.South)) numRocks++;
				if (!HasDir (location, Dir.East)) numRocks++;
				if (!HasDir (location, Dir.West)) numRocks++;
				if (!HasDir (location, Dir.NorthEast)) numRocks++;
				if (!HasDir (location, Dir.NorthWest)) numRocks++;
				if (!HasDir (location, Dir.SouthEast)) numRocks++;
				if (!HasDir (location, Dir.SouthWest)) numRocks++;
				
				if (numRocks > 4) {
					
					caveCellGrid[location.x, location.y, location.z] = CellType.Rock;
				}
				else caveCellGrid[location.x, location.y, location.z] = CellType.RockFloor;
			}
		}
	}
	
	protected bool CavesCanReplace (CellType type) {
		
		if (type == CellType.Empty || type == CellType.Perimeter || type == CellType.Blocked || type == CellType.StairsPerimeter) {
			
			return true;
		}
		return false;
	}
	
	public void AddCavesToDungeon (Floor floor) {
		
		for (int i = 0; i < dungeonGenerator.numCellsX; i++) {
			
			for (int j = 0; j < dungeonGenerator.numCellsZ; j++) {
			
				CellType testType = dungeonGenerator.cellTypeGrid[i, floor.floorNumber, j];
				
				if (CavesCanReplace (testType)) {
					
					CellLocation location = new CellLocation(i, floor.floorNumber, j);
					
					dungeonGenerator.cellTypeGrid[location.x, location.y, location.z] = caveCellGrid[location.x, location.y, location.z];
				}
			}
		}
	}
	
	public bool HasDir (CellLocation location, Dir dir) {
		
		CellType testType;
		
		if (dir == Dir.North)
			testType = caveCellGrid[location.x, location.y, location.z + 1];
		
		else if (dir == Dir.South) 
			testType = caveCellGrid[location.x, location.y, location.z - 1];
		
		else if (dir == Dir.East) 
			testType = caveCellGrid[location.x + 1, location.y, location.z];
		
		else if (dir == Dir.West) 
			testType = caveCellGrid[location.x - 1, location.y, location.z];
		
		else if (dir == Dir.NorthEast) 
			testType = caveCellGrid[location.x + 1, location.y, location.z + 1];
		
		else if (dir == Dir.NorthWest) 
			testType = caveCellGrid[location.x - 1, location.y, location.z + 1];
		
		else if (dir == Dir.SouthEast) 
			testType = caveCellGrid[location.x + 1, location.y, location.z - 1];
		
		else 
			testType = caveCellGrid[location.x - 1, location.y, location.z - 1];
		
		if (testType == CellType.RockFloor) {
			
			return true;
		}
		
		return false;
	}
	
	// Use this for initialization
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		
		caveCellGrid = new CellType[dungeonGenerator.numCellsX, dungeonGenerator.numFloors, dungeonGenerator.numCellsZ];
	}
}