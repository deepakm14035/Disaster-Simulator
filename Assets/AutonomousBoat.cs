using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Exodrifter.UnityPython.Examples
{
	public class AutonomousBoat : MonoBehaviour {

		NavMeshAgent agent;
		GameObject controller;
		public GameObject personToReach;
		public bool following=false;
		public int count =0;
		public GameObject[] safehouses;
		public float waitTime = 10f;
		float waitedTime=0f;
		public int maxCount=4;
		Vector3 positionTenSecsAgo;
		float time;


		// Use this for initialization
		void Start () {
			agent = GetComponent<NavMeshAgent> ();
			time = 0f;
			positionTenSecsAgo = transform.position;
			agent.autoRepath =true;
			agent.autoBraking = true;
			agent.acceleration = agent.speed * 1.25f;
			controller = GameObject.FindGameObjectWithTag ("boatController");
			safehouses = GameObject.FindGameObjectsWithTag ("safehouse");
		}
		public void setDestination(int[] arr){
			agent.SetDestination (new Vector3(arr[0],arr[1],arr[2]));
		}

		public void goToClosestSafehouse(){
			float mindistance = 9999f;
			GameObject temp = safehouses [0];
			foreach (GameObject safehouse in safehouses) {
				if (Vector3.Distance (safehouse.transform.position, transform.position) < mindistance) {
					mindistance = Vector3.Distance (safehouse.transform.position, transform.position);
					temp = safehouse;
				}
			}
			gameObject.GetComponent<NavMeshAgent> ().SetDestination (temp.transform.position);
			gameObject.tag = "toSafehouse";
			personToReach = temp;
		}

		public void uploadPerson(GameObject person){
			personToReach = person;
			count++;
			gameObject.tag = "boat";
			personToReach.tag = "Finish";
			personToReach.GetComponent<moveObjectInWater2> ().toFollow = gameObject;
			personToReach.GetComponent<moveObjectInWater2> ().boatmoveflag = 1;
			following = false;
			//statistics.detected_count--;
			statistics.pickedup_count++;
			waitedTime = 0f;
			statistics.updateDisplay ();
			Debug.Log (gameObject.name+"  --  "+ person.name);
		}

		// Update is called once per frame
		void Update () {
			if (following) {
				time += Time.deltaTime;
				if (time > 8f) {
					if (Vector3.Distance (transform.position, positionTenSecsAgo) < 20f) {
						agent.ResetPath ();
						agent.SetDestination (personToReach.transform.position);

					}
					time = 0f;
				}
				//Debug.Log (gameObject.name+" dest-"+agent.destination);
				if (Vector3.Distance (agent.destination, transform.position) < 8f) {
					if (Vector3.Distance (personToReach.transform.position, transform.position) > 12f) {
						agent.SetDestination(personToReach.transform.position);
						return;
					}
					waitedTime = 0f;
					agent.ResetPath ();
					if (gameObject.tag.Equals ("toSafehouse")) {
						gameObject.tag = "boat";
						following = false;
						count = 0;
						Debug.Log ("at safehouse, count=0");
					} else {
						
						if (personToReach && 
							Vector3.Distance (personToReach.transform.position, transform.position) < 15f) {
							uploadPerson (personToReach);
							personToReach.GetComponent<moveObjectInWater2> ().getOntheBoat (gameObject);
						}

					}


				}
			} else {
				if (count >= 1) {
					waitedTime += Time.deltaTime;
					if (waitedTime > waitTime) {
						goToClosestSafehouse ();
						following = true;
						time = 0f;
						positionTenSecsAgo = transform.position;
						waitedTime = 0f;
					}
				}
			}
		}
	}
}