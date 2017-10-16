using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAi : MonoBehaviour {

	[SerializeField]
	private float findNeighborRadius = 2f;

	//private GameObject gm;

	// Use this for initialization
	void Start() {
		//gm = this.gameObject;
	}

	// Update is called once per frame
	void Update() {
		ComputeAlignment();
		//gm.transform.Translate(Vector3.forward * Time.deltaTime);
	}

	public void ComputeAlignment() {

		//Vector3 v = new Vector3();
		int neighhborCount = 0;

		gameObject.GetComponent<Renderer>().material.color = Color.white;


		Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, findNeighborRadius);

		int i = 0;
		while(i < hitColliders.Length && hitColliders[i].transform != transform) {	
			if(hitColliders[i].tag == "Flock") {
				gameObject.GetComponent<Renderer>().material.color = Color.green;
				Debug.Log(hitColliders[i] + " Is your neighbor[ s" + hitColliders[i].tag + " ]");
			}
			i++;
		}

		neighhborCount = hitColliders.Length;

		Debug.Log(neighhborCount);

	}


	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		//Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
		Gizmos.DrawWireSphere(transform.position, findNeighborRadius);
	}

}
