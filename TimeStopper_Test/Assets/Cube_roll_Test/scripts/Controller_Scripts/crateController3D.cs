using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class crateController3D : MonoBehaviour {

	public LayerMask collisionMask;

	BoxCollider crateCollider;

	const float skinWidth = 0.1f;
	const float marginWidth = 0.1f;

	public int rayCount;
	float XRaySpacing;
	float YRaySpacing;
	float ZRaySpacing;
	static GameObject nullObject;

	public CollisionInfo collisions;
	RaycastOrigins raycastOrigins;
	// Use this for initialization
	void Start () {
		crateCollider = GetComponent<BoxCollider> ();
		nullObject = GameObject.FindGameObjectWithTag ("null");
		collisions.Reset ();
	}

	public void Move(Vector3 velocity){
		CalculateRaySpacing ();

		UpdateRaycastOrigins ();
		collisions.Reset ();


		XpCollisions (ref velocity);
		XnCollisions (ref velocity);

		YpCollisions (ref velocity);
		YnCollisions (ref velocity);

		ZCollisions (ref velocity);

		transform.Translate (velocity,Space.World);
	}

	void XpCollisions(ref Vector3 velocity){
		float directionX = System.Math.Sign (velocity.x);
		//float rayLength = (directionX == 1 ? Mathf.Abs (velocity.x) : 0) + skinWidth - marginWidth;
		float rayLength =  Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = raycastOrigins.brf;
					
				rayOrigin +=  (directionX == 1 ? Vector3.right * 0 : Vector3.right * velocity.x) + Vector3.up * (YRaySpacing * i + velocity.y) + Vector3.back * (ZRaySpacing * j + velocity.z);
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.right,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.right * rayLength, Color.red);
				if (onhit) {
					
					//crateTurn (hit);
					velocity.x = (hit.distance - skinWidth);

					rayLength = hit.distance;

					collisions.right = true;
					collisions.xColObject = hit.collider.gameObject;
				}
			}
		}

	}
	void XnCollisions(ref Vector3 velocity){
		float directionX = System.Math.Sign (velocity.x);
		//float rayLength = (directionX == -1 ? Mathf.Abs (velocity.x) : 0) + skinWidth;
		float rayLength =  Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = raycastOrigins.blf;

				rayOrigin += (directionX == -1 ? Vector3.right * 0 : Vector3.right * velocity.x) + Vector3.up * (YRaySpacing * i + velocity.y) + Vector3.back * (ZRaySpacing * j + velocity.z );
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.left ,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.left * rayLength, Color.red);
				if (onhit) {
					
					velocity.x = (hit.distance - skinWidth) * -1;

					rayLength = hit.distance;

					collisions.left = true;
					collisions.xColObject = hit.collider.gameObject;
				}
			}
		}

	}
		
	void YpCollisions(ref Vector3 velocity){
		float directionY = System.Math.Sign (velocity.y);
		float rayLength = (directionY == 1 ? Mathf.Abs (velocity.y) : 0) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = raycastOrigins.tlf;
				rayOrigin += (directionY == 1 ? Vector3.up * 0 : Vector3.up * velocity.y) + Vector3.right * (XRaySpacing * i + velocity.x) + Vector3.back * (ZRaySpacing * j +velocity.z);
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.up,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.up * rayLength, Color.red);
				if (onhit) {
					/*if (hit.collider.tag == "Player") {
						Player3D p = hit.collider.gameObject.GetComponent<Player3D> ();
						velocity.y = Mathf.Min (0, p.getVelocity ().y);		
					} else {
						velocity.y = (hit.distance - skinWidth);
					}*/

					//crateTurn (hit);
					velocity.y = (hit.distance - skinWidth);

					rayLength = hit.distance;

					collisions.above = true;
					collisions.yColObject = hit.collider.gameObject;
				}
			}
		}
	}

	void YnCollisions(ref Vector3 velocity){
		float directionY = System.Math.Sign (velocity.y);
		float rayLength = (directionY == -1 ? Mathf.Abs (velocity.y) : 0) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = raycastOrigins.blf;
				rayOrigin += (directionY == -1 ? Vector3.up * 0 : Vector3.up * velocity.y) +  Vector3.right * (XRaySpacing * i + velocity.x) + Vector3.back * (ZRaySpacing * j +velocity.z);
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.down,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.down * rayLength, Color.red);
				if (onhit) {
					
					velocity.y = (hit.distance - skinWidth) * -1;

					rayLength = hit.distance;

					collisions.below = true;
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
		Bounds bounds = crateCollider.bounds; 
		bounds.Expand (marginWidth * -2);

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
		Bounds bounds = crateCollider.bounds;
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

	void crateTurn(RaycastHit hit)
	{
		turnCubeController t = hit.collider.tag == "turncube" ? hit.collider.gameObject.GetComponent<turnCubeController> () : nullObject.GetComponent<turnCubeController> ();

		if (t.isTransitioning ()) {
			transform.SetParent (t.gameObject.transform);
			gameObject.GetComponent<cratePlayer3D> ().setGravityAxis (Vector3.zero);
		} else {
			transform.SetParent (null);
		}
	}
}