using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class SaveRanking : MonoBehaviour {

	private string URLAddRanking = "intracellular-burgl.000webhostapp.com/AddNewHighscore.php?";
	private string secret = "dsadsafrqwr51asa";

	private GameObject statsGo;
	private GameObject nameGo;
	private GameObject cvs;


	// Use this for initialization
	void Start () {
		statsGo = GameObject.Find ("Stats");
		nameGo = GameObject.Find ("Level");
		cvs = GameObject.Find ("Canvas");

		//StartCoroutine (AddPlayer ("hola", 1, 2));
	}
	
	// Update is called once per frame
	void Update () {
		if (cvs.GetComponent<NextLevel>().passedALevel) {
			cvs.GetComponent<NextLevel> ().passedALevel = false;
			StartCoroutine (AddPlayer (nameGo.GetComponent<SetName> ().nameOfPlayer, statsGo.GetComponent<SaveStats> ().totalScore, statsGo.GetComponent<SaveStats> ().totalMovements));
		}
	}

	public string Md5Sum(string strToEncrypt) {
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
		byte[] bytes = ue.GetBytes (strToEncrypt);
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
		byte[] hashBytes = md5.ComputeHash (bytes);
		string hashString = "";
		for(int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString (hashBytes [i], 16).PadLeft (2, '0');
		}

		return hashString.PadLeft (32, '0');
	}

	IEnumerator AddPlayer(string na, int sc, int mov) {
		string hash = Md5Sum (na + sc + mov + secret);
		string postURL = URLAddRanking + "name=" + WWW.EscapeURL (na) + "&score=" + sc + "&movements=" + mov + "&hash=" + hash;

		WWW DataPost = new WWW ("http://" + postURL);
		yield return DataPost;

		if (DataPost.error != null && DataPost.error != "")
			print ("Could not post player save: " + DataPost.error);
		else
			Debug.Log ((System.Text.Encoding.UTF8.GetString (DataPost.bytes)));
	
	}
}
