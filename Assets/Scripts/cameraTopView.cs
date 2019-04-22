using UnityEngine;
using System.Collections;

public class cameraTopView : MonoBehaviour {
	public Camera topView;
	Vector3 prevPos;
	int noTouch;
	bool fullView=false;
	public Camera camera;
	int width,height;
	// Use this for initialization
	void Start () {
		prevPos = Input.mousePosition;
		noTouch = 1;
		width=Screen.width;
		height=Screen.height;
	}
	
	// Update is called once per frame
	void Update () {

		float edges=600-topView.orthographicSize;
		if (Input.touchCount == 2) {
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			camera.orthographicSize += deltaMagnitudeDiff * 50f;
			
			// Make sure the orthographic size never drops below zero.
			camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 50f,600f);


		}
		if (Input.touchCount == 1) {
			Touch touchZero = Input.GetTouch(0);
			Vector3 dirn = touchZero.deltaPosition;
			topView.transform.position+=(dirn*20f);
		}
		if (Input.GetButtonDown ("Fire1") && !fullView) {
			Vector3 mousePos=Input.mousePosition;
			if(mousePos.x>0.6f*width && mousePos.y>0.6*height){
				fullView=true;
				topView.rect=new Rect(0.0f,0.0f,1.0f,1.0f);
			}

		}
		if (Input.GetButtonDown ("Cancel") && fullView) {
			fullView=false;
			topView.rect=new Rect(0.7f,0.7f,0.3f,0.3f);
		}
		if (Input.GetButton ("Vertical") && fullView) {
			Vector3 movement = new Vector3 (0.0f, 0.0f, 20.0f);
			if(transform.position.z<790+edges && transform.position.z>-900-edges)
				topView.transform.position+=(movement*Input.GetAxis ("Vertical"));
			if(transform.position.z>790+edges)
				transform.position-=new Vector3(0,0,20);
			if(transform.position.z<-900-edges)
				transform.position+=new Vector3(0,0,20);

		}
		if (Input.GetButton ("Horizontal") && fullView) {
			Vector3 movement = new Vector3 (20.0f, 0.0f, 0.0f);
			if(transform.position.x<700+edges && transform.position.x>-700-edges)
				topView.transform.position+=(movement*Input.GetAxis ("Horizontal"));
			if(transform.position.x>700+edges)
				transform.position-=new Vector3(20,0,0);
			if(transform.position.x<-700-edges)
				transform.position+=new Vector3(20,0,0);
		}

		if (Input.GetButton ("updown") && fullView) {
			if(topView.orthographicSize<600 && topView.orthographicSize>30)
				topView.orthographicSize+=(10*Input.GetAxis ("updown"));
			if(topView.orthographicSize>=600)
				topView.orthographicSize=599;
			if(topView.orthographicSize<50)
				topView.orthographicSize=40;
		}
		//787,-900 - z
		//-700,700 - x
		/*if (Input.GetButton("Fire1")) {
			Debug.Log(Input.mousePosition);
			//445, 95
			if(topView.orthographicSize>200){
				topView.orthographicSize-=100;
			}

		}
		if (Input.GetButtonUp("Fire1")) {
			Debug.Log(Input.mousePosition);
			//445, 95

			if(noTouch==1){
				noTouch=0;
				topView.orthographicSize=500;
			}
			else
				noTouch=1;
		}
		topView.orthographicSize+=(Input.GetAxis ("Mouse ScrollWheel")*400);
		Vector3 movement = Input.mousePosition - prevPos;
		movement = new Vector3 (movement.x, 0.0f, movement.y);
		if(noTouch==0)
			topView.transform.position+=(movement*10);
		prevPos=Input.mousePosition;
		*/
	}
}