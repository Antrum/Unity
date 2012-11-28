using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (TerrainTextureGenerator))]

public class TerrainGenerator : MonoBehaviour {
	
	protected DungeonGenerator dungeonGenerator;
	protected TerrainToolkit toolkit;
	protected TerrainTextureGenerator textureGenerator;
	protected TerrainCarver terrainCarver;
	public Terrain ter;
	
	public float[] slopeStops;
	public float[] heightStops;
	public Texture2D[] textures;
	
	public void GenerateTerrain () {
		
		PositionTerrain(ter);
		GenerateCaveShape (ter, dungeonGenerator.numCellsX, dungeonGenerator.numCellsZ);
		terrainCarver.CarveTerrain ();
		
		toolkit.SmoothTerrain (10, 0.2f);
		toolkit.FastThermalErosion (10, 40.0f, 0.0f);
		//textureGenerator.GenerateTexture (ter);
		
		GenerateNoise(ter, dungeonGenerator.numCellsX, dungeonGenerator.numCellsZ);
		
		toolkit.TextureTerrain (slopeStops, heightStops, textures);
	}
	
	public void PositionTerrain (Terrain terrain) {
		
		terrain.transform.position = new Vector3 (-4, -dungeonGenerator.floorNumber*4 -4.1f, -4);
	}
	
	protected void GenerateCaveShape (Terrain terrain, float cellsX, float cellsZ) {
		
		float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		
		for (int i = 0; i < terrain.terrainData.heightmapWidth; i++) {
			
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++) {
				
				heights[i, k] = Mathf.PerlinNoise ((float)i / (float)terrain.terrainData.heightmapWidth * cellsX / 3, (float)k / (float)terrain.terrainData.heightmapHeight * cellsZ / 3);
				
				if (heights[i, k] > 0.5f) heights[i, k] = 1.0f;
				else if (heights[i, k] < 0.3f) heights[i, k] = 0f;
				else heights[i, k] = 0.5f;
			}
		}
		
		terrain.terrainData.SetHeights(0, 0, heights);
	}
	
	protected void GenerateNoise (Terrain terrain, float cellsX, float cellsZ) {
		
		float[,] heightsBefore = terrain.terrainData.GetHeights (0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
		float[,] heightsA = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		float[,] heightsB = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		float[,] newHeights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		
		for (int i = 0; i < terrain.terrainData.heightmapWidth; i++) {
			
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++) {
				
				heightsA[i, k] = Mathf.PerlinNoise ((float)i / (float)terrain.terrainData.heightmapWidth * cellsX *1, (float)k / (float)terrain.terrainData.heightmapHeight * cellsZ *1);
				heightsA[i, k] = heightsA[i, k] / 15.0f;
				
				heightsB[i, k] = Mathf.PerlinNoise ((float)i / (float)terrain.terrainData.heightmapWidth * cellsX *2, (float)k / (float)terrain.terrainData.heightmapHeight * cellsZ *2);
				heightsB[i, k] = heightsB[i, k] / 25.0f;
				
				newHeights[i, k] = heightsBefore[i, k] - heightsA[i, k] - heightsB[i, k];
			}
		}
		
		terrain.terrainData.SetHeights(0, 0, newHeights);
	}
	
	public void GenerateHeights(Terrain terrain, float tileSize) {
		
        float[,] heightsA = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		float[,] heightsB = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		float[,] heightsC = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		float[,] heightsFinal = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
 
        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++) {
			
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++) {
				
                heightsA[i, k] = Mathf.PerlinNoise(((float)i / (float)terrain.terrainData.heightmapWidth) * (tileSize / 10), ((float)k / (float)terrain.terrainData.heightmapHeight) * (tileSize / 10));
				
				if (heightsA[i, k] > 0.5f) heightsA[i, k] = 1.0f;
				else heightsA[i, k] = 0.0f;
				
				heightsA[i, k] = heightsA[i, k];
				
				heightsB[i, k] = Mathf.PerlinNoise(((float)i / (float)terrain.terrainData.heightmapWidth) * (tileSize / 1), ((float)k / (float)terrain.terrainData.heightmapHeight) * (tileSize / 1));
				
				heightsB[i, k] = heightsB[i, k] / 15.0f;
				
				heightsC[i, k] = Mathf.PerlinNoise(((float)i / (float)terrain.terrainData.heightmapWidth) * (tileSize / 5), ((float)k / (float)terrain.terrainData.heightmapHeight) * (tileSize / 5));
				
				heightsC[i, k] = heightsC[i, k] / 3.0f;
				
				heightsFinal[i, k] = heightsA[i, k] + heightsB[i, k] + heightsC[i, k];
            }
        }
		
        terrain.terrainData.SetHeights(0, 0, heightsFinal);
    }
	
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		terrainCarver = GetComponent<TerrainCarver> ();
		textureGenerator = GetComponent<TerrainTextureGenerator> ();
		
		toolkit = ter.GetComponent<TerrainToolkit> ();
	}
}
