using UnityEditor;
using UnityEngine;
using System.Collections;
 
public class TerrainPerlinNoise : ScriptableWizard {
 
    public float Tiling = 10.0f;
 
    [MenuItem("Terrain/Generate from Perlin Noise")]
    public static void CreateWizard(MenuCommand command)
    {
        ScriptableWizard.DisplayWizard("Perlin Noise Generation Wizard", typeof(TerrainPerlinNoise));
    }
 
    void OnWizardUpdate()
    {
        helpString = "This small generation tool allows you to generate perlin noise for your terrain.";
    }
 
    void OnWizardCreate()
    {
        GameObject obj = Selection.activeGameObject;
 
        if (obj.GetComponent<Terrain>())
        {
            GenerateHeights(obj.GetComponent<Terrain>(), Tiling);
        }
    }
	
    public void GenerateHeights(Terrain terrain, float tileSize)
    {
        float[,] heightsA = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		float[,] heightsB = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		float[,] heightsC = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		float[,] heightsFinal = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
 
        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
            {
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
 
//		for (int i = 30; i < terrain.terrainData.heightmapWidth - 30; i++)
//        {
//            for (int k = 30; k < terrain.terrainData.heightmapHeight - 30; k++)
//            {
//				heightsFinal[i, k] = 0;
//			}
//		}
		
        terrain.terrainData.SetHeights(0, 0, heightsFinal);
    }
}