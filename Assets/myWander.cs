using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class myWander: MonoBehaviour {
	int start=0;
	RaycastHit hit;
	GameObject target;
	public float speed=1f;
	int move=0;
	Quaternion movement;
	float time=0;
	public GameObject server;
	Vector3 positionToFollow;
	public Vector3 startRect, endRect;
	bool follow=false;

	int[] arr;

	void Start () {
		move = 2;
		arr=new int[26];
		NavMeshTriangulation area = NavMesh.CalculateTriangulation();
		/*for (int i = 0; i < 10; i++) {
			Debug.Log ("verts-"+area.vertices[i]);
			Debug.Log ("tris-"+area.indices[i]);

		}*/
		//if(server.GetComponent<server2>()||!server.GetComponent<server2>().enabled)
		//	gameObject.GetComponent<myWander>().enabled = false;
		
	}
	int flag=0;
	Vector3 temp;
	public void goTo(Vector3 pos){
		positionToFollow = pos;
		follow = true;
	}

	bool isOutsideBounds(Vector3 pos){
		if (pos.x > startRect.x && pos.x < endRect.x) {
			if (pos.z > startRect.z && pos.z < endRect.z) {
				return false;
			}
		}
		return true;
	}

	Quaternion selectDirection(){
		int maxangle = -999;
		float maxdistance = 0f;
		for (int i = -150; i < 150; i+=5) {
			Physics.Raycast (transform.position,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i)*transform.forward,out hit);
			//Debug.DrawRay (transform.position+transform.up*0.5f,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i).eulerAngles);
			//Debug.Log (gameObject.name+"-"+Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i)*transform.forward);
			if (hit.distance > maxdistance) {
				maxangle = i;
				maxdistance = hit.distance;
			}
		}
		return Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*maxangle);
	}

	Quaternion selectDirection1(){
		float maxangle = -999f;
		float maxdistance = 0f;
		bool positive = false;
		for (float i = -150f; Mathf.Abs(i) < 3f; i=-(positive?i-2.5f:i+2.5f)) {
			Physics.Raycast (transform.position,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i)*transform.forward,out hit);
			//Debug.DrawRay (transform.position+transform.up*0.5f,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i).eulerAngles);
			//Debug.Log (gameObject.name+"-"+Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i)*transform.forward);
			if (hit.distance > maxdistance) {
				maxangle = i;
				maxdistance = hit.distance;
			}
		}
		return Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*maxangle);
	}

	Quaternion selectRandomDirection(){
		int maxangle;
		int count = 0;
		do{
			maxangle=Random.Range(-150,150);
			Physics.Raycast (transform.position,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*maxangle)*transform.forward,out hit);
			count++;
		}while(hit.distance<3f&&count<100);
		return Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*maxangle);
	}

	bool getMaxChange(){
		int maxangle = -999;
		float maxdistance = 0f;
		for (int i = -120; i < -60; i+=5) {
			Physics.Raycast (transform.position,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i)*transform.forward,out hit);
			//Debug.DrawRay (transform.position+transform.up*0.5f,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i).eulerAngles);
			//Debug.Log (gameObject.name+"-"+Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i)*transform.forward);
			if (hit.distance > maxdistance) {
				maxangle = i;
				maxdistance = hit.distance;
			}
		}

		for (int i = 60; i < 120; i+=5) {
			Physics.Raycast (transform.position,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i)*transform.forward,out hit);
			//Debug.DrawRay (transform.position+transform.up*0.5f,Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i).eulerAngles);
			//Debug.Log (gameObject.name+"-"+Quaternion.Euler(transform.rotation.eulerAngles+Vector3.up*i)*transform.forward);
			if (hit.distance > maxdistance) {
				maxangle = i;
				maxdistance = hit.distance;
			}
		}
		return false;
	}


	
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.DrawLine(transform.position,transform.forward*5f+transform.position,Color.green);
		//Debug.DrawLine(transform.position,transform.position+5f*(movement*transform.forward));

		/*
		float rand = 3f*(Random.value - Random.value);
		transform.rotation = Quaternion.Euler (new Vector3(0f,transform.rotation.eulerAngles.y+rand,0f));//Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(movement),Time.deltaTime*2f);
		if(flag==0)
			transform.position+=transform.forward*0.5f*speed;//Vector3.Lerp(transform.position,transform.position+movement,Time.deltaTime*speed);
		flag=0;
		if (Physics.Raycast (transform.position, transform.forward, 3f)) {
			flag = 1;
			float rand1 = 3f*(Random.value - Random.value);
			transform.rotation = Quaternion.Euler (new Vector3(0f,transform.rotation.eulerAngles.y+rand1,0f));//Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(movement),Time.deltaTime*2f);

		}*/

		///*
		if(follow){
			if (Mathf.Abs (transform.rotation.eulerAngles.y - Quaternion.LookRotation (positionToFollow - transform.position).eulerAngles.y) < 5f) {
				if (Vector3.Distance (transform.position, positionToFollow) < 3f) {
					GetComponent<detectPeople> ().detectNearbyPeople (positionToFollow);
					follow = false;
				} else {
					transform.position += transform.forward * 0.5f * speed;
				}

			} else {
				transform.rotation = Quaternion.Lerp (transform.rotation,Quaternion.LookRotation(positionToFollow-transform.position),Time.deltaTime);

			}
		}
		else{
			//Debug.DrawRay(transform.position, transform.forward);

			time += Time.deltaTime;
			if (time > 5f) {
				move=0;
				time = 0f;
				//Debug.Log ("time over");
			}
			if (move == 0) {
				//movement = (transform.forward * (0.5f - Random.value) + transform.right * (0.5f-Random.value))*8f;
				movement=selectDirection();
				//movement+=Vector3.Scale(transform.position,new Vector3(0,1,0));
				//movement=transform.position+movement;
				//movement=new Vector3((Random.value*10f)-3f,0f,(Random.value*10f)-5f);
				//movement=transform.position+transform.forward*movement.x+transform.right*movement.z;
				move=1;
			}
			else if (move == 1) {
				//transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(movement),Time.deltaTime*4f);
				transform.rotation=Quaternion.Lerp(transform.rotation,movement,Time.deltaTime*4f);

				//Debug.Log(Vector3.Angle(transform.forward,movement));
				//Debug.Log(transform.forward+", "+movement.eulerAngles.normalized);
				if(Vector3.Magnitude(transform.rotation.eulerAngles-movement.eulerAngles)<0.05f){
					move=2;
					//Debug.Log("moving");
				}
				time=0f;
			}
			else if (move == 2) {
				if ((Physics.Raycast (transform.position, transform.forward, out hit, 3f))) {
					if (!(hit.transform.root.gameObject.tag.Equals ("detected") || hit.transform.root.gameObject.tag.Equals ("boat")
					    || hit.transform.root.gameObject.name.Equals ("boats")
					    || hit.transform.root.gameObject.name.Equals ("safehouses")
					    || hit.transform.root.gameObject.name.Equals ("sea")
					    )) {
						move = 1;
//						Debug.Log ("htting a obstacle. changing-"+gameObject.name+","+hit.transform.root.tag+", name-"+hit.transform.root.gameObject.name);
						movement = selectRandomDirection ();//Quaternion.Euler (transform.rotation.eulerAngles + Vector3.up * (90f + 90f * Random.value)) ;

					} else {
						move = 0;
						move = 1;
						//						Debug.Log ("htting a obstacle. changing-"+gameObject.name+","+hit.transform.root.tag+", name-"+hit.transform.root.gameObject.name);
						movement = selectRandomDirection ();
					}
				} 
				else if (isOutsideBounds (transform.position + 2*transform.forward)) {
					move = 1;
					//Debug.Log ("out of bounds. changing-"+gameObject.name);
					movement = selectDirection1 ();//Quaternion.Euler (transform.rotation.eulerAngles + Vector3.up * (90f + 90f * Random.value)) ;
				}
				else{
					transform.position+=transform.forward*0.5f*speed;//Vector3.Lerp(transform.position,transform.position+movement,Time.deltaTime*speed);
				}
			}
		//*/
		}
	}
}
