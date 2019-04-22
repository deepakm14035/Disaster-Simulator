using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class generateRandomObstacles : MonoBehaviour {
	public GameObject obstacle,bottomLeftEnd, upperRightEnd,seaLevel, boatPrefab,personPrefab,botPrefab;
	float time=0f;
	public float interval=10f;

	// Use this for initialization
	void Awake () {
		GameObject t = GameObject.Find("mainmenu");

		Debug.Log ("mainmenu");
		if (t != null && t.GetComponent<mainmenucontroller> ().isAuto) {
			int noofpeople=t.GetComponent<mainmenucontroller> ().people;
			int noofboats=t.GetComponent<mainmenucontroller> ().boats;
			int noofbots=t.GetComponent<mainmenucontroller> ().bots;

			GameObject[] people = GameObject.FindGameObjectsWithTag ("undetected");
			GameObject[] boats= GameObject.FindGameObjectsWithTag ("boat");
			GameObject[] bots= GameObject.FindGameObjectsWithTag ("wanderBots");
			Random.seed =2;
			for (int i = boats.Length; i < noofboats; i++) {
				Vector3 temp = new Vector3 ((upperRightEnd.transform.position.x - bottomLeftEnd.transform.position.x) * Random.value + bottomLeftEnd.transform.position.x,
					              seaLevel.transform.position.y, (upperRightEnd.transform.position.z - bottomLeftEnd.transform.position.z) * Random.value + bottomLeftEnd.transform.position.z);
				NavMeshHit hit;
				NavMesh.SamplePosition (temp,out hit,1000f,1);
				Instantiate (boatPrefab,hit.position,Quaternion.identity).name="boat"+i;
			}
			//Random.seed =2;
			for (int i = people.Length; i < noofpeople; i++) {
				NavMeshHit hit;
				do {
					Vector3 temp = new Vector3 ((upperRightEnd.transform.position.x - bottomLeftEnd.transform.position.x) * Random.value + bottomLeftEnd.transform.position.x,
						seaLevel.transform.position.y, (upperRightEnd.transform.position.z - bottomLeftEnd.transform.position.z) * Random.value + bottomLeftEnd.transform.position.z);
					NavMesh.SamplePosition (temp, out hit, 1000f, 1);
					if (!Physics.Raycast (hit.position, Vector3.forward, 2f)&&!Physics.Raycast (hit.position, Vector3.right, 2f)) {
						break;
					}
				} while(true);
				GameObject t1 = Instantiate (personPrefab,hit.position,Quaternion.identity);
				t1.GetComponent<moveObjectInWater2>().sea=seaLevel;
				t1.name = "person"+i;
			}
			Random.seed = Random.Range (0,1000);
			int columns = bottomLeftEnd.GetComponent<assignAreaToBots>().rows;
			Vector3 cellSize=new Vector3();
			cellSize = (upperRightEnd.transform.position - bottomLeftEnd.transform.position)/columns;
			Debug.Log (cellSize);
			int gridno = 0;
			for (int i = bots.Length; i < noofbots; i++) {
				Vector3 temp = new Vector3 (cellSize.x * Random.value ,
					0f, cellSize.z * Random.value) + bottomLeftEnd.transform.position;
				temp.y = seaLevel.transform.position.y;
				temp += new Vector3 (cellSize.x*Mathf.FloorToInt(gridno/columns),0f,cellSize.z*(gridno%columns));
				Debug.Log (Mathf.FloorToInt(gridno/columns)+","+gridno%columns);
				NavMeshHit hit;
				NavMesh.SamplePosition (temp,out hit,1000f,1);
				GameObject t1= Instantiate (botPrefab,hit.position,Quaternion.identity);
				t1.name = "bot" + i;
				if (t1.GetComponent<createMap> () != null) {
					t1.GetComponent<createMap> ().map = bots [0].GetComponent<createMap> ().map;
					t1.GetComponent<createMap> ().myIcon= bots [0].GetComponent<createMap> ().myIcon;
					t1.GetComponent<createMap> ().nonTraversableIcon = bots [0].GetComponent<createMap> ().nonTraversableIcon;
					t1.GetComponent<createMap> ().origin = bots [0].GetComponent<createMap> ().origin;
					t1.GetComponent<createMap> ().originOnScreen= bots [0].GetComponent<createMap> ().originOnScreen;

				}
				gridno++;
				gridno %= (columns * columns);
			}

		}
	}

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > interval) {
			Instantiate (obstacle, new Vector3 ((upperRightEnd.transform.position.x-bottomLeftEnd.transform.position.x)*Random.value+bottomLeftEnd.transform.position.x,
				seaLevel.transform.position.y,(upperRightEnd.transform.position.z-bottomLeftEnd.transform.position.z)*Random.value+bottomLeftEnd.transform.position.z),Quaternion.identity);
			time = 0f;
		}
	}
}
