using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sword {
	
	public Sword (int _bladeWeaponSet, int _gripWeaponSet, int _guardWeaponSet, int _pommelWeaponSet) {
		
//		transform.gameObject.AddComponent<Blade> ();
//		transform.gameObject.AddComponent<Grip> ();
//		transform.gameObject.AddComponent<Guard> ();
//		transform.gameObject.AddComponent<Pommel> ();
//		
//		blade = transform.gameObject.GetComponent<Blade> ();
//		grip = transform.gameObject.GetComponent<Grip> ();
//		guard = transform.gameObject.GetComponent<Guard> ();
//		pommel = transform.gameObject.GetComponent<Pommel> ();
		
		blade = new Blade (_bladeWeaponSet);
		grip = new Grip (_gripWeaponSet);
		guard = new Guard (_guardWeaponSet);
		pommel = new Pommel (_pommelWeaponSet);
	}
	
	public Transform transform;
	
	public Blade blade;
	public Grip grip;
	public Guard guard;
	public Pommel pommel;
	
	void Start () {
	
	}
}