using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScore : MonoBehaviour {

	public UnityEngine.UI.Text nameOwnPlayer;
	public UnityEngine.UI.Text score;
	public UnityEngine.UI.Text movements; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setScore(string namePlayer, string scorePlayer, string movementsPlayer) { 
		nameOwnPlayer.GetComponent<UnityEngine.UI.Text> ().text = namePlayer;
		score.GetComponent<UnityEngine.UI.Text> ().text = scorePlayer;
		movements.GetComponent<UnityEngine.UI.Text> ().text = movementsPlayer;
	}
}
