using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour {

	public int levelSelected;
	// Use this for initialization
	void Start () {
		levelSelected = 1;
	}

	void Awake () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
