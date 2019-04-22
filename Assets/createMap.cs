using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createMap : MonoBehaviour {
    public GameObject nonTraversableIcon;
    public GameObject map;
    public Image myIcon,originOnScreen;
    public GameObject origin;
    Vector3 myPositionOnScreen,eulerPose,tempPosition;
	RaycastHit hit;
	Quaternion rotation;
	float startangle;
	List<Vector3> positions;
	Vector3 offset = new Vector3 (100f,100f,0f);
    public void addObstacle(Vector3 position){
		tempPosition = positionOnCanvas (position);


		foreach(Vector3 pos in positions)
		{
			if (Vector3.Distance (tempPosition,pos)<0.6f) {
				//Debug.Log ("dist-"+Vector3.Distance (tempPosition,pos));
				return;
			}
		}

		positions.Add (tempPosition);

		//for (int i = 0; i < positions.Count; i++) {
			
		//}
		GameObject t=Instantiate(nonTraversableIcon,tempPosition,Quaternion.identity);
		t.GetComponent<RectTransform> ().position = positionOnCanvas (position);
        t.transform.parent=map.transform;
		t.transform.SetSiblingIndex (0);
    }
    
    public Vector3 positionOnCanvas(Vector3 pos){
        Vector3 temp1=(pos+origin.transform.position)/21f;
		//Debug.Log ("temp1-"+temp1);
		Vector3 temp=new Vector3(temp1.x,temp1.z,0f);
		//Debug.Log ("temp-"+temp);
		//Debug.Log ("final-"+temp+originOnScreen.GetComponent<RectTransform>().position);

		return temp+originOnScreen.GetComponent<RectTransform>().position+offset;
    }
    
	// Use this for initialization
	void Start () {
		//Debug.Log ("origin on screen-"+originOnScreen.GetComponent<RectTransform>().position);
		//Debug.Log ("origin -"+origin.transform.position);

		//Debug.Log ("pos-"+transform.position);
		//Debug.Log ("pos on screen-"+positionOnCanvas(transform.position));

		positions= new List<Vector3>();

		myIcon.GetComponent<RectTransform>().position=positionOnCanvas(transform.position);
		eulerPose = new Vector3 (0f,0f,0f);
		startangle = -180f;
	}
	
	// Update is called once per frame
	void Update () {
		myIcon.GetComponent<RectTransform>().position=positionOnCanvas(transform.position);
		myIcon.GetComponent<RectTransform> ().rotation = Quaternion.Euler (new Vector3(0f,0f,transform.rotation.eulerAngles.y));
		for (float i=startangle; i<startangle+60f; i+=5f) {
			//Debug.DrawLine(transform.position, people[i].transform.position+transform.up);
			eulerPose.y=i;
			rotation = Quaternion.Euler (eulerPose);
			if (Physics.Raycast (transform.position+transform.up, rotation*Vector3.forward,out hit,50f)) {
				//Debug.Log("name-"+hit.transform.root.gameObject.name);
				addObstacle (hit.point);

			}
		}
		startangle += 60f;
		if (startangle > 120f) {
			startangle = -180f;
		}
	}
}
