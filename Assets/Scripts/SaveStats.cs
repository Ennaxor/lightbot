using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStats : MonoBehaviour {

	public int totalScore;
	public int totalMovements;

	// Use this for initialization
	void Start () {
		
	}

	void Awake () {
		DontDestroyOnLoad (this);
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
