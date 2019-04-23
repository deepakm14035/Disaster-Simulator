using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class mainmenucontroller : MonoBehaviour {
	public Toggle client, server, self,auto,autoWithoutPython;
	public int selection=0;
	public int port;
	public string ipaddress;
	public InputField ip,portno,peopleInput,boatInput, botsInput;
	public int people, boats, bots;
	public int mode=0;
	public void buttonpressed(){
		Debug.Log ("button pressed");
		if (client.isOn)
			selection = 1;
		ipaddress = ip.text;
		port = int.Parse (portno.text);
		people = int.Parse (peopleInput.text);
		boats = int.Parse (boatInput.text);
		bots = int.Parse (botsInput.text);
		if (auto.isOn)
			mode = 0;
		else if (self.isOn)
			mode = 1;
		else if (autoWithoutPython.isOn)
			mode = 2;
		Debug.Log ("button pressed -"+mode);
		//if(!isAuto)
			DontDestroyOnLoad (transform.gameObject);
		SceneManager.LoadSceneAsync ("Final");

	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
