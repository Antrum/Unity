/// <summary>
/// Void generator.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoidGenerator : MonoBehaviour {
	
	/// <summary>
	/// The dungeon generator.
	/// </summary>
	protected DungeonGenerator dungeonGenerator;
	
	/// <summary>
	/// The floor generator.
	/// </summary>
	protected FloorGenerator floorGenerator;
	
	/// <summary>
	/// The entrance generator.
	/// </summary>
	protected EntranceGenerator entranceGenerator;
	
	/// <summary>
	/// Cell is void.
	/// </summary>
	/// <returns>Is void.</returns>
	/// <param name='type'>If set to <c>true</c> type.</param>
	protected bool CellIsVoid (CellType type) {
		
		if (type == CellType.Empty || type == CellType.Perimeter || type == CellType.StairsPerimeter) {
			
			// Cell is void
			return true;
		}
		
		// Cell is not void
		return false;
	}
	
	/// <summary>
	/// All the void cells.
	/// </summary><returns>
	/// The void cells.</returns>
	public List<Void> AllVoidCells () {
		
		// All the void cells
		List<Void> allVoidCells = new List<Void> ();
		
		for (int i = 1; i < dungeonGenerator.numCellsX - 1; i++) {
			
			for (int k = 1; k < dungeonGenerator.numCellsZ - 1; k++) {
				
				if (CellIsVoid (dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, k])) {
					
					// Void location
					CellLocation voidLocation = new CellLocation (i, dungeonGenerator.floorNumber, k);
					
					// Void cell
					Void voidCell = new Void (voidLocation);
					
					// Add the void cell to all void cells
					allVoidCells.Add (voidCell);
				}
			}
		}
		
		// Return all of the void cells
		return allVoidCells;
	}
	
	/// <summary>
	/// The void cluster is an island.
	/// </summary>
	/// <returns>Is an island.</returns>
	/// <param name='voidCluster'>If set to <c>true</c> void cluster.</param>
	public bool VoidClusterIsIsland (VoidCluster voidCluster) {
		
		foreach (Void voidCell in voidCluster.allVoidCells) {
			
			if (voidCell.location.x == 1 || voidCell.location.x == dungeonGenerator.numCellsX - 2
				|| voidCell.location.z == 1 || voidCell.location.z == dungeonGenerator.numCellsZ - 2) {
				
				// Void cluster is not island
				return false;
			}
		}
		
		// Void cluster is island
		return true;
	}
	
	/// <summary>
	/// Alls the void cluster islands.
	/// </summary>
	/// <returns>The void cluster islands.</returns>
	public List<VoidCluster> AllVoidClusterIslands () {
		
		// The the void cluster islands
		List<VoidCluster> allVoidClusterIslands = new List<VoidCluster> ();
		
		// The the void clusters
		List<VoidCluster> allVoidClusters = new List<VoidCluster> ();
		
		// The unclustered void cells
		List<Void> unclusteredVoidCells = new List<Void> ();
		
		foreach (Void voidCell in AllVoidCells ()) {
			
			// Add void to unclustered void cells
			unclusteredVoidCells.Add (voidCell);
		}
		
		bool finished = false;
		
		while (!finished) {
			
			// The void cluster cells
			List<Void> voidClusterCells = new List<Void> ();
		
			// The known existing connections
			ArrayList knownExistingConnections = new ArrayList();
			
			foreach (Void voidCell in unclusteredVoidCells) {
				
				if (VoidConnectedToVoid(voidCell, unclusteredVoidCells[0], ref knownExistingConnections)) {
					
					// Add void cell to void cluster cells
					voidClusterCells.Add (voidCell);
				}
			}
			
			foreach (Void voidCell in voidClusterCells) {
				
				// Remove the void cell from unclustered void cells
				unclusteredVoidCells.Remove (voidCell);
			}
			
			// The void cluster
			VoidCluster voidCluster = new VoidCluster ();
			
			// Add the void cluster cells to the void clusteres void cells
			voidCluster.allVoidCells = voidClusterCells;
			
			// Add the void cluster to all void clusters
			allVoidClusters.Add (voidCluster);
			
			if (unclusteredVoidCells.Count == 0) {
				
				// Finished clustering void cells
				finished = true;
			}
		}
		
		foreach (VoidCluster voidCluster in allVoidClusters) {
			
			if (VoidClusterIsIsland (voidCluster)) {
				
				// Add void cluster to all void cluster islands
				allVoidClusterIslands.Add (voidCluster);
			}
		}
		
		// Return all the void cluster islands
		return allVoidClusterIslands;
	}
	
	/// <summary>
	/// The entrance locations touching.
	/// </summary>
	/// <returns>The locations touching.</returns>
	/// <param name='voidCluster'>Void cluster.</param>
	protected List<CellLocation> EntranceLocationsTouching (VoidCluster voidCluster) {
		
		// The entrance locations touching
		List<CellLocation> entranceLocationsTouching = new List<CellLocation> ();
		
		foreach (Void voidCell in voidCluster.allVoidCells) {
			
			// The northern location
			CellLocation north = new CellLocation (voidCell.location.x, voidCell.location.y, voidCell.location.z + 1);
			
			// The southern location
			CellLocation south = new CellLocation (voidCell.location.x, voidCell.location.y, voidCell.location.z - 1);
			
			// The eastern location
			CellLocation east = new CellLocation (voidCell.location.x + 1, voidCell.location.y, voidCell.location.z);
			
			// The western location
			CellLocation west = new CellLocation (voidCell.location.x - 1, voidCell.location.y, voidCell.location.z);
			
			if (dungeonGenerator.cellTypeGrid[north.x, north.y, north.z] == CellType.Entrance) 
				
				// Add the northern location to entrances touching
				entranceLocationsTouching.Add (north);
			
			if (dungeonGenerator.cellTypeGrid[south.x, south.y, south.z] == CellType.Entrance) 
				
				// Add the southern location to entrances touching
				entranceLocationsTouching.Add (south);
			
			if (dungeonGenerator.cellTypeGrid[east.x, east.y, east.z] == CellType.Entrance) 
				
				// Add the eastern location to entrances touching
				entranceLocationsTouching.Add (east);
			
			if (dungeonGenerator.cellTypeGrid[west.x, west.y, west.z] == CellType.Entrance) 
				
				// Add the western location to entrances touching
				entranceLocationsTouching.Add (west);
		}
		
		// Return the entrance locations touching
		return entranceLocationsTouching;
	}
	
	/// <summary>
	/// The entrance locations to destroy.
	/// </summary>
	/// <returns>The locations to destroy.</returns>
	/// <param name='allVoidClusters'>All void clusters./param>
	protected List<CellLocation> EntranceLocationsToDestroy (List<VoidCluster> allVoidClusters) {
		
		// The entrance locations to destroy
		List<CellLocation> entranceLocationsToDestroy = new List<CellLocation> ();
		
		foreach (VoidCluster voidCluster in allVoidClusters) {
			
			if (EntranceLocationsTouching (voidCluster).Count > 0) {
				
				// The random index
				int randomIndex = Random.Range (0, EntranceLocationsTouching (voidCluster).Count);
			
				// The chosen location
				CellLocation chosenLocation = EntranceLocationsTouching (voidCluster)[randomIndex];
			
				// Add the location to entrance locations to destroy
				entranceLocationsToDestroy.Add (chosenLocation);
			}
		}
		
		// Return the entrance locations to destroy
		return entranceLocationsToDestroy;
	}
	
	/// <summary>
	/// Destroys the entrances.
	/// </summary>
	public void DestroyEntrances () {
			
		// The entrances to destroy
		List<Entrance> entrancesToDestroy = new List<Entrance> ();
		
		foreach (CellLocation location in EntranceLocationsToDestroy (AllVoidClusterIslands ())) {
				
			foreach (Entrance entrance in entranceGenerator.allEntrances) {
				
				if (entrance.location.x == location.x && entrance.location.z == location.z) {
					
					if (!entrance.entranceToStairs) 
						
						// Add entrance to entrances to destroy
						entrancesToDestroy.Add (entrance);
				}
			}
		}
		
		foreach (Entrance entrance in entrancesToDestroy) {
			
			// Remove the entrance
			entranceGenerator.RemoveEntrance (entrance);
		}
	}
	
	/// <summary>
	/// Voids is connected to void.
	/// </summary>
	/// <returns>
	/// Connected to void.
	/// </returns>
	/// <param name='startVoid'>If set to <c>true</c> start void.</param>
	/// <param name='goalVoid'>If set to <c>true</c> goal void.</param>
	/// <param name='knownExistingConnections'>If set to <c>true</c> known existing connections.</param>
	protected bool VoidConnectedToVoid (Void startVoid, Void goalVoid, ref ArrayList knownExistingConnections) {
		
		// Already searched locations
        ArrayList alreadySearchedList = new ArrayList();
		
		// Locations to search
        ArrayList toSearchList = new ArrayList();
		
        bool foundPath = false;
        bool doneWithSearch = false;
		
		// Add start void to the to search list
        toSearchList.Add(new CellLocation(startVoid.location.x, dungeonGenerator.floorNumber, startVoid.location.z));

        while (!doneWithSearch) {
			
            if (toSearchList.Count == 0) {
				
				// Search done
                doneWithSearch = true;
                break;
            }

			// Location to search
            CellLocation toSearch = (CellLocation)toSearchList[0];
			
			// Remove location from to search list
            toSearchList.RemoveAt(0);
			
            if (alreadySearchedList.Contains(toSearch) == false) {
				
				// Add location to already searched list
                alreadySearchedList.Add(toSearch);
            }

            if ((toSearch.x == goalVoid.location.x && toSearch.z == goalVoid.location.z) || knownExistingConnections.Contains(toSearch)) {
                
				// Search done
				doneWithSearch = true;
				
				// Path found
                foundPath = true;
                
                foreach (CellLocation pos in alreadySearchedList) {
                    
					// Add location to known existing connections
					knownExistingConnections.Add(pos);
                }

                foreach (CellLocation pos in toSearchList) {
                    
					// Add location to known existing connections
					knownExistingConnections.Add(pos);
                }                

                break;
            }
            else {
				
                if (!dungeonGenerator.HasDir(toSearch, Dir.East) && toSearch.x < dungeonGenerator.numCellsX - 2) {
					
					// The new location
                    CellLocation newLocation = new CellLocation(toSearch.x + 1, dungeonGenerator.floorNumber, toSearch.z);
					
                    if (toSearchList.Contains(newLocation) == false && alreadySearchedList.Contains(newLocation) == false) {
						
						// Add the location to the to search list
                        toSearchList.Add(newLocation);
                    }
                }

                if (!dungeonGenerator.HasDir(toSearch, Dir.West) && toSearch.x > 1) {
					
					// The new location
                    CellLocation newLocation = new CellLocation(toSearch.x - 1, dungeonGenerator.floorNumber, toSearch.z);
					
                    if (toSearchList.Contains(newLocation) == false && alreadySearchedList.Contains(newLocation) == false) {
						
						// Add the location to the to search list
                        toSearchList.Add(newLocation);
                    }
                }

                if (!dungeonGenerator.HasDir(toSearch, Dir.North) && toSearch.z < dungeonGenerator.numCellsZ - 2) {
					
					// The new location
                    CellLocation newLocation = new CellLocation(toSearch.x, dungeonGenerator.floorNumber, toSearch.z + 1);
                    
					if (toSearchList.Contains(newLocation) == false && alreadySearchedList.Contains(newLocation) == false) {
						
						// Add the location to the to search list
                        toSearchList.Add(newLocation);
                    }
                }

                if (!dungeonGenerator.HasDir(toSearch, Dir.South) && toSearch.z > 1) {
					
					// The new location
                    CellLocation newLocation = new CellLocation(toSearch.x, dungeonGenerator.floorNumber, toSearch.z - 1);
					
                    if (toSearchList.Contains(newLocation) == false && alreadySearchedList.Contains(newLocation) == false) {
						
						// Add the location to the to search list
                        toSearchList.Add(newLocation);
                    }
                }
            }
        }
		
		// Path found
        return foundPath;
    }
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	public void Start () {
	
		// Get the dungeon generator component
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		
		// Get the floor generator component
		floorGenerator = GetComponent<FloorGenerator> ();
		
		// Get the entrance generator component
		entranceGenerator = GetComponent<EntranceGenerator> ();
	}
}