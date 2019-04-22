using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class markObjectsOnScreen2 : MonoBehaviour {
	public Camera topview;
	public Text template;
	Text[] boats;
	GameObject[] go;
	
	Text[] peoplefollow;
	GameObject[] people;
	int start1=0;
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		if (start1 == 0) {
			go = GameObject.FindGameObjectsWithTag ("boat");
			boats = new Text[go.Length];
			Debug.Log (go.Length);
			Quaternion angle = Quaternion.AngleAxis (90, new Vector3 (1, 0, 0));
			for (int i=0; i<go.Length; i++) {
				Text text = (Text)Instantiate (template,new Vector3(go[i].transform.position.x,topview.transform.position.y-10,go[i].transform.position.z), angle);
				boats [i] = text;
				boats [i].text="boat "+i;
				boats[i].transform.parent=transform;
				boats[i].color=Color.white;
				boats[i].fontSize=20;
			}
			
			people = GameObject.FindGameObjectsWithTag ("savePerson");
			peoplefollow = new Text[people.Length];
			Debug.Log ("ppl-"+people.Length);
			angle = Quaternion.AngleAxis (90, new Vector3 (1, 0, 0));
			for (int i=0; i<people.Length; i++) {
				Text text = (Text)Instantiate (template,new Vector3(people[i].transform.position.x,topview.transform.position.y-10,people[i].transform.position.z), angle);
				peoplefollow [i] = text;
				peoplefollow [i].text="city "+i;
				peoplefollow[i].transform.parent=transform;
				peoplefollow[i].color=Color.white;
				peoplefollow[i].fontSize=20;
			}
			start1=1;
		}
		for (int i=0; i<boats.Length; i++) {
			//topview.orthographicSize
			boats[i].transform.position+=new Vector3(go[i].transform.position.x,topview.transform.position.y-10,go[i].transform.position.z)-boats[i].transform.position;
		}
		for (int i=0; i<people.Length; i++) {
			peoplefollow[i].transform.position+=new Vector3(people[i].transform.position.x,topview.transform.position.y-10,people[i].transform.position.z)-peoplefollow[i].transform.position;
		}
	}
}
