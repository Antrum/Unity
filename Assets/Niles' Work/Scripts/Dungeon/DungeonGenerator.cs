using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (FloorGenerator))]
[RequireComponent (typeof (SceneryGenerator))]

/// <summary>
/// Used to generate the dungeon.
/// </summary>
public class DungeonGenerator : MonoBehaviour {
	
	// INSTANCES:
	
	protected FloorGenerator floorGenerator;
	protected SceneryGenerator sceneryGenerator;
	
	public Hero Hero;
	
	// VARIABLES:
	
	public Floor[] floors;
	
	public Transform PrefabCellParent;
    public Transform OtherStuffParent;
	
	public int numCellsX = 27;
    public int numCellsZ = 27;
	public int numFloors = 100;
	
	[HideInInspector]
    public int floorNumber = 0;
	
	public CellType[,,] cellTypeGrid;

	// DUNGEON GENERATION:
	
	public CellLocation CenterOfGrid () {
		
		CellLocation centerOfGrid = new CellLocation ((numCellsX - 1) / 2, floorNumber, (numCellsZ - 1) / 2);
		
		return centerOfGrid;
	}
	
	void FillDungeon () {
		
		for (int floor = 0; floor < numFloors; floor++) {
			
			floors[floor] = new Floor(floor);
		}
		
		for (int i = 0; i < numCellsX; i++) {
			
            for (int j = 0; j < numCellsZ; j++) {
				
				for (int floor = 0; floor < numFloors; floor++) {
					
					cellTypeGrid[i, floor, j] = CellType.Empty;
				}
            }
        }
	}
	
	public bool CellConnectedToCell (CellLocation startLocation, CellLocation goalLocation, ref ArrayList knownExistingConnections)
    {
        ArrayList alreadySearchedList = new ArrayList();
        ArrayList toSearchList = new ArrayList();

        bool foundPath = false;
        bool doneWithSearch = false;
        toSearchList.Add(new CellLocation(startLocation.x, floorNumber, startLocation.z));

        while (!doneWithSearch)
        {
            if (toSearchList.Count == 0)
            {
                doneWithSearch = true;
                break;
            }

            CellLocation toSearch = (CellLocation)toSearchList[0];
            toSearchList.RemoveAt(0);
            if (alreadySearchedList.Contains(toSearch) == false)
            {
                alreadySearchedList.Add(toSearch);
            }

            if ((toSearch.x == goalLocation.x && toSearch.z == goalLocation.z) || knownExistingConnections.Contains(toSearch))
            {
                doneWithSearch = true;
                foundPath = true;
                
                foreach (CellLocation pos in alreadySearchedList)
                {
                    knownExistingConnections.Add(pos);
                }

                foreach (CellLocation pos in toSearchList)
                {
                    knownExistingConnections.Add(pos);
                }                

                break;
            }
            else
            {
				
                if (HasDir(toSearch, Dir.East))
                {
                    CellLocation newLocation = new CellLocation(toSearch.x + 1, floorNumber, toSearch.z);
                    if (toSearchList.Contains(newLocation) == false && alreadySearchedList.Contains(newLocation) == false)
                    {
                        toSearchList.Add(newLocation);
                    }
                }

                if (HasDir(toSearch, Dir.West))
                {
                    CellLocation newLocation = new CellLocation(toSearch.x - 1, floorNumber, toSearch.z);
                    if (toSearchList.Contains(newLocation) == false && alreadySearchedList.Contains(newLocation) == false)
                    {
                        toSearchList.Add(newLocation);
                    }
                }

                if (HasDir(toSearch, Dir.North))
                {
                    CellLocation newLocation = new CellLocation(toSearch.x, floorNumber, toSearch.z + 1);
                    if (toSearchList.Contains(newLocation) == false && alreadySearchedList.Contains(newLocation) == false)
                    {
                        toSearchList.Add(newLocation);
                    }
                }

                if (HasDir(toSearch, Dir.South))
                {
                    CellLocation newLocation = new CellLocation(toSearch.x, floorNumber, toSearch.z - 1);
                    if (toSearchList.Contains(newLocation) == false && alreadySearchedList.Contains(newLocation) == false)
                    {
                        toSearchList.Add(newLocation);
                    }
                }
            }
        }

        return foundPath;
    }
	
	public bool HasDir (CellLocation location, Dir dir) {
		
		CellType testType;
		
		if (dir == Dir.North && location.z < numCellsZ - 1)
			testType = cellTypeGrid[location.x, floorNumber, location.z + 1];
		
		else if (dir == Dir.South && location.z > 0) 
			testType = cellTypeGrid[location.x, floorNumber, location.z - 1];
		
		else if (dir == Dir.East && location.x < numCellsX - 1) 
			testType = cellTypeGrid[location.x + 1, floorNumber, location.z];
		
		else if (dir == Dir.West && location.x > 0) 
			testType = cellTypeGrid[location.x - 1, floorNumber, location.z];
		
		else if (dir == Dir.NorthEast && location.z < numCellsZ - 1 && location.x < numCellsX - 1) 
			testType = cellTypeGrid[location.x + 1, floorNumber, location.z + 1];
		
		else if (dir == Dir.NorthWest && location.z < numCellsZ - 1 && location.x > 0) 
			testType = cellTypeGrid[location.x - 1, floorNumber, location.z + 1];
		
		else if (dir == Dir.SouthEast && location.z > 0 && location.x < numCellsX - 1) 
			testType = cellTypeGrid[location.x + 1, floorNumber, location.z - 1];
		
		else if (dir == Dir.SouthWest && location.z > 0 && location.x > 0)
			testType = cellTypeGrid[location.x - 1, floorNumber, location.z - 1];
		
		else return false;
		
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance || testType == CellType.StairsUp || testType == CellType.StairsDown) {
			
			return true;
		}
		
		return false;
	}
	
	// START AND UPDATE:
	
	void Start () {
		
		floorGenerator = GetComponent<FloorGenerator> ();
		sceneryGenerator = GetComponent<SceneryGenerator> ();
		
		floorGenerator.Start ();
		sceneryGenerator.Start ();
		
        cellTypeGrid = new CellType[numCellsX, numFloors, numCellsZ];
		floors = new Floor[numFloors];
		
		FillDungeon();
		
        floorGenerator.CreateFloor();
	}
	
	void Update () {
		
		if (Input.GetKeyUp(KeyCode.N)) floorGenerator.GoToNextFloor();
		if (Input.GetKeyUp(KeyCode.B)) floorGenerator.GoToPreviousFloor();
	}
	
	// HERO:
	
	public void PlaceHeroAtRandom () {
		
		for (int i = 0; i < numCellsX; i++) {
			
            for (int j = 0; j < numCellsZ; j++) {
				
				CellLocation testLocation = new CellLocation(i, floorNumber, j);
				
				CellType testType = cellTypeGrid[i, floorNumber, j];
				
				
				if (testType == CellType.Room && floors[floorNumber].type == FloorType.Dungeon) {
					
					PlaceHero(testLocation);
					break;
				}
				
				if (testType == CellType.RockFloor && floors[floorNumber].type == FloorType.Cave) {
					
					PlaceHero(testLocation);
					break;
				}
			}
		}
	}
	
	void PlaceHero (CellLocation newLocation)
    {
        Hero.heroCellLocation = new Vector3(newLocation.x, floorNumber, newLocation.z);
		
		//print (heroLocation_square.x + " " + heroLocation_square.z);
		
        float heroPositionX = transform.position.x + (newLocation.x * sceneryGenerator.prefabCellWidth);
        float heroPositionZ = transform.position.z + (newLocation.z * sceneryGenerator.prefabCellHeight);

        Hero.transform.position = new Vector3(heroPositionX, floorNumber + 2f, heroPositionZ);
	}
	
	public void SetHeroLocation () {
		
		float heroLocationX = (Hero.transform.position.x - transform.position.x)/sceneryGenerator.prefabCellWidth;
		float heroLocationZ = (Hero.transform.position.z - transform.position.z)/sceneryGenerator.prefabCellHeight;
		
		int heroCellX = (int)(heroLocationX + 0.5f);
		int heroCellZ = (int)(heroLocationZ + 0.5f);
		
		Hero.heroLocation = new Vector3(heroLocationX, floorNumber + 5f, heroLocationZ);
		Hero.heroCellLocation = new Vector3(heroCellX, floorNumber, heroCellZ);
		
	}
}
