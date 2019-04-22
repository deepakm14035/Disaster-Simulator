using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
public class client2 : MonoBehaviour {
	bool socketReady;
	TcpClient socket;
	NetworkStream stream;
	StreamReader reader;
	StreamWriter writer;
	public static string host;
	public static int port;
	public Text serverorclient;
	public string clientname;
	const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want

	public void sendData(string data){
		if (!socketReady)
			return;
		Debug.Log ("sending..");
		writer.WriteLine (data+","+clientname);
		writer.Flush ();
	}
	
	public void connectToServer(){
		if (socketReady)
			return;

		/*string h;
		int p;
		h = GameObject.Find ("host").GetComponent<InputField> ().text;
		if (h != null)
			host = h;
		int.TryParse (GameObject.Find("port").GetComponent<InputField>().text,out p);
		if(p!=0)
			port=p;
*/
		try{
			socket=new TcpClient(host,port);
			stream=socket.GetStream();
			writer=new StreamWriter(stream);
			reader=new StreamReader(stream);
			socketReady=true;
			Debug.Log ("connected to server");
		}
		catch(Exception e){
			Debug.Log("socket error:"+e.Message);
		}
	}
	
	// Use this for initialization
	void Start () {
		serverorclient.text="Client";
		int charAmount = 5+Mathf.FloorToInt( UnityEngine.Random.value*5f); //set those to the minimum and maximum length of your string
		for(int i=0; i<charAmount; i++)
		{
			clientname += glyphs[Mathf.FloorToInt(UnityEngine.Random.value*glyphs.Length)];
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (socketReady) {
			if (stream.DataAvailable) {
				string data = reader.ReadLine();
				if (data != null) {
					onincomingdata (data);
				}
			}
		} else
			connectToServer ();
	}
	private void onincomingdata(string data){
		Debug.Log ("client:"+data);
		string[] dat = data.Split (',');
		if (dat.Length < 6) {
			if(dat[0].Equals("settarget")){
				Debug.Log("setting target");
				if(!clientname.Equals(dat[2])){
					GameObject tempboat=GameObject.Find(dat[1]);
					tempboat.GetComponent<obstacle2>().canMove=false;
				}
			}
			else if(dat[0].Equals("addobstacle")){
				Vector3 pos = new Vector3 (float.Parse (dat [1]), float.Parse (dat [2]), float.Parse (dat [3]));
				addObstacle.putObstacle(pos);
			}
			GameObject go = GameObject.Find (dat [0]);
			Debug.Log("someone is moving "+dat[0]);
			Vector3 destin = new Vector3 (float.Parse (dat [1]), float.Parse (dat [2]), float.Parse (dat [3]));
			go.GetComponent<obstacle2> ().setDestination (destin);
		} else {
			Debug.Log("positioning");
			string[] positions=data.Split ('\t');
			foreach(string position in positions){
				string[] dat1 = position.Split (',');
				GameObject go = GameObject.Find (dat1 [0]);
				Vector3 destin = new Vector3 (float.Parse (dat1 [1]), float.Parse (dat1 [2]), float.Parse (dat1 [3]));
				go.transform.position=destin;
				go.transform.rotation=Quaternion.Euler(new Vector3(0f,float.Parse (dat1 [4]),0f));
				go.GetComponent<obstacle2>().destination=destin;
				//serverorclient.text+=dat[0]+"\n";
			}
		}
	}
	
}
