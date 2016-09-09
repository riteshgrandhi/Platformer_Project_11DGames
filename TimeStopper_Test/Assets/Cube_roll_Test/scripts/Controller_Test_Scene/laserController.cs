using UnityEngine;
using System.Collections;

public class laserController : MonoBehaviour {
	float laserLength=20;
	void Update () {
		shootLaser ();
	}

	void shootLaser()
	{
		LineRenderer l = GetComponent<LineRenderer> ();
		Vector3 rotationVector = transform.rotation * Vector3.right;
		RaycastHit hit;
		Ray ray = new Ray();

		ray.origin = transform.position;
		ray.direction = rotationVector;

		bool onhit = Physics.Raycast (ray, out hit, laserLength);//, LayerMask.NameToLayer ("Obstacles"));
		//Debug.DrawRay (ray.origin, rotationVector * laserLength, Color.green);

		if (!onhit) {
			laserLength = Mathf.Clamp (laserLength + 20 * Time.deltaTime, 0, 20);

		} else {
			laserLength = hit.distance;
			if (hit.collider.gameObject.tag == "Player") {
				hit.collider.gameObject.GetComponent<Player3D> ().isdead = true;
			}
		}
		l.SetPosition (1, new Vector3 (laserLength, 0, 0));
	}
}
