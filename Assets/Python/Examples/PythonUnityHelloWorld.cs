using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
namespace Exodrifter.UnityPython.Examples
{
	public class PythonUnityHelloWorld : MonoBehaviour
	{
		string code;
		Microsoft.Scripting.Hosting.ScriptEngine engine;
		Microsoft.Scripting.Hosting.ScriptScope scope;
		public float time=0f;
		public Text timeDisplay;
		void Start()
		{
			engine = global::UnityPython.CreateEngine();
			scope = engine.CreateScope();
			//scope.SetVariable ("setDestination",setDestinationForBoat);
			//scope.SetVariable ();

			//string code = "import UnityEngine\n";
			//code += "UnityEngine.Debug.Log('Hello world!')";
			string fileName="assets/start.py";
			executeAlgo (fileName);

		}

		void setDestinationForBoat(GameObject boat, int[] arr){
			boat.GetComponent<NavMeshAgent> ().SetDestination (new Vector3(arr[0],arr[1],arr[2]));
		}



		void executeAlgo(string fileName){
			engine.CreateScriptSourceFromFile(fileName).Execute(scope);

			IList<object> d=(IList<object>)scope.GetVariable ("setBoatPositions");
			bool[] destinationAvailable=new bool[d.Count];
			int c = 0;
			foreach (object d1 in d) {
				//Debug.Log((Vector3)d1+"");
				destinationAvailable [c++] = (bool)d1;
			}

			d=(IList<object>)scope.GetVariable ("destinations");
			Vector3[] destinations=new Vector3[d.Count];
			c = 0;
			foreach (object d1 in d) {
				//Debug.Log (destinations[i,0]+","+destinations[i,1]+","+destinations[i,2]);
				//IList<object> temp=(IList<object>)d1;
				//foreach(object t in temp){
				//Debug.Log((Vector3)d1+"");
				destinations [c++] = (Vector3)d1;
				//}
			}

			d=(IList<object>)scope.GetVariable ("destination_people");
			GameObject[] destinations_person=new GameObject[d.Count];
			c = 0;
			foreach (object d1 in d) {
				//Debug.Log (destinations[i,0]+","+destinations[i,1]+","+destinations[i,2]);
				//IList<object> temp=(IList<object>)d1;
				//foreach(object t in temp){
				//Debug.Log((Vector3)d1+"");
				destinations_person[c++] = (GameObject)d1;
				//}
			}

			d=(IList<object>)scope.GetVariable ("boats");
			GameObject[] boats=new GameObject[d.Count];
			int count=0;
			foreach (object d1 in d) {
				//Debug.Log(((GameObject)d1).name+"");
				boats[count++]=(GameObject)d1;
				if (Vector3.Distance (boats [count - 1].transform.position, destinations [count - 1]) > 5f
					&& boats [count - 1].GetComponent<AutonomousBoat> ().count<boats [count - 1].GetComponent<AutonomousBoat> ().maxCount) {
					boats [count - 1].GetComponent<NavMeshAgent> ().SetDestination (destinations [count - 1]);
					boats [count - 1].tag = "inProcess";
					destinations_person [count - 1].tag="savePerson";
					//Debug.Log (boats[count-1].name+", "+destinations_person[count-1].name);
					boats [count - 1].GetComponent<AutonomousBoat> ().personToReach = destinations_person [count - 1];
					boats [count - 1].GetComponent<AutonomousBoat> ().following = true;
				}
			}
		}


		void Update(){
			executeAlgo ("assets/update.py");
			time += Time.deltaTime;
			timeDisplay.text = "time spent-" + time;

		}
		void LateUpdate(){
			if (Mathf.FloorToInt (time) % 10 == 0) {
				if (GameObject.FindGameObjectsWithTag ("savePerson").Length == 0
					&& GameObject.FindGameObjectsWithTag ("undetected").Length == 0
					&& GameObject.FindGameObjectsWithTag ("Finish").Length == 0
					&& GameObject.FindGameObjectsWithTag ("detected").Length == 0) {
					Time.timeScale = 0f;
					Debug.Log (statistics.result);
					//TextWriter tw = new TextWriter ();
				}
			}
		}
	}
}