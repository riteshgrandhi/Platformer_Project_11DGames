using UnityEngine;
using System.Collections;

public class Cube_behaviour : MonoBehaviour {
	bool incontact=false;
	bool intransition=false;
	float hor_value;
	Vector3 current,target;
	GameObject hero;
	public int AllowX=1,AllowY=1,AllowZ=1;
	// Use this for initialization
	void Start () {
		current = Vector3.zero;
		hero=GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Control ();
		StartCoroutine ("Lerper");
	}
	void Control()
	{
		/*if (!intransition) {
			hero.transform.rotation=Quaternion.Euler(0,0,0);
		}*/
		if (incontact) {
			Vector3 getgrav=hero.GetComponent<Hero_controller>().getgravity();
			Vector3 vel = new Vector3(Mathf.Floor(Mathf.Abs(Input.GetAxis("RightHorizontal")))*Mathf.Sign(Input.GetAxis("RightHorizontal")),
			                          Mathf.Floor(Mathf.Abs(Input.GetAxis("RightVertical")))*Mathf.Sign(Input.GetAxis("RightVertical")),0);

			//calcside();
			Vector3 tside=hero.GetComponent<Hero_controller>().getside();
			if(Vector3.Cross(getgrav,tside)==Vector3.zero && getgrav.normalized==-tside){
				if(Input.GetKeyDown(KeyCode.Joystick1Button5) && !intransition)
				{
					Vector3 crossvel=Vector3.Cross(tside,vel);

					if(Vector3.Magnitude(crossvel)!=0){
						turnCube(crossvel);
						intransition=true;
					}
					else{
						if(Vector3.Dot(tside,vel)<0){
							crossvel=Vector3.Cross(vel,new Vector3(0,0,1));
							turnCube(crossvel);
							intransition=true;
						}
					}
				}
			}
		}
	}
	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Player") {
			incontact=true;
		}
	}
	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "Player") {
			incontact=false;
			//side=Vector3.zero;
		}
	}
	void turnCube(Vector3 v)
	{
		Rigidbody hrigid=hero.GetComponent<Rigidbody>();
		GetComponent<FixedJoint>().connectedBody=hrigid;
		
		target = current + (90 * v);
		
		Vector3 getgrav=hero.GetComponent<Hero_controller>().getgravity();
		Vector3 targrav = -Vector3.Cross(getgrav,v).normalized;
		hero.GetComponent<Hero_controller>().setgravity(9.81f*targrav);
	}
	void Lerper()
	{ 
		if (intransition) {
			Vector3 cur = transform.localEulerAngles;
			transform.localEulerAngles = new Vector3 (Mathf.LerpAngle (cur.x, target.x, Time.deltaTime * 10),
		                                         Mathf.LerpAngle (cur.y, target.y, Time.deltaTime * 10),
		                                         Mathf.LerpAngle (cur.z, target.z, Time.deltaTime * 10));


			float a=(Mathf.Round(Mathf.Abs (Vector3.Magnitude (cur))));
			if (a==90 || a==270)
			{
				GetComponent<FixedJoint> ().connectedBody = null;
				transform.localEulerAngles = current;
				intransition=false;
			}
		}
	}
	/*void calcside()
	{
		Vector3 tside=Vector3.zero;
		Vector3 p = hero.transform.position - transform.position;
		float pth = 0.7f;
		if (p.x>=pth || p.x<=-pth) {
			tside = new Vector3 (Mathf.Sign(p.x), 0, 0);
		} else if (p.y>=pth || p.y<=-pth) {
			tside = new Vector3 (0, Mathf.Sign(p.y), 0);
		} else if (p.z>=pth || p.z<=-pth) {
			tside = new Vector3 (0, 0, Mathf.Sign(p.z));
		} else {
			tside = new Vector3(2,2,2);
		}
		//Debug.Log (tside.ToString ());
		hero.GetComponent<Hero_controller>().setside (tside);
	}*/
}