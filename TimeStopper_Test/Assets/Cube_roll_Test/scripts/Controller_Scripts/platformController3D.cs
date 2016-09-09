using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(BoxCollider))]
public class platformController3D : MonoBehaviour {

	public LayerMask collisionMask;

	BoxCollider platformCollider;

	const float skinWidth = 0.06f;
	const float marginWidth = 0.05f;

	List<PassengerMovement>[] passengerMovement = new List<PassengerMovement>[5];

	public int rayCount;
	float XRaySpacing;
	float YRaySpacing;
	float ZRaySpacing;
	static GameObject nullObject;

	public CollisionInfo collisions;
	RaycastOrigins raycastOrigins;
	// Use this for initialization
	void Start () {
		

		platformCollider = GetComponent<BoxCollider> ();
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

		MovePassengers (true);
		transform.Translate (velocity,Space.World);
		MovePassengers (false);
	}

	void MovePassengers(bool moveBeforePlatform)
	{
		
		for (int i = 0; i < 5; i++) {
				foreach (PassengerMovement passenger in passengerMovement[i]) {
					if (passenger.moveBefore == moveBeforePlatform) {
						if (passenger.passengerType == 0) {
						passenger.transform.GetComponent<Controller3D> ().Move (passenger.moveVector,true);
						}
						if (passenger.passengerType == 1) {
							passenger.transform.GetComponent<crateController3D> ().Move (passenger.moveVector);
						}
					}
				}
			}
	}

	void XpCollisions(ref Vector3 velocity){
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		passengerMovement[0] = new List<PassengerMovement> ();

		float directionX = System.Math.Sign (velocity.x);
		float rayLength = (directionX == 1 ? Mathf.Abs (velocity.x) : 2*Mathf.Abs (velocity.x)) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = raycastOrigins.brf;
					
				rayOrigin +=  (directionX == 1 ? Vector3.right * 0 : Vector3.right * velocity.x) + Vector3.up * (YRaySpacing * i + velocity.y) + Vector3.back * (ZRaySpacing * j + velocity.z);
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.right,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.right * rayLength, Color.red);
				if (onhit) {
					if (hit.collider.tag == "Player" && !movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						Player3D p = hit.collider.gameObject.GetComponent<Player3D> ();

						Vector3 hitMovement = Vector3.zero;
						if (p.getGravityAxis ().x < 0) {
							hitMovement = new Vector3 (directionX == -1 ? velocity.x : 0, velocity.y, 0);
							passengerMovement[0].Add (new PassengerMovement (0,hit.transform, hitMovement, directionX == 1));

						}
						/*if (p.getGravityAxis ().y != 0 && directionX == 1) {
							hitMovement=new Vector3 (velocity.x, 0, 0);
							passengerMovement[0].Add (new PassengerMovement (0,hit.transform, hitMovement, directionX == 1));
						}*/
					}
					if (hit.collider.tag == "crate" && !movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						cratePlayer3D p = hit.collider.gameObject.GetComponent<cratePlayer3D> ();

						Vector3 hitMovement = Vector3.zero;
						if (p.getGravityAxis ().x < 0) {
							hitMovement = new Vector3 (directionX == -1 ? velocity.x : 0, velocity.y, 0);
							passengerMovement[0].Add (new PassengerMovement (1,hit.transform, hitMovement, directionX == 1));
						}
						/*if (p.getGravityAxis ().y != 0 && directionX == 1) {
							hitMovement=new Vector3 (velocity.x, 0, 0);
							passengerMovement[0].Add (new PassengerMovement (1,hit.transform, hitMovement, directionX == 1));
						}*/
					}

					rayLength = hit.distance;

					collisions.right = true;
					collisions.xColObject = hit.collider.gameObject;
				}
			}
		}

	}
	void XnCollisions(ref Vector3 velocity){
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		passengerMovement[1] = new List<PassengerMovement> ();

		float directionX = System.Math.Sign (velocity.x);
		float rayLength = (directionX == -1 ? Mathf.Abs (velocity.x) : 2*Mathf.Abs (velocity.x)) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = raycastOrigins.blf;

				rayOrigin += (directionX == -1 ? Vector3.right * 0 : Vector3.right * velocity.x) + Vector3.up * (YRaySpacing * i + velocity.y) + Vector3.back * (ZRaySpacing * j + velocity.z);
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.left ,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.left * rayLength, Color.red);
				if (onhit) {
					if (hit.collider.tag == "Player" && !movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						Player3D p = hit.collider.gameObject.GetComponent<Player3D> ();
						Vector3 hitMovement = Vector3.zero;

						if (p.getGravityAxis ().x > 0) {
							hitMovement = new Vector3 (directionX == 1 ? velocity.x : 0, velocity.y, 0);
							passengerMovement[1].Add (new PassengerMovement (0,hit.transform, hitMovement, directionX == -1));

						}
						/*if (p.getGravityAxis ().y != 0 && directionX == -1) {
							hitMovement = new Vector3 (velocity.x, 0, 0);
							passengerMovement[1].Add (new PassengerMovement (0,hit.transform, hitMovement, directionX == -1));

						}*/
					}
					if (hit.collider.tag == "crate" && !movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						cratePlayer3D p = hit.collider.gameObject.GetComponent<cratePlayer3D> ();
						Vector3 hitMovement = Vector3.zero;

						if (p.getGravityAxis ().x > 0) {
							hitMovement = new Vector3 (directionX == 1 ? velocity.x : 0, velocity.y, 0);
							passengerMovement[1].Add (new PassengerMovement (1,hit.transform, hitMovement, directionX == -1));
						}
						/*if (p.getGravityAxis ().y != 0 && directionX == -1) {
							hitMovement = new Vector3 (velocity.x, 0, 0);
							passengerMovement[1].Add (new PassengerMovement (1,hit.transform, hitMovement, directionX == -1));
						}*/
					}

					rayLength = hit.distance;

					collisions.left = true;
					collisions.xColObject = hit.collider.gameObject;
				}
			}
		}

	}
		
	void YpCollisions(ref Vector3 velocity){
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		passengerMovement[2] = new List<PassengerMovement> ();

		float directionY =  System.Math.Sign (velocity.y);
		float rayLength = (directionY == 1 ? Mathf.Abs (velocity.y) : 2*Mathf.Abs (velocity.y)) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = raycastOrigins.tlf;
				rayOrigin += (directionY == 1 ? Vector3.up * 0 : Vector3.up * velocity.y) + Vector3.right * (XRaySpacing * i + velocity.x ) + Vector3.back * (ZRaySpacing * j +velocity.z);
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.up,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.up * rayLength, Color.red);
				if (onhit) {
					if (hit.collider.tag == "Player" && !movedPassengers.Contains(hit.transform)) {

						movedPassengers.Add (hit.transform);
						Player3D p = hit.collider.gameObject.GetComponent<Player3D> ();
						Vector3 hitMovement = Vector3.zero;

						if (p.getGravityAxis ().y < 0) {
							hitMovement = new Vector3 (velocity.x, directionY == -1 ? velocity.y : 0, 0);
							passengerMovement[2].Add (new PassengerMovement (0,hit.transform, hitMovement, directionY == 1));
						}
						/*if (p.getGravityAxis ().x != 0 && directionY == 1) {
							hitMovement = new Vector3 (0, velocity.y, 0);
							passengerMovement[2].Add (new PassengerMovement (0,hit.transform, hitMovement, directionY == 1));
						} */
					}
					if (hit.collider.tag == "crate" && !movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						cratePlayer3D p = hit.collider.gameObject.GetComponent<cratePlayer3D> ();
						Vector3 hitMovement = Vector3.zero;

						if (p.getGravityAxis ().y < 0) {
							hitMovement = new Vector3 (velocity.x, directionY == -1 ? velocity.y : 0, 0);
							passengerMovement[2].Add (new PassengerMovement (1,hit.transform, hitMovement, directionY == 1));
						}
						/*if (p.getGravityAxis ().x != 0 && directionY == 1) {
							hitMovement = new Vector3 (0, velocity.y , 0);
							passengerMovement[2].Add (new PassengerMovement (1,hit.transform, hitMovement, directionY == 1));
						}*/
					}
					rayLength = hit.distance;

					collisions.above = true;
					collisions.yColObject = hit.collider.gameObject;
				}
			}
		}
	}

	void YnCollisions(ref Vector3 velocity){
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		passengerMovement[3] = new List<PassengerMovement> ();

		float directionY =  System.Math.Sign (velocity.y);
		float rayLength = (directionY == -1 ? Mathf.Abs (velocity.y) : 2*Mathf.Abs (velocity.y)) + skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = raycastOrigins.blf;
				rayOrigin += (directionY == -1 ? Vector3.up * 0 : Vector3.up * velocity.y) +  Vector3.right * (XRaySpacing * i + velocity.x) + Vector3.back * (ZRaySpacing * j +velocity.z);
				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.down,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.down * rayLength, Color.red);
				if (onhit) {
					if (hit.collider.tag == "Player" && !movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						Player3D p = hit.collider.gameObject.GetComponent<Player3D> ();
						Vector3 hitMovement = Vector3.zero;

						if (p.getGravityAxis ().y > 0) {
							hitMovement=new Vector3 (velocity.x, directionY == 1 ? velocity.y : 0, 0);
							passengerMovement[3].Add (new PassengerMovement (0,hit.transform, hitMovement, directionY == -1));
						}
						/*if (p.getGravityAxis ().x != 0 && directionY == -1) {
							hitMovement=new Vector3 (0, velocity.y, 0);
							passengerMovement[3].Add (new PassengerMovement (0,hit.transform, hitMovement, directionY == -1));
						}*/
					}
					if (hit.collider.tag == "crate" && !movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						cratePlayer3D p = hit.collider.gameObject.GetComponent<cratePlayer3D> ();
						Vector3 hitMovement = Vector3.zero;

						if (p.getGravityAxis ().y > 0) {
							hitMovement=new Vector3 (velocity.x, directionY == 1 ? velocity.y : 0, 0);
							passengerMovement[3].Add (new PassengerMovement (1,hit.transform, hitMovement, directionY == -1));
						}
						/*if (p.getGravityAxis ().x != 0 && directionY == -1) {
							hitMovement=new Vector3 (0, velocity.y, 0);
							passengerMovement[3].Add (new PassengerMovement (1,hit.transform, hitMovement, directionY == -1));
						}*/
					}
		
					rayLength = hit.distance;

					collisions.below = true;
					collisions.yColObject = hit.collider.gameObject;
				}
			}
		}
	}

	void ZCollisions(ref Vector3 velocity){
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		passengerMovement[4] = new List<PassengerMovement> ();

		float directionZ = -1* Mathf.Sign (velocity.z);
		float rayLength = Mathf.Abs (velocity.z) + 2*skinWidth;

		for (int i = 0; i < rayCount; i++) {
			for (int j = 0; j < rayCount; j++) {
				Vector3 rayOrigin = (directionZ == -1) ? raycastOrigins.blb : raycastOrigins.blf;
				rayOrigin += Vector3.right * ((XRaySpacing - 1*marginWidth) * i + velocity.x + 1*marginWidth) + Vector3.up * ((YRaySpacing - 1*marginWidth)* j + velocity.y + 1*marginWidth );

				RaycastHit hit;
				bool onhit = Physics.Raycast (rayOrigin, Vector3.forward * directionZ,out hit,rayLength, collisionMask);

				Debug.DrawRay (rayOrigin, Vector3.forward * directionZ * rayLength, Color.red);
				if (onhit) {
					if (hit.collider.tag == "Player" && !movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);
						Vector3 hitMovement = new Vector3 (velocity.x, velocity.y, 0);
						passengerMovement[4].Add (new PassengerMovement (0,hit.transform, hitMovement, true));
					}
					if (hit.collider.tag == "crate" && !movedPassengers.Contains(hit.transform) ) {
						movedPassengers.Add (hit.transform);
						Vector3 hitMovement = new Vector3 (velocity.x, velocity.y, 0);
						passengerMovement[4].Add (new PassengerMovement (1,hit.transform, hitMovement, true));
					}
					rayLength = hit.distance;

					collisions.back = true;
					collisions.zColObject = hit.collider.gameObject;
				}
			}
		}
	}

	void UpdateRaycastOrigins(){
		Bounds bounds = platformCollider.bounds; 
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
		Bounds bounds = platformCollider.bounds;
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

	public struct PassengerMovement{
		public int passengerType;
		public Transform transform;
		public Vector3 moveVector;
		public bool moveBefore;
		public PassengerMovement(int _passengerType,Transform _transform,Vector3 _moveVector,bool _moveBefore){
			passengerType=_passengerType;
			transform=_transform;
			moveVector=_moveVector;
			moveBefore=_moveBefore;
		}
	}
}