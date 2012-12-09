using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordGenerator : MonoBehaviour {
	
	public Mesh[] meshBlades;
	public Mesh[] meshGrips;
	public Mesh[] meshPommels;
	public Mesh[] meshGuards;
	
	public Transform weaponsParent;
	
	public Material[] materials;
	
	public void CreateSword1 () {
		
		Sword sword = new Sword (1, 1, 1, 1);
		
		ChangeMaterial (sword.blade.transform, materials[0]);
		ChangeMaterial (sword.grip.transform, materials[0]);
		ChangeMaterial (sword.guard.transform, materials[0]);
		ChangeMaterial (sword.pommel.transform, materials[0]);
		
		ChangeMesh (sword.blade.transform, meshBlades[0]);
		ChangeMesh (sword.grip.transform, meshGrips[0]);
		ChangeMesh (sword.guard.transform, meshGuards[0]);
		ChangeMesh (sword.pommel.transform, meshPommels[0]);
		
		InstantiateSword (sword.transform);
	}
	
	protected void ChangeMaterial (Transform part, Material material) {
		
		if (part.gameObject.renderer) {
			
			part.gameObject.renderer.material = material;
		}
	}
	
	protected void ChangeMesh (Transform part, Mesh mesh) {
		
		MeshFilter meshFilter;
		MeshCollider meshCollider;
		
		meshFilter = part.gameObject.GetComponent<MeshFilter> ();
		//meshCollider = part.gameObject.GetComponent<MeshCollider> ();
		
		meshFilter.mesh = mesh;
		//meshCollider.sharedMesh = mesh;
	}
	
	public void InstantiateSword (Transform transform) {
		
		float instantiateXPosition = 0;//transform.position.x + (location.x * prefabCellWidth);
		float instantiateYPosition = 8;//transform.position.y - (location.y * floorHeight);
    	float instantiateZPosition = 0;//transform.position.z + (location.z * prefabCellHeight);
		
    	Transform createTransform = (Transform)Instantiate(transform, new Vector3(instantiateXPosition, instantiateYPosition, instantiateZPosition), Quaternion.identity);
		createTransform.Rotate (0, 0, 0);
		//ChangeMaterial(createPrefab);
						
        createTransform.parent = weaponsParent;
	}
	
	void Start () {
	
		//CreateSword1 ();
	}
}
