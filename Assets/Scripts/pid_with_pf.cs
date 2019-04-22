using UnityEngine;
using System.Collections;

public class pid_with_pf : MonoBehaviour {

	public GameObject t1;
	public GameObject t2;
	public GameObject t3;
	public GameObject t4;
	public Rigidbody rb;
	public Vector3 targetPos=Vector3.zero;
	public float maxForce=100f;
	public float pGain=20f;
	public float iGain=0.5f;
	public float dGain=0.5f;
	private Vector3 integrator = Vector3.zero;
	private Vector3 lastError=Vector3.zero;
	Vector3 curPos=Vector3.zero;
	Vector3 force=Vector3.zero;

	public GameObject target;
	float[,,] space;
	int flag=1;
	int numberOfIterations=0;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		space = new float[20,20,20];
		flag = 0;
		for (int i=0; i<20; i++) 
			for (int j=0; j<20; j++)
				for (int k=0; k<20; k++)
					space[i,j,k]=0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		curPos = transform.position;
		Vector3 error = targetPos - curPos;
		integrator += error * Time.deltaTime;
		Vector3 diff = (error - lastError) / Time.deltaTime;
		lastError = error;
		force = (error * pGain) + (integrator * iGain) + (diff * dGain);
		force = Vector3.ClampMagnitude (force, maxForce);
		Vector3 t = (Physics.gravity * rb.mass / 4);
		rb.AddForceAtPosition (-t+force, t1.transform.position);
		rb.AddForceAtPosition (-t+force, t2.transform.position);
		rb.AddForceAtPosition (-t+force, t3.transform.position);
		rb.AddForceAtPosition (-t+force, t4.transform.position);
		if(Vector3.Distance(targetPos,curPos)<=0.5f){
			if(flag==1)
				flag=0;
			if(flag==2)
				flag=3;
		}
		if (flag == 0) {
			float distance_to_target = Vector3.Distance (transform.position, target.transform.position);
			distance_to_target += Random.Range (-0.2f, 0.2f);
			
			int maxx=0, maxy=0, maxz=0;
			float max_val = 0.0f;
			float max_val_second = 0.0f;
			
			for (int i=0; i<20; i++) {
				for (int j=0; j<20; j++) {
					for (int k=0; k<20; k++) {
						float dist = Mathf.Abs (Vector3.Distance (transform.position, new Vector3 (i, j, k)));
						float weight=1/(Mathf.Abs(dist-distance_to_target));
						if (weight > max_val) {
							max_val = weight;
						}
						
					}
				}
			}
			float maximum=max_val;
			max_val = 0.0f;
			for (int i=0; i<20; i++) {
				for (int j=0; j<20; j++) {
					for (int k=0; k<20; k++) {
						float dist = Mathf.Abs (Vector3.Distance (transform.position, new Vector3 (i, j, k)));
						float weight=1/(Mathf.Abs(dist-distance_to_target));
						space [i, j, k] += weight/maximum;
						if(Mathf.Abs(dist-distance_to_target)>4){
							space [i, j, k] -= 2/(Mathf.Abs(dist-distance_to_target)*50);
						}
						if (space [i, j, k] > max_val) {
							maxx = i;
							maxy = j;
							maxz = k;
							max_val_second = max_val;
							max_val = space [i, j, k];
						}
						
					}
				}
			}
			Debug.Log (Mathf.Abs (max_val - max_val_second));
			Debug.Log (maxx+" "+maxy+" "+maxz);
			
			if (numberOfIterations > 10) {
				flag=2;
				Debug.Log("asdasdasd");
				Vector3 movetowards=new Vector3(maxx-transform.position.x, maxy-transform.position.y, maxz-transform.position.z);
				targetPos = transform.position+movetowards;
			}
			else{
				float moveRandomx = Random.Range (-2.0f, 2.0f);
				float moveRandomy = Random.Range (-2.0f, 2.0f);
				float moveRandomz = Random.Range (-2.0f, 2.0f);
				targetPos = transform.position+new Vector3 (moveRandomx, moveRandomy, moveRandomz);
				float limitx=Mathf.Clamp(targetPos.x, 0.0f, 20.0f);
				float limity=Mathf.Clamp(targetPos.y, 0.0f, 20.0f);
				float limitz=Mathf.Clamp(targetPos.z, 0.0f, 20.0f);
				targetPos=new Vector3(limitx, limity, limitz);
				numberOfIterations++;
				flag=1;
			}
		}




	}
}
