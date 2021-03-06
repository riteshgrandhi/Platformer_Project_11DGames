using UnityEngine;
using System.Collections;

[RequireComponent(typeof(crateController3D))]
public class cratePlayer3D : MonoBehaviour {

	public GameObject nullObject;
	public GameObject hero;
	public Vector3 gravityaxis;

	Vector3 velocity;
	float gravity = 25f;
	crateController3D controller;

	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		controller = GetComponent<crateController3D> ();
	}

	void Update () {
		movementHandler ();
		onCrateDeath ();
	}

	void movementHandler()
	{
		changePlayerSpeed ();
		turnCubeActions ();
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
		if (controller.collisions.left || controller.collisions.right) {
			velocity.x = 0;
		}
		if (controller.collisions.back) {
			velocity.z = 0;
		}
		//velocity.x = 10;
		velocity += gravity * getGravityAxis() * Time.deltaTime;
		controller.Move (velocity*Time.deltaTime);
	}

	void changePlayerSpeed()
	{
		if ((controller.collisions.xColObject == hero && Input.GetAxisRaw ("Horizontal") != 0)
		    || (controller.collisions.yColObject == hero && Input.GetAxisRaw ("Vertical") != 0)) {
			hero.GetComponent<Player3D> ().setMoveSpeed (4);
		} else {
			hero.GetComponent<Player3D> ().setMoveSpeed (4);
		}
	}

	void turnCubeActions(){
		turnCubeController tCubeController = nullObject.GetComponent<turnCubeController> ();
		if ((controller.collisions.xColObject.tag == "turncube")) {
			tCubeController = controller.collisions.xColObject.GetComponent<turnCubeController> ();
		} else if ((controller.collisions.yColObject.tag == "turncube")) {
			tCubeController = controller.collisions.yColObject.GetComponent<turnCubeController> ();
		} else if ((controller.collisions.zColObject.tag == "turncube")) {
			tCubeController = controller.collisions.zColObject.GetComponent<turnCubeController> ();
		} else {
		}

		/*if (tCubeController.isTransitioning()) {
			tCubeController.setAttatchment (gameObject);
			setGravityAxis (Vector3.zero);
		}
		setGravityAxis (hero.GetComponent<Player3D> ().getGravityAxis ());*/
	}

	void onCrateDeath()
	{
		if ((controller.collisions.xColObject.tag == "Finish") || (controller.collisions.yColObject.tag == "Finish") || (controller.collisions.zColObject.tag == "Finish")) {
			transform.position = startPos;
		}
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
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              