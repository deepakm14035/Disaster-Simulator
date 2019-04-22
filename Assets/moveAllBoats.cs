using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveAllBoats : MonoBehaviour {
	public GameObject sea;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up*(sea.transform.position.y - transform.position.y);
	}
}
