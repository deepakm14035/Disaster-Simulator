using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;


public class Server : MonoBehaviour {
	private List<ServerClient> clients,disconnectList;
	public int port = 6321;
	private TcpListener server;
	public bool serverStarted=false;
	// Use this for initialization
	void Start () {
		clients = new List<ServerClient> ();
		disconnectList = new List<ServerClient> ();
		try{
			server=new TcpListener(IPAddress.Any,port);
			server.Start();
			startListening();
			serverStarted=true;
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
		clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
		startListening ();
		broadcast (clients[clients.Count-1].clientname+" has connected",clients);
	}

	void broadcast(string data,List<ServerClient> cl){
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
					}
				}
			}
		}
	}
	private void onincomingdata(ServerClient c, string data){
		Debug.Log (c.clientname+" : "+data);
	}
}
public class ServerClient{
	public TcpClient tcp;
	public string clientname;
	public ServerClient(TcpClient client){
		clientname = "aclient";
		tcp = client;

	}
}