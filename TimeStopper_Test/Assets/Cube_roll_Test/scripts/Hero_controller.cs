using UnityEngine;
using System.Collections;

public class Hero_controller : MonoBehaviour {
	public float speed;
	public float jumpheight;
	public GameObject cam;

	Vector3 side;
	Vector3 targetG;
	Vector3 gravity;

	bool onground=false;
	//bool sideview=true;

	Rigidbody r;
	// Use this for initialization
	void Start () {
		side = new Vector3 (0, 1, 0);
		gravity = new Vector3 (0, -9.81f, 0);
		targetG = gravity;
		r = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		r.AddForce (gravity);
		Control ();
		camControl ();
		if (!Mathf.Approximately(Vector3.Magnitude(targetG),Vector3.Magnitude(gravity))) {
			gravity = Vector3.Lerp (gravity, targetG, Time.deltaTime * 2);
		}
	}
	void Control()
	{	
		float inputx = Input.GetAxis ("Horizontal")*speed*600*Time.deltaTime;
		float inputy = Input.GetAxis ("Vertical")*speed*600*Time.deltaTime;
		//Vector3 vel = Vector3.Cross (targetG, new Vector3 (0, 0, -1)).normalized;
		Vector3 vel = new Vector3(inputx,0,0);
		Vector3 tempvel = r.velocity;

		if ((Mathf.Abs(side.x)==1||Mathf.Abs(side.y)==1) && side.z==0 && (targetG.y!=0 || targetG.x!=0)) {
			if(Mathf.Abs(targetG.y)==9.81f){
				vel=new Vector3(inputx,0,0);
				tempvel = (0.05f * vel) + new Vector3 (0, tempvel.y, 0);
			}
			if(Mathf.Abs(targetG.x)==9.81f)
			{
				vel=new Vector3(0,inputy,0);
				tempvel = (0.05f * vel) + new Vector3 (tempvel.x, 0, 0);
			}
			r.constraints=RigidbodyConstraints.FreezePositionZ;
			//transform.rotation=Quaternion.Euler(Vector3.zero);
			transform.position=new Vector3(transform.position.x,
			                               transform.position.y,
			                               Mathf.Lerp(transform.position.z,0,Time.deltaTime*4));
			//r.constraints=RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
			//r.constraints=RigidbodyConstraints.FreezeRotation;
		} else if(Mathf.Abs(targetG.z)==9.81f){
			vel=new Vector3(inputx,inputy,0);
			r.constraints=RigidbodyConstraints.None;

			tempvel = (0.05f*vel) + new Vector3 (0, 0, tempvel.z);
		}

		//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,Input.GetAxis ("Horizontal")*45), Time.deltaTime);
		//transform.rotation = Quaternion.Euler (0, 0, Input.GetAxis ("Horizontal") * 90);

		if (onground && targetG.z==0) {
			if(Input.GetKeyDown(KeyCode.Joystick1Button0)||Input.GetKeyDown(KeyCode.Space)){
				//r.AddForce (-targetG.normalized*jumpheight*100);
				tempvel = -jumpheight*targetG.normalized;
			}
		}
		r.velocity = tempvel;
	}
	void camControl()
	{
		Vector3 camOld=cam.transform.position;
		Vector3 heroxy = new Vector3 (transform.position.x, transform.position.y, camOld.z);

		cam.transform.position = heroxy;
	}
	public void setgravity(Vector3 t)
	{
		targetG = t;
		//gravity = Vector3.Lerp (gravity, targetG, Time.deltaTime * 2);
		gravity = targetG;
	}
	public Vector3 getgravity()
	{
		return targetG;
	}
	public void setside(Vector3 s)
	{
		side = s;
	}
	public Vector3 getside()
	{
		return side;
	}
	void OnTriggerStay(Collider col)
	{
		/*if (col.gameObject.tag == "electric") {
			//Destroy(gameObject);
			//GetComponent<Hero_controller>().enabled=false;
			Debug.Log("Dead");
		}*/
		if (col.gameObject.tag == "side+x") {
			//side=new Vector3(1,0,0);
			setside(new Vector3(1,0,0));
		}
		if (col.gameObject.tag == "side-x") {
			//side=new Vector3(-1,0,0);
			setside(new Vector3(-1,0,0));
		}
		if (col.gameObject.tag == "side+y") {
			//side=new Vector3(0,1,0);
			setside(new Vector3(0,1,0));
		}
		if (col.gameObject.tag == "side-y") {
			//side=new Vector3(0,-1,0);
			setside(new Vector3(0,-1,0));
		}
		if (col.gameObject.tag == "side+z") {
			//side=new Vector3(0,0,1);
			setside(new Vector3(0,0,-1));
		}
		//Debug.Log (getside ().ToString ());
	}
	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "turncube") {
			onground = true;
		}
		else {
			onground = false;
		}
	}
	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "turncube") {
			onground = false;
		}
	}
}
