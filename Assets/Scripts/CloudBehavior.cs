using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class CloudBehavior : MonoBehaviour {

	private Vector3 spawnPoint;
	private Vector3 endPoint;

	private float amountToMove;
	private float speed;
	private float myY, myZ;
	// Use this for initialization

	void Start () {
		amountToMove = 1f;
		//speed = 1f;
		speed = Random.Range(0.5f, 1.2f);


		myY = transform.localPosition.y;
		myZ = transform.localPosition.z;
		endPoint = new Vector3 (-900, myY, myZ);
		spawnPoint = new Vector3 (700, myY, myZ);
	}
	
	// Update is called once per frame
	void Update () {
		/* move clouds */
		amountToMove = Time.deltaTime * speed;
		transform.Translate (Vector3.left * amountToMove);

		if (transform.localPosition.x <= endPoint.x) {
			transform.localPosition = spawnPoint;
		}
		
	}
}
