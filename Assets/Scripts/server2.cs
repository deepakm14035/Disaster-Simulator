using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
using UnityEngine.UI;

public class server2 : MonoBehaviour {
	private List<ServerClient> clients,disconnectList;
	public static int port;
	public static byte[] addr;
	private TcpListener server;
	public bool serverStarted=false;
	public Text serverorclient,timetaken;
	GameObject[] boats,people,savedppl;
	ServerClient sendpositionsto;
	int flag=0,sendflag=0,savedpeople=0;
	float totaltime=0f;
	// Use this for initialization
	void Start () {
		boats = GameObject.FindGameObjectsWithTag ("boat");
		people = GameObject.FindGameObjectsWithTag ("savePerson");
		savedpeople = people.Length;
		clients = new List<ServerClient> ();
		disconnectList = new List<ServerClient> ();
		try{
			IPAddress ipa=new IPAddress(addr);
			server=new TcpListener(ipa,port);
			server.Start();
			startListening();
			serverStarted=true;
			Debug.Log ("server created");
		}
		catch(SocketException e){
			Debug.Log(e.Message);
		}
	}
	
	void startListening(){
		server.BeginAcceptTcpClient (acceptTcpClient,server);
	}
	
	void acceptTcpClient (IAsyncResult ar){
		TcpListener listener = (TcpListener)ar.AsyncState;
		ServerClient sc = new ServerClient (listener.EndAcceptTcpClient (ar));
		Debug.Log ("server-"+sc.clientname);
		clients.Add(sc);
		startListening ();
		//broadcast (clients[clients.Count-1].clientname+" has connected",clients);
		sendflag = 1;
		sendpositionsto = sc;
	}
	
	void broadcast(string data,List<ServerClient> cl){
		Debug.Log ("broadcasting..");
		foreach (ServerClient c in cl) {
			try{
				StreamWriter writer=new StreamWriter(c.tcp.GetStream());
				writer.WriteLine(data);
				writer.Flush();
			}
			catch(Exception e){
				Debug.Log("write error"+e.Message);
			}
		}
	}
	
	private bool isConnected(TcpClient c){
		try{
			if(c!=null&&c.Client!=null&&c.Client.Connected){
				if(c.Client.Poll (0,SelectMode.SelectRead))
					return !(c.Client.Receive(new byte[1],SocketFlags.Peek)==0);
				return true;
			}
			else
				return false;
		}
		catch(SocketException e){
			return false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		savedppl = GameObject.FindGameObjectsWithTag ("finished");
		if (savedppl.Length < savedpeople) {
			totaltime += Time.deltaTime;
			timetaken.text = "time : " + totaltime;
		}
		if (flag == 0) {
			serverorclient.text = "Server";
			flag=1;
		}

		if (sendflag == 1) {
			Debug.Log("sending");
			StreamWriter writer=new StreamWriter(sendpositionsto.tcp.GetStream());
			string setpositions = "";
			foreach (GameObject boat in boats) {
				setpositions+=boat.name+","+boat.transform.position.x+","+boat.transform.position.y+","+boat.transform.position.z+","+boat.transform.rotation.eulerAngles.y+"\t";
			}
			foreach (GameObject person in people) {
				setpositions+=person.name+","+person.transform.position.x+","+person.transform.position.y+","+person.transform.position.z+","+person.transform.rotation.eulerAngles.y+"\t";
			}
			writer.WriteLine (setpositions);
			writer.Flush ();
			sendflag=0;
		}

		if (!serverStarted)
			return;
		foreach (ServerClient sc in clients) {
			if(!isConnected(sc.tcp)){
				sc.tcp.Close();
				disconnectList.Add(sc);
				continue;
			}
			else{
				NetworkStream ns=sc.tcp.GetStream();
				if(ns.DataAvailable){
					StreamReader reader= new StreamReader(ns,true);
					string data=reader.ReadLine();
					if(data!=null){
						onincomingdata(sc,data);
						broadcast(data,clients);
					}
				}
			}
		}
	}
	private void onincomingdata(ServerClient c, string data){
		Debug.Log (c.clientname+" : "+data);
	}
}
