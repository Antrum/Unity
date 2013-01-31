using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to generate the scenery.
/// </summary>
public class SceneryGenerator : MonoBehaviour {

	private DungeonGenerator dungeonGenerator;
	
	public Transform shopPrefab;
	
	public Transform Cell_Nothing;
	public Transform Cell_Empty;
	public Transform Cell_Floor;
	public Transform Cell_Wall_Corner;
	public Transform Cell_Wall;
	public Transform Cell_Arch;
	public Transform Cell_Torch;
	public Transform Cell_Door;
	public Transform Cell_StairsUp;
	public Transform Cell_StairsDown;
	public Transform Cell_Perimeter;
	public Transform Cell_WoodBlock;
	public Transform Cell_Rock;
	public Transform Cell_RockFloor;
	
	public Transform Cell_Chest;
	
	public Material[] Materials_Floor;
	public Material[] Materials_Wall;
	public Mesh[] Meshs_Wall;
	
	public Mesh SandMesh_Wall;
	public Mesh SandMesh_PillarLarge;
	public Mesh SandMesh_PillarSmall;
	public Mesh SandMesh_DoorFrame;
	public Mesh SandMesh_Door;
	
	public Material SandMaterial_Misc;
	public Material SandMaterial_Wall;
	
	
	[HideInInspector]
	public float prefabCellWidth;
	[HideInInspector]
    public float prefabCellHeight;
	[HideInInspector]
	public float floorHeight;
	
	Transform GetPrefabFromCellType (CellType cellType) {
		
        switch (cellType) {
			
		case CellType.Room:
		case CellType.Entrance:
		case CellType.Corridor:
			return Cell_Floor;
			
		case CellType.Rock:
			return Cell_Rock;
			
		case CellType.RockFloor:
			return Cell_RockFloor;
			
		case CellType.StairsUp:
		case CellType.StairsDown:
			return Cell_Nothing;
			
		default:
			return Cell_Empty;
			//return Cell_Nothing;
        }
    }
	
	public void InstantiateCellsFromGrid () {

        for (int i = 0; i < dungeonGenerator.numCellsX; i++) {
			
            for (int j = 0; j < dungeonGenerator.numCellsZ; j++) {
				
                if (dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] != CellType.None) {
					
					Transform prefabToMake;
					
					prefabToMake = GetPrefabFromCellType(dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j]);
					
                    float instantiateXPosition = transform.position.x + (i * prefabCellWidth);
					float instantiateYPosition = transform.position.y - (dungeonGenerator.floorNumber * floorHeight);
                    float instantiateZPosition = transform.position.z + (j * prefabCellHeight);
					
                    Transform createCell = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, instantiateYPosition, instantiateZPosition), Quaternion.identity);

                    createCell.parent = dungeonGenerator.PrefabCellParent;
                }
            }
        }
    }
	
	public void InstantiateShop (Shop shop) {
		
		print (shop.location.x);
		
	    float instantiateXPosition = transform.position.x + (shop.location.x * prefabCellWidth);
		float instantiateYPosition = transform.position.y - (shop.location.y * floorHeight);
	    float instantiateZPosition = transform.position.z + (shop.location.z * prefabCellHeight);
		
	    shop.transform = (Transform)Instantiate(shopPrefab, new Vector3(instantiateXPosition, instantiateYPosition, instantiateZPosition), Quaternion.identity);
		
		//shop.transform.animation.Play ("Take 001");
		shop.transform.animation.Rewind ("Take 001");
		shop.transform.animation.Stop ("Take 001");
		if (shop.transform.animation.IsPlaying("Take 001")) {
			
			print ("playing animation");
		}
		
		
		print (shop.transform.position.x + " " + shop.transform.position.y + " " + shop.transform.position.z);
	}
	
	public void InstantiateChests (List<Chest> allChests) {
		
		foreach (Chest chest in allChests) {
			
			InstantiatePrefab (Cell_Chest, chest.location, chest.rotation);
		}
	}
	
	public void InstantiateWallsFromGrid () {

        for (int i = 1; i < dungeonGenerator.numCellsX - 1; i++) {
			
            for (int j = 1; j < dungeonGenerator.numCellsZ - 1; j++) {
				
				bool hasNorth = false;
				bool hasSouth = false;
				bool hasEast = false;
				bool hasWest = false;
				bool hasNorthEast = false;
				bool hasNorthWest = false;
				bool hasSouthEast = false;
				bool hasSouthWest = false;
				
				CellLocation location = new CellLocation(i, dungeonGenerator.floorNumber, j);
					
				if (dungeonGenerator.HasDir(location, Dir.North)) hasNorth = true;
				if (dungeonGenerator.HasDir(location, Dir.South)) hasSouth = true;
				if (dungeonGenerator.HasDir(location, Dir.East)) hasEast = true;
				if (dungeonGenerator.HasDir(location, Dir.West)) hasWest = true;
				if (dungeonGenerator.HasDir(location, Dir.NorthEast)) hasNorthEast = true;
				if (dungeonGenerator.HasDir(location, Dir.NorthWest)) hasNorthWest = true;
				if (dungeonGenerator.HasDir(location, Dir.SouthEast)) hasSouthEast = true;
				if (dungeonGenerator.HasDir(location, Dir.SouthWest)) hasSouthWest = true;
				
				if (dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] == CellType.StairsUp || dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] == CellType.StairsDown) {
					
					if (hasNorth || hasSouth) {
						InstantiatePrefab (Cell_Wall, location, 0);
						InstantiatePrefab (Cell_Wall, location, 180);
					}
					
					if (hasEast || hasWest) {
						InstantiatePrefab (Cell_Wall, location, 90);
						InstantiatePrefab (Cell_Wall, location, 270);
					}
				}
				
				if (dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] == CellType.Room || dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] == CellType.Corridor || dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] == CellType.Entrance) {
					
					// Walls
					if (!hasNorth) InstantiatePrefab (Cell_Wall, location, 90);
					if (!hasSouth) InstantiatePrefab (Cell_Wall, location, 270);
					if (!hasEast) InstantiatePrefab (Cell_Wall, location, 180);
					if (!hasWest) InstantiatePrefab (Cell_Wall, location, 0);
					
					// Corners
					if (!hasNorthEast) {
						if (hasNorth && hasEast) InstantiatePrefab (Cell_Wall_Corner, location, 90);
						if (!hasNorth && !hasEast) InstantiatePrefab (Cell_Wall_Corner, location, 90);
						if (hasNorth && hasWest && !hasEast && !hasSouth && !hasNorthWest) InstantiatePrefab (Cell_Wall_Corner, location, 90);
						if (!hasNorth && !hasWest && hasEast && hasSouth && !hasSouthEast) InstantiatePrefab (Cell_Wall_Corner, location, 90);
					}
					if (!hasNorthWest) {
						if (hasNorth && hasWest) InstantiatePrefab (Cell_Wall_Corner, location, 0);
						if (!hasNorth && !hasWest) InstantiatePrefab (Cell_Wall_Corner, location, 0);
						if (hasNorth && hasEast && !hasWest && !hasSouth && !hasNorthEast) InstantiatePrefab (Cell_Wall_Corner, location, 0);
						if (!hasNorth && !hasEast && hasWest && hasSouth && !hasSouthWest) InstantiatePrefab (Cell_Wall_Corner, location, 0);
					}
					if (!hasSouthEast) {
						if (hasSouth && hasEast) InstantiatePrefab (Cell_Wall_Corner, location, 180);
						if (!hasSouth && !hasEast) InstantiatePrefab (Cell_Wall_Corner, location, 180);
						if (hasSouth && hasWest && !hasEast && !hasNorth && !hasSouthWest) InstantiatePrefab (Cell_Wall_Corner, location, 180);
						if (!hasSouth && !hasWest && hasEast && hasNorth && !hasNorthEast) InstantiatePrefab (Cell_Wall_Corner, location, 180);
					}
					if (!hasSouthWest) {
						if (hasSouth && hasWest) InstantiatePrefab (Cell_Wall_Corner, location, 270);
						if (!hasSouth && !hasWest) InstantiatePrefab (Cell_Wall_Corner, location, 270);
						if (hasSouth && hasEast && !hasWest && !hasNorth && !hasSouthEast) InstantiatePrefab (Cell_Wall_Corner, location, 270);
						if (!hasSouth && !hasEast && hasWest && hasNorth && !hasNorthWest) InstantiatePrefab (Cell_Wall_Corner, location, 270);
					}
				}
				
				// Doors
				if (dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] == CellType.Entrance) {
					if (hasNorth && hasSouth && !hasEast && !hasWest) InstantiatePrefab (Cell_Door, location, 90);
					if (!hasNorth && !hasSouth && hasEast && hasWest) InstantiatePrefab (Cell_Door, location, 0);
				}
				
				// Arches
				if (dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] == CellType.Corridor) {
					if (hasNorth && hasSouth && !hasEast && !hasWest) InstantiatePrefab (Cell_Arch, location, 90);
					if (!hasNorth && !hasSouth && hasEast && hasWest) InstantiatePrefab (Cell_Arch, location, 0);
				}
				
				if (dungeonGenerator.cellTypeGrid[i, dungeonGenerator.floorNumber, j] == CellType.Room) {
					
					// Torches
//					if (i % 2 == 0 && j % 2 != 0) {
//						if (!hasNorth) InstantiatePrefab (Cell_Torch, location, 90);
//						if (!hasSouth) InstantiatePrefab (Cell_Torch, location, 270);
//						if (!hasEast) InstantiatePrefab (Cell_Torch, location, 180);
//						if (!hasWest) InstantiatePrefab (Cell_Torch, location, 0);
//					}
//					if (i % 2 != 0 && j % 2 == 0) {
//						if (!hasNorth) InstantiatePrefab (Cell_Torch, location, 90);
//						if (!hasSouth) InstantiatePrefab (Cell_Torch, location, 270);
//						if (!hasEast) InstantiatePrefab (Cell_Torch, location, 180);
//						if (!hasWest) InstantiatePrefab (Cell_Torch, location, 0);
//					}
				}
			}
		}
	}
	
	public void InstantiateStairsUp (List<Stairs> allStairsUp) {
		
		foreach (Stairs stairs in allStairsUp) {
			
			InstantiatePrefab (Cell_StairsUp, stairs.location, stairs.rotation);
		}
	}
	
	public void InstantiateStairsDown (List<Stairs> allStairsDown) {
		
		foreach (Stairs stairs in allStairsDown) {
			
			InstantiatePrefab (Cell_StairsDown, stairs.location, stairs.rotation);
		}
	}
	
	public void InstantiatePrefab (Transform prefabToMake, CellLocation location, int rotation) {
		
		float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
		float instantiateYPosition = transform.position.y - (location.y * floorHeight);
    	float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
		
    	Transform createPrefab = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, instantiateYPosition, instantiateZPosition), Quaternion.identity);
		createPrefab.Rotate (0, rotation, 0);
		//ChangeMaterial(createPrefab);
						
        createPrefab.parent = dungeonGenerator.OtherStuffParent;
	}
	
	protected void ChangeMaterial (Transform prefab, Material material) {
		
		if (prefab.gameObject.renderer) {
			
			prefab.gameObject.renderer.material = material;
		}
	}
	
	protected void ChangeMaterial (Transform prefab, Material oldMaterial, Material newMaterial) {
		
		if (prefab.gameObject.renderer) {
			
			oldMaterial = newMaterial;
		}
	}
	
	protected void ChangeMaterials (Transform prefab, Material[] materials) {
		
		if (prefab.gameObject.renderer) {
			
			for (int i = 0; i < materials.Length; i++) {
				
				prefab.gameObject.renderer.materials[i] = materials[i];
			}
			
		}
	}
	
	protected void ChangeMesh (Transform prefab, Mesh mesh) {
		
		MeshFilter meshFilter;
		MeshCollider meshCollider;
		
		meshFilter = prefab.gameObject.GetComponent<MeshFilter> ();
		meshCollider = prefab.gameObject.GetComponent<MeshCollider> ();
		
		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
	}
	
	public void GenerateFloorMaterial(Floor floor) {
		
		int randomIndex = Random.Range (0, Materials_Floor.Length);
		Material material = Materials_Floor[randomIndex];
		floor.floorMaterial = material;
	}
	
	public void GenerateWallMaterial(Floor floor) {
		
		int randomIndex = Random.Range (0, Materials_Wall.Length);
		Material material = Materials_Wall[randomIndex];
		floor.wallMaterial = material;
	}
	
	public void GenerateWallMesh(Floor floor) {
		
		int randomIndex = Random.Range (0, Meshs_Wall.Length);
		Mesh mesh = Meshs_Wall[randomIndex];
		floor.wallMesh = mesh;
	}
	
	public void ChangeFloorMaterial (Material material) {
		
		ChangeMaterial (Cell_Floor.GetChild (0), material);
		
		//ChangeMaterial (Cell_StairsDown.GetChild (0), material);
		//ChangeMaterial (Cell_StairsUp.GetChild (0), material);
	}
	
	public void InstantiateFloorTheme (Floor floor) {
		
		InstantiateCellsFromGrid ();
		InstantiateWallsFromGrid ();
		InstantiateStairsUp (floor.allStairsUp);
		InstantiateStairsDown (floor.allStairsDown);
		InstantiateChests (floor.allChests);
		
		if (floor.theme == FloorTheme.Sand) {
		
			ChangeMesh (Cell_Wall.GetChild (0), SandMesh_Wall);
			
			ChangeMesh (Cell_Door.GetChild (2), SandMesh_PillarSmall);
			ChangeMesh (Cell_Door.GetChild (1), SandMesh_PillarSmall);
			ChangeMesh (Cell_Door.GetChild (0), SandMesh_DoorFrame);
			
			ChangeMesh (Cell_Arch.GetChild (2), SandMesh_PillarSmall);
			ChangeMesh (Cell_Arch.GetChild (1), SandMesh_PillarSmall);
			ChangeMesh (Cell_Arch.GetChild (0), SandMesh_DoorFrame);
			
			ChangeMesh (Cell_Wall_Corner.GetChild (0), SandMesh_PillarLarge);
			
			Cell_Wall.GetChild (0).renderer.materials = new Material[2]{SandMaterial_Wall, SandMaterial_Misc};
			Cell_Wall_Corner.GetChild (0).renderer.materials = new Material[2]{SandMaterial_Wall, SandMaterial_Misc};
			
			ChangeMesh (Cell_StairsDown.GetChild (1), SandMesh_Wall);
			ChangeMesh (Cell_StairsDown.GetChild (2), SandMesh_Wall);
			Cell_StairsDown.GetChild (1).renderer.materials = new Material[2]{SandMaterial_Wall, SandMaterial_Misc};
			Cell_StairsDown.GetChild (2).renderer.materials = new Material[2]{SandMaterial_Wall, SandMaterial_Misc};
			
			Cell_Wall.GetChild (0).renderer.materials = new Material[2]{SandMaterial_Wall, SandMaterial_Misc};
			Cell_Wall_Corner.GetChild (0).renderer.materials = new Material[2]{SandMaterial_Wall, SandMaterial_Misc};
			
			ChangeMaterial (Cell_Door.GetChild (0), SandMaterial_Misc);
			ChangeMaterial (Cell_Door.GetChild (1), SandMaterial_Misc);
			ChangeMaterial (Cell_Door.GetChild (2), SandMaterial_Misc);
			
			ChangeMaterial (Cell_Arch.GetChild (0), SandMaterial_Misc);
			ChangeMaterial (Cell_Arch.GetChild (1), SandMaterial_Misc);
			ChangeMaterial (Cell_Arch.GetChild (2), SandMaterial_Misc);
		}
	}
	
	public void ChangeWallMaterial (Material material) {
		
		ChangeMaterial (Cell_Wall_Corner.GetChild (0), material);
		
		ChangeMaterial (Cell_Wall.GetChild (0), material);
		
		ChangeMaterial (Cell_Arch.GetChild (0), material);
		ChangeMaterial (Cell_Arch.GetChild (1), material);
		ChangeMaterial (Cell_Arch.GetChild (2), material);
		
		ChangeMaterial (Cell_Door.GetChild (0), material);
		ChangeMaterial (Cell_Door.GetChild (1), material);
		ChangeMaterial (Cell_Door.GetChild (2), material);
		
		ChangeMaterial (Cell_StairsDown.GetChild (1), material);
		ChangeMaterial (Cell_StairsDown.GetChild (2), material);
		
		ChangeMaterial (Cell_StairsDown.GetChild (0), material);
		ChangeMaterial (Cell_StairsUp.GetChild (0), material);
		ChangeMaterial (Cell_StairsUp.GetChild (1), material);
	}
	
	public void ChangeWallMesh (Mesh mesh) {
		
		ChangeMesh (Cell_Wall.GetChild (0), mesh);
		ChangeMesh (Cell_StairsDown.GetChild (1), mesh);
		ChangeMesh (Cell_StairsDown.GetChild (2), mesh);
		
	}
	
	/// <summary>
	/// Used for initialization.
	/// </summary>
	public void Start () {
	
		dungeonGenerator = GetComponent<DungeonGenerator> ();
		
		prefabCellWidth = Cell_Empty.localScale.x;
        prefabCellHeight = Cell_Empty.localScale.z;
		floorHeight = Cell_Empty.localScale.y / 2;
	}
}
