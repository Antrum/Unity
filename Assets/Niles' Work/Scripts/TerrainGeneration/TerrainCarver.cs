using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainCarver : MonoBehaviour {
	
	protected DungeonGenerator dungeonGenerator;
	protected TerrainGenerator terrainGenerator;
	protected TerrainTextureGenerator textureGenerator;
	
	public Terrain terrain;
	public TerrainToolkit terrainToolkit;
	
	public void CarveFloor () {
		
		float mapRes = terrain.terrainData.heightmapResolution;
		float cellRes = mapRes / dungeonGenerator.numCellsX;
		
		List<CellLocation> carveLocations = new List<CellLocation> ();
		
		float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		
		heights = terrain.terrainData.GetHeights (0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
		
		for (var i = 0; i < dungeonGenerator.numCellsX; i++) {
			
			for (var j = 0; j < dungeonGenerator.numCellsZ; j++) {
			
				CellType testType = dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j];
				
				if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance || testType == CellType.StairsDown || testType == CellType.StairsUp) {
					
					CellLocation location = new CellLocation(i, dungeonGenerator.floorNumber, j);
					
					carveLocations.Add (location);
				}
			}
		}
		
		foreach (CellLocation location in carveLocations) {
			
			for (var i = (int)((float)location.z * cellRes); i < (int)((float)location.z * cellRes + cellRes) + 1; i++) {
				
				for (var j = (int)((float)location.x * cellRes); j < (int)((float)location.x * cellRes + cellRes) + 1; j++) {
					
					heights[i, j] = 0f;
				}
			}
		}
		
		terrain.terrainData.SetHeights (0, 0, heights);
	}
	
	public void CarveTerrain () {
		
		float mapRes = terrain.terrainData.heightmapResolution;
		float cellRes = mapRes / dungeonGenerator.numCellsX;
		
		List<CellLocation> carveLocations = new List<CellLocation> ();
		List<CellLocation> rockLocations = new List<CellLocation> ();
		
		float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		
		heights = terrain.terrainData.GetHeights (0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
		
		for (var i = 0; i < dungeonGenerator.numCellsX; i++) {
			
			for (var j = 0; j < dungeonGenerator.numCellsZ; j++) {
			
				CellType testType = dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j];
				
				if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance || testType == CellType.StairsDown || testType == CellType.StairsUp) {
					
					CellLocation location = new CellLocation(i, dungeonGenerator.floorNumber, j);
					
					carveLocations.Add (location);
				}
			}
		}
		
		foreach (CellLocation location in carveLocations) {
			
			for (var i = (int)((float)location.z * cellRes); i < (int)((float)location.z * cellRes + cellRes) + 1; i++) {
				
				for (var j = (int)((float)location.x * cellRes); j < (int)((float)location.x * cellRes + cellRes) + 1; j++) {
					
					heights[i, j] = 0.5f;
				}
			}
		}
		
		for (var i = 0; i < dungeonGenerator.numCellsX; i++) {
			
			for (var j = 0; j < dungeonGenerator.numCellsZ; j++) {
			
				CellType testType = dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j];
				
				if (testType == CellType.DungeonPerimeter) {
					
					CellLocation location = new CellLocation(i, dungeonGenerator.floorNumber, j);
					
					rockLocations.Add (location);
				}
			}
		}
		
		foreach (CellLocation location in rockLocations) {
			
			int startX;
			int startZ;
			int widthX;
			int widthZ;
			
			if (location.z != 0) startX = (int)((float)location.z * cellRes) - 1;
			else startX = (int)((float)location.z * cellRes);
			
			if (location.x != 0) startZ = (int)((float)location.x * cellRes) - 1;
			else startZ = (int)((float)location.x * cellRes);
			
			if (location.z != dungeonGenerator.numCellsZ - 1) widthX = (int)cellRes + 2;
			else widthX = (int)cellRes;
			
			if (location.x != dungeonGenerator.numCellsX - 1) widthZ = (int)cellRes + 2;
			else widthZ = (int)cellRes;
			
			for (var i = startX; i < startX + widthX + 1; i++) {
				
				for (var j = startZ; j < startZ + widthZ + 1; j++) {
					
					heights[i, j] = 1;
				}
			}
		}
		
		terrain.terrainData.SetHeights (0, 0, heights);
	}
	
	// Use this for initialization
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		terrainGenerator = GetComponent<TerrainGenerator> ();
		textureGenerator = GetComponent<TerrainTextureGenerator> ();
		
		terrainToolkit = terrain.GetComponent<TerrainToolkit> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
