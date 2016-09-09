using UnityEngine;
using System.Collections;

public class portalController : MonoBehaviour {
	public Transform otherPortal;
	public Vector3 portalAxis;
	public LayerMask pL;

	private bool occupied=false;
	void Start () {
	
	}
	
	void Update () {
		if (!occupied) {
			RaycastHit hit;
			bool onhit = Physics.Raycast (transform.position - (0.49f * portalAxis), 0.3f * portalAxis, out hit, 0.3f, pL);
			Debug.DrawRay (transform.position - (0.49f * portalAxis), 0.3f * portalAxis, Color.green);

			if (onhit) {
				GameObject player = hit.collider.gameObject;
				occupied = true;
				OnPlayerEnterPortal (player);
			}
		}
	}
	void OnPlayerEnterPortal(GameObject player){
		Vector3 playerVel = player.GetComponent<Player3D> ().getVelocity ();
		Vector3 otherPortalAxis = otherPortal.GetComponent<portalController> ().portalAxis;
		Vector3 setPlayerVel=Vector3.zero;

		player.GetComponent<Player3D> ().transform.position = otherPortal.position + 0.4f*otherPortalAxis;
		if (Vector3.Dot (portalAxis, otherPortalAxis) != 0) {
			if (Vector3.Cross (portalAxis, otherPortalAxis).x != 0) {
				setPlayerVel = new Vector3 (-playerVel.x, playerVel.y, playerVel.z);
			} else {
				setPlayerVel = new Vector3 (playerVel.x, -playerVel.y, playerVel.z);
			}
			setPlayerVel *= Vector3.Dot (portalAxis, otherPortalAxis);
		} else {
			setPlayerVel = new Vector3 (Vector3.Cross (portalAxis, otherPortalAxis).z * playerVel.y,-Vector3.Cross (portalAxis, otherPortalAxis).z * playerVel.x, playerVel.z);
			player.transform.Rotate (new Vector3(0,0,90),Space.Self);
		}
		player.GetComponent<Player3D> ().setGravityAxis (-otherPortalAxis);
		player.GetComponent<Player3D> ().setVelocity (setPlayerVel);
		occupied = false;
	}
}
