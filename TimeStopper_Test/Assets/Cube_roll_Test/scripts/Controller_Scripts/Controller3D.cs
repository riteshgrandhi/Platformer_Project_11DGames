using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Controller3D : MonoBehaviour {

	public LayerMask collisionMask;

	BoxCollider playerCollider;

	const float skinWidth = 0.1f;
	public int rayCount;
	float XRaySpacing;
	float YRaySpacing;
	float ZRaySpacing;
	static GameObject nullObject;

	public CollisionInfo collisions;
	RaycastOrigins raycastOrigins;
	// Use this for initialization
	void Start () {
		playerCollider = GetComponent<BoxCollider> ();
		nullObject = GameObject.FindGameObjectWithTag ("null");
		collisions.Reset ();
	}

	public void Move(Vector3 velocity,bool standingOnPlatform=false){
		CalculateRaySpacing ();

		UpdateRaycastOrigins ();
		collisions.Reset ();

		//if (Vector3.Magnitude(velocity)!= 0) {
			XCollisions (ref velocity);
			YCollisions (ref velocity);
			ZCollisions (ref velocity);
		//}

		if (standingOnPlatform) {
			Vector3 pGrav = GetComponent<Player3D> ().getGravityAxis ();
			if (pGrav.x > 0) {
				collisions.right = true;
				collisions.left = false;
			} else if(pGrav.x < 0) {
				collisions.left = true;
				collisions.right = false;
			}
			if (pGrav.y > 0) {
				collisions.above = true;
				collisions.below = false;
			} else if (pGrav.y < 0) {
				collisions.below = true;
				collisions.above = false;
			}
			if (pGrav.z != 0) {
				collisions.back = true;
			}
		}
		transform.Translate (velocity,Space.World);
	}

	void XCollisions(ref Vector3 velocity){
		float directionX = System.Math.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.blf : raycastOrigins.brf;
				//Vector3 rayOrigin = raycastOrigins.blf;
					
				rayOrigin += Vector3.up * (YRaySpacing * i + velocity.y ) + Vector3.back * (ZRaySpacing * j + velocity.z );
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.right * directionX,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.right * directionX * rayLength, Color.red);
				if (onhit) {
					if (hit.collider.tag == "crate") {
						crateController3D crateCon = hit.collider.gameObject.GetComponent<crateController3D> ();
						crateCon.Move (new Vector3 (velocity.x, 0, 0));
					}
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
					collisions.xColObject = hit.collider.gameObject;
				}
			}
		}

	}
		
	void YCollisions(ref Vector3 velocity){
		float directionY = System.Math.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = (directionY == -1) ? raycastOrigins.blf : raycastOrigins.tlf;
				rayOrigin += Vector3.right * (XRaySpacing * i + velocity.x ) + Vector3.back * (ZRaySpacing * j +velocity.z );
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.up * directionY,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.up * directionY * rayLength, Color.red);
				if (onhit) {
					if (hit.collider.tag == "crate") {
						crateController3D crateCon = hit.collider.gameObject.GetComponent<crateController3D> ();
						crateCon.Move (new Vector3 (0, velocity.y, 0));
						//cratePlayer3D cP = hit.collider.gameObject.GetComponent<cratePlayer3D> ();
					}
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;

					collisions.below = directionY == -1;
					collisions.above = directionY == 1;
					collisions.yColObject = hit.collider.gameObject;
				}
			}
		}
	}

	void ZCollisions(ref Vector3 velocity){
		float directionZ = System.Math.Sign (velocity.z);
		float rayLength = Mathf.Abs (velocity.z) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = (directionZ == -1) ? raycastOrigins.blb : raycastOrigins.blf;
				rayOrigin += Vector3.right * (XRaySpacing * i + velocity.x ) + Vector3.up * (YRaySpacing * j + velocity.y );
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.forward * directionZ,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.forward * directionZ * rayLength, Color.red);
				if (onhit) {
					velocity.z = (hit.distance - skinWidth) * directionZ;
					rayLength = hit.distance;

					collisions.back = true;
					collisions.zColObject = hit.collider.gameObject;

				}
			}
		}
	}

	void UpdateRaycastOrigins(){
		Bounds bounds = playerCollider.bounds; 
		bounds.Expand (skinWidth * -2);

		raycastOrigins.blf = new Vector3 (bounds.min.x, bounds.min.y, bounds.max.z);
		raycastOrigins.blb = new Vector3 (bounds.min.x, bounds.min.y, bounds.min.z);
		raycastOrigins.tlf = new Vector3 (bounds.min.x, bounds.max.y, bounds.max.z);
		raycastOrigins.tlb = new Vector3 (bounds.min.x, bounds.max.y, bounds.min.z);
		raycastOrigins.brf = new Vector3 (bounds.max.x, bounds.min.y, bounds.max.z);
		raycastOrigins.brb = new Vector3 (bounds.max.x, bounds.min.y, bounds.min.z);
		raycastOrigins.trf = new Vector3 (bounds.max.x, bounds.max.y, bounds.max.z);
		raycastOrigins.trb = new Vector3 (bounds.max.x, bounds.max.y, bounds.min.z);
	}

	void CalculateRaySpacing(){
		Bounds bounds = playerCollider.bounds;
		bounds.Expand (skinWidth * -2);

		rayCount =  Mathf.Clamp (rayCount, 2, int.MaxValue);
		XRaySpacing = bounds.size.x / (rayCount - 1);
		YRaySpacing = bounds.size.y / (rayCount - 1);
		ZRaySpacing = bounds.size.z / (rayCount - 1);
	}

	struct RaycastOrigins{
		public Vector3 tlf,tlb,trf,trb,blf,blb,brf,brb;
	}

	public struct CollisionInfo{
			public bool above,below,left,right,back;
		public GameObject xColObject, yColObject, zColObject;

		public void Reset(){
			above = below = false;
			left = right = false;
			back = false;
			xColObject = nullObject;
			yColObject = nullObject;
			zColObject = nullObject;
		}
	}
}
