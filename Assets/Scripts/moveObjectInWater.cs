using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class moveObjectInWater : MonoBehaviour {
	public GameObject sea;
	public float flowSpeed=0.005f;
	public GameObject toFollow;
	int finished=0,boatmoveflag=0;
	GameObject boattomoveto;
	Animator animator;
	// Use this for initialization
	void Start () {
		toFollow = null;
		animator = gameObject.GetComponent<Animator> ();
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
		if (toFollow == null&&finished==0) {
			GameObject[] go = GameObject.FindGameObjectsWithTag ("boat");
			for (int i=0; i<go.Length; i++) {
				if (Vector3.Distance (go [i].transform.position, transform.position) <= 10.0f) {
					if(UIControl.passengers[i]<6f){
						go[i].GetComponent<obstacle2>().waitForPerson=1;
						//transform.position = Vector3.Slerp (transform.position, go [i].transform.position + (transform.up * 0.3f), Time.deltaTime*0.15f);
						toFollow = go [i].gameObject;
						UIControl.passengers[i]++;
						UIControl.noOfPassengers [i].text="boat "+i+": "+UIControl.passengers[i];
						if (gameObject.tag.Equals ("savePerson")) {
							gameObject.tag = "Finish";
							toFollow = gameObject;
							boatmoveflag = 1;
							//statistics.detected_count--;
							statistics.pickedup_count++;
						}

						boatmoveflag=1;
						if(UIControl.passengers[i]>5){
							UIControl.noOfPassengers [i].color=Color.red;
						}
						else
							UIControl.noOfPassengers [i].color=Color.black;
						break;
					}
				}
			}

		} 
		if (gameObject.tag.Equals ("finished")&&finished==0) {
			//toFollow = null;
			finished=1;
			transform.parent=null;
		}

		if (gameObject.tag.Equals ("Finish")) {
			GameObject[] safes=GameObject.FindGameObjectsWithTag("safehouse");
			for(int i=0;i<safes.Length;i++){
				if(Vector3.Distance(safes[i].transform.position,transform.position)<=10f){
					gameObject.tag="finished";
				}
			}
		}
		
		if (boatmoveflag == 1) {
			transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(toFollow.transform.position-transform.position),Time.deltaTime);
			Vector3 targetposition=toFollow.transform.position;
			targetposition.y=transform.position.y;
			transform.position = Vector3.Slerp (transform.position,targetposition, Time.deltaTime*0.5f);
			animator.SetFloat("walk",0.5f);
			if(Vector3.Distance(transform.position,toFollow.transform.position)<4f){
				boatmoveflag=0;
				animator.SetFloat("walk",0f);
				transform.parent=toFollow.transform;
				toFollow.GetComponent<obstacle2>().waitForPerson=0;

				//set x and z rotation to 0
				Vector3 rotation=transform.rotation.eulerAngles;
				rotation.x=0f;
				rotation.z=0f;
				transform.rotation=Quaternion.Euler(rotation);
				toFollow.GetComponent<obstacle2>().waitForPerson=0;
				//if(toFollow.transform.position.y-transform.position.y<3f)
			}
		}

	}
}
