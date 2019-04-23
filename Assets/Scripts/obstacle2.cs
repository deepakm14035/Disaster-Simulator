using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class obstacle2 : MonoBehaviour {
	public float range=15.0f;
	public Vector3 destination, destination2;
	public GameObject sea;
	public Camera topView;
	public GameObject safehouse;
	public static int manAuto=1;
	bool setTarget=false;
	int previousflag=0;
	int targetSet=0;
	public GameObject person;
	public int index;
	int lastbutton=0;
	public int waitForPerson=0;
	public GameObject client;
	public NavMeshAgent agent;
	public string heldBy;
	public bool canMove=true;
	// Use this for initialization
	void Start () {
		person = null;
		//transform.position += transform.up*(sea.transform.position.y - transform.position.y);
		agent=GetComponent<NavMeshAgent>();
		destination = transform.position;
		destination2 = transform.position;
		GameObject[] go = GameObject.FindGameObjectsWithTag ("boat");
		for (int i=0; i<go.Length; i++) {
			double weight=Vector3.Distance(transform.position,go[i].transform.position);
			if(weight<0.0001f){
				index=i;
				break;
			}
		}
	}

	void OnMouseDown(){
		if(canMove)
			setTarget = true;
		client.GetComponent<client2> ().sendData ("settarget,"+gameObject.name);
	}

	void OnGUI(){
		Vector3 displayPos = topView.WorldToScreenPoint (transform.position);
		//Debug.Log (displayPos);
		GUI.Box (new Rect(displayPos.x-100f,displayPos.z-50f,displayPos.x,displayPos.z),gameObject.name);

	}

	public void setDestination(Vector3 destin){
		destination = destin;
		agent.SetDestination (destination);
		setTarget = false;
		Debug.Log ("moving");
		canMove=true;
	}

	// Update is called once per frame
	void FixedUpdate () {
		//transform.position += transform.up*(sea.transform.position.y - transform.position.y);//new Vector3(transform.position.x,sea.transform.position.y+5f,transform.position.z);

		if (manAuto == 1) {
			if(lastbutton==1){
				lastbutton=0;
				destination=transform.position;
				agent.SetDestination(transform.position);
			}
			if (Input.GetMouseButtonDown (0)) {
				if (setTarget) {
					Ray ray1 = topView.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit1;
					Vector3 p = new Vector3 (ray1.origin.x, sea.transform.position.y, ray1.origin.z);
					Debug.Log (destination);
					client.GetComponent<client2>().sendData(gameObject.name+","+ray1.origin.x+","+ sea.transform.position.y+","+ray1.origin.z);
					//agent.SetDestination(p);
					canMove=true;
				}
			}

			GameObject[] people=GameObject.FindGameObjectsWithTag("detected");
			foreach(GameObject person in people){
				if(Vector3.Distance(person.transform.position,transform.position)<=10f){
					person.tag="savePerson";
					statistics.pickedup_count++;
					statistics.updateDisplay ();
					break;
				}
			}

			if (Vector3.Distance (transform.position,destination) <= 10.0f) {
				GameObject[] safes=GameObject.FindGameObjectsWithTag("safehouse");
				for(int i=0;i<safes.Length;i++){
					if(Vector3.Distance(safes[i].transform.position,transform.position)<=10f){
						GameObject[] peoples=GameObject.FindGameObjectsWithTag("savePerson");
						foreach(GameObject person in peoples){
							if(Vector3.Distance(person.transform.position,transform.position)<=10f){
								person.tag="finished";
								person.transform.parent=null;
								statistics.saved_count++;
								statistics.updateDisplay ();
							}
						}

						break;
					}
				}
				UIControl.passengers[index]=0;
				UIControl.noOfPassengers [index].text="boat "+index+": "+UIControl.passengers[index];
				UIControl.noOfPassengers[index].color=Color.white;

			} else {
				if (previousflag == 1 && waitForPerson==0) {
					previousflag = 0;
				}
			}
		} 

		//auto mode
		else {
			lastbutton=1;
			Debug.Log("setting target");
			if(person!=null&&person.tag.Equals("Finish")){
				targetSet=0;
				person=null;
			}
			if(targetSet==0||person==null){
				GameObject[] people = GameObject.FindGameObjectsWithTag ("savePerson");
				double minweight=99999;
				person=null;
				for (int i=0; i<people.Length; i++) {
					double weight=Vector3.Distance(safehouse.transform.position,people[i].transform.position);
					if(weight<minweight && weight<800f){
						person=people[i];
						minweight=weight;
					}
				}
				if(person!=null){
					targetSet=1;
					Debug.Log("fouund one");
					agent.SetDestination(person.transform.position);

				}
				else if(minweight==99999 || people.Length==0 || (UIControl.passengers[index]>=5)){

					GameObject[] safes = GameObject.FindGameObjectsWithTag ("safehouse");
					double minweight1=99999;
					person=null;
					for (int i=0; i<safes.Length; i++) {
						double weight=Vector3.Distance(transform.position,safes[i].transform.position);
						if(weight<minweight1){
							person=safes[i];
							minweight1=weight;
						}
					}
					targetSet=1;
					agent.SetDestination(person.transform.position);
				}
			}
			else if(targetSet==1&& (person.tag.Equals("savePerson") || person.tag.Equals("safehouse"))){

				destination2=person.transform.position;
				if (Vector3.Distance (transform.position, person.transform.position) <= 10.0f) {
					targetSet=0;
					if(person.tag.Equals("safehouse")){
						targetSet=2;
						GameObject[] persons=GameObject.FindGameObjectsWithTag("Finish");
						for(int i=0;i<persons.Length;i++){
							if(Vector3.Distance(persons[i].transform.position,transform.position)<=10f){
								persons[i].transform.position=(person.transform.position+transform.up*10)+new Vector3(Random.value*5f,0f,Random.value*5f);
								persons[i].tag="finished";
								statistics.saved_count++;
								statistics.updateDisplay ();
							}
						}
						GameObject[] people = GameObject.FindGameObjectsWithTag ("savePerson");
						if(people.Length<=0){
							targetSet=0;
							person=null;
						}
						UIControl.passengers[index]=0;
						UIControl.noOfPassengers [index].text="boat "+index+": "+UIControl.passengers[index];
						UIControl.noOfPassengers[index].color=Color.white;
					}
					else{
						person.tag="Finish";
					}

				} else {

					if (previousflag == 1 && waitForPerson==0) {
						previousflag = 0;
					}
					
				}

			}
		}
	}
	Vector3 eeulervector(Vector3 euler)//convert euler to vector3
	{
		float elevation = Mathf.Deg2Rad*(euler.x);
		float heading = Mathf.Deg2Rad *(euler.y);
		return  new Vector3(Mathf.Cos(elevation) * Mathf.Sin(heading), Mathf.Sin(elevation), Mathf.Cos(elevation) * Mathf.Cos(heading));
	}

	
}



