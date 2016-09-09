using UnityEngine;
using System.Collections;

public class movementController : MonoBehaviour {
	public float rotateSpeed=10;

	void Update () {
		transform.Rotate (0, 0, rotateSpeed * Time.deltaTime);
	}
}
