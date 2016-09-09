using UnityEngine;
using System.Collections;

public class turnCubeController : MonoBehaviour {

	public float rotateSpeed;
	public GameObject guide;
	public int AllowX=1, AllowY=1, AllowZ=1;

	bool inTransition=false;
	Vector3 current;
	Vector3 currentguide;
	Vector3 target;
	GameObject hero;
	Player3D player3d;
	void Start () {
	}
	
	void Update () {
		StartCoroutine ("TurnCube", target);
	}

	public void UpdateTarget(Vector3 targetRot,GameObject h)
	{
		if (h != null) {
			hero = h;
		}
		player3d = hero.GetComponent<Player3D> ();
		targetRot = new Vector3 (AllowX * targetRot.x, AllowY * targetRot.y, AllowZ * targetRot.z);
		current = transform.localEulerAngles;
	
		target = current + (90 * targetRot);

		if (h != null && target!=current) {
			inTransition = true;
			guide.transform.localPosition = 1 * new Vector3 (-player3d.getGravityAxis ().x,
														-player3d.getGravityAxis ().y,
														-player3d.getGravityAxis ().z);
			//guide.transform.position = hero.transform.position;
			player3d.setGravityAxis (-Vector3.Cross (player3d.getGravityAxis(), targetRot));
		}
	}

	public Vector3 GetCurrent()
	{
		//return transform.localEulerAngles;
		return current;
	}

	public bool isTransitioning()
	{
		return inTransition;
	}

	void TurnCube(Vector3 targetRotation)
	{
		if (inTransition) {

			player3d.setGravityValue (0);
			hero.transform.position = Vector3.Lerp(hero.transform.position, guide.transform.position, Time.deltaTime*20);
			hero.transform.eulerAngles = new Vector3 (Mathf.LerpAngle (hero.transform.eulerAngles.x, guide.transform.eulerAngles.x, Time.deltaTime * 100),
													  Mathf.LerpAngle(hero.transform.eulerAngles.y, guide.transform.eulerAngles.y, Time.deltaTime * 100),
													  Mathf.LerpAngle(hero.transform.eulerAngles.z, guide.transform.eulerAngles.z, Time.deltaTime * 100));
			
			Vector3 cur = transform.localEulerAngles;

			transform.localEulerAngles = new Vector3 (Mathf.LerpAngle (cur.x, targetRotation.x, Time.deltaTime * rotateSpeed),
													  Mathf.LerpAngle (cur.y, targetRotation.y, Time.deltaTime * rotateSpeed),
													  Mathf.LerpAngle (cur.z, targetRotation.z, Time.deltaTime * rotateSpeed));

			float a=(Mathf.Round(Mathf.Abs (Vector3.Magnitude (cur))));
			if (a==90 || a==270)
			{
				transform.localEulerAngles = current;

				currentguide = calcGuideAngle ();
				guide.transform.eulerAngles = currentguide;
				hero.transform.eulerAngles = currentguide;
				guide.transform.position = transform.position;

				player3d.setGravityValue (25);
				inTransition=false;

			}
		}
	}

	Vector3 calcGuideAngle()
	{
		Vector3 g = player3d.getGravityAxis ();
		if (g.x != 0) {
			return(new Vector3 ((g.x > 0 ? 180 : 0), 0, 90 * -Mathf.Sign (g.x)));
		} else if (g.y != 0) {
			return (new Vector3 ((g.y > 0 ? 0 : 180), 0, 0));
		} else if (g.z != 0) {
			return(new Vector3 (90, 0, 0));
		} else {
			return Vector3.zero;
		}
	}
}