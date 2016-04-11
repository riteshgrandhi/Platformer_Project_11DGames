using UnityEngine;
using System.Collections;

public class tile_tex_change : MonoBehaviour {

	public Texture2D[] texturepack;

	Texture2D mainbumptex;
	//public Texture2D secbumptex;
	// Use this for initialization
	void Start () {

		int i = textureIndex (transform.localPosition);
		mainbumptex = texturepack [i];
		MeshRenderer rend = GetComponent<MeshRenderer> ();
		Material tilemat = new Material (rend.material);

		float j = i * 0.05f;
		tilemat.color = new Color (tilemat.color.r-j, tilemat.color.g-j, tilemat.color.b-j);
		tilemat.SetTexture ("_BumpMap", mainbumptex);
		//tilemat.SetTexture ("_DetailNormalMap", secbumptex);
		rend.material = tilemat;
	}

	int textureIndex(Vector3 pos)
	{
		float tilepos = pos.magnitude*3;
		int index = (Mathf.RoundToInt (tilepos)) % (texturepack.Length);
		return index;
	}
}
