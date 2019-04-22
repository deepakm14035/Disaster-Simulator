using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using UnityEngine.UI;
public class movementFile : MonoBehaviour {
	int noOfSafehouses,noOfBoats,noOfPeople;
	public GameObject safehouse,boat,person, path;
	GameObject[] safes,boats,people;
	int flag = 0;
	Vector3[,] poss;
	int[] indexes,obstacleflag;
	RaycastHit hitPoint;
	Vector3 initial,laststop;
	StreamWriter writer;
	public Text textt;
	int flag2=0;
	Vector3[] positions;
	// Use this for initialization
	void Start () {
		positions=new Vector3[1000];
		textt.text = "";
		string line;
		StreamReader theReader = new StreamReader("out.txt", Encoding.Default);
		int flag = 0;
		using (theReader)
		{
			line = theReader.ReadLine();
			noOfSafehouses=System.Convert.ToInt32(line);
			safes=new GameObject[noOfSafehouses];
			noOfBoats=noOfSafehouses;
			boats=new GameObject[noOfBoats];
			obstacleflag=new int[noOfBoats];
			indexes=new int[noOfBoats];
			for(int i=0;i<noOfSafehouses;i++){
				line = theReader.ReadLine();
				string[] entries = line.Split(',');
				Vector3 pos=new Vector3(System.Convert.ToInt32(entries[0]),7,System.Convert.ToInt32(entries[1]));
				safes[i]=Instantiate(safehouse,pos,Quaternion.identity);
				pos.y=7f;
				boats[i]=Instantiate(boat,pos,Quaternion.identity);
				boats[i].tag="boat";
				boats[i].name="boat"+i;
				obstacleflag[i]=0;
				indexes[i]=1;
			}
			line = theReader.ReadLine();
			noOfPeople=System.Convert.ToInt32(line);
			people=new GameObject[noOfPeople];
			for(int i=0;i<noOfPeople;i++){
				line = theReader.ReadLine();
				string[] entries = line.Split(',');
				Vector3 pos=new Vector3(float.Parse(entries[0]),7f,float.Parse(entries[1]));
				people[i]=Instantiate(person,pos,Quaternion.identity);
				people[i].tag="savePerson";
			}
			line = theReader.ReadLine();
			int frames=System.Convert.ToInt32(line);
			poss=new Vector3[frames,noOfBoats];
			for(int i=0;i<frames;i++){
				for(int j=0;j<noOfBoats;j++){
					line = theReader.ReadLine();
					string[] entries = line.Split(',');
					Vector3 pos=new Vector3(float.Parse(entries[0]),7,float.Parse(entries[1]));
					//for(int k=0;k<noOfPeople;k++){
					//	if(Vector3.Distance(pos,people[k].transform.position)<1f){
							poss[i,j]=pos;
							Instantiate(path,pos,Quaternion.identity);
					//		break;
					//	}
					//}

				}
			}
			theReader.Close();
		}
		laststop = boats [0].transform.position;
		string path1 = "positions.txt";
		writer = new StreamWriter(path1, true);
		initial = boats [0].transform.position;
		writer.WriteLine(boats[0].transform.position);
		textt.text=boats[0].transform.position.x+","+boats[0].transform.position.z+"\n";
	}

// Update is called once per frame
	void Update () {
		if (flag == 0) {
			for (int i=0; i<boats.Length; i++) {
				//if(obstacleflag[i]==0){
					Quaternion neededRotation = Quaternion.LookRotation (poss [indexes [i]+1, i] - poss [indexes [i], i]);
					boats [i].transform.rotation = Quaternion.Slerp (boats [i].transform.rotation, neededRotation, Time.deltaTime * 3f);//.LookAt(poss[index+1,i]);
					boats [i].transform.position += Vector3.Normalize ((poss [indexes [i]+1, i] - poss [indexes [i], i])) * 1f;

				//}
				//else
				//	boats [i].transform.position += boats [i].transform.forward*0.1f;
				/*Vector3 changeDirection = Vector3.Normalize (poss [indexes [i]+1, i] - poss [indexes [i], i]);
				Vector3 changeDirection2 = Vector3.Normalize (poss [indexes [i]+1, i] - poss [indexes [i], i]);
				

				if (Physics.Raycast (boats [i].transform.position, boats [i].transform.forward, out hitPoint, 28.0f)) {
					float direction = 20.0f;
					float j = 0.0f;
					Debug.Log ("obstacels for"+boats[i].name);
					for (j=direction; j<=80; j+=20) {
						if (!Physics.Raycast (boats [i].transform.position + boats [i].transform.right, boats [i].transform.forward + (boats [i].transform.right * j / 100.0f), out hitPoint, 25.0f)) {
							changeDirection += (boats [i].transform.right * j * 30.0f);
							break;
						}
						if (!Physics.Raycast (boats [i].transform.position - boats [i].transform.right, boats [i].transform.forward + (-boats [i].transform.right * j / 100.0f), out hitPoint, 25.0f)) {
							changeDirection += (-boats [i].transform.right * j * 30.0f);
							break;
						}
						
					}
					Quaternion target = Quaternion.LookRotation (changeDirection);
					boats [i].transform.rotation = Quaternion.Slerp (boats [i].transform.rotation, target, Time.deltaTime*4f);

					obstacleflag[i]=1;
			
				}else{
					obstacleflag[i]=0;
					Quaternion neededRotation = Quaternion.LookRotation (poss [indexes [i]+1, i] - boats [i].transform.position);
					boats [i].transform.rotation = Quaternion.Slerp (boats [i].transform.rotation, neededRotation, Time.deltaTime * 4f);//.LookAt(poss[index+1,i]);

				}*/
				/*if (!Physics.Raycast (boats [i].transform.position, (poss [indexes [i]+1, i]-boats [i].transform.position), out hitPoint, 15.0f)){
					Quaternion neededRotation = Quaternion.LookRotation (poss [indexes [i]+1, i] - boats [i].transform.position);
					boats [i].transform.rotation = Quaternion.Slerp (boats [i].transform.rotation, neededRotation, Time.deltaTime * 4f);//.LookAt(poss[index+1,i]);

				}*/
				//boats [i].transform.position += boats [i].transform.forward*1f;

				if (Vector3.Distance (poss [indexes [i]+1, i], boats [i].transform.position) < Vector3.Distance (poss [indexes [i], i], boats [i].transform.position))
					indexes [i]++;
			}
			if(Mathf.Min(indexes)>1999)
				flag=1;
		}
		//if (Vector3.Distance (initial, boats [0].transform.position) >= 2.5f&&flag2==0) {
			//writer.WriteLine(boats[0].transform.position+"");
		//	textt.text+=boats[0].transform.position.x+","+boats[0].transform.position.z+"\n";
		//	initial=boats[0].transform.position;
			/*for(int i=0;i<people.Length;i++){
				if(Vector3.Distance (initial, people[i].transform.position)<1f){
					flag2=1;
					Debug.Log("done");
					string filename=initial"";
					StreamWriter sw=new StreamWriter();
					writer.Close();
				}
			}*/

		//}
	}
}
