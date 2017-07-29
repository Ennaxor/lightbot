using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR 
using UnityEditor;
using UnityEditor.Callbacks;
#endif



public class PlayerBehavior : MonoBehaviour {

	private UnityEngine.UI.Button button_f;
	private UnityEngine.UI.Button button_r;
	private UnityEngine.UI.Button button_l;
	private UnityEngine.UI.Button button_light;
	public UnityEngine.UI.Button button_play;
	public UnityEngine.UI.Button button_rewind;

	public UnityEngine.UI.Button button_speed;
	public UnityEngine.UI.Button button_speedUp;

	public UnityEngine.UI.Button button_musicOn;
	public UnityEngine.UI.Button button_musicOff;

	public UnityEngine.UI.Button button_menu;

	public UnityEngine.UI.Text text_lvl;
	public UnityEngine.UI.Text text_movements;
	public UnityEngine.UI.Text text_score;

	private GameObject statsGo;

	private AudioSource audioSrc;

	private GameObject pan; 
	private GameObject floor;
	private GameObject cvs;

	private int numMovements;

	// players position
	private Vector3 initialPos;
	private Vector3 position;
	private Vector3 auxPos;
	private int lookingAt;
	private int lookingAtInit;
	private Quaternion initialRot;
	private int angle;

	// movement array
	List<GameObject> movementArr = new List<GameObject>();
	public int lightTiles;
	public int lightTilesInit;
	public int panelCount;
	private int maxPanelCount;

	private bool musicOn;

	/* ANIMATIONS */
	private Animator anim;
	private bool walk;
	private bool inMovement;
	public bool playing;

	public float speedAnim;
	public Sprite fasterSprite;
	public Sprite slowerSprite;

	// Use this for initialization
	void Start () {
		//initialize buttons
		button_f = GameObject.Find ("Front-btn").GetComponent<UnityEngine.UI.Button> ();
		button_r = GameObject.Find ("Right-btn").GetComponent<UnityEngine.UI.Button> ();
		button_l = GameObject.Find ("Left-btn").GetComponent<UnityEngine.UI.Button> ();
		button_light = GameObject.Find ("Light-btn").GetComponent<UnityEngine.UI.Button> ();
		button_play = GameObject.Find ("Play-btn").GetComponent<UnityEngine.UI.Button> ();
		button_rewind = GameObject.Find ("Rewind-btn").GetComponent<UnityEngine.UI.Button> ();
		button_speed = GameObject.Find ("Speed-btn").GetComponent<UnityEngine.UI.Button> ();
		button_speedUp = GameObject.Find ("SpeedUp-btn").GetComponent<UnityEngine.UI.Button> ();

		button_musicOn = GameObject.Find ("MusicOn-btn").GetComponent<UnityEngine.UI.Button> ();
		button_musicOff = GameObject.Find ("MusicOff-btn").GetComponent<UnityEngine.UI.Button> ();

		button_menu = GameObject.Find ("Menu-btn").GetComponent<UnityEngine.UI.Button> ();

		text_lvl = GameObject.Find ("LevelON").GetComponent<UnityEngine.UI.Text> ();
		text_movements = GameObject.Find ("Movements").GetComponent<UnityEngine.UI.Text> ();
		text_score = GameObject.Find ("Score").GetComponent<UnityEngine.UI.Text> ();

		statsGo = GameObject.Find ("Stats");

		numMovements = 0;
		text_movements.text = "MOVEMENTS: " + numMovements;

		button_rewind.gameObject.SetActive (false);
		button_speedUp.gameObject.SetActive (false);
		button_musicOff.gameObject.SetActive (false);

		pan = GameObject.Find ("Panel");
		floor = GameObject.Find ("Floor");
		cvs = GameObject.Find ("Canvas");
		cvs.GetComponent<NextLevel> ().enabled = true;

		lightTiles = 0;
		GameObject[] num = GameObject.FindGameObjectsWithTag ("tileOff");
		lightTiles = num.Length;
		lightTilesInit = lightTiles;

		position = transform.localPosition;
		initialPos = position;

		panelCount = 0;
		maxPanelCount = 11;
		/* LOOK AT */
		/* 0 looking front
		 * 1 looking left
		 * 2 looking back
		 * 3 looking right */

		anim = GetComponent<Animator> ();
		walk = false;
		inMovement = false;
		playing = false;
		musicOn = true;
		speedAnim = 1.0f;

		audioSrc = Camera.main.transform.GetComponent<AudioSource> ();

		angle = floor.GetComponent<CreateLevel> ().rot;
		transform.Rotate (new Vector3(0, angle, 0), Space.Self);
		if (angle == 0)
			lookingAt = 0;
		else if (angle == 90)
			lookingAt = 1;
		else if (angle == 180)
			lookingAt = 2;
		else if (angle == -90)
			lookingAt = 3;

		lookingAtInit = lookingAt;
		initialRot = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		//movement control
		//first remove all listeners
		button_f.onClick.RemoveAllListeners();
		button_r.onClick.RemoveAllListeners();
		button_l.onClick.RemoveAllListeners();
		button_light.onClick.RemoveAllListeners();
		button_play.onClick.RemoveAllListeners();
		button_rewind.onClick.RemoveAllListeners();
		button_speed.onClick.RemoveAllListeners();
		button_speedUp.onClick.RemoveAllListeners();
		button_musicOn.onClick.RemoveAllListeners();
		button_musicOff.onClick.RemoveAllListeners();
		button_menu.onClick.RemoveAllListeners();

		if (!playing) {
			/* colors */
			button_f.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
			button_l.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
			button_r.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
			button_light.GetComponent<UnityEngine.UI.Image> ().color = Color.white;

			button_f.onClick.AddListener (() => Move ("front"));
			button_l.onClick.AddListener (() => Move ("left"));
			button_r.onClick.AddListener (() => Move ("right"));
			button_light.onClick.AddListener (() => Move ("light"));
			button_play.onClick.AddListener (() => StartCoroutine (playMovements ()));
		} else {
			button_f.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (200, 200, 200, 255);
			button_l.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (200, 200, 200, 255);
			button_r.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (200, 200, 200, 255);
			button_light.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (200, 200, 200, 255);
		}


		text_lvl.text = "Level " + floor.GetComponent<CreateLevel> ().lvlOn;

		text_movements.text = "MOVEMENTS: " + numMovements;

		text_score.text = "SCORE: " + statsGo.GetComponent<SaveStats>().totalScore;

		button_speed.onClick.AddListener (() => changeSpeed());
		button_speedUp.onClick.AddListener (() => changeSpeed());
		button_rewind.onClick.AddListener (() => resetPlayer());
		button_musicOn.onClick.AddListener (() => setMusic());
		button_musicOff.onClick.AddListener (() => setMusic());
		button_menu.onClick.AddListener (() => goToMenu());

		anim.SetBool ("walking", walk);
	}

	void changeSpeed() {
		if (speedAnim == 1.0f) {
			speedAnim = 2.0f;
			button_speed.gameObject.SetActive (false);
			button_speedUp.gameObject.SetActive (true);
		} else {
			speedAnim = 1.0f;
			button_speed.image.sprite = slowerSprite;
			button_speed.gameObject.SetActive (true);
			button_speedUp.gameObject.SetActive (false);
		}

	}

	void resetPlayer() {
		transform.localPosition = initialPos;
		transform.localRotation = initialRot;
		lookingAt = lookingAtInit;
		button_rewind.gameObject.SetActive (false);
		button_play.gameObject.SetActive (true);
		playing = false;
		resetTileLight ();
	}

	void goToMenu() {
		//out of game - new ranking
		cvs.GetComponent<NextLevel> ().passedALevel = true;
		SceneManager.LoadScene ("menu");
	}

	void setMusic() {
		if (musicOn) {
			musicOn = false;
			audioSrc.mute = true;
			button_musicOn.gameObject.SetActive (false);
			button_musicOff.gameObject.SetActive (true);
		} else {
			musicOn = true;
			audioSrc.mute = false;
			button_musicOn.gameObject.SetActive (true);
			button_musicOff.gameObject.SetActive (false);
		}
	}

	void resetTileLight() {
		lightTiles = lightTilesInit;
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("tileOff");
		foreach (GameObject t in tiles) {
			t.GetComponent<Renderer> ().material.color = new Color32 (32, 188, 245, 255);
		}
	}

	void setActives() { 
		button_rewind.gameObject.SetActive (true);
		button_speedUp.gameObject.SetActive (true);
		button_speed.gameObject.SetActive (true);
		button_play.gameObject.SetActive (true);
		button_musicOn.gameObject.SetActive (true);
		button_musicOff.gameObject.SetActive (true);

	}

	IEnumerator playMovements() {
		int step; 
		playing = true;
		position = initialPos;
		transform.localPosition = initialPos;
		transform.localRotation = initialRot;
		lookingAt = lookingAtInit;


		foreach (GameObject g in movementArr) {
			step = g.GetComponent<MovementElement> ().getTypeMove ();
			var c = g.GetComponent<UnityEngine.UI.Button> ().colors;

			/* ** ** LIGHT ** ** */
			if (step == 3) {
				c.normalColor = new Color32(255, 201, 176, 180);
				g.GetComponent<UnityEngine.UI.Button> ().colors = c;
				if (checkTileLight () != null) {
					GameObject tileL = checkTileLight ();
					tileL.GetComponent<Renderer> ().material.color = new Color32 (234, 255, 33, 255);
					lightTiles -= 1;
					if (lightTiles == 0)
						setActives ();
				}

				yield return new WaitForSeconds (0.5f);
				c.normalColor = Color.white;
				g.GetComponent<UnityEngine.UI.Button> ().colors = c;	 
			}

			/* ** ** MOVEMENT ** ** */
			if (lookingAt == 0) { //looking at front side
				frontMovement (step);
				while (inMovement) {
					c.normalColor = new Color32(105, 255, 147, 255);
					g.GetComponent<UnityEngine.UI.Button> ().colors = c;
					yield return new WaitForSeconds (0.05f);
				}
				c.normalColor = Color.white;
				g.GetComponent<UnityEngine.UI.Button> ().colors = c;
			} else if (lookingAt == 1) { //looking at left side
				leftMovement (step);
				while (inMovement) {
					c.normalColor = new Color32(105, 255, 147, 255);
					g.GetComponent<UnityEngine.UI.Button> ().colors = c;
					yield return new WaitForSeconds (0.05f);
				}
				c.normalColor = Color.white;
				g.GetComponent<UnityEngine.UI.Button> ().colors = c;
			} else if (lookingAt == 2) { //looking at back side
				backMovement (step);
				while (inMovement) {
					c.normalColor = new Color32(105, 255, 147, 255);
					g.GetComponent<UnityEngine.UI.Button> ().colors = c;
					yield return new WaitForSeconds (0.05f);
				}
				c.normalColor = Color.white;
				g.GetComponent<UnityEngine.UI.Button> ().colors = c;
			} else if (lookingAt == 3) { //looking at right side
				rightMovement (step);
				while (inMovement) {
					c.normalColor = new Color32(105, 255, 147, 255);
					g.GetComponent<UnityEngine.UI.Button> ().colors = c;
					yield return new WaitForSeconds (0.05f);
				}
				c.normalColor = Color.white;
				g.GetComponent<UnityEngine.UI.Button> ().colors = c;
			} 
		}

		button_rewind.gameObject.SetActive (true);
		button_play.gameObject.SetActive (false);
	}

	GameObject checkTileLight(){
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("tileOff");
		foreach (GameObject t in tiles) {
			if (System.Math.Round(transform.localPosition.x) == t.transform.localPosition.x && System.Math.Round(transform.localPosition.z) == t.transform.localPosition.z) {
				return t;
			}
		}
		return null;
	}


	bool checkLimits(Vector3 futurePos){
		int width = floor.GetComponent<CreateLevel> ().lvl_w; //4
		int height = floor.GetComponent<CreateLevel> ().lvl_h; //3 

		string st = floor.GetComponent<CreateLevel> ().checkStep (futurePos); //(2,1)

	//	print ("futurepos: " + (int)futurePos.z + " - width map: " + width + " - height map: " + height + " -casilla: "+st);
		if(!st.Equals("N") && !st.Equals("")){ 
			if ((System.Math.Round(futurePos.x) >= 0 && System.Math.Round(futurePos.x) < width) && (System.Math.Round(futurePos.z) <= 0 && System.Math.Round(futurePos.z) > -1 * height))				
				return true;// WIDTH 
		}
		return false;
	}

	/* ************* SMOOTH MOVEMENT **************** */

	IEnumerator smoothMove(Vector3 spawnPoint) {	
		//print("in smoothMove");	
		inMovement = true;
		Vector3 deltaPos = spawnPoint - transform.localPosition;
		deltaPos.y = 0.15f;
		walk = true;
		float timePassed = 0;
		while (timePassed < 1.0f/speedAnim) {			
			timePassed += Time.deltaTime;
			position.x += deltaPos.x * Time.deltaTime * speedAnim;
			position.y = 0.15f;
			position.z += deltaPos.z * Time.deltaTime * speedAnim;

			transform.localPosition = position;
			yield return null;	
		}
		walk = false;
		yield return new WaitForSeconds (0.05f);	
		//print("leave smoothmove");	
		inMovement = false;
	}


	IEnumerator smoothRotation(Vector3 rotPoint) {
		//print("in smootRot");
		inMovement = true;
		//Vector3 deltaRot = rotPoint - transform.localRotation.eulerAngles;
		float timePassed = 0;
		float speed = 3.0f;
		while (timePassed < 0.33f) {
			timePassed += Time.deltaTime;
			transform.Rotate (rotPoint * Time.deltaTime* speed, Space.Self);
			yield return null;	
		}
		yield return new WaitForSeconds (0.05f);
		//print("leave smootRot");	
		inMovement = false;
	}


	/* ************* END SMOOTH MOVEMENT **************** */

	void frontMovement(int s) {
		switch (s) {
			case 0: //move in +Z
				if (checkLimits (new Vector3 (position.x, 0, position.z + 1))) {					
					StartCoroutine (smoothMove (new Vector3 (position.x, 0.15f, position.z + 1))); //position.z += 1;					
				}
				break;
			case 1: // ROTATE -90y
				lookingAt = 3;
				StartCoroutine (smoothRotation (new Vector3(0, -90, 0))); //	transform.Rotate(new Vector3(0, -90, 0), Space.Self);
				break;
			case 2: //ROTATE +90y
				lookingAt = 1;
				StartCoroutine (smoothRotation (new Vector3(0, 90, 0)));
				break;				
		}
	}

	void leftMovement(int s) {
		switch (s) {
		case 0: //move in +X
				if (checkLimits (new Vector3 (position.x + 1, 0, position.z))) {
					StartCoroutine (smoothMove (new Vector3 (position.x+1, 0.15f, position.z))); //position.x += 1;
				}
				break;
			case 1: // ROTATE -90y
				lookingAt = 0;
				StartCoroutine (smoothRotation (new Vector3(0, -90, 0)));
				break;
			case 2: // ROTATE +90y
				lookingAt = 2;
				StartCoroutine (smoothRotation (new Vector3(0, 90, 0)));
				break;				
		}

	}

	void backMovement(int s) {
		switch (s) {
			case 0: //move in -Z
				if(checkLimits(new Vector3(position.x, 0, position.z-1))) {
					StartCoroutine (smoothMove (new Vector3 (position.x, 0.15f, position.z-1))); //position.z -= 1;
				}
				break;
			case 1: // ROTATE -90y
				lookingAt = 1;
				StartCoroutine (smoothRotation (new Vector3(0, -90, 0)));
				break;
			case 2: // ROTATE +90y
				lookingAt = 3;
				StartCoroutine (smoothRotation (new Vector3(0, 90, 0)));
				break;				
		}

	}

	void rightMovement(int s) {
		switch (s) {
			case 0: //move in -X
				if (checkLimits (new Vector3 (position.x - 1, 0, position.z))) {
					StartCoroutine (smoothMove (new Vector3 (position.x-1, 0.15f, position.z))); //position.x -= 1;
				}
				break;
		case 1: //ROTATE -90y
				lookingAt = 2;
				StartCoroutine (smoothRotation (new Vector3 (0, -90, 0)));
				break;
			case 2: //ROTATE +90y
				lookingAt = 0;
				StartCoroutine (smoothRotation (new Vector3(0, 90, 0)));
				break;				
		}

	}

	void Move(string pAction){
		switch (pAction) {
			case "front": // 0
				/* ADD MOVEMENT FRONT */
				CreatePanelEl ("front-ui", 0);				
				break;
			case "left": // 1
				/* ADD MOVEMENT LEFT */
				CreatePanelEl ("left-ui", 1);					
				break;
			case "right": // 2
				/* ADD MOVEMENT RIGHT */
				CreatePanelEl ("right-ui", 2);	
				break;
			case "light": // 3
				/* ADD MOVEMENT LIGHT */
				CreatePanelEl ("light-ui", 3);	
				break;
		}
	}

	public void DeleteFromArr(GameObject g) {
		panelCount--;
		numMovements--;
		movementArr.Remove (g);
	}

	void CreatePanelEl(string movementPrefab, int t){
		if (panelCount > maxPanelCount)
			return;
		panelCount++;
		GameObject go = Instantiate (Resources.Load (movementPrefab)) as GameObject;
		movementArr.Add (go);
		numMovements++;
		go.transform.SetParent (pan.transform, false);
		go.AddComponent<MovementElement> ();
		go.GetComponent<MovementElement> ().setType (t);
	}
}
