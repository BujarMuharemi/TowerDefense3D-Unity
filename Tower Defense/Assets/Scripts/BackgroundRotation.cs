using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 	@ Map.Plane.-Object
	Des: used to rotate the objects in the Background
*/

public class BackgroundRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Rotate (Vector3.right * Time.deltaTime*4);
		transform.Rotate (Vector3.up * Time.deltaTime*4);

	}
}
