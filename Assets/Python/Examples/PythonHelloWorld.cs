using UnityEngine;
using IronPython.Hosting;
using System.Collections.Generic;

namespace Exodrifter.UnityPython.Examples
{
	public class PythonHelloWorld : MonoBehaviour
	{
		string[] boatNames;
		GameObject[] boats;

		string[] getBoatNames(){
			boats = GameObject.FindGameObjectsWithTag ("boat");
			boatNames=new string[boats.Length];
			for (int i = 0; i < boats.Length; i++) {
				boatNames [i] = boats [i].name;
			}
			return boatNames;
		}

		string[] getBoatPositions(){
			boats = GameObject.FindGameObjectsWithTag ("boat");
			int[,] pos=new int[boats.Length,3];

			for (int i = 0; i < boats.Length; i++) {
				boatNames [i] = boats [i].name;
			}
			return boatNames;
		}


		void Start()
		{
			


			string dir=@"D:\python27\Lib";
			string dir1=@"D:\python27\DLLs";
			string dir2=@"D:\python27";
			string dir3=@"D:\python27\Lib\site-packages";

			var engine = Python.CreateEngine();
			var scope = engine.CreateScope();

			ICollection<string> paths = engine.GetSearchPaths();
			paths.Add (dir);
			paths.Add (dir1);
			paths.Add (dir2);
			paths.Add (dir3);

			engine.SetSearchPaths (paths);
			//engine.LoadAssembly(Assembly.GetAssembly(typeof(GameObject)));
			//string code = "def function_name(name):\n\t\"\"\"docstring\"\"\"\n\tprint name\n\treturn name+' is my name'\n\t\nmname=function_name('deepak')";
			//var source = engine.CreateScriptSourceFromString(code);
			//source.Execute(scope);
			scope.SetVariable("boatNames",boatNames);
			string fileName="assets/test.py";
			engine.CreateScriptSourceFromFile(fileName).Execute(scope);
			//engine.ExecuteFile("assets/test.py");

			Debug.Log (scope.GetVariable<string>("mname"));
			//Debug.Log();
		}
	}
}