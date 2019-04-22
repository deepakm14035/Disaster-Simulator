using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class markObjectsOnScreen : MonoBehaviour {
	public Camera topview;
	public Text template;
	Text[] boats;
	GameObject[] go;

	static Text[] peoplefollow, exploreText;
	static GameObject[] people,explorationBots;

	// Use this for initialization
	void Start () {
		go = GameObject.FindGameObjectsWithTag ("boat");
		boats = new Text[go.Length];
		//Debug.Log (go.Length);
		Quaternion angle = Quaternion.AngleAxis (90, new Vector3 (1, 0, 0));
		for (int i=0; i<go.Length; i++) {
			go[i].GetComponent<obstacle2>().index=i;
			Text text = (Text)Instantiate (template,new Vector3(go[i].transform.position.x,topview.transform.position.y-10,go[i].transform.position.z), angle);
			boats [i] = text;
			boats [i].text="boat "+i;
			boats[i].transform.parent=transform;
			boats[i].color=Color.white;
			boats[i].fontSize=20;
		}

		people = GameObject.FindGameObjectsWithTag ("undetected");
		peoplefollow = new Text[people.Length];
		Debug.Log ("ppl-----------------------------"+people.Length);
		angle = Quaternion.AngleAxis (90, new Vector3 (1, 0, 0));
		for (int i=0; i<people.Length; i++) {
			Text text = (Text)Instantiate (template,new Vector3(people[i].transform.position.x,topview.transform.position.y-10,people[i].transform.position.z), angle);
			peoplefollow [i] = text;
			//peoplefollow [i].text="person "+i;
			peoplefollow [i].text=people[i].name;

			peoplefollow[i].transform.parent=transform;
			peoplefollow[i].color=Color.red;
			peoplefollow[i].fontSize=20;
		}


		explorationBots = GameObject.FindGameObjectsWithTag ("wanderBots");
		exploreText = new Text[explorationBots.Length];
		Debug.Log ("bots-"+explorationBots.Length);
		angle = Quaternion.AngleAxis (90, new Vector3 (1, 0, 0));
		for (int i=0; i<explorationBots.Length; i++) {
			Text text = (Text)Instantiate (template,new Vector3(explorationBots[i].transform.position.x,topview.transform.position.y-10,explorationBots[i].transform.position.z), angle);
			exploreText [i] = text;
			//exploreText [i].text="bot "+i;
			exploreText [i].text=explorationBots[i].name;

			exploreText[i].transform.parent=transform;
			exploreText[i].color=Color.red;
			exploreText[i].fontSize=18;

		}


	}
	public static void updatePeople(){
		for (int i=0; i<people.Length; i++) {
			if(people[i].tag.Equals("savePerson") || people[i].tag.Equals("detected"))
				peoplefollow[i].color=Color.white;
		}
	}
	// Update is called once per frame
	void Update () {
		for (int i=0; i<boats.Length; i++) {
			//topview.orthographicSize
			boats[i].transform.position+=new Vector3(go[i].transform.position.x,topview.transform.position.y-10,go[i].transform.position.z)-boats[i].transform.position;
		}
		for (int i=0; i<people.Length; i++) {
			peoplefollow[i].transform.position+=new Vector3(people[i].transform.position.x,topview.transform.position.y-10,people[i].transform.position.z)-peoplefollow[i].transform.position;
		}
		for (int i=0; i<exploreText.Length; i++) {
			exploreText[i].transform.position+=new Vector3(explorationBots[i].transform.position.x,topview.transform.position.y-10,explorationBots[i].transform.position.z)-exploreText[i].transform.position;
		}
	}
}
