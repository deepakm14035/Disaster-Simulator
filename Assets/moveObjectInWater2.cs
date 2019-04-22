using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Exodrifter.UnityPython.Examples;
public class moveObjectInWater2 : MonoBehaviour {
	public GameObject sea;
	public float flowSpeed=0.005f;
	public GameObject toFollow;
	public int finished=0,boatmoveflag=0;
	GameObject boattomoveto;
	Animator animator;
	GameObject[] safes;
	// Use this for initialization
	void Start () {
		toFollow = null;
		animator = gameObject.GetComponent<Animator> ();
		safes=GameObject.FindGameObjectsWithTag("safehouse");
	}

	public void getOntheBoat(GameObject boat){
		
		//go[i].GetComponent<obstacle2>().waitForPerson=1;
		//transform.position = Vector3.Slerp (transform.position, go [i].transform.position + (transform.up * 0.3f), Time.deltaTime*0.15f);
		toFollow = boat;
		//UIControl.passengers[i]++;
		//UIControl.noOfPassengers [i].text="boat "+i+": "+UIControl.passengers[i];
		boatmoveflag=1;
		/*if(UIControl.passengers[i]>4){
			UIControl.noOfPassengers [i].color=Color.red;
		}
		else
			UIControl.noOfPassengers [i].color=Color.black;
			*/
	}

	// Update is called once per frame
	void Update () {
		if (toFollow==null&&transform.position.y <= sea.transform.position.y-(transform.lossyScale.y/2)&&finished==0) {
			Vector3 pos=transform.position;
			pos.y=sea.transform.position.y-(7*transform.lossyScale.y/8);
			transform.position=Vector3.Slerp(transform.position,pos, Time.deltaTime);
			transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,0,0), 2*Time.time);
		}
		if (toFollow==null&&Mathf.Abs (transform.position.y - sea.transform.position.y) < 1.0f&&finished==0) {

			Ray ray=new Ray(transform.position+transform.up,transform.forward);
			RaycastHit hit;
			Vector3 direction=transform.forward;
			if(!Physics.Raycast(ray, out hit,5.0f)){
				Vector3 pos=transform.position;
				pos.x=2.0f;
				transform.position=Vector3.Slerp(transform.position,pos, Time.deltaTime*flowSpeed);
			}

		}
		/*if (toFollow == null&&finished==0) {
			GameObject[] go = GameObject.FindGameObjectsWithTag ("inProcess");
			for (int i=0; i<go.Length; i++) {
				if (Vector3.Distance (go [i].transform.position, transform.position) <= 8.0f&&go[i].GetComponent<AutonomousBoat>().personToReach.name.Equals(gameObject.name)) {
					if(UIControl.passengers[i]<4){
						getOntheBoat (go[i]);
						break;
					}
				}
			}

		} */
		if (gameObject.tag.Equals ("finished")&&finished==0) {
			//toFollow = null;
			finished=1;
			transform.parent=null;
		}

		if (gameObject.tag.Equals ("Finish")) {
			
			for(int i=0;i<safes.Length;i++){
				if(Vector3.Distance(safes[i].transform.position,transform.position)<=15f){
					gameObject.tag="finished";
					//statistics.pickedup_count--;
					statistics.saved_count++;
					boatmoveflag = 1;
					toFollow = safes [i];
					statistics.updateDisplay ();

					break;
				}
			}
		}

		if (boatmoveflag == 1) {
			transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(toFollow.transform.position-transform.position),Time.deltaTime);
			Vector3 targetposition=toFollow.transform.position;
			targetposition.y=transform.position.y;
			transform.position = Vector3.Slerp (transform.position,targetposition, Time.deltaTime*0.5f);
			animator.SetFloat("walk",0.5f);
			//Debug.Log ("walking-"+gameObject.name);
			if(Vector3.Distance(transform.position,toFollow.transform.position)<3f){
				boatmoveflag=0;
				animator.SetFloat("walk",0f);
				transform.parent=toFollow.transform;
				//toFollow.GetComponent<obstacle2>().waitForPerson=0;

				//set x and z rotation to 0
				Vector3 rotation=transform.rotation.eulerAngles;
				rotation.x=0f;
				rotation.z=0f;
				transform.rotation=Quaternion.Euler(rotation);
				//toFollow.GetComponent<obstacle2>().waitForPerson=0;
				//if(toFollow.transform.position.y-transform.position.y<3f)
			}
		}

	}
}
