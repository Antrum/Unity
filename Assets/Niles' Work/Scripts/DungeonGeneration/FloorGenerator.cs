/// <summary>
/// Floor generator.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (StairsGenerator))]
[RequireComponent (typeof (RoomGenerator))]
[RequireComponent (typeof (PerimeterGenerator))]
[RequireComponent (typeof (EntranceGenerator))]
[RequireComponent (typeof (CorridorGenerator))]
[RequireComponent (typeof (VoidGenerator))]
[RequireComponent (typeof (PitGenerator))]
[RequireComponent (typeof (CaveGenerator))]
[RequireComponent (typeof (TerrainCarver))]
[RequireComponent (typeof (TerrainTextureGenerator))]

public class FloorGenerator : MonoBehaviour {
	
	// INSTANCES:
	
	/// <summary>
	/// The dungeon generator.
	/// </summary>
	protected DungeonGenerator dungeonGenerator;
	
	/// <summary>
	/// The scenery generator.
	/// </summary>
	protected SceneryGenerator sceneryGenerator;
	
	/// <summary>
	/// The stairs generator.
	/// </summary>
	protected StairsGenerator stairsGenerator;
	
	/// <summary>
	/// The room generator.
	/// </summary>
	protected RoomGenerator roomGenerator;
	
	/// <summary>
	/// The perimeter generator.
	/// </summary>
	protected PerimeterGenerator perimeterGenerator;
	
	/// <summary>
	/// The entrance generator.
	/// </summary>
	protected EntranceGenerator entranceGenerator;
	
	/// <summary>
	/// The corridor generator.
	/// </summary>
	protected CorridorGenerator corridorGenerator;
	
	/// <summary>
	/// The void generator.
	/// </summary>
	protected VoidGenerator	voidGenerator;
	
	/// <summary>
	/// The pit generator.
	/// </summary>
	protected PitGenerator pitGenerator;
	
	/// <summary>
	/// The cave generator.
	/// </summary>
	protected CaveGenerator caveGenerator;
	
	/// <summary>
	/// The terrain carver.
	/// </summary>
	protected TerrainCarver terrainCarver;
	
	/// <summary>
	/// The terrain generator.
	/// </summary>
	protected TerrainGenerator terrainGenerator;
	
	/// <summary>
	/// The texture generator.
	/// </summary>
	protected TerrainTextureGenerator textureGenerator;
	
	// VARIABLES:
	
	/// <summary>
	/// All floors.
	/// </summary>
	public Floor[] allFloors;
	
	// FLOOR GENERATION:
	
	/// <summary>
	/// Creates the floor.
	/// </summary>
	public void CreateFloor () {
		
		// Generate the floor
        GenerateFloor (dungeonGenerator.floors[dungeonGenerator.floorNumber]);
		
		// Instantiate the floor
		sceneryGenerator.InstantiateFloorTheme (dungeonGenerator.floors[dungeonGenerator.floorNumber]);
		//InstantiateFloor ();
		
		// Place the hero at random if it is the first floor
		if (dungeonGenerator.floorNumber == 0) dungeonGenerator.PlaceHeroAtRandom ();
		
		AstarPath.active.Scan();
    }
	
	/// <summary>
	/// Instantiates the floor.
	/// </summary>
	protected void InstantiateFloor () {
		
		sceneryGenerator.InstantiateCellsFromGrid ();
		sceneryGenerator.InstantiateWallsFromGrid ();
		sceneryGenerator.InstantiateStairsUp (dungeonGenerator.floors[dungeonGenerator.floorNumber].allStairsUp);
		sceneryGenerator.InstantiateStairsDown (dungeonGenerator.floors[dungeonGenerator.floorNumber].allStairsDown);
		
		
		sceneryGenerator.ChangeFloorMaterial (dungeonGenerator.floors[dungeonGenerator.floorNumber].floorMaterial);
		sceneryGenerator.ChangeWallMaterial (dungeonGenerator.floors[dungeonGenerator.floorNumber].wallMaterial);
		sceneryGenerator.ChangeWallMesh (dungeonGenerator.floors[dungeonGenerator.floorNumber].wallMesh);
	}
	
	/// <summary>
	/// Removes the floor.
	/// </summary>
	protected void RemoveFloor () {
		
        for (int i = 0; i < dungeonGenerator.PrefabCellParent.childCount; i++) {
			
			// Destroy all prefab cells
            Destroy(dungeonGenerator.PrefabCellParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < dungeonGenerator.OtherStuffParent.childCount; i++) {
			
			// Destroy all other prefabs
            Destroy(dungeonGenerator.OtherStuffParent.GetChild(i).gameObject);
        }
		
		// Clear the generated lists of objects from the components
		roomGenerator.roomList.Clear ();
		entranceGenerator.allEntrances.Clear ();
		corridorGenerator.allCorridors.Clear ();
		stairsGenerator.allStairs.Clear ();
		stairsGenerator.allStairsUp.Clear ();
		stairsGenerator.allStairsDown.Clear ();
    }
	
	/// <summary>
	/// Gos to next floor.
	/// </summary>
	public void GoToNextFloor ()
    {
		// Remove the current floor
        RemoveFloor();
		
		// Create a new floor or just instantiate the next floor if it has already been visited
		dungeonGenerator.floorNumber++;
        if (!dungeonGenerator.floors[dungeonGenerator.floorNumber].generated) CreateFloor();
		else {
			
			sceneryGenerator.InstantiateFloorTheme (dungeonGenerator.floors[dungeonGenerator.floorNumber]);
		
			AstarPath.active.Scan();
		}
    }
	
	/// <summary>
	/// Gos to previous floor.
	/// </summary>
	public void GoToPreviousFloor ()
    {
		// Remove the current floor
        RemoveFloor();
		
		// Instantiate the previous floor
		dungeonGenerator.floorNumber--;
        sceneryGenerator.InstantiateFloorTheme (dungeonGenerator.floors[dungeonGenerator.floorNumber]);
		
		AstarPath.active.Scan();
    }
	
	/// <summary>
	/// Generates the floor.
	/// </summary>
	/// <param name='floor'>Floor.</param>
	protected void GenerateFloor (Floor floor) {
		
		// Generate the theme of the floor
		GenerateFloorTheme (floor);
		
		if (floor.type == FloorType.Dungeon) {
			
			// Order the algorithms needed to generate the floor
			BlockEdges (floor);
			stairsGenerator.GenerateStairsUp (floor);
			stairsGenerator.GenerateStairsDown (floor);
			roomGenerator.GenerateAllRooms (floor);
			entranceGenerator.GenerateAllEntrances (roomGenerator.roomList, floor);
			corridorGenerator.StartNewCorridor ();
			corridorGenerator.CleanDeadEnds ();
			entranceGenerator.CleanDeadEntrances ();
			roomGenerator.AttemptConnectRoomsToCenter ();
			roomGenerator.AttemptConnectRoomsToCenter ();
			roomGenerator.AttemptConnectRoomsToCenter ();
			voidGenerator.DestroyEntrances ();
			voidGenerator.DestroyEntrances ();
			corridorGenerator.CleanDeadEnds ();
			entranceGenerator.CleanDeadEntrances ();
			
			//entranceGenerator.GenerateClosestEntrancePair (roomGenerator.roomList[0], roomGenerator.roomList[1]);
			//entranceGenerator.GenerateClosestEntrancePair (roomGenerator.roomList[0], roomGenerator.roomList[2]);
			//entranceGenerator.GenerateClosestEntrancePair (roomGenerator.roomList[1], roomGenerator.roomList[2]);
			
			perimeterGenerator.ConvertToDungeonPerimeter ();
			
			terrainGenerator.GenerateTerrain ();
			
			// Add all of the lists of generated objects to the floors data
			foreach (Stairs stairs in stairsGenerator.allStairsUp) floor.allStairsUp.Add (stairs);
			foreach (Stairs stairs in stairsGenerator.allStairsDown) floor.allStairsDown.Add (stairs);
			foreach (Room room in roomGenerator.roomList) floor.allRooms.Add (room);
			foreach (Entrance entrance in entranceGenerator.allEntrances) floor.allEntrances.Add (entrance);
			foreach (Corridor corridor in corridorGenerator.allCorridors) floor.allCorridors.Add (corridor);
			
			// Generate the materials and meshs for the floor
//			sceneryGenerator.GenerateFloorMaterial (floor);
//			sceneryGenerator.GenerateWallMaterial (floor);
//			sceneryGenerator.GenerateWallMesh (floor);
			
		}
		
		else if (floor.type == FloorType.Cave) {
			
			caveGenerator.GenerateCave (floor);
			caveGenerator.AddCavesToDungeon (floor);
		}
		
		// Set the floor to generated so that it can be re-visited
		floor.generated = true;
    }
	
	/// <summary>
	/// Generates the floor theme.
	/// </summary>
	/// <param name='floor'>Floor.</param>
	protected void GenerateFloorTheme (Floor floor) {
		
		// Generate random reflection settings using 0s and 1s
		int reflectRoomsX = Random.Range (0, 2);
		int reflectRoomsZ = Random.Range (0, 2);
		int reflectEntrancesX = Random.Range (0, 2);
		int reflectEntrancesZ = Random.Range (0, 2);
		
		// Turn those 0s and 1s into bool values
		if (reflectRoomsX == 1) floor.reflectRoomsX = true; else floor.reflectRoomsX = false;
		if (reflectRoomsZ == 1) floor.reflectRoomsZ = true; else floor.reflectRoomsZ = false;
		if (reflectEntrancesX == 1) floor.reflectEntrancesX = true; else floor.reflectEntrancesX = false;
		if (reflectEntrancesZ == 1) floor.reflectEntrancesZ = true; else floor.reflectEntrancesZ = false;
		
		// Used for bug hunting
		floor.reflectRoomsX = false;
		floor.reflectRoomsZ = false;
		floor.reflectEntrancesX = true;
		floor.reflectEntrancesZ = true;
		floor.type = FloorType.Dungeon;
		
		//int floorType = Random.Range (0, 2);
		//if (floorType == 1) floor.type = FloorType.Dungeon; else floor.type = FloorType.Cave;
	}
	
	/// <summary>
	/// Blocks the edges.
	/// </summary>
	/// <param name='floor'>Floor.</param>
	protected void BlockEdges (Floor floor) {
		
		for (var i = 0; i < dungeonGenerator.numCellsX; i++) {
			
			// Block off the top and bottom edges of the floor
			dungeonGenerator.cellTypeGrid[i, floor.floorNumber, 0] = CellType.Blocked;
			dungeonGenerator.cellTypeGrid[i, floor.floorNumber, dungeonGenerator.numCellsZ - 1] = CellType.Blocked;
		}
		
		for (var j = 0; j < dungeonGenerator.numCellsZ; j++) {
			
			// Block off the side edges of the floor
			dungeonGenerator.cellTypeGrid[0, floor.floorNumber, j] = CellType.Blocked;
			dungeonGenerator.cellTypeGrid[dungeonGenerator.numCellsX - 1, floor.floorNumber, j] = CellType.Blocked;
		}
	}
	
	// START:
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		sceneryGenerator = GetComponent<SceneryGenerator> ();
		
		stairsGenerator = GetComponent<StairsGenerator> ();
		roomGenerator = GetComponent<RoomGenerator> ();
		perimeterGenerator = GetComponent<PerimeterGenerator> ();
		entranceGenerator = GetComponent<EntranceGenerator> ();
		corridorGenerator = GetComponent<CorridorGenerator> ();
		voidGenerator = GetComponent<VoidGenerator> ();
		pitGenerator = GetComponent<PitGenerator> ();
		caveGenerator = GetComponent<CaveGenerator> ();
		
		terrainCarver = GetComponent<TerrainCarver> ();
		terrainGenerator = GetComponent<TerrainGenerator> ();
		textureGenerator = GetComponent<TerrainTextureGenerator> ();
		
		// Initialize the required components
		stairsGenerator.Start ();
		roomGenerator.Start ();
		perimeterGenerator.Start ();
		entranceGenerator.Start ();
		corridorGenerator.Start ();
		voidGenerator.Start ();
		pitGenerator.Start ();
		caveGenerator.Start ();
		
		terrainCarver.Start ();
		terrainGenerator.Start ();
		textureGenerator.Start ();
		
		// Initialize the floor array
		allFloors = new Floor[dungeonGenerator.numFloors];
	}
}
