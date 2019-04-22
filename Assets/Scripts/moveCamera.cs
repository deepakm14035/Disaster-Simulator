using UnityEngine;
using System.Collections;

public class moveCamera : MonoBehaviour {
	public GameObject player;
	private Vector3 offsetPosition;
	// Use this for initialization
	void Start () {
		offsetPosition = transform.position - player.transform.position;

	}

	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position + offsetPosition;

	}
}