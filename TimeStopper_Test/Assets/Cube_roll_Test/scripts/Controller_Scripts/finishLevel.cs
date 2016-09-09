using UnityEngine;
using System.Collections;

public class finishLevel : MonoBehaviour {

	public Vector3 finishGravity;
	public LayerMask pL;
	public int nextLevelIndex;

	void Update()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}
		RaycastHit hit;
		bool onhit = Physics.Raycast (transform.position,0.6f*finishGravity,out hit,0.6f,pL);
		Debug.DrawRay (transform.position,0.6f*finishGravity,Color.green);

		if (onhit) {
			if (hit.transform.GetComponent<Player3D> ().getGravityAxis () == -finishGravity) {
				print ("Level Complete");
				//EditorApplication.isPlaying = false;
				Application.LoadLevel(nextLevelIndex);
			}
		}
	}
}
