using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class RotorController : MonoBehaviour {

	float speed = 50000.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate (0.0f,0.0f,Time.deltaTime * speed);
		//transform.rotation=Quaternion.AngleAxis(Time.deltaTime*speed,new Vector3(0.0f,1,0.0f));
	}
}
