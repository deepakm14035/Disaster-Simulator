using UnityEngine;
using System.Collections;

public class PID_controller : MonoBehaviour {

	public GameObject t1;
	public GameObject t2;
	public GameObject t3;
	public GameObject t4;
	public Rigidbody rb;
	public float height;

	public float pid_switch=0;


	private Vector3 targetPos;
	public float maxForce=100f;
	public float pGain=20f;
	public float iGain=0.5f;
	public float dGain=0.5f;
	private Vector3 integrator = Vector3.zero;
	private Vector3 lastError=Vector3.zero;
	Vector3 curPos=Vector3.zero;
	Vector3 force=Vector3.zero;

	Vector3 force1=new Vector3(10,10,10);
	Vector3 force2=new Vector3(10,10,10);


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//For T1 and T4
		Vector3 basePos = transform.position;
		Vector3 vectorBetween_t1_t4 = t4.transform.position - t1.transform.position;
		Debug.DrawLine(t4.transform.position, t1.transform.position, Color.green);
		Vector3 midPoint = (t1.transform.position + t4.transform.position) / 2.0f;
		Debug.DrawLine(transform.position,midPoint, Color.cyan);
		Vector3 m1 = t4.transform.position - t1.transform.position;
		Vector3 m2 = transform.position - midPoint;
		Vector3 cross = Vector3.Cross (m1, m2);

		cross = cross.normalized;


		//For T2 and T3
		Vector3 basePos1 = transform.position;
		Vector3 vectorBetween_t2_t3 = t3.transform.position - t2.transform.position;
		Debug.DrawLine(t3.transform.position, t2.transform.position, Color.green);
		Vector3 midPoint1 = (t2.transform.position + t3.transform.position) / 2.0f;
		Debug.DrawLine(transform.position,midPoint1, Color.cyan);
		Vector3 m11 = t3.transform.position - t2.transform.position;
		Vector3 m22 = transform.position - midPoint1;
		Vector3 cross1 = Vector3.Cross (m11, m22);
		cross1 = cross1.normalized;

		//For T2 and T4
		Vector3 basePos2 = transform.position;
		Vector3 vectorBetween_t2_t4 = t4.transform.position - t2.transform.position;
		Debug.DrawLine(t4.transform.position, t2.transform.position, Color.green);
		midPoint = (t2.transform.position + t4.transform.position) / 2.0f;
		Debug.DrawLine(transform.position,midPoint, Color.cyan);
		m1 = t4.transform.position - t2.transform.position;
		m2 = transform.position - midPoint;
		Vector3 cross2 = Vector3.Cross (m1, m2);
		cross2 = cross2.normalized;


		//For T1 and T3
		Vector3 vectorBetween_t1_t3 = t3.transform.position - t2.transform.position;
		Debug.DrawLine(t3.transform.position, t1.transform.position, Color.green);
		midPoint1 = (t1.transform.position + t3.transform.position) / 2.0f;
		Debug.DrawLine(transform.position,midPoint1, Color.cyan);
		m11 = t3.transform.position - t1.transform.position;
		m22 = transform.position - midPoint1;
		Vector3 cross3 = Vector3.Cross (m11, m22);
		cross3 = cross3.normalized;




		targetPos = new Vector3 (transform.position.x,height,transform.position.z);
		curPos = transform.position;
		Vector3 error = targetPos - curPos;
		integrator += error * Time.deltaTime;
		Vector3 diff = (error - lastError) / Time.deltaTime;
		lastError = error;
		force = (error * pGain) + (integrator * iGain) + (diff * dGain);
		force = Vector3.ClampMagnitude (force, maxForce);
		Vector3 t = (Physics.gravity * rb.mass/4);

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis ("Vertical");
			if (h == 1) {
				rb.AddForceAtPosition (cross/5.0f, t1.transform.position,ForceMode.Impulse);
			rb.AddForceAtPosition (cross/5.0f, t3.transform.position,ForceMode.Impulse);
			}
			else if(h==-1){
			rb.AddForceAtPosition(cross1/5.0f, t2.transform.position,ForceMode.Impulse);
			rb.AddForceAtPosition (cross1/5.0f, t4.transform.position,ForceMode.Impulse);
			}

		if (v == -1) {
			rb.AddForceAtPosition (cross2/5.0f, t2.transform.position,ForceMode.Impulse);
			rb.AddForceAtPosition (cross2/5.0f, t3.transform.position,ForceMode.Impulse);
		}
		else if(v==1){
			rb.AddForceAtPosition(cross3/5.0f, t1.transform.position,ForceMode.Impulse);
			rb.AddForceAtPosition (cross3/5.0f, t4.transform.position,ForceMode.Impulse);
		}




		cross = cross * t.magnitude;
		cross1 = cross1 * t.magnitude;
		force = force * pid_switch;//Temporary

		float ang1 = Vector3.Angle (cross, new Vector3(0,1,0));
		float ang2 = Vector3.Angle (cross1, new Vector3(0,1,0));
		float cross_t = cross.magnitude * (float)System.Math.Cos(Mathf.PI/180*ang1);
		float cross1_t = cross1.magnitude * (float)System.Math.Cos(Mathf.PI/180*ang2);


	//	print ("Mag cross:" + cross_t);
	//	print ("Mag cross1:" + cross1_t);


		if (cross_t < cross.magnitude) {
			cross_t = cross.magnitude;
			cross = cross.normalized;
			cross = cross*((cross_t) * 1 /(float) System.Math.Cos (Mathf.PI / 180 * ang1));

		}
		if (cross1_t < cross1.magnitude) {
			cross1_t = cross1.magnitude;
			cross1 = cross1.normalized;
			cross1 = cross1*((cross1_t) * 1 /(float) System.Math.Cos (Mathf.PI / 180 * ang2));
		}
	//	print ("Cross:" + cross.magnitude);
	//	print ("Cross1:" + cross1.magnitude);
	//	cross = Vector3.ClampMagnitude (cross, 1000);
	//	cross1 = Vector3.ClampMagnitude (cross1, 1000);


		rb.AddForceAtPosition (cross+force, t1.transform.position);
		rb.AddForceAtPosition (cross1+force, t2.transform.position);
		rb.AddForceAtPosition (cross+force, t3.transform.position);
		rb.AddForceAtPosition (cross1+force, t4.transform.position);
	}
}
