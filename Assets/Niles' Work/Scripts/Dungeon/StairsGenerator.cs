using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to generate stairs.
/// </summary>
public class StairsGenerator : MonoBehaviour {
	
	protected DungeonGenerator dungeonGenerator;
	protected RoomGenerator roomGenerator;
	protected PerimeterGenerator perimeterGenerator;
	protected EntranceGenerator entranceGenerator;
	protected SceneryGenerator sceneryGenerator;
	
	public int NumStairsDown = 1;
	
	public List<Stairs> allStairs = new List<Stairs> ();
	public List<Stairs> allStairsUp = new List<Stairs> ();
	public List<Stairs> allStairsDown = new List<Stairs> ();

	protected bool StairsCanReplace (CellType type) {
		
		if (type != CellType.StairsUp && type != CellType.StairsDown && type != CellType.StairsPerimeter) {
			
			return true;
		}
		return false;
	}
	
	protected List<CellLocation> PotentialStairsLocations () {
		
		List<CellLocation> potentialStairsLocations = new List<CellLocation> ();
		
		for (var i = 3; i < dungeonGenerator.numCellsX - 3; i+= 2) {
			
			for (var j = 3; j < dungeonGenerator.numCellsZ - 3; j += 2) {
				
				CellLocation location = new CellLocation (i, dungeonGenerator.floorNumber, j);
				
				potentialStairsLocations.Add (location);
			}
		}
		
		return potentialStairsLocations;
	}
	
	protected List<CellLocation> PossibleStairsLocations () {
		
		List<CellLocation> possibleStairsLocations = new List<CellLocation> ();
		
		int maxRoomLength = roomGenerator.maxRoomLength;
		
		foreach (CellLocation location in PotentialStairsLocations ()) {
			
			CellType testType = dungeonGenerator.cellTypeGrid[location.x, location.y, location.z];
			
			if (StairsCanReplace(testType)) {
				
				possibleStairsLocations.Add (location);
			}
			
			if (location.x > dungeonGenerator.CenterOfGrid ().x - (maxRoomLength + 1) / 2
				&& location.x < dungeonGenerator.CenterOfGrid ().x + (maxRoomLength + 1) / 2
				&& location.z > dungeonGenerator.CenterOfGrid ().z - (maxRoomLength + 1) / 2
				&& location.z < dungeonGenerator.CenterOfGrid ().z + (maxRoomLength + 1) / 2) {
				
				possibleStairsLocations.Remove (location);
			}
			
			foreach (Stairs stairs in allStairs) {
				
				if (location.x > stairs.location.x - 5
				&& location.x < stairs.location.x + 5
				&& location.z > stairs.location.z - 5
				&& location.z < stairs.location.z + 5) {
					
					possibleStairsLocations.Remove (location);
				}
			}
		}
		
		return possibleStairsLocations;
	}
	
	protected Dir RandomDirection (Stairs stairs) {
		
		int randomInt = Random.Range (1, 5);
		Dir randomDirection;
		
		if (randomInt == 1) randomDirection = Dir.North;
		else if (randomInt == 2) randomDirection = Dir.South;
		else if (randomInt == 3) randomDirection = Dir.East;
		else randomDirection = Dir.West;
		
		return randomDirection;
	}
	
	public void GenerateStairsUp (Floor floor) {
		
		if (floor.floorNumber != 0) {
		
			foreach (Stairs stairsDown in dungeonGenerator.floors[dungeonGenerator.floorNumber - 1].allStairsDown) {
				
				CellLocation location = new CellLocation (stairsDown.location.x, dungeonGenerator.floorNumber, stairsDown.location.z);
				
				Stairs stairsUp = new Stairs (location, 0);
				
				stairsUp.direction = stairsDown.direction;
				stairsUp.rotation = stairsDown.rotation;
				
				AddStairsUp (stairsUp);
			}
		}
	}
	
	public void GenerateStairsDown (Floor floor) {
		
		for (int i = 0; i < NumStairsDown; i++) {
			
			int randomIndex = Random.Range (1, PossibleStairsLocations ().Count) - 1;
			
			CellLocation chosenLocation = PossibleStairsLocations ()[randomIndex];
			
			Stairs stairs = new Stairs (chosenLocation, 0);
			
			stairs.direction = RandomDirection (stairs);
			if (stairs.direction == Dir.North) stairs.rotation = 0;
			else if (stairs.direction == Dir.East) stairs.rotation = 90;
			else if (stairs.direction == Dir.South) stairs.rotation = 180;
			else stairs.rotation = 270;
		
		AddStairsDown (stairs);
			
		}
	}
	
	public void AddStairsUp (Stairs stairs) {
		
		stairs.stairsUp = true;
		
		allStairs.Add (stairs);
		allStairsUp.Add (stairs);
		
		dungeonGenerator.cellTypeGrid[stairs.location.x, stairs.location.y, stairs.location.z] = CellType.StairsUp;
		
		entranceGenerator.GenerateEntranceToStairs (stairs);
		perimeterGenerator.GenerateStairsPerimeter (stairs);
	}
	
	public void AddStairsDown (Stairs stairs) {
		
		stairs.stairsDown = true;
		
		allStairs.Add (stairs);
		allStairsDown.Add (stairs);
		
		dungeonGenerator.cellTypeGrid[stairs.location.x, stairs.location.y, stairs.location.z] = CellType.StairsDown;
		
		entranceGenerator.GenerateEntranceToStairs (stairs);
		perimeterGenerator.GenerateStairsPerimeter (stairs);
	}
	
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		roomGenerator = GetComponent<RoomGenerator> ();
		perimeterGenerator = GetComponent<PerimeterGenerator> ();
		entranceGenerator = GetComponent<EntranceGenerator> ();
		sceneryGenerator = GetComponent<SceneryGenerator> ();
		
	}
}
