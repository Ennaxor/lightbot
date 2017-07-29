using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Runtime.Remoting.Messaging;
using System.Net.Sockets;



public class ShowRanking : MonoBehaviour {

	private string URLRanking = "intracellular-burgl.000webhostapp.com/ShowRanking.php?";

	private List<Player> rankingPlayers = new List<Player> ();
	private string[] currentArray = null;
	public Transform loadData;
	public UnityEngine.UI.Text txtLoading;
	public GameObject panelPre;

	// Use this for initialization
	void Start () {		
		StartCoroutine (getPlayers ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator getPlayers() {
		txtLoading.text = "Loading...";
		WWW DataServer = new WWW ("http://" + URLRanking);
	
		yield return DataServer;
		if (DataServer.error != null && DataServer.error != "") {
			print ("Error loading ranking");
			txtLoading.text = "Error loading ranking. Try later.";
			Debug.Log (DataServer.error);
		} else { 
			txtLoading.text = "";
			getLoadedData(DataServer);
			drawLoadedData();
		}
	}

	void getLoadedData(WWW DataServer) { 
		currentArray = System.Text.Encoding.UTF8.GetString (DataServer.bytes).Split (";"[0]); 

		for (int i = 0; i < currentArray.Length-1; i = i + 3) {
			rankingPlayers.Add(new Player(currentArray[i], currentArray[i+1], currentArray[i+2]));
		
		}
			
	}

	void drawLoadedData() { 
		for (int i = 0; i < rankingPlayers.Count; i++) {
			GameObject obj = Instantiate (panelPre);
			Player p = rankingPlayers [i];
			//print (p.namepl);

			obj.GetComponent<SetScore> ().setScore (p.namepl, p.scorepl, p.movementspl);
			obj.transform.SetParent (loadData);
			obj.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		}
	
	}
}

public class Player {
	public string namepl;
	public string scorepl;
	public string movementspl;

	public Player(string nameP, string scoreP, string movementsP) { 	
		namepl = nameP;
		scorepl = scoreP;
		movementspl = movementsP;
	}
}
