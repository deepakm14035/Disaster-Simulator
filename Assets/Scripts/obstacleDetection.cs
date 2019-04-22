using UnityEngine;
using System.Collections;

public class obstacleDetection : MonoBehaviour {
	public Rigidbody body; 
	public float velocity=5.0f;
	public float diagonalAngle=0.5f;
	public float range=0.0f;
	public Vector3 destination;
	Vector3 src,dest;
	bool checkSides=false,checkLeft=false, checkRight=false;
	// Use this for initialization
	void Start () {
		src = Vector3.zero;
		dest = Vector3.zero;
		destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("stop"+transform.forward);
		Vector3 rotationa=new Vector3(-Mathf.Sin(30.0f*Mathf.Deg2Rad), 0.0f, Mathf.Cos(30.0f*Mathf.Deg2Rad));
		Debug.DrawLine(src+transform.up,dest+transform.up,Color.yellow);

		//Debug.Log ("asd-"+val);
		if (Input.GetButton ("Vertical")) {
			body.AddForce(transform.forward*velocity*Input.GetAxis("Vertical"));
			//Debug.Log ("forward");
		}
		if (Input.GetButton ("Horizontal")) {
			transform.RotateAround (transform.up, 0.2f*Input.GetAxis("Horizontal"));
			//Debug.Log ("turn");
		}
		RaycastHit hitPoint;
		if (!checkRight && !checkLeft && (Physics.Raycast (transform.position, transform.forward, out hitPoint, range)
		    || Physics.Raycast (transform.position, Quaternion.AngleAxis (Mathf.Deg2Rad * diagonalAngle, transform.up).eulerAngles, range)
		    || Physics.Raycast (transform.position, Quaternion.AngleAxis (-Mathf.Deg2Rad * diagonalAngle, transform.up).eulerAngles, range))) {
			Debug.DrawLine (transform.position, hitPoint.point, Color.black);
			body.velocity = Vector3.zero;
			float angle = 20.0f;
			float dist = range + 3.0f;
			//Debug.Log ("stop"+transform.forward);
			checkSides=true;

			while (angle<70) {
				Vector3 rotation = eeulervector (Quaternion.AngleAxis (-angle, transform.up).eulerAngles);
				Debug.Log ("rotation" + rotation);
				Debug.Log ("forward" + transform.forward);
				Debug.Log (Physics.Raycast (transform.position, rotation, out hitPoint, dist));
				
				if (!Physics.Raycast (transform.position, rotation, out hitPoint, dist)) {
					Debug.DrawLine (transform.position, hitPoint.point, Color.black);
					src = transform.position;
					dest = hitPoint.point;
					Debug.Log ("left");
					transform.position+=(transform.forward*9);
					moveLeft ();
					checkRight=true;
					checkLeft=false;
					break;
				}
				Vector3 rotation2 = eeulervector (Quaternion.AngleAxis (angle, transform.up).eulerAngles);
				
				if (!Physics.Raycast (transform.position, rotation2, out hitPoint, dist)) {
					Debug.DrawLine (transform.position, hitPoint.point, Color.green);
					Debug.Log ("right");
					transform.position+=(transform.forward*9);
					moveRight ();
					checkLeft=true;
					checkRight=false;
					break;
				}
				angle += 1.0f;
				dist = 0 + (5 * Mathf.Sin (angle * Mathf.Deg2Rad));
			}
			/*
			while (angle<70) {
				//Vector3 rotation=new Vector3(-Mathf.Sin(angle*Mathf.Deg2Rad), 0.0f, Mathf.Cos(angle*Mathf.Deg2Rad));
				//rotation+=transform.rotation.eulerAngles;
				Vector3 rotation = eeulervector (Quaternion.AngleAxis (-angle, transform.up).eulerAngles);
				Debug.Log ("rotation" + rotation);
				Debug.Log ("forward" + transform.forward);
				Debug.Log (Physics.Raycast (transform.position, rotation, out hitPoint, dist));

				if (!Physics.Raycast (transform.position, rotation, out hitPoint, dist)) {
					Debug.DrawLine (transform.position, hitPoint.point, Color.black);
					src = transform.position;
					dest = hitPoint.point;
					Debug.Log ("left");
					moveLeft ();
					break;
				}
				Vector3 rotation2 = eeulervector (Quaternion.AngleAxis (angle, transform.up).eulerAngles);

				if (!Physics.Raycast (transform.position, rotation2, out hitPoint, dist)) {
					Debug.DrawLine (transform.position, hitPoint.point, Color.green);
					Debug.Log ("right");
					moveRight ();
					break;
				}
				angle += 1.0f;
				dist = 5 + (5 * Mathf.Sin (angle * Mathf.Deg2Rad));
			}*/
		} 
		if (checkLeft) {
			if (Physics.Raycast (transform.position, -transform.right, out hitPoint, range)) {
				body.transform.position+= (transform.forward*0.1f);
			}
			else{
				checkLeft=false;
				transform.position+=(transform.forward*10.0f);
				transform.RotateAround (transform.up, 90*Mathf.Deg2Rad);
			}
		}
		if (checkRight) {
			if (Physics.Raycast (transform.position, transform.right, out hitPoint, range)) {
				body.transform.position+= (transform.forward*0.1f);
			}
			else{
				checkRight=false;
				transform.position+=(transform.forward*5.0f);
				transform.RotateAround (transform.up, -90*Mathf.Deg2Rad);
			}
		}
		else {
			if(checkSides){
				if(Physics.Raycast (transform.position, transform.right, out hitPoint, 10.0f)){
					body.transform.position+= (transform.forward*0.1f);
					Debug.DrawLine (transform.position, hitPoint.point, Color.red);
				}
				else{
					//body.transform.LookAt(destination,transform.up);
					checkSides=false;
				}
			}
			else
				body.transform.position+= (Vector3.Normalize(destination-transform.position)*0.1f);
			Debug.Log (destination-transform.position);
		}

	}

	Vector3 eeulervector(Vector3 euler)//convert euler to vector3
	{
		float elevation = Mathf.Deg2Rad*(euler.x);
		float heading = Mathf.Deg2Rad *(euler.y);
		return  new Vector3(Mathf.Cos(elevation) * Mathf.Sin(heading), Mathf.Sin(elevation), Mathf.Cos(elevation) * Mathf.Cos(heading));
	}


	void moveLeft(){
		//Quaternion target = Quaternion.AngleAxis(transform.rotation.y+2, transform.up);
		//transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2);
		transform.RotateAround (transform.up, -90*Mathf.Deg2Rad);

	}

	void moveRight(){
		//Quaternion target = Quaternion.AngleAxis(transform.rotation.y-2, transform.up);
		//Debug.Log (transform.rotation.y-2);
		//transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2);
		transform.RotateAround (transform.up, 90*Mathf.Deg2Rad);
	}

}
