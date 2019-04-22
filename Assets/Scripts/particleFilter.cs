using UnityEngine;
using System.Collections;

public class particleFilter : MonoBehaviour {
	public GameObject target;
	float[,,] space;
	int flag=1;
	int numberOfIterations=0;
	// Use this for initialization
	void Start () {

		space = new float[20,20,20];
		flag = 0;
		for (int i=0; i<20; i++) 
			for (int j=0; j<20; j++)
				for (int k=0; k<20; k++)
					space[i,j,k]=0.0f;
		
	}
	
	// Update is called once per frame
	void Update () {

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

			if (numberOfIterations > 100) {
				flag=1;
				Vector3 movetowards=new Vector3(maxx-transform.position.x, maxy-transform.position.y, maxz-transform.position.z);
				transform.position += movetowards;
			}
			float moveRandomx = Random.Range (-2.0f, 2.0f);
			float moveRandomy = Random.Range (-2.0f, 2.0f);
			float moveRandomz = Random.Range (-2.0f, 2.0f);

			int plus_or_minus1 = Mathf.RoundToInt (Random.Range (0.0f, 2.0f));
			Vector3 move = new Vector3 (moveRandomx, moveRandomy, moveRandomz);
			if (plus_or_minus1 % 2 == 0)
				transform.position += new Vector3 (moveRandomx, moveRandomy, moveRandomz);
			else
				transform.position -= new Vector3 (moveRandomx, moveRandomy, moveRandomz);
			float limitx=Mathf.Clamp(transform.position.x, 0.0f, 20.0f);
			float limity=Mathf.Clamp(transform.position.y, 0.0f, 20.0f);
			float limitz=Mathf.Clamp(transform.position.z, 0.0f, 20.0f);
			transform.position=new Vector3(limitx, limity, limitz);
			numberOfIterations++;
		}

	}
}
