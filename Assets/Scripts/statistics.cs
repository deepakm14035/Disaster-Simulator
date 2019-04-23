using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class statistics : MonoBehaviour {
	static GameObject[] people;
	public static int undetected_count = 0;
	public static int detected_count = 0;
	public static int pickedup_count = 0;
	public static int saved_count = 0;
	public static Text stats, timeText;
	public static float timeTaken=0f;
	// Use this for initialization
	void Start () {
		people = GameObject.FindGameObjectsWithTag ("undetected");
		undetected_count = people.Length;
		people = GameObject.FindGameObjectsWithTag ("detected");
		detected_count = people.Length;
		people = GameObject.FindGameObjectsWithTag ("Finish");
		pickedup_count = people.Length;
		people = GameObject.FindGameObjectsWithTag ("finished");
		saved_count = people.Length;
		stats = GameObject.FindGameObjectWithTag ("Statistics").GetComponent<Text>();
		timeText = GameObject.Find ("timetaken").GetComponent<Text>();
		updateDisplay ();
	}

	float time=0f;

	public static void updateDisplay(){
		stats.text = "undetected-" + undetected_count + "\n"
		+ "detected-" + detected_count + "\n" +
		"picked up-" + pickedup_count + "\n" +
		"saved-" + saved_count;
		Debug.Log (stats.text);
	}
	public static string result="";
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > 8f) {
			result+=undetected_count+","+detected_count+","+pickedup_count+","+saved_count+"\n";
			time = 0f;
			if (GameObject.FindGameObjectsWithTag ("savePerson").Length == 0
				&& GameObject.FindGameObjectsWithTag ("undetected").Length == 0
				&& GameObject.FindGameObjectsWithTag ("Finish").Length == 0
				&& GameObject.FindGameObjectsWithTag ("detected").Length == 0) {
				Time.timeScale = 0f;
				Debug.Log (statistics.result);
				//TextWriter tw = new TextWriter ();
			}
		}
		timeTaken += Time.deltaTime;
		timeText.text = "time spent-" + timeTaken;

	}
}
