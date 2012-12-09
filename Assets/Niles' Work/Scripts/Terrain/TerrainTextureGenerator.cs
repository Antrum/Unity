using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class TerrainTextureGenerator : MonoBehaviour {
	
	protected TerrainGenerator terrainGenerator;
	public TerrainToolkit terrainToolkit;
	
	public SplatPrototype[] splatPrototypes;
	public float slopeBlendMinAngle = 15.0f;
	public float slopeBlendMaxAngle = 25.0f;
	public List<float> heightBlendPoints;
	
	public void GenerateTexture(Terrain terrain) {
		
		if (terrain == null) return;
		
		TerrainData terrainData = terrain.terrainData;
		
		splatPrototypes = terrainData.splatPrototypes;
		
		int numTextures = splatPrototypes.Length;
		if (numTextures < 2) return;
		
		int terrainWidth = terrainData.heightmapWidth - 1;
		int terrainHeight = terrainData.heightmapHeight - 1;
		
		float[,] heightMapData = new float[terrainWidth, terrainHeight];
		float[,] slopeMapData = new float[terrainWidth, terrainHeight];
		
		float[,,] splatMapData;
		
		terrainData.alphamapResolution = terrainWidth;
		
		splatMapData = terrainData.GetAlphamaps(0, 0, terrainWidth, terrainWidth);
		
		// Angles to difference...
		Vector3 terrainSize = terrainData.size;
		
		float slopeBlendMinimum = ((terrainSize.x / terrainWidth) * Mathf.Tan(slopeBlendMinAngle * Mathf.Deg2Rad)) / terrainSize.y;
		float slopeBlendMaximum = ((terrainSize.x / terrainWidth) * Mathf.Tan(slopeBlendMaxAngle * Mathf.Deg2Rad)) / terrainSize.y;
		
		
		
		float greatestHeight = 0.0f;
		
		int xNeighbours;
		int yNeighbours;
		int xShift;
		int yShift;
		int xIndex;
		int yIndex;
		
		float[,] heightMap = terrainData.GetHeights(0, 0, terrainWidth, terrainHeight);
		
		for (int i = 0; i < terrainWidth; i++) {
			
			if (i == 0) {
				
				xNeighbours = 2;
				xShift = 0;
				xIndex = 0;
			} 
			else if (i == terrainWidth - 1) {
				
				xNeighbours = 2;
				xShift = -1;
				xIndex = 1;
			} 
			else {
				
				xNeighbours = 3;
				xShift = -1;
				xIndex = 1;
			}
		
			for (int j = 0; j < terrainHeight; j++) {
				
				if (j == 0) {
					
					yNeighbours = 2;
					yShift = 0;
					yIndex = 0;
				} 
				else if (j == terrainHeight - 1) {
					
					yNeighbours = 2;
					yShift = -1;
					yIndex = 1;
				} 
				else {
					
					yNeighbours = 3;
					yShift = -1;
					yIndex = 1;
				}
				
				// Get height...
				float height = heightMap[i + xIndex + xShift, j + yIndex + yShift];
				
				if (height > greatestHeight) greatestHeight = height;
				
				// ...and apply to height map...
				heightMapData[i, j] = height;
				
				// Calculate slope...
				float tCumulative = 0.0f;
				float nNeighbours = xNeighbours * yNeighbours - 1;
				
				int Ny;
				int Nx;
				float t;
				
				for (Nx = 0; Nx < xNeighbours; Nx++) {
					
					for (Ny = 0; Ny < yNeighbours; Ny++) {
						
						if (Nx != xIndex || Ny != yIndex) {
							
							t = Mathf.Abs(height - heightMap[i + Nx + xShift, j + Ny + yShift]);
							tCumulative += t;
						}
					}
				}
				
				float tAverage = tCumulative / nNeighbours;
				
				// ...and apply to the slope map...
				slopeMapData[i, j] = tAverage;
			}
		}
		
		// Blend slope...
		float sBlended;
		int Px;
		int Py;
		for (Py = 0; Py < terrainHeight; Py++) {
			for (Px = 0; Px < terrainWidth; Px++) {
				sBlended = slopeMapData[Px, Py];
				if (sBlended < slopeBlendMinimum) {
					sBlended = 0;
				} else if (sBlended < slopeBlendMaximum) {
					sBlended = (sBlended - slopeBlendMinimum) / (slopeBlendMaximum - slopeBlendMinimum);
				} else if (sBlended > slopeBlendMaximum) {
					sBlended = 1;
				}
				slopeMapData[Px, Py] = sBlended;
				splatMapData[Px, Py, 0] = sBlended;
			}
		}
		
		// Blend height maps...
		for (int i = 1; i < numTextures; i++) {
			
			for (Py = 0; Py < terrainHeight; Py++) {
				
				for (Px = 0; Px < terrainWidth; Px++) {
					
					float hBlendInMinimum = 0;
					float hBlendInMaximum = 0;
					float hBlendOutMinimum = 1;
					float hBlendOutMaximum = 1;
					float hValue;
					float hBlended = 0;
					if (i > 1) {
						hBlendInMinimum = (float) terrainToolkit.heightBlendPoints[i * 2 - 4];
						hBlendInMaximum = (float) terrainToolkit.heightBlendPoints[i * 2 - 3];
					}
					if (i < numTextures - 1) {
						hBlendOutMinimum = (float) terrainToolkit.heightBlendPoints[i * 2 - 2];
						hBlendOutMaximum = (float) terrainToolkit.heightBlendPoints[i * 2 - 1];
					}
					hValue = heightMapData[Px, Py];
					if (hValue >= hBlendInMaximum && hValue <= hBlendOutMinimum) {
						// Full...
						hBlended = 1;
					} else if (hValue >= hBlendInMinimum && hValue < hBlendInMaximum) {
						// Blend in...
						hBlended = (hValue - hBlendInMinimum) / (hBlendInMaximum - hBlendInMinimum);
					} else if (hValue > hBlendOutMinimum && hValue <= hBlendOutMaximum) {
						// Blend out...
						hBlended = 1 - ((hValue - hBlendOutMinimum) / (hBlendOutMaximum - hBlendOutMinimum));
					}
					// Subtract slope...
					float sValue = slopeMapData[Px, Py];
					hBlended -= sValue;
					if (hBlended < 0) {
						hBlended = 0;
					}
					splatMapData[Px, Py, i] = hBlended;
				}
			}
		}
		// Generate splat maps...
		terrainData.SetAlphamaps(0, 0, splatMapData);
		// Clean up...
		heightMapData = null;
		slopeMapData = null;
		splatMapData = null;
	}
	
	public void Start () {
	
		terrainGenerator = GetComponent<TerrainGenerator> ();
	}
}