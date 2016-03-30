using UnityEngine;
using System.Collections;

public class character : MonoBehaviour {
	public float speed;
	bool onground=false;
	bool onwall=false;
	Rigidbody r;
	// Use this for initialization
	void Start () {
		r = GetComponent<Rigidbody> ();
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		Control ();
		Debug.Log (onground.ToString());
	}
	void Control(){
		if (onground) {
			if (Input.GetKey (KeyCode.RightArrow)) {
				//r.velocity += new Vector3 (speed / 40, 0, 0);
				r.velocity = new Vector3 (speed, 0, 0);
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				r.velocity = new Vector3 (-speed, 0, 0);
			}
			//r.AddForce (0, -9.81f, 0);
			if (Input.GetKeyDown (KeyCode.Space)) {
				r.velocity += new Vector3 (0, speed/2, 0);
				//r.AddForce(0,10,0);
			}
		}
		if (!onground) {
			if (Input.GetKey (KeyCode.RightArrow)) {
				r.velocity += new Vector3 (speed / 100, 0, 0);
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				r.velocity += new Vector3 (-speed / 100, 0, 0);
			}
		}
		if (onwall) {
				if (Input.GetKey (KeyCode.RightArrow) && Input.GetKeyDown (KeyCode.Space)) {
					r.velocity = new Vector3 (speed/2, speed, 0);
				}
				else if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKeyDown (KeyCode.Space)) {
					r.velocity = new Vector3 (-speed/2, speed, 0);
				}
		}
	}
	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "floor")
			onground = true;
		if (col.gameObject.tag == "wall")
			onwall = true;
	}
	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "floor")
			onground = false;
		if (col.gameObject.tag == "wall")
			onwall = false;
	}
}