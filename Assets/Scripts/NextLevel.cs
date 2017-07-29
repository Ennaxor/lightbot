using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

	private GameObject playerGo;
	private GameObject floor;
	private GameObject pan;

	private GameObject statsGo;

	private UnityEngine.UI.Button button_lvl;

	public bool passedALevel;

	// Use this for initialization
	void Start () {		
		playerGo = GameObject.Find("cat(Clone)");
		floor = GameObject.Find ("Floor");
		pan = GameObject.Find ("Panel");
		button_lvl = GameObject.Find ("Next-btn").GetComponent<UnityEngine.UI.Button> ();
		button_lvl.gameObject.SetActive (false);

		passedALevel = false;

		statsGo = GameObject.Find ("Stats");
	}
	
	// Update is called once per frame
	void Update () {
		// CHECK END GAME
		button_lvl.onClick.RemoveAllListeners();
		button_lvl.onClick.AddListener (() => sendMsgCreation ());

		if (GameObject.Find ("cat(Clone)") != null) {
			playerGo = GameObject.Find("cat(Clone)");
			if (playerGo.GetComponent<PlayerBehavior> ().lightTiles == 0) {
				//add score
				statsGo.GetComponent<SaveStats>().totalScore += 1;
				statsGo.GetComponent<SaveStats> ().totalMovements += playerGo.GetComponent<PlayerBehavior> ().panelCount;
				//set new level
				playerGo.GetComponent<PlayerBehavior> ().lightTiles = 1;
				floor.GetComponent<CreateLevel> ().lvlOn++;
				if (floor.GetComponent<CreateLevel> ().lvlOn > 6) {
					SceneManager.LoadScene ("main");
				}
				button_lvl.gameObject.SetActive (true);			
			}
		}

	}

	void sendMsgCreation () {
		playerGo.GetComponent<PlayerBehavior> ().button_play.gameObject.SetActive (true);
		floor.GetComponent<CreateLevel> ().deleteChildren ();
		deletePanel ();

		floor.GetComponent<CreateLevel> ().createLevel ();
		button_lvl.gameObject.SetActive (false);
	}

	void deletePanel(){
		foreach (Transform child in pan.transform) {
			Destroy (child.gameObject);
		}
	}
}
