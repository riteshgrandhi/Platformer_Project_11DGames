using UnityEngine;
using System.Collections;


public class platformPlayer3D : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;
	public float platformSpeed;
	platformController3D controller;

	Vector3 destination;

	void Start () {
		//startPos += transform.position;
		//endPos += transform.position;
		destination = endPos;
		controller = GetComponent<platformController3D> ();
	}
	
	void FixedUpdate () {
		//transform.Translate ((destination - transform.position).normalized * platformSpeed * Time.fixedDeltaTime);
		controller.Move ((destination - transform.position).normalized * platformSpeed * Time.fixedDeltaTime);

		if (Vector3.Distance (transform.position, destination) < platformSpeed * Time.fixedDeltaTime) {
			destination = destination == startPos ? endPos : startPos;
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube (startPos, transform.localScale);

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (endPos, transform.localScale);
	}
}
