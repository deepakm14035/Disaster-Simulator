using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assignAreaToBots : MonoBehaviour {
	public Transform start,end;
	public int rows;

	int columns;
	Vector3 cellSize;
	// Use this for initialization
	void Start () {
		columns = rows;
		cellSize=new Vector3();
		cellSize = (end.position - start.position)/rows;
		GameObject[] boats = GameObject.FindGameObjectsWithTag ("wanderBots");
		for (int i = 0; i < boats.Length; i++) {
			if (boats [i].transform.position.x > start.position.x && boats [i].transform.position.z > start.position.z &&
			   	boats [i].transform.position.x < end.position.x && boats [i].transform.position.z < end.position.z) {
				boats [i].GetComponent<myWander> ().startRect = new Vector3(
					Mathf.Floor ((boats [i].transform.position.x - start.position.x) / cellSize.x) * cellSize.x +start.position.x,
					0f,
					Mathf.Floor ((boats [i].transform.position.z - start.position.z) / cellSize.z) * cellSize.z +start.position.z);
				boats [i].GetComponent<myWander> ().endRect = new Vector3(
					Mathf.Ceil ((boats [i].transform.position.x - start.position.x) / cellSize.x) * cellSize.x +start.position.x,
					0f,
					Mathf.Ceil ((boats [i].transform.position.z - start.position.z) / cellSize.z) * cellSize.z +start.position.z);
				//Debug.Log ("start-"+boats [i].GetComponent<myWander> ().startRect +", end-"+boats [i].GetComponent<myWander> ().endRect );
				
			}
		}
		Debug.Log ("cell size- "+cellSize);
	}
	
	// Update is called once per frame
	void Update () {
		for (int x = 0; x <= columns; x++) {
			Debug.DrawLine (new Vector3(start.position.x+x*cellSize.x,100f,start.position.z),new Vector3(start.position.x+x*cellSize.x,100f,end.position.z));
			Debug.DrawLine (new Vector3(start.position.x,100f,start.position.z+x*cellSize.z),new Vector3(end.position.x,100f,start.position.z+x*cellSize.z));

		}
	}
}
