using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {
	public GameObject sealevel;
	public Slider slider;
	public Text sliderText;
	public Text displayHeight;
	public InputField infi;
	public Text looking;
	public float dir;
	public GameObject boat;
	public static Text[] noOfPassengers;
	public static int[] passengers;
	public Vector3 sliderposition;
	public Text template;
	public Button manual;
	public Button auto;
	// Use this for initialization
	void Start () {
		Debug.Log (Screen.width);
		Debug.Log (Screen.height);
		sliderposition = new Vector3 (60,(Screen.height)-10,0);
		GameObject[] boats = GameObject.FindGameObjectsWithTag ("boat");
		noOfPassengers = new Text[boats.Length];
		passengers = new int[boats.Length];
		boat = null;
		for (int i=0; i<boats.Length; i++) {
			Text text = (Text)Instantiate (template,new Vector3(-10,-50-(i*20),0)+sliderposition, Quaternion.identity);
			text.color=Color.white;
			text.fontStyle=FontStyle.Bold;

			noOfPassengers [i] = text;
			noOfPassengers [i].text="boat "+i+": "+0;
			noOfPassengers[i].transform.SetParent(transform);
			passengers[i]=0;
		}

		slider.transform.position += (new Vector3(70,0,0) + sliderposition-slider.transform.position);
		sliderText.transform.position += (new Vector3(-10,0,0)+sliderposition-sliderText.transform.position);
		displayHeight.transform.position += (new Vector3(160,0,0)+sliderposition-displayHeight.transform.position);
		infi.transform.position += (new Vector3(60,-30,0)+sliderposition-infi.transform.position);
		looking.transform.position += (new Vector3(-10,-40,0)+sliderposition-looking.transform.position);
		manual.transform.position += (new Vector3(Screen.width/2,-10,0)+sliderposition-manual.transform.position);
		auto.transform.position += (new Vector3(Screen.width/2,-50,0)+sliderposition-auto.transform.position);

	}




	public void manualOrAuto(int a){
		Debug.Log (a);
		obstacle2.manAuto = a;
	}

	public void followBoat(string boat){
		int boatNo=int.Parse (boat);
		GameObject[] go = GameObject.FindGameObjectsWithTag ("boat");
		if (boatNo >= go.Length)
			return;
		move (go[boatNo]);
		dir = 5.0f;
	}

	void move(GameObject go){
		Camera.main.transform.position+=(go.transform.position+new Vector3(-go.transform.forward.x*35,go.transform.position.y+15,-go.transform.forward.x)-Camera.main.transform.position);
		Camera.main.transform.LookAt (go.transform.position);
		boat = go;
	}

	public void sliderChanged(float value){
		risingWater.targetHeight += (5*(value-0.5f));
		slider.value = 0.5f;
		displayHeight.text = "height- "+risingWater.targetHeight;
	}


	
	// Update is called once per frame
	void Update () {

		if (boat != null) {
			Camera.main.transform.position=(boat.transform.forward*-15f)+boat.transform.up*7f+boat.transform.position;//new Vector3(-boat.transform.forward.x*35,boat.transform.position.y+15,-boat.transform.forward.x)-Camera.main.transform.position;
			Camera.main.transform.LookAt (boat.transform.position);
		}

		/*for (int i=0; i<4; i++) {
			noOfPassengers [i].text="boat "+i+": "+passengers[i];
		}*/

	}
}
