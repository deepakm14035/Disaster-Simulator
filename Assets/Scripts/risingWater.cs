using UnityEngine;
using System.Collections;

public class risingWater : MonoBehaviour {
	float initial;
	public static float targetHeight;
	// Use this for initialization
	void Start () {
		initial = transform.position.y;
		targetHeight = 10f;
	}
	
	// Update is called once per frame
	void Update () {
		float difference = Mathf.Abs (transform.position.y - targetHeight);
		if(difference>0.01f)
			transform.position+=(transform.up*0.01f*(targetHeight-transform.position.y)/difference);

	}
}
