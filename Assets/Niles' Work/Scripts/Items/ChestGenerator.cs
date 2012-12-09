using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestGenerator : MonoBehaviour {

	protected DungeonGenerator dungeonGenerator;
	
	public int numChests;
	
	public List<Chest> allChests = new List<Chest> ();
	
	protected bool ChestCanBePlaced (CellType type) {
		
		switch (type) {
			
		case CellType.Room:
			return true;
			
		default:
			return false;
		}
	}
	
	protected List<CellLocation> PotentialChestLocations (Floor floor) {
		
		List<CellLocation> potentialChestLocations = new List<CellLocation> ();
		
		int j = floor.floorNumber;
		
		for (int i = 1; i < dungeonGenerator.numCellsX - 1; i += 2) {
			
			for (int k = 2; k < dungeonGenerator.numCellsZ - 2; k += 2) {
				
				potentialChestLocations.Add (new CellLocation (i, j, k));
			}
		}
		
		for (int i = 2; i < dungeonGenerator.numCellsX - 2; i += 2) {
			
			for (int k = 1; k < dungeonGenerator.numCellsZ - 1; k += 2) {
				
				potentialChestLocations.Add (new CellLocation (i, j, k));
			}
		}
		
		return potentialChestLocations;
	}
	
	protected List<CellLocation> PossibleChestLocations (Floor floor) {
		
		List<CellLocation> possibleChestLocations = new List<CellLocation> ();
		
		foreach (CellLocation location in PotentialChestLocations (floor)) {
			
			if (ChestCanBePlaced (dungeonGenerator.cellTypeGrid[location.x, location.y, location.z])) {
				
				possibleChestLocations.Add (location);
			}
		}
		
		return possibleChestLocations;
	}
	
	public void GenerateChests (Floor floor) {
		
		bool finished = false;
		
		while (!finished) {
			
			int randomIndex = Random.Range (0, PossibleChestLocations (floor).Count);
			
			Chest chest = new Chest (PossibleChestLocations (floor)[randomIndex]);
			
			AddChest (chest);
			
			if (allChests.Count == numChests || PossibleChestLocations (floor).Count == 0) {
				
				finished = true;
			}
		}
	}
	
	protected void AddChest (Chest chest) {
		
		allChests.Add (chest);
	}
	
	protected void RemoveChest (Chest chest) {
		
		allChests.Remove (chest);
	}
	
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
	}
}
