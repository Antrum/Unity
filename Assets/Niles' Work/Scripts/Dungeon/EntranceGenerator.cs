/// <summary>
/// Entrance generator.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ENTRANCE GENERATOR:

public class EntranceGenerator : MonoBehaviour {
	
	// INSTANCES:
	
	/// <summary>
	/// The dungeon generator.
	/// </summary>
	protected DungeonGenerator dungeonGenerator;
	
	protected CorridorGenerator corridorGenerator;
	
	// VARIABLES:
	
	/// <summary>
	/// Reflect entrances in x.
	/// </summary>
	[HideInInspector]
	public bool reflectX;
	
	/// <summary>
	/// Reflect entrances in z.
	/// </summary>
	[HideInInspector]
	public bool reflectZ;
	
	/// <summary>
	/// All entrances.
	/// </summary>
	public List<Entrance> allEntrances = new List<Entrance> ();
	
	// ENTRANCE SPAWN AREA:
	
	/// <summary>
	/// The spawn area.
	/// </summary>
	/// <returns>The area.</returns>
	protected EntranceSpawnArea SpawnArea () {
		
		// Spawn area location
		CellLocation spawnAreaLocation = new CellLocation (1, dungeonGenerator.floorNumber, 1);
		
		// Spawn area size
		IntVector3 spawnAreaSize = new IntVector3 (dungeonGenerator.numCellsX - 2, 1, dungeonGenerator.numCellsZ - 2);
		
		// Half the size in x
		if (reflectX) spawnAreaSize.x = (spawnAreaSize.x + 1) / 2;
		
		// Half the size in z
		if (reflectZ) spawnAreaSize.z = (spawnAreaSize.z + 1) / 2;
		
		// The spawn area
		EntranceSpawnArea spawnArea = new EntranceSpawnArea(spawnAreaLocation, spawnAreaSize);
		
		// Return the spawn area
		return spawnArea;
	}
	
	/// <summary>
	/// Within the spawn area.
	/// </summary>
	/// <returns>Within spawn area.</returns>
	/// <param name='location'>If set to <c>true</c> location.</param>
	protected bool WithinSpawnArea (CellLocation location) {
		
		if (location.x >= SpawnArea ().location.x && location.x < SpawnArea ().location.x + SpawnArea ().size.x) {
			
			if (location.z >= SpawnArea ().location.z && location.z < SpawnArea ().location.z + SpawnArea ().size.z) {
				
				// Within the spawn area
				return true;
			}
		}
		
		// Outside of the spawn area
		return false;
	}
	
	// POTENTIAL ENTRANCE LOCATIONS:
	
	/// <summary>
	/// All potential entrance locations.
	/// </summary>
	/// <returns>The entrance locations.</returns>
	/// <param name='room'>Room.</param>
	/// <param name='ignoreReflection'>Ignore reflection.</param>
	protected List<CellLocation> PotentialEntranceLocations (Room room, bool ignoreReflection) {
		
		// All of the potential entrance locations
		List<CellLocation> potentialEntranceLocations = new List<CellLocation> ();
		
		// All the directions
		List<Dir> directions = new List<Dir> ();
		
		// Add each direction to directions
		directions.Add (Dir.North);
		directions.Add (Dir.South);
		directions.Add (Dir.East);
		directions.Add (Dir.West);
		
		foreach (CellLocation location in PotentialEntranceLocations (room, directions, ignoreReflection))
			
			// Add location to potential entrance locations
			potentialEntranceLocations.Add (location);
		
		// Return all of the potential entrance locations
		return potentialEntranceLocations;
	}
	
	/// <summary>
	/// Potential entrance locations.
	/// </summary>
	/// <returns>The entrance locations.</returns>
	/// <param name='room'>Room.</param>
	/// <param name='ignoreReflection'>Ignore reflection.</param>
	protected List<CellLocation> PotentialEntranceLocations (Room room, List<Dir> directions, bool ignoreReflection) {
		
		// All of the potential entrance locations
		List<CellLocation> potentialEntranceLocations = new List<CellLocation> ();
		
		foreach (Dir direction in directions) {
			
			if (direction == Dir.North) {
			
				for (var i = room.location.x - (room.size.x - 1) / 2; i < room.location.x + (room.size.x + 1) / 2; i += 2) {
			
					// The potential northern location
					CellLocation potentialNorthLocation = new CellLocation(i, room.location.y, room.location.z + (room.size.z + 2) / 2);
					
					if (WithinSpawnArea (potentialNorthLocation) || ignoreReflection) 
						
						// Add to the potential northern entrance location
						potentialEntranceLocations.Add (potentialNorthLocation);
				}
			}
			
			if (direction == Dir.South) {
				
				for (var i = room.location.x - (room.size.x - 1) / 2; i < room.location.x + (room.size.x + 1) / 2; i += 2) {
			
					// The potential southern location
					CellLocation potentialSouthLocation = new CellLocation(i, room.location.y, room.location.z - (room.size.z + 1) / 2);
					
					if (WithinSpawnArea (potentialSouthLocation) || ignoreReflection) 
						
						// Add to the potential southern entrance location
						potentialEntranceLocations.Add (potentialSouthLocation);
				}
			}
			
			if (direction == Dir.East) {
				
				for (var j = room.location.z - (room.size.z - 1) / 2; j < room.location.z + (room.size.z + 1) / 2; j += 2) {
					
					// The potential eastern location
					CellLocation potentialEastLocation = new CellLocation(room.location.x + (room.size.x + 2) / 2, room.location.y, j);
					
					if (WithinSpawnArea (potentialEastLocation) || ignoreReflection) 
						
						// Add to the potential eastern entrance locations
						potentialEntranceLocations.Add (potentialEastLocation);
				}
			}
			
			if (direction == Dir.West) {
				
				for (var j = room.location.z - (room.size.z - 1) / 2; j < room.location.z + (room.size.z + 1) / 2; j += 2) {
					
					// The potential western location
					CellLocation potentialWestLocation = new CellLocation(room.location.x - (room.size.x + 1) / 2, room.location.y, j);
					
					if (WithinSpawnArea (potentialWestLocation) || ignoreReflection) 
						
						// Add to the potential western entrance locations
						potentialEntranceLocations.Add (potentialWestLocation);
				}
			}
		}
		
		// Return all of the potential entrance locations in direction
		return potentialEntranceLocations;
	}
	
	// POSSIBLE ENTRANCE LOCATIONS:
	
	/// <summary>
	/// Entrance can replace.
	/// </summary>
	/// <returns>Can replace.</returns>
	/// <param name='type'>If set to <c>true</c> type.</param>
	protected bool EntranceCanReplace (CellType type) {
		
		if (type == CellType.Blocked || type == CellType.StairsPerimeter || type == CellType.Entrance) {
			
			// Entrance cannot replace type
			return false;
		}
		
		// Entrance can replace type
		return true;
	}
	
	/// <summary>
	/// All possible entrance locations.
	/// </summary>
	/// <returns>The entrance locations.</returns>
	/// <param name='room'>Room.</param>
	/// <param name='ignoreReflection'>Ignore reflection.</param>
	public List<CellLocation> PossibleEntranceLocations (Room room, bool ignoreReflection) {
		
		// The possible entrance locations
		List<CellLocation> possibleEntranceLocations = new List<CellLocation> ();
		
		foreach (CellLocation location in PotentialEntranceLocations(room, ignoreReflection)) {
			
			// The test type
			CellType testType = dungeonGenerator.cellTypeGrid[location.x, location.y, location.z];
			
			if (EntranceCanReplace(testType)) {
				
				// Add to potential entrance locations
				possibleEntranceLocations.Add (location);
			}
		}
		
		// Return the possible entrance locations
		return possibleEntranceLocations;
	}
	
	/// <summary>
	/// Possible entrance locations.
	/// </summary>
	/// <returns>The entrance locations.</returns>
	/// <param name='room'>Room./param>
	/// <param name='directions'>Directions./param>
	/// <param name='ignoreReflection'>Ignore reflection.</param>
	public List<CellLocation> PossibleEntranceLocations (Room room, List<Dir> directions, bool ignoreReflection) {
		
		// The possible entrance locations
		List<CellLocation> possibleEntranceLocations = new List<CellLocation> ();
		
		foreach (CellLocation location in PotentialEntranceLocations(room, directions, ignoreReflection)) {
			
			// The test type
			CellType testType = dungeonGenerator.cellTypeGrid[location.x, location.y, location.z];
			
			if (EntranceCanReplace(testType)) {
				
				// Add to potential entrance locations
				possibleEntranceLocations.Add (location);
			}
		}
		
		// Return the possible entrance locations
		return possibleEntranceLocations;
	}
	
	// DECIDE ON ENTRANCES:
	
	private List<Entrance> ClosestEntrancePair (Room roomA, List<Dir> directionsA, Room roomB, List<Dir> directionsB) {
		
		List<Entrance> closestEntrancePair = new List<Entrance> ();
		
		CellLocation locationA = PossibleEntranceLocations (roomA, directionsA, true)[0];
		CellLocation locationB = PossibleEntranceLocations (roomB, directionsB, true)[0];
		
		int shortestDistance = 100;
		
		foreach (CellLocation testLocationA in PossibleEntranceLocations (roomA, directionsA, true)) {
			
			foreach (CellLocation testLocationB in PossibleEntranceLocations (roomB, directionsB, true)) {
			
				int currentDistance = Mathf.Abs (testLocationA.x - testLocationB.x) + Mathf.Abs (testLocationA.z - testLocationB.z);
				
				if (currentDistance < shortestDistance) {
					
					shortestDistance = currentDistance;
					locationA = testLocationA;
					locationB = testLocationB;
				}
			}
		}
		
		Entrance entranceA = new Entrance (locationA);
		entranceA.direction = directionsA[0];
		
		Entrance entranceB = new Entrance (locationB);
		entranceB.direction = directionsB[0];
		
		closestEntrancePair.Add (entranceA);
		closestEntrancePair.Add (entranceB);
		
		return closestEntrancePair;
	}
	
	public void GenerateClosestEntrancePair (Room roomA, List<Dir> directionsA, Room roomB, List<Dir> directionsB) {
		
		Entrance entranceA = ClosestEntrancePair (roomA, directionsA, roomB, directionsB)[0];
		Entrance entranceB = ClosestEntrancePair (roomA, directionsA, roomB, directionsB)[1];
		
		AddEntrance (entranceA);
		AddEntrance (entranceB);
		
		corridorGenerator.ConnectEntrances (entranceA, entranceB);
	}
	
	// ENTRANCE GENERATION:

	/// <summary>
	/// Reasonable number of entrances.
	/// </summary>
	/// <returns>The number of entrances.</returns>
	/// <param name='room'>Room.</param>
	protected int ReasonableNumberOfEntrances (Room room) {
		
		// The minimum ammount of entrances to make per room
		int minEntrancesPerRoom = 2;
		
		// The maximum ammount of entrances to make per room
		int targetMaxEntrancesPerRoom = 4;
		
		// The possible number of entrances
		int possibleNumberOfEntrances = PossibleEntranceLocations (room, false).Count;
		
		// The reasonable number of entrances
		int reasonableNumberOfEntrances;
		
		if (possibleNumberOfEntrances < minEntrancesPerRoom) {
			
			// The maximum reasonable number of entrances
			reasonableNumberOfEntrances = possibleNumberOfEntrances;
			
		} else {
			
			// Set the reasonable number of entrances
			reasonableNumberOfEntrances = (int)(possibleNumberOfEntrances / 4) - 1;
			
			if (reasonableNumberOfEntrances < minEntrancesPerRoom) {
				
				// The minimum reasonable number of entrances
				reasonableNumberOfEntrances = minEntrancesPerRoom;
			}
			
			else if (reasonableNumberOfEntrances > targetMaxEntrancesPerRoom) {
				
				// The maximum reasonable number of entrances
				reasonableNumberOfEntrances = targetMaxEntrancesPerRoom;
			}
		}
		
		// Return the reasonable number of entrances
		return reasonableNumberOfEntrances;
	}
	
	/// <summary>
	/// Generates the entrance to stairs.
	/// </summary>
	/// <param name='stairs'>Stairs.</param>
	public void GenerateEntranceToStairs (Stairs stairs) {
		
		// The entrance location
		CellLocation entranceLocation;
			
		if (stairs.stairsUp) {
			
			if (stairs.direction == Dir.North) 
				
				// Set the entrance location north of the upward stairs
				entranceLocation = new CellLocation (stairs.location.x, stairs.location.y, stairs.location.z + 1);
			
			else if (stairs.direction == Dir.South) 
				
				// Set the entrance location south of the upward stairs
				entranceLocation = new CellLocation (stairs.location.x, stairs.location.y, stairs.location.z - 1);
			
			else if (stairs.direction == Dir.East) 
				
				// Set the entrance location east of the upward stairs
				entranceLocation = new CellLocation (stairs.location.x + 1, stairs.location.y, stairs.location.z);
			
			else 
				
				// Set the entrance location west of the upward stairs
				entranceLocation = new CellLocation (stairs.location.x - 1, stairs.location.y, stairs.location.z);
		}
		else {
			
			if (stairs.direction == Dir.North) 
				
				// Set the entrance location north of the downward stairs
				entranceLocation = new CellLocation (stairs.location.x, stairs.location.y, stairs.location.z - 1);
			
			else if (stairs.direction == Dir.South) 
				
				// Set the entrance location south of the downward stairs
				entranceLocation = new CellLocation (stairs.location.x, stairs.location.y, stairs.location.z + 1);
			
			else if (stairs.direction == Dir.East) 
				
				// Set the entrance location east of the downward stairs
				entranceLocation = new CellLocation (stairs.location.x - 1, stairs.location.y, stairs.location.z);
			
			else 
				
				// Set the entrance location west of the downward stairs
				entranceLocation = new CellLocation (stairs.location.x + 1, stairs.location.y, stairs.location.z);
		}
		
		// The entrance
		Entrance entrance = new Entrance (entranceLocation);
		
		// State that it is an entrance to stairs
		entrance.entranceToStairs = true;
		
		// Add the entrance
		AddEntrance (entrance);
	}
	
	/// <summary>
	/// Generates all entrances.
	/// </summary>
	/// <param name='allRooms'>All rooms.</param>
	/// <param name='floor'>Floor.</param>
	public void GenerateAllEntrances (List<Room> allRooms, Floor floor) {
		
		// Override reflections in x
		if (floor.reflectEntrancesX && floor.reflectRoomsX) reflectX = true;
		else reflectX = false;
		
		// Override reflections in z
		if (floor.reflectEntrancesZ && floor.reflectRoomsZ) reflectZ = true;
		else reflectZ = false;
		
		foreach (Room room in allRooms) {
			
			// Generate entrances
			GenerateEntrancesToRoom (room);
		}
		
		// Reflect entrances in x
		if (reflectX) ReflectX ();
		
		// Reflect entrances in z
		if (reflectZ) ReflectZ ();
	}
	
	/// <summary>
	/// Generates the entrances to room.
	/// </summary>
	/// <param name='room'>Room.</param>
	public void GenerateEntrancesToRoom (Room room) {
		
		bool finished = false;
		
		// Decide reasonable number of entrances
		room.reasonableNumberOfEntrances = ReasonableNumberOfEntrances (room);
		
		while (!finished) {
			
			if (PossibleEntranceLocations (room, false).Count > 0 && room.reasonableNumberOfEntrances > room.entrances.Count) {
				
				// Generate an entrance
				GenerateEntrance (room);
				
			} else {
				
				// Finished generating entrances to room
				finished = true;
			}
		}
	}
	
	/// <summary>
	/// Generates the entrance.
	/// </summary>
	/// <param name='room'>m.</param>
	protected void GenerateEntrance (Room room) {
		
		if (PossibleEntranceLocations (room, false).Count > 0) {
			
			// The random index
			int randomIndex = Random.Range (1, PossibleEntranceLocations (room, false).Count);
			
			// The chosen cell location
			CellLocation chosenLocation = PossibleEntranceLocations (room, false)[randomIndex - 1];
			
			// The entrance
			Entrance entrance = new Entrance (chosenLocation);
			
			// Add the entrance
			AddEntrance (entrance);
			
			// Add the entrance to the rooms data
			room.entrances.Add (entrance);
		}
	}
	
	/// <summary>
	/// Reflects the x.
	/// </summary>
	protected void ReflectX () {
		
		// The entrances to reflect
		List<Entrance> entrancesToReflect = new List<Entrance> ();
		
		foreach (Entrance entrance in allEntrances) {
			
			if (!entrance.entranceToStairs) {
				
				// Add entrance to the entrances to reflect
				entrancesToReflect.Add (entrance);
			}
		}
		
		foreach (Entrance entrance in entrancesToReflect) {
				
			// The reflection location
			CellLocation reflectionLocation = new CellLocation (dungeonGenerator.numCellsX - entrance.location.x - 1, dungeonGenerator.floorNumber, entrance.location.z);
			
			// The reflected entrance
			Entrance reflectedEntrance = new Entrance (reflectionLocation);
			
			// Add the reflected entrance
			AddEntrance (reflectedEntrance);
		}
	}
	
	/// <summary>
	/// Reflects the z.
	/// </summary>
	protected void ReflectZ () {
		
		// The entrances to reflect
		List<Entrance> entrancesToReflect = new List<Entrance> ();
		
		foreach (Entrance entrance in allEntrances) {
			
			if (!entrance.entranceToStairs) {
				
				// Add entrance to the entrances to reflect
				entrancesToReflect.Add (entrance);
			}
		}
		
		foreach (Entrance entrance in entrancesToReflect) {
				
			// The reflection location
			CellLocation reflectionLocation = new CellLocation (entrance.location.x, dungeonGenerator.floorNumber, dungeonGenerator.numCellsZ - entrance.location.z - 1);
			
			// The reflected entrance
			Entrance reflectedEntrance = new Entrance (reflectionLocation);
			
			// Add the reflected entrance
			AddEntrance (reflectedEntrance);
		}
	}
	
	/// <summary>
	/// Cleans the dead entrances.
	/// </summary>
	public void CleanDeadEntrances () {
		
		// The entrances to remove
		List<Entrance> entrancesToRemove = new List<Entrance> ();
		
		foreach (Entrance entrance in allEntrances) {
			
			// The number of directions
			int numDirections = 0;
			
			if (dungeonGenerator.HasDir (entrance.location, Dir.North)) 
				
				// Increment the number of directions
				numDirections++;
			
			if (dungeonGenerator.HasDir (entrance.location, Dir.South)) 
				
				// Increment the number of directions
				numDirections++;
			
			if (dungeonGenerator.HasDir (entrance.location, Dir.East)) 
				
				// Increment the number of directions
				numDirections++;
			
			if (dungeonGenerator.HasDir (entrance.location, Dir.West)) 
				
				// Increment the number of directions
				numDirections++;
			
			if (numDirections < 2 && !entrance.entranceToStairs) 
				
				// Add the entrance to entrances to remove
				entrancesToRemove.Add (entrance);
		}
		
		foreach (Entrance entrance in entrancesToRemove) {
			
			// Remove entrance
			RemoveEntrance (entrance);
		}
	}
	
	/// <summary>
	/// Adds the entrance.
	/// </summary>
	/// <param name='entrance'>Entrance.</param>
	public void AddEntrance (Entrance entrance) {
		
		// The location
		CellLocation location = entrance.location;
		
		// The locations' previus type
		CellType type = dungeonGenerator.cellTypeGrid[location.x, location.y, location.z];
		
		if (EntranceCanReplace (type)) {
			
			if (location.x < dungeonGenerator.CenterOfGrid ().x) 
				
				// State that the entrance is reflectable in x
				entrance.reflectableInX = true;
			
			if (location.z < dungeonGenerator.CenterOfGrid ().z)
				
				// State that the entrance is reflectable in z
				entrance.reflectableInZ = true;
			
			// Replace the type with an entrance
			dungeonGenerator.cellTypeGrid[location.x, location.y, location.z] = CellType.Entrance;
		
			// Add the entrance to data
			allEntrances.Add (entrance);
		}
	}
	
	/// <summary>
	/// Removes the entrance.
	/// </summary>
	/// <param name='entrance'>Entrance.</param>
	public void RemoveEntrance (Entrance entrance) {
		
		// The location
		CellLocation location = entrance.location;
		
		// Change its type back to perimeter
		dungeonGenerator.cellTypeGrid[location.x, location.y, location.z] = CellType.Perimeter;
		
		// Remove the entrance from data
		allEntrances.Remove (entrance);
	}
	
	// START:
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	public void Start () {
	
		// Get the dungeon generator component
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		
		corridorGenerator = GetComponent<CorridorGenerator> ();
	}
}
