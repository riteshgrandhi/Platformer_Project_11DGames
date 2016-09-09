using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Controller3D))]
public class Player3D : MonoBehaviour {

	public GameObject cam;
	public float camSpeed;
	public GameObject nullObject;
	public bool isdead = false;
	public Text infoDisp;
	float accelerationTimeAirborne = 0.1f;
	float accelerationTimeGrounded = 0.02f;
	float moveSpeed = 4;
	float jumpVelocity = 9;

	float velocityXSmoothing;
	float velocityYSmoothing;
	GameObject attatchedObject;
	bool inTransition = false;

	Vector3 velocity;
	float gravity = 25f;
	Vector3 gravityaxis = new Vector3 (0, -1, 0);
	Controller3D controller;

	// Use this for initialization
	void Start () {
		QualitySettings.vSyncCount = 0;
		controller = GetComponent<Controller3D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isdead) {
			//InfoDisp ();
			if (controller.collisions.above || controller.collisions.below) {
				velocity.y = 0;
			}
			if (controller.collisions.left || controller.collisions.right) {
				velocity.x = 0;
			}
			if (controller.collisions.back) {
				velocity.z = 0;
			}
			//specialCollisionHandler ();
			cameraHandler ();

			inputCubeHandler ();

			inputMovementHandler ();

		} else {
			infoDisp.text = "Player Dead, Game Over!";
		}
	}

	void inputMovementHandler()
	{
		Vector2 input = Vector2.zero;
		if (!inTransition) {
			input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		}	

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) {
			if ((getGravityAxis().y == 1 && controller.collisions.above) || (getGravityAxis().y == -1 && controller.collisions.below)) {
				velocity.y = -jumpVelocity * Mathf.Sign (getGravityAxis().y);
			} else if ((getGravityAxis().x == 1 && controller.collisions.right) || (getGravityAxis().x == -1 && controller.collisions.left)) {
				velocity.x = -jumpVelocity * Mathf.Sign (getGravityAxis().x);
			} 
		}
			
		if (getGravityAxis().y != 0) {
			input = new Vector2 (input.x, 0);
			velocity.x = Mathf.SmoothDamp (velocity.x, input.x * moveSpeed, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		} else if (getGravityAxis().x != 0) {
			input = new Vector2 (0, input.y);
			velocity.y = Mathf.SmoothDamp (velocity.y, input.y * moveSpeed, ref velocityYSmoothing, (controller.collisions.left || controller.collisions.right)?accelerationTimeGrounded:accelerationTimeAirborne);
		} else if (getGravityAxis().z != 0) {
			input = new Vector2 (input.x, input.y);
			velocity.x = Mathf.SmoothDamp (velocity.x, input.x * moveSpeed, ref velocityXSmoothing, (controller.collisions.below || controller.collisions.above)?accelerationTimeGrounded:accelerationTimeAirborne);
			velocity.y = Mathf.SmoothDamp (velocity.y, input.y * moveSpeed, ref velocityYSmoothing, (controller.collisions.left || controller.collisions.right)?accelerationTimeGrounded:accelerationTimeAirborne);
		}

		velocity += gravity * getGravityAxis() * Time.deltaTime;
		controller.Move (velocity*Time.deltaTime);
	}

	void inputCubeHandler()
	{
		Vector3 rInput = new Vector3 (Mathf.Floor (Mathf.Abs (Input.GetAxisRaw ("RightHorizontal"))) * Mathf.Sign (Input.GetAxisRaw ("RightHorizontal")),
			                 		  Mathf.Floor (Mathf.Abs (Input.GetAxisRaw ("RightVertical"))) * Mathf.Sign (Input.GetAxisRaw ("RightVertical")),
			                 		  0);
		
		GameObject colObj = (gravityaxis.x != 0) ? controller.collisions.xColObject : ((gravityaxis.y != 0) ? controller.collisions.yColObject : ((gravityaxis.z != 0) ? controller.collisions.zColObject : null));
		turnCubeController colObjController = null;

		if (colObj != null) {
			colObjController = colObj.GetComponent<turnCubeController> ();
			inTransition = colObj.tag == "turncube" ? colObjController.isTransitioning () : false;
			if (colObj.tag == "Finish") {
				Application.LoadLevel(Application.loadedLevel);
			}
		}

		if ((rInput.x * rInput.y==0 && rInput.magnitude!=0) && colObj.tag=="turncube") {
			if (!inTransition) {
				Vector3 crossvel = Vector3.Cross (-getGravityAxis(), rInput);

				if (crossvel.magnitude != 0) {
					colObjController.UpdateTarget (crossvel, gameObject);
				} else {
					if (Vector3.Dot (-getGravityAxis(), rInput) < 0) {
						crossvel = Vector3.Cross (rInput, new Vector3 (0, 0, 1));
						colObjController.UpdateTarget (crossvel, gameObject);
					} else {
						crossvel = Vector3.zero;
						colObjController.UpdateTarget (crossvel, null);
					}
				}
			} 
			velocity = Vector3.zero;
		}
	}

	void cameraHandler()
	{
		Vector3 v = Vector3.zero;
		Vector3 camTarget = transform.position + Vector3.back * 10;
		cam.transform.position = Vector3.SmoothDamp (cam.transform.position, camTarget, ref v, 0.01f * camSpeed);
	}

	public void setGravityAxis(Vector3 g)
	{
		gravityaxis = g;
	}

	public Vector3 getGravityAxis()
	{
		return gravityaxis;
	}

	public void setGravityValue(float g)
	{
		gravity = g;
	}

	public Vector3 getVelocity()
	{
		return velocity;
	}

	public void setVelocity(Vector3 vel){
		velocity = vel;
	}

	public void setMoveSpeed(float ms)
	{
		moveSpeed = ms;
	}

	void InfoDisp()
	{
		infoDisp.text = "Above:" + controller.collisions.above
			+ "\nBelow:" + controller.collisions.below
			+ "\nLeft:" + controller.collisions.left
			+ "\nRight:" + controller.collisions.right
			+ "\nfront:" + controller.collisions.back
			+ "\n\nXCol:" + controller.collisions.xColObject
			+ "\nYCol:" + controller.collisions.yColObject
			+ "\nZCol:" + controller.collisions.zColObject;
	}
}