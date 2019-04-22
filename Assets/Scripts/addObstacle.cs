using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addObstacle : MonoBehaviour {
	bool buttonPressed=false;
	public Camera topView;
	float startX;
	GameObject lastSpawn;
	float startSize,sizingFactor=0.02f;
	public GameObject sea;
	public static GameObject cubePrefab;
	public GameObject client;
	// Use this for initialization
	void Start () {
		cubePrefab = (GameObject)Resources.Load("obstacle");
	}

	public void buttonHandler(){
		buttonPressed = true;
	}
	public static void putObstacle(Vector3 position){
		Instantiate (cubePrefab, position,Quaternion.identity);
	}
	// Update is called once per frame
	void Update () {
		if (buttonPressed) {
			if (Input.GetMouseButtonDown (0)) {
				float positionZ = 10.0f;
				Ray ray1 = topView.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit1;
				Vector3 p = new Vector3 (ray1.origin.x, sea.transform.position.y, ray1.origin.z);
				lastSpawn = Instantiate (cubePrefab, p,transform.rotation) as GameObject;
				startSize = lastSpawn.transform.localScale.z;
			}
			
			if (Input.GetMouseButton (0)) {
				Vector3 size = lastSpawn.transform.localScale;
				size.x = startSize + (Input.mousePosition.x - startX) * sizingFactor;
				lastSpawn.transform.localScale = size;
				buttonPressed=false;
				client.GetComponent<client2>().sendData("addobstacle,"+lastSpawn.transform.position.x+","+lastSpawn.transform.position.y+","+lastSpawn.transform.position.z);
			}
		}
	}
}
