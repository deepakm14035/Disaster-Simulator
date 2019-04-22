using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
public class Client : MonoBehaviour {
	bool socketReady;
	TcpClient socket;
	NetworkStream stream;
	StreamReader reader;
	StreamWriter writer;
	public InputField message;
	public void sendData(){
		if (!socketReady)
			return;
		Debug.Log ("sending..");
		writer.WriteLine (message.text);
		writer.Flush ();
	}

	public void connectToServer(){
		if (socketReady)
			return;
		string host = "127.0.0.1";
		int port = 6321;
		string h;
		int p;
		h = GameObject.Find ("host").GetComponent<InputField> ().text;
		if (h != null)
			host = h;
		int.TryParse (GameObject.Find("port").GetComponent<InputField>().text,out p);
		if(p!=0)
			port=p;
		try{
			socket=new TcpClient(host,port);
			stream=socket.GetStream();
			writer=new StreamWriter(stream);
			reader=new StreamReader(stream);
			socketReady=true;
		}
		catch(Exception e){
			Debug.Log("socket error:"+e.Message);
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (socketReady) {
			if(stream.DataAvailable){
				string data=reader.ReadLine();
				if(data!=null){
					onincomingdata(data);
				}
			}
		}
	}
	private void onincomingdata(string data){
		Debug.Log (data);
	}

}
