using UnityEngine;
using System.Collections;

public class PID_angle : MonoBehaviour {

	Vector3 targetAngle;
	public int mode=0;
	public float angle = 0;
	private Vector3 curAngle=Vector3.zero;
	Vector3 accel;
	Vector3 angSpeed=Vector3.zero;
	public float maxAcc=100;
	public float maxASpeed=50;
	public float dGain = 10;
	public float pGain = 20;

	Vector3 lastError=Vector3.zero;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		curAngle = transform.rotation.eulerAngles;
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (mode == 0)
			targetAngle = new Vector3 (angle, transform.rotation.y, angle);
		else {
			targetAngle = new Vector3 (-angle, transform.rotation.y, angle);
		}
		Vector3 error = targetAngle - curAngle;
		Vector3 diff = (error - lastError) / Time.deltaTime;
		lastError = error;
		accel = error * pGain + diff * dGain;
		accel = Vector3.ClampMagnitude(accel,maxAcc);
		angSpeed += accel * Time.deltaTime;
		angSpeed = Vector3.ClampMagnitude(angSpeed,maxASpeed);
		curAngle += angSpeed * Time.deltaTime;
		rb.rotation = Quaternion.Euler (curAngle);
	}
}
