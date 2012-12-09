using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {
	
	public Transform target;
	
	public float smoothing = 20f;
	
	private Vector3 offset = new Vector3 (-8f, 15f, -8f);
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = transform.position = Vector3.Lerp (transform.position, target.position + offset, Time.deltaTime * smoothing);
	}
}