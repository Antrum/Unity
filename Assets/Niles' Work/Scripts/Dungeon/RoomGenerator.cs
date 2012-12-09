using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to generate rooms.
/// </summary>
/// <description>This component is part of, and requires the DungeonGenerator component. 
/// All room corner positions and room lengths MUST be an odd number for the dungeon generator
/// to work properly. This means that its center position will need even or odd values depending
/// on where its corner positions will be.</description>
public class RoomGenerator : MonoBehaviour {
	
	protected DungeonGenerator dungeonGenerator;
	protected PerimeterGenerator perimeterGenerator;
	protected EntranceGenerator entranceGenerator;
	
	public List<Room> roomList = new List<Room> ();
	
	public int minRoomLength = 3;
	public int maxRoomLength = 5;
	
	public int targetNumOfRooms = 10;
	public bool asManyAsPossible = false;
	
	public bool reflectX;
	public bool reflectZ;
	
	/// <summary>
    /// Used to make sure that making a room cell type will not replace anything it shouldnt.
    /// </summary>
    /// <returns>Returns true if the test cell type can be replaced by a room cell type.</returns>
    /// <param name="type">Cell type to test</param>
	protected bool RoomCanReplace (CellType type) {
		
		if (type == CellType.Blocked || type == CellType.StairsPerimeter || type == CellType.Perimeter || type == CellType.Room) {
			
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Used to decide what area to initially spawn rooms in
	/// </summary>
	protected RoomSpawnArea SpawnArea () {
		
		CellLocation spawnAreaLocation = new CellLocation ((minRoomLength + 1) / 2, dungeonGenerator.floorNumber, (minRoomLength + 1) / 2);
		IntVector3 spawnAreaSize = new IntVector3 (dungeonGenerator.numCellsX - (minRoomLength + 1), 1, dungeonGenerator.numCellsZ - (minRoomLength + 1));
		
		if (reflectX) spawnAreaSize.x = (spawnAreaSize.x - 1) / 2 - (maxRoomLength - 1) / 2;
		if (reflectZ) spawnAreaSize.z = (spawnAreaSize.z - 1) / 2 - (maxRoomLength - 1) / 2;
		
		RoomSpawnArea spawnArea = new RoomSpawnArea(spawnAreaLocation, spawnAreaSize);
		
		return spawnArea;
	}
	
	/// <summary>
	/// Used to find out which locations can potentially be the location of a room. 
	/// </summary>
    /// <returns>Returns a list of potential cell locations in which a room of minimum size can 
    /// be placed depending on the size of the floor.</returns>
	protected List<CellLocation> PotentialRoomLocations () {
		
		List<CellLocation> potentialRoomLocations = new List<CellLocation> ();
		
		for (var i = SpawnArea ().location.x; i < SpawnArea ().location.x + SpawnArea ().size.x; i++) {
			
			for (var k = SpawnArea ().location.z; k < SpawnArea ().location.z + SpawnArea ().size.z; k++) {
				
				potentialRoomLocations.Add (new CellLocation (i, dungeonGenerator.floorNumber, k));
			}
		}
		
		if (reflectX) {
			
			for (var k = SpawnArea ().location.z; k < SpawnArea ().location.z + SpawnArea ().size.z; k++) {
				
				potentialRoomLocations.Add (new CellLocation (dungeonGenerator.CenterOfGrid ().x, dungeonGenerator.floorNumber, k));
			}
		}
		
		if (reflectZ) {
			
			for (var i = SpawnArea ().location.x; i < SpawnArea ().location.x + SpawnArea ().size.x; i++) {
				
				potentialRoomLocations.Add(new CellLocation(i, dungeonGenerator.floorNumber, dungeonGenerator.CenterOfGrid ().z));
			}
		}
		
		return potentialRoomLocations;
	}
	
	/// <summary>
	/// Used find out where the possible locations are for rooms
	/// </summary>
	/// <returns>Returns a list of possible cell locations in which a room of minimum size can be 
	/// placed by calling the RoomCanFit function for each location returned by PotentialRoomLocations.
    /// </returns>
	protected List<CellLocation> PossibleRoomLocations () {
		
		List<CellLocation> possibleRoomLocations = new List<CellLocation> ();
		
		foreach (CellLocation location in PotentialRoomLocations ()) {
			
			Room smallestRoom = new Room (location, MinRoomSizeForLocation(location));
			
			if (RoomCanFit (smallestRoom)) {
				
				possibleRoomLocations.Add (location);
			}
		}
		
		return possibleRoomLocations;
	}
	
	/// <summary>
	/// Used to find out the minimum room size for a location
	/// </summary>
	/// <param name="location">Location of room</param>
	protected IntVector3 MinRoomSizeForLocation (CellLocation location) {
		
		IntVector3 minRoomSize = new IntVector3(minRoomLength, 1, minRoomLength);
		IntVector3 minRoomSizeForLocation = minRoomSize;
			
		if (MinRoomSizeNeedsOddLocation ()) {
			
			if (LocationHasOddX (location)) minRoomSizeForLocation.x = minRoomSize.x;
			else minRoomSizeForLocation.x = minRoomSize.x + 2;
			
			if (LocationHasOddZ (location)) minRoomSizeForLocation.z = minRoomSize.z;
			else minRoomSizeForLocation.z = minRoomSize.z + 2;
		}
		else {
			
			if (LocationHasOddX (location)) minRoomSizeForLocation.x = minRoomSize.x + 2;
			else minRoomSizeForLocation.x = minRoomSize.x;
			
			if (LocationHasOddZ (location)) minRoomSizeForLocation.z = minRoomSize.z + 2;
			else minRoomSizeForLocation.z = minRoomSize.z;
		}
		
		return minRoomSizeForLocation;
	}
	
	/// <summary>
	/// Used to find out if a room needs an odd location X
	/// </summary>
	/// <param name="size">Size of room</param>
	protected bool RoomNeedsOddLocationX (IntVector3 size) {
		
		if ((float)(size.x + 1) % 4 == 0) {
			
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Used to find out if a room needs an odd location Z
	/// </summary>
	/// <param name="size">Size of room</param>
	protected bool RoomNeedsOddLocationZ (IntVector3 size) {
		
		if ((float)(size.z + 1) % 4 == 0) {
			
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Used to find out if the minimum room size needs an odd or even location
	/// </summary>
	protected bool MinRoomSizeNeedsOddLocation () {
		
		IntVector3 minSize = new IntVector3(minRoomLength, 1, minRoomLength);
		
		if (RoomNeedsOddLocationX (minSize)) {
			
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Used to find out if a location has an odd X
	/// </summary>
	/// <param name="location">Location to test</param>
	protected bool LocationHasOddX (CellLocation location) {
		
		if ((float)location.x % 2 == 0) {
			
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Used to find out if a location has an odd Z
	/// </summary>
	/// <param name="location">Location to test</param>
	protected bool LocationHasOddZ (CellLocation location) {
		
		if ((float)location.z % 2 == 0) {
			
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Used to find out which rooms are connected to the center
	/// </summary>
	protected List<Room> ConnectedRooms () {
		
		List<Room> connectedRooms = new List<Room> ();
		
		foreach (Room room in roomList) {
			
			if (RoomConnectedToCenter (room)) connectedRooms.Add (room);
		}
		
		return connectedRooms;
	}
	
	/// <summary>
	/// Used to find out which rooms are not connected to the center
	/// </summary>
	protected List<Room> UnconnectedRooms () {
		
		List<Room> unconnectedRooms = new List<Room> ();
		
		foreach (Room room in roomList) {
			
			if (!RoomConnectedToCenter (room)) unconnectedRooms.Add (room);
		}
		
		return unconnectedRooms;
	}
	
	protected Room ClosestConnectedRoom (CellLocation location) {
		
		float closestDistance = 100.0f;
		Room closestRoom = ConnectedRooms ()[0];
		
		foreach (Room room in ConnectedRooms ()) {
			
			float xDistance = Mathf.Abs (room.location.x - location.x);
			float zDistance = Mathf.Abs (room.location.z - location.z);
			
			float distance = Mathf.Sqrt((xDistance * xDistance) + (zDistance * zDistance));
			
			if (distance < closestDistance) {
				
				closestDistance = distance;
				closestRoom = room;
			}
		}
		
		return closestRoom;
	}
	
	protected Dir DirectionFromLocationToRoom (Room room, CellLocation location) {
		
		Dir direction = Dir.None;
		
		if (room.location.x < location.x && room.location.z < location.z) direction = Dir.SouthWest;
		else if (room.location.x < location.x && room.location.z > location.z) direction = Dir.NorthWest;
		else if (room.location.x > location.x && room.location.z < location.z) direction = Dir.SouthEast;
		else if (room.location.x > location.x && room.location.z > location.z) direction = Dir.NorthEast;
		
		else if (room.location.x < location.x) direction = Dir.West;
		else if (room.location.x > location.x) direction = Dir.East;
		else if (room.location.z < location.z) direction = Dir.South;
		else direction = Dir.North;
		
		return direction;
	}
	
	protected Dir DirectionFromAToB (Room roomA, Room roomB) {
		
		foreach (CellLocation locationA in roomA.cellLocations) {
			
			foreach (CellLocation locationB in roomB.cellLocations) {
				
				if (locationA.x == locationB.x) {
					
					if (locationA.z > locationB.z) {
						
						return Dir.South;
					}
					else {
						
						return Dir.North;
					}
				}
				
				if (locationA.z == locationB.z) {
					
					if (locationA.x > locationB.x) {
						
						return Dir.West;
					}
					else {
						
						return Dir.East;
					}
				}
			}
		}
		
		foreach (CellLocation locationA in roomA.cellLocations) {
			
			foreach (CellLocation locationB in roomB.cellLocations) {
				
				if (locationA.x > locationB.x) {
					
					if (locationA.z > locationB.z) {
						
						return Dir.SouthWest;
					}
					else {
						
						return Dir.NorthWest;
					}
				}
				
				else {
					
					if (locationA.z > locationB.z) {
						
						return Dir.SouthEast;
					}
					else {
						
						return Dir.NorthEast;
					}
				}
			}
		}
		
		return Dir.None;
	}
	
	protected void ConnectRooms (Room roomA, Room roomB) {
		
		List<Dir> directionsA = new List<Dir> ();
		List<Dir> directionsB = new List<Dir> ();
		
		Dir directionFromAToB = DirectionFromAToB (roomA, roomB);
		
		if (directionFromAToB == Dir.North) {
			
			directionsA.Add (Dir.North);
			directionsB.Add (Dir.South);
		}
		else if (directionFromAToB == Dir.South) {
			
			directionsA.Add (Dir.South);
			directionsB.Add (Dir.North);
		}
		else if (directionFromAToB == Dir.East) {
			
			directionsA.Add (Dir.East);
			directionsB.Add (Dir.West);
		}
		else if (directionFromAToB == Dir.West) {
			
			directionsA.Add (Dir.West);
			directionsB.Add (Dir.East);
		}
		else if (directionFromAToB == Dir.NorthEast) {
			
			int random = Random.Range (0, 2);
			
			if (random == 0) {
				
				directionsA.Add (Dir.North);
				directionsB.Add (Dir.West);
			}
			else {
				
				directionsA.Add (Dir.East);
				directionsB.Add (Dir.South);
			}
		}
		else if (directionFromAToB == Dir.NorthWest) {
			
			int random = Random.Range (0, 2);
			
			if (random == 0) {
				
				directionsA.Add (Dir.North);
				directionsB.Add (Dir.East);
			}
			else {
				
				directionsA.Add (Dir.West);
				directionsB.Add (Dir.South);
			}
		}
		else if (directionFromAToB == Dir.SouthEast) {
			
			int random = Random.Range (0, 2);
			
			if (random == 0) {
				
				directionsA.Add (Dir.South);
				directionsB.Add (Dir.West);
			}
			else {
				
				directionsA.Add (Dir.East);
				directionsB.Add (Dir.North);
			}
		}
		else {
			
			int random = Random.Range (0, 2);
			
			if (random == 0) {
				
				directionsA.Add (Dir.South);
				directionsB.Add (Dir.East);
			}
			else {
				
				directionsA.Add (Dir.West);
				directionsB.Add (Dir.North);
			}
		}
		
		entranceGenerator.GenerateClosestEntrancePair (roomA, directionsA, roomB, directionsB);
	}
	
	/// <summary>
	/// Used to attempt to connect all rooms to the center
	/// </summary>
	public void AttemptConnectRoomsToCenter () {
		
		foreach (Room roomA in UnconnectedRooms ()) {
			
			foreach (Room roomB in ConnectedRooms ()) {
				
				AttemptConnection (roomA, roomB);
			}
		}
	}
	
	/// <summary>
	/// Used to find out the possible connections for two rooms
	/// </summary>
	/// <param name="roomA">Room A</param>
	/// <param name="roomB">Room B</param>
	protected List<CellLocation> PossibleEntranceConnectionLocations (Room roomA, Room roomB) {
		
		List<CellLocation> possibleEntranceConnectionLocations = new List<CellLocation> ();
		
		List <CellLocation> possibleEntranceLocationsA = new List<CellLocation> ();
		List <CellLocation> possibleEntranceLocationsB = new List<CellLocation> ();
		
		possibleEntranceLocationsA = entranceGenerator.PossibleEntranceLocations (roomA, true);
		possibleEntranceLocationsB = entranceGenerator.PossibleEntranceLocations (roomB, true);
		
		foreach (CellLocation locationA in possibleEntranceLocationsA) {
			
			foreach (CellLocation locationB in possibleEntranceLocationsB) {
				
				if (locationA.x == locationB.x && locationA.y == locationB.y && locationA.z == locationB.z) {
					
					possibleEntranceConnectionLocations.Add (locationA);
				}
			}
		}
		
		return possibleEntranceConnectionLocations;
	}
	
	/// <summary>
	/// Used to choose a random connection location for two rooms
	/// </summary>
	/// <param name="roomA">Room A</param>
	/// <param name="roomB">Room B</param>
	protected CellLocation ChosenConnectionLocation (Room roomA, Room roomB) {
			
		int randomIndex =  Random.Range (0, PossibleEntranceConnectionLocations (roomA, roomB).Count);
		
		CellLocation chosenConnectionLocation = PossibleEntranceConnectionLocations (roomA, roomB)[randomIndex];
		
		return chosenConnectionLocation;
	}
	
	/// <summary>
	/// Used to attempt to connect two rooms
	/// </summary>
	/// <param name="roomA">Room A</param>
	/// <param name="roomB">Room B</param>
	protected void AttemptConnection (Room roomA, Room roomB) {
		
		if (PossibleEntranceConnectionLocations (roomA, roomB).Count == 0) return;
			
		CellLocation chosenConnectionLocation = ChosenConnectionLocation (roomA, roomB);
		
		Entrance chosenEntrance = new Entrance (chosenConnectionLocation);
		
		entranceGenerator.AddEntrance (chosenEntrance);
	}
	
	/// <summary>
	/// Used to find out if a room is connected to the central room
	/// </summary>
	/// <param name="room">Room to test</param>
	protected bool RoomConnectedToCenter (Room room) {
		
		ArrayList knownExistingConnections = new ArrayList();
		
		if (dungeonGenerator.CellConnectedToCell(room.location, dungeonGenerator.CenterOfGrid (), ref knownExistingConnections)) {
			
			return true;
		}
		
		return false;
	}
	
	/// <summary>
	/// Used to make sure a room can fit where it wants to.
	/// </summary>
	/// <returns>Returns true if every cell in the room can replace the cell below it using 
	/// RoomCanReplace.</returns>
    /// <param name="room">Room to test</param>
	protected bool RoomCanFit (Room room) {
		
		if (room.location.x + (room.size.x + 1) / 2 > dungeonGenerator.numCellsX
			|| room.location.x < (room.size.x + 1) / 2) {
			
			return false;
		}
		else if (room.location.z + (room.size.z + 1) / 2 > dungeonGenerator.numCellsZ
			|| room.location.z < (room.size.z + 1) / 2) {
			
			return false;
		}
		
		for (var i = room.location.x - (room.size.x - 1) / 2; i < room.location.x + (room.size.x + 1) / 2; i++) {
				
			for (var k = room.location.z - (room.size.z - 1) / 2; k < room.location.z + (room.size.z + 1) / 2; k++) {
				
				if (!RoomCanReplace(dungeonGenerator.cellTypeGrid[i, room.location.y, k])) {
					
					return false;
				}
			}
		}
		
		return true;
	}
	
	/// <summary>
	/// Used to populate the possible size lists
	/// </summary>
	/// <param name="sizesList">List to populate</param>
	/// <param name="startSize">Size to start with</param>
	protected void PopulatePossibleSizesList (List<int> sizesList, int startSize) {
		
		int size = startSize;
		
		bool finished = false;
		
		while (!finished) {
			
			if (size <= maxRoomLength) {
				
				if (size >= minRoomLength) sizesList.Add (size);
				size += 4;
			}
			else {
				
				finished = true;
			}
		}
	}
	
	/// <summary>
	/// Used to find out possible sizes for an odd location
	/// </summary>
	protected List<int> PossibleSizesForOddLocation () {
		
		List<int>possibleSizesForOddLocation = new List<int> ();
		
		PopulatePossibleSizesList (possibleSizesForOddLocation, 5);
		
		return possibleSizesForOddLocation;
	}
	
	/// <summary>
	/// Used to find out possible sizes for an even location
	/// </summary>
	protected List<int> PossibleSizesForEvenLocation () {
		
		List<int>possibleSizesForEvenLocation = new List<int> ();
		
		PopulatePossibleSizesList (possibleSizesForEvenLocation, 3);
		
		return possibleSizesForEvenLocation;
	}
	
	/// <summary>
	/// Used to generate a random size for an odd location
	/// </summary>
	protected int RandomSizeForOddLocation () {
		
		if (PossibleSizesForOddLocation ().Count == 0) return 5;
			
		int randomIndex = Random.Range (0, PossibleSizesForOddLocation ().Count);
		
		int randomSize = PossibleSizesForOddLocation ()[randomIndex];
		
		return randomSize;
	}
	
	/// <summary>
	/// Used to generate a random size for an even location
	/// </summary>
	protected int RandomSizeForEvenLocation () {
		
		if (PossibleSizesForEvenLocation ().Count == 0) return 3;
			
		int randomIndex = Random.Range (1, PossibleSizesForEvenLocation ().Count + 1) - 1;
		
		int randomSize = PossibleSizesForEvenLocation ()[randomIndex];
		
		return randomSize;
	}
	
	/// <summary>
	/// Used to reflect the rooms in X
	/// </summary>
	protected void ReflectX () {
		
		List<Room> roomsToReflect = new List<Room> ();
		
		foreach (Room room in roomList) {
			
			if (room.reflectableInX) roomsToReflect.Add (room);
		}
		
		foreach (Room room in roomsToReflect) {
				
			CellLocation reflectionLocation = new CellLocation (dungeonGenerator.numCellsX - room.location.x - 1, dungeonGenerator.floorNumber, room.location.z);
			
			Room reflectedRoom = new Room (reflectionLocation, room.size);
		
			if (RoomCanFit (reflectedRoom)) {
				
				reflectedRoom.reflectedRoomX = true;
				
				AddRoom (reflectedRoom);
			}
		}
	}
	
	/// <summary>
	/// Used to reflect the rooms in Z
	/// </summary>
	protected void ReflectZ () {
		
		List<Room> roomsToReflect = new List<Room> ();
		
		foreach (Room room in roomList) {
			
			if (room.reflectableInZ) roomsToReflect.Add (room);
		}
		
		foreach (Room room in roomsToReflect) {
				
			CellLocation reflectionLocation = new CellLocation (room.location.x, dungeonGenerator.floorNumber, dungeonGenerator.numCellsZ - room.location.z - 1);
			
			Room reflectedRoom = new Room (reflectionLocation, room.size);
		
			if (RoomCanFit (reflectedRoom))  {
				
				if (room.reflectedRoomX) reflectedRoom.reflectedRoomX = true;
				reflectedRoom.reflectedRoomZ = true;
				AddRoom (reflectedRoom);
			}
		}
	}
	
	/// <summary>
	/// Used to generate all rooms.
	/// </summary>
	/// <description> Generates all rooms by calling the GenerateRoom function until there is either
	/// no more possible room locations in the list PossibleRoomLocations or the amount of rooms made
    /// has hit the target number of rooms specified in the inspector. If it has been specified in the 
    /// inspector to make as many rooms as possible, the target number of rooms will be changed to a 
    /// very large number which would not be possible to complete. </description>
    /// <param name="floor">Floor to generate rooms on</param>
	public void GenerateAllRooms (Floor floor) {
		
		if (floor.reflectRoomsX) reflectX = true;
		else reflectX = false;
		
		if (floor.reflectRoomsZ) reflectZ = true;
		else reflectZ = false;
		
		if (asManyAsPossible) targetNumOfRooms = 1000;
		
		bool finished = false;
		
		GenerateCentralRoom ();
		
		while (!finished) {
			
			if (PossibleRoomLocations ().Count > 0 && targetNumOfRooms > roomList.Count) {
				
				GenerateRoom ();
			}
			else finished = true;
		}
		
		if (reflectX) ReflectX ();
		if (reflectZ) ReflectZ ();
		
		//ConnectRooms (roomList[0], roomList[1]);
	}
	
	/// <summary>
	/// Used to generate a room.
	/// </summary>
	/// <description>Generates a room by first choosing a random index of all possible room 
	/// locations in the list PossibleRoomLocations, choosing a random width and height for the 
	/// room depending on the minimum and maximum room length specified in the inspector, and 
	/// clamping the width and height to be a maximum difference of two if it is specified in 
	/// the inspector to make them more square. Once size and location are chosen, it will use 
	/// the method AddRoom to add the room to the floor depending if it can fit or not.
	/// </description>
	protected void GenerateRoom () {
		
		if (PossibleRoomLocations ().Count == 0) return;
			
		int randomIndex = Random.Range (1, PossibleRoomLocations ().Count) - 1;
		
		CellLocation chosenLocation = PossibleRoomLocations ()[randomIndex];
		
		int randomSizeX;
		if (chosenLocation.x % 2 == 0) randomSizeX = RandomSizeForEvenLocation ();
		else randomSizeX = RandomSizeForOddLocation ();
		
		int randomSizeZ;
		if (chosenLocation.z % 2 == 0) randomSizeZ = RandomSizeForEvenLocation ();
		else randomSizeZ = RandomSizeForOddLocation ();
		
		IntVector3 chosenSize = new IntVector3(randomSizeX, 1, randomSizeZ);
		
		Room testRoom = new Room (chosenLocation, chosenSize);
		
		if (RoomCanFit (testRoom)) AddRoom (testRoom);
	}
	
	/// <summary>
	/// Used to generate the central room
	/// </summary>
	protected void GenerateCentralRoom () {
		
		if (PossibleRoomLocations ().Count == 0) return;
			
		CellLocation chosenLocation = dungeonGenerator.CenterOfGrid ();
		
		int randomSizeX;
		if (chosenLocation.x % 2 == 0) randomSizeX = RandomSizeForEvenLocation ();
		else randomSizeX = RandomSizeForOddLocation ();
		
		int randomSizeZ;
		if (chosenLocation.z % 2 == 0) randomSizeZ = RandomSizeForEvenLocation ();
		else randomSizeZ = RandomSizeForOddLocation ();
		
		IntVector3 chosenSize = new IntVector3(randomSizeX, 1, randomSizeZ);
		
		Room testRoom = new Room (chosenLocation, chosenSize);
		
		if (RoomCanFit (testRoom)) {
			
			testRoom.reflectableInX = false;
			testRoom.reflectableInZ = false;
			testRoom.type = RoomType.Central;
			
			AddRoom (testRoom);
		}
	}
	
	/// <summary>
	/// Used to add a room.
	/// </summary>
    /// <description>Adds the room to the list of all rooms, and turns each cell within the room 
    /// into a room cell. It then calls the perimeter generator's GeneratePerimeter method and the 
    /// entrance generator's GenerateEntrancesToRoom method.</description>
    /// <param name="room">Room to add</param>
	protected void AddRoom (Room room) {
		
		if (!RoomCanFit (room)) return;
			
		roomList.Add (room);
	
		for (var i = room.westMost; i < room.eastMost + 1; i++) {
			
			for (var k = room.southMost; k < room.northMost + 1; k++) {
				
				dungeonGenerator.cellTypeGrid[i, room.location.y, k] = CellType.Room;
			}
		}
		
		perimeterGenerator.GeneratePerimeter (room);
	}
	
	/// <summary>
	/// Used to remove a room.
	/// </summary>
    /// <description>Removes the room from the list of all rooms, and turns each cell within the 
    /// room into an empty cell.</description>
    /// <param name="room">Room to remove</param>
	protected void RemoveRoom (Room room) {
		
		roomList.Remove (room);
		
		for (var i = room.location.x - (room.size.x - 1) / 2; i < room.location.x + (room.size.x + 1) / 2; i++) {
			
			for (var k = room.location.z - (room.size.z - 1) / 2; k < room.location.z + (room.size.x + 1) / 2; k++) {
				
				dungeonGenerator.cellTypeGrid[i, room.location.y, k] = CellType.Empty;
			}
		}
	}
	
	/// <summary>
	/// Used for initialization.
	/// </summary>
	public void Start () {
		
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		perimeterGenerator = GetComponent<PerimeterGenerator> ();
		entranceGenerator = GetComponent<EntranceGenerator> ();
	}
}
