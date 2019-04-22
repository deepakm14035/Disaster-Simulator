using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class twoPtMove : MonoBehaviour {
	public Vector3 pt1,destination;
	Vector3[] path;
	public GameObject boat;
	int a1=0,b1=1;
	int flagNext=0;
	Vector3[] arr1;
	int previousflag=0,count=0;
	// Use this for initialization
	void Start () {
		int[,] arr = new int[,] {
			{60, 80},
			{80, 410},
			{200, 200},
			{370, 110},
			{280, 360},
			{100, 220},
			{250, 90},
			{410, 210},
			{180, 380}, 
			{80, 30}, 
			{110, 460}, 
			{250, 130}, 
			{350, 390}, 
			{270, 290}, 
			{410, 350}, 
			{240, 280}, 
			{50, 250}};
		//Debug.Log (arr.GetLength(0));
		arr1=new Vector3[arr.GetLength(0)];
		for (int i=0; i<arr.GetLength(0); i++) {
			arr1[i]=new Vector3(arr[i,0],7f,arr[i,1]);
		}
		transform.position = arr1 [0];
		destination = arr1 [1];
		transform.rotation=Quaternion.LookRotation(arr1[b1]-arr1[a1]);
		path=new Vector3[300];
		path[0]=arr1[a1];
	}
	
	// Update is called once per frame
	void Update () {
		if (flagNext == 1) {

			savedata();

			flagNext=0;
			b1++;
			if(a1==b1)
				b1++;
			if(b1>=arr1.Length){
				b1=0;
				a1++;
			}
			transform.position=arr1[a1];
			transform.rotation=Quaternion.LookRotation(arr1[b1]-arr1[a1]);
			destination=arr1[b1];
			count=0;
			path[0]=arr1[a1];
		}

		//transform.position += (transform.up * (sea.transform.position.y - transform.position.y) + 2f * transform.up);
		RaycastHit hitPoint;
		Vector3 changeDirection = Vector3.Normalize (destination - transform.position);
		Vector3 changeDirection2 = Vector3.Normalize (destination - transform.position);

		if (Physics.Raycast (transform.position, transform.forward, out hitPoint, 28.0f)) {
			float direction = 20.0f;
			float i = 0.0f;
			
			for (i=direction; i<=80; i+=20) {
				if (!Physics.Raycast (transform.position + transform.right, transform.forward + (transform.right * i / 100.0f), out hitPoint, 35.0f)) {
					changeDirection += (transform.right * i * 30.0f);
					previousflag = 1;
					break;
				}
				if (!Physics.Raycast (transform.position - transform.right, transform.forward + (-transform.right * i / 100.0f), out hitPoint, 35.0f)) {
					changeDirection += (-transform.right * i * 30.0f);
					previousflag = 1;
					break;
				}
				
			}

		}

		Vector3 dest = new Vector3 (destination.x, transform.position.y, destination.z);
		Vector3 current = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		
		if (Vector3.Distance (current, dest) <= 3f) {
			flagNext=1;
		} else {
			Quaternion target = Quaternion.LookRotation (changeDirection);
			if (previousflag == 1 && changeDirection2 == changeDirection) {
				transform.position += (transform.forward * 0.8f);
				previousflag = 0;
			} else {
				transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime*2f);
				
			}
			transform.position += (transform.forward * 0.4f);
			
		}
		//Debug.Log (count);
		if (Vector3.Distance (path [count], transform.position )>= 2.5f) {
			count++;
			path[count]=transform.position;
		}
		
	}

	void savedata(){
		count++;
		path [count] = destination;
		string filename="("+path[0].x+","+path[0].z+","+path[count].x+","+path[count].z+").txt";
		StreamWriter sw = new StreamWriter (filename);
		for (int i=0; i<=count; i++) {
			sw.Write(path[i].x+","+path[i].z+"\n");
		}
		sw.Close();
	}

}
