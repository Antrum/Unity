using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformManager : MonoBehaviour {

	public DungeonGenerator dungeonGenerator;
	
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
		
		if (!prefab.gameObject.renderer) return;
			
		prefab.gameObject.renderer.material = material;
	}
	
	protected void ChangeMaterial (Transform prefab, Material oldMaterial, Material newMaterial) {
		
		if (!prefab.gameObject.renderer) return;
			
		oldMaterial = newMaterial;
	}
	
	protected void ChangeMaterials (Transform prefab, Material[] materials) {
		
		if (!prefab.gameObject.renderer) return;
			
		for (int i = 0; i < materials.Length; i++) {
			
			prefab.gameObject.renderer.materials[i] = materials[i];
		}
	}
	
	protected void ChangeMeshFilter (Transform prefab, Mesh mesh) {
		
		MeshFilter meshFilter = prefab.gameObject.GetComponent<MeshFilter> ();
		
		meshFilter.mesh = mesh;
	}
	
	protected void ChangeMeshCollider (Transform prefab, Mesh mesh) {
		
		MeshCollider meshCollider = prefab.gameObject.GetComponent<MeshCollider> ();
		
		meshCollider.sharedMesh = mesh;
	}
}
