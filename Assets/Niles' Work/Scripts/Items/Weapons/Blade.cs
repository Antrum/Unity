using UnityEngine;
using System.Collections;

public class Blade {
	
	public Blade (int _weaponSet) {
		
		weaponSet = _weaponSet;
	}
	
	public Transform transform;
	
	public int weaponSet;
	
	void Start () {
	
		transform.gameObject.AddComponent<MeshFilter> ();
		transform.gameObject.AddComponent<MeshRenderer> ();
	}
}