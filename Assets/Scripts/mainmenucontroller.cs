using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class mainmenucontroller : MonoBehaviour {
	public Toggle client, server, self,auto;
	public int selection=0;
	public int port;
	public string ipaddress;
	public InputField ip,portno,peopleInput,boatInput, botsInput;
	public int people, boats, bots;
	public bool isAuto=false;
	public void buttonpressed(){
		Debug.Log ("button pressed");
		if (client.isOn)
			selection = 1;
		ipaddress = ip.text;
		port = int.Parse (portno.text);
		people = int.Parse (peopleInput.text);
		boats = int.Parse (boatInput.text);
		bots = int.Parse (botsInput.text);
		isAuto=auto.isOn;
		if(!isAuto)
			DontDestroyOnLoad (transform.gameObject);
		SceneManager.LoadSceneAsync ("scene_t1");

	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
