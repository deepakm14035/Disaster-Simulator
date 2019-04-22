using UnityEngine;
using System.Collections;

public class PID_controller_basic : MonoBehaviour {

	public GameObject t1;
	public GameObject t2;
	public GameObject t3;
	public GameObject t4;

	public Rigidbody rb;


	public Vector3 targetPos=Vector3.zero;
	public float maxForce=100f;
	public float pGain=20f;
	public float iGain=0.5f;
	public float dGain=0.5f;
	private Vector3 integrator = Vector3.zero;
	private Vector3 lastError=Vector3.zero;
	Vector3 curPos=Vector3.zero;
	Vector3 force=Vector3.zero;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		curPos = transform.position;
		Vector3 error = targetPos - curPos;
		integrator += error * Time.deltaTime;
		Vector3 diff = (error - lastError) / Time.deltaTime;
		lastError = error;
		force = (error * pGain) + (integrator * iGain) + (diff * dGain);
		force = Vector3.ClampMagnitude (force, maxForce);
		Vector3 t = (Physics.gravity * rb.mass / 4);
		rb.AddForceAtPosition (-t+force, t1.transform.position);
		rb.AddForceAtPosition (-t+force, t2.transform.position);
		rb.AddForceAtPosition (-t+force, t3.transform.position);
		rb.AddForceAtPosition (-t+force, t4.transform.position);
	}
}
