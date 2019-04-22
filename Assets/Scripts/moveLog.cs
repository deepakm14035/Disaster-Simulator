using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLog : MonoBehaviour {
	public Vector3 direction;
	// Use this for initialization
	void Start () {
		direction = new Vector3 (0.1f,0f,0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		gameObject.transform.position += direction;
	}
}
