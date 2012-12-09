using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public Transform player;
	
	public Transform[] cameraTargets;
	int cameraIndex;
	
	CameraSettings cameraSettings;
	
	public float cameraRotX = 0f;
	public float turnSpeed = 100f;
	
	// Use this for initialization
	void Start () {
		
		cameraSettings = cameraTargets[cameraIndex].GetComponent<CameraSettings> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.Tab)) {
			
			cameraIndex++;
			
			if (cameraIndex >= cameraTargets.Length)
				cameraIndex = 0;
			
			cameraSettings = cameraTargets[cameraIndex].GetComponent<CameraSettings> ();
		}
	
		if (cameraTargets[cameraIndex]) {
			
			if (cameraSettings.newCameraType) {
				
			}
			
			else {
				
				if (cameraSettings.smoothing == 0f) {
				
					transform.position = cameraTargets[cameraIndex].position;
					
				} else {
					
					transform.position = Vector3.Lerp (transform.position, cameraTargets[cameraIndex].position, Time.deltaTime * cameraSettings.smoothing);
				}
				
				if (cameraSettings.rotatable) {
					
					transform.rotation = cameraTargets[cameraIndex].rotation;
				
				} else {
					
					if (transform.rotation != cameraTargets[cameraIndex].rotation) {
						
						transform.rotation = Quaternion.Lerp (transform.rotation, cameraTargets[cameraIndex].rotation, Time.deltaTime * cameraSettings.smoothing);
					}
				}
				
				if (cameraSettings.rotatable) {
					
					cameraRotX -= Input.GetAxis ("Mouse Y") * turnSpeed * Time.deltaTime;
					cameraRotX = Mathf.Clamp(cameraRotX, -cameraSettings.cameraPitchMax, cameraSettings.cameraPitchMax);
					
					//Camera.main.transform.forward = transform.forward;
					Camera.main.transform.Rotate (cameraRotX, 0f, 0f);
				}
			}
		}
	}
}
