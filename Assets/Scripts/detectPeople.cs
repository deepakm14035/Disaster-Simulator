using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectPeople : MonoBehaviour {
	public static GameObject[] peopleDetected;
	GameObject[] people;
	GameObject temp;
	public static int noOfPeople=0;
	int len;
	public float visibleRadius=200f;
	void Start () {
		len = GameObject.FindGameObjectsWithTag ("undetected").Length;
		//Debug.Log ("len-"+len);
		peopleDetected=new GameObject[len];
		people = GameObject.FindGameObjectsWithTag ("undetected");
	}
	int i,j,flag;
	RaycastHit hit;


	public void detectNearbyPeople(Vector3 position){
		for (int i = 0; i < people.Length; i++) {
			if (Vector3.Distance (people [i].transform.position, position) < 5f) {
				if(people[i].transform.tag.Equals("undetected")){
					temp.tag="detected";
					peopleDetected[noOfPeople]=temp.transform.gameObject;
					noOfPeople++;
					//Debug.Log("found person-"+temp.transform.name);
					markObjectsOnScreen.updatePeople();
					statistics.undetected_count--;
					statistics.detected_count++;
					statistics.updateDisplay ();
				}
			}
		}
	}


	// Update is called once per frame
	void FixedUpdate () {
		for (i=0; i<len; i++) {
			//Debug.DrawLine(transform.position, people[i].transform.position+transform.up);

			if (Physics.Linecast (transform.position, people[i].transform.position+transform.up,out hit)) {
				//Debug.Log("name-"+hit.transform.root.gameObject.name);
				if (Vector3.Distance (transform.position, hit.transform.position) < visibleRadius) {
					temp = hit.transform.root.gameObject;

					if (temp.transform.tag.Equals ("undetected")) {
						temp.tag = "detected";
						peopleDetected [noOfPeople] = temp.transform.gameObject;
						noOfPeople++;
						//Debug.Log("found person-"+temp.transform.name);
						statistics.detected_count++;
						statistics.undetected_count--;
						markObjectsOnScreen.updatePeople ();
						if (GetComponent<myWander> ()) {
							//Debug.Log ("going towards person");
							GetComponent<myWander> ().goTo (people [i].transform.position);
							break;
						}

					}
				}
			}
		}
	}
}
