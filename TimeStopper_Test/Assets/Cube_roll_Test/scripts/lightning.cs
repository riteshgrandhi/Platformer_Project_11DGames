using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class lightning : MonoBehaviour {
	//public Text t;
	MovieTexture m;
	// Use this for initialization
	void Start () {
		m = (MovieTexture)GetComponent<Renderer> ().material.mainTexture;
		m.loop = true;
		m.Play();
	}
	
	// Update is called once per frame
	void Update () {
		//t.text = (1 / Time.deltaTime).ToString ();

	}
}
