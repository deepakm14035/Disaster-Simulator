using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setNetworkType : MonoBehaviour {
	GameObject networktype;
	// Use this for initialization
	void Awake () {
		networktype = GameObject.Find ("mainmenu");
		if (networktype != null && !networktype.GetComponent<mainmenucontroller> ().isAuto) {
			client2.port = networktype.GetComponent<mainmenucontroller> ().port;
			string ip = networktype.GetComponent<mainmenucontroller> ().ipaddress;
			client2.host = ip;
			server2.port = client2.port;
			string[] ipa = ip.Split ('.');
			byte[] arr = new byte[4];
			for (int i=0; i<4; i++) {
				arr [i] = byte.Parse (ipa [i]);
			}
			server2.addr = arr;
			if (networktype != null && networktype.GetComponent<mainmenucontroller> ().selection == 1) {
				gameObject.GetComponent<server2> ().enabled = false;
			}
		} else {
			client2.port=3456;
			client2.host="192.168.55.245";
			server2.port=client2.port;
			server2.addr=new byte[]{192,168,55,245};
		}
	}

}
