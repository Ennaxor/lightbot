using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CreateLevel : MonoBehaviour {

	public GameObject tile_neutral, tile_light, player;

	private int ls;

	public const string t_startF = "SF";
	public const string t_startB = "SB";
	public const string t_startR = "SR";
	public const string t_startL = "SL";
	public const string t_n = "0";
	public const string t_null = "N";
	public const string t_off = "1";

	public int rot;

	public int lvl_w;
	public int lvl_h;

	public string[][] floor;

	public int lvlOn;

	private Vector3 aux;

	// Use this for initialization
	void Start () {
		//read file level
		ls = GameObject.Find("Level").GetComponent<LevelSelection>().levelSelected;

		lvlOn = ls;
		createLevel ();
		//set floor rotation and scale
		this.gameObject.transform.rotation = Quaternion.Euler (20, 130, -20);
		this.gameObject.transform.localScale += new Vector3 (0.6f, 0.6f, 0.6f);

	}

	public void createLevel() {
		aux.z = -15;

		if(lvlOn == 1) {
			aux.x = 1;
			Camera.main.transform.position = aux;
		}
		else if(lvlOn == 2) {
			aux.x = 0;
			Camera.main.transform.position = aux;
		}
		else if(lvlOn == 3 || lvlOn == 4 || lvlOn == 5 || lvlOn == 6) {
			aux.x = -1;
			Camera.main.transform.position = aux;
		}

		TextAsset txt = Resources.Load("levels/level"+lvlOn) as TextAsset;
		floor = readFile(txt.text);
		lvl_w = floor[0].Length;
		lvl_h = floor.Length;
		for (int y = 0; y < floor.Length; y++) {
			for (int x = 0; x < floor [0].Length; x++) {
				switch (floor [y] [x]) {
				/* INSTATIATE PLAYER WITH ROT */
				case t_startF:
					GameObject gTileF = Instantiate (tile_neutral, new Vector3 (x, 0, -y), Quaternion.identity) as GameObject;
					gTileF.transform.SetParent(this.gameObject.transform, false);
					gTileF.transform.localPosition = new Vector3 (x, 0, -y);

					GameObject gPlayer = Instantiate (player, new Vector3 (x, 0.15f, -y), Quaternion.identity) as GameObject;
					gPlayer.transform.SetParent(this.gameObject.transform, false);
					gPlayer.transform.localPosition =  new Vector3 (x, 0.15f, -y);
					rot = 0;
					break;
				case t_startB:
					GameObject gTileFB = Instantiate (tile_neutral, new Vector3 (x, 0, -y), Quaternion.identity) as GameObject;
					gTileFB.transform.SetParent(this.gameObject.transform, false);
					gTileFB.transform.localPosition = new Vector3 (x, 0, -y);

					GameObject gPlayer1 = Instantiate (player, new Vector3 (x, 0.15f, -y), Quaternion.identity) as GameObject;
					gPlayer1.transform.SetParent(this.gameObject.transform, false);
					gPlayer1.transform.localPosition =  new Vector3 (x, 0.15f, -y);
					rot = 180;
					break;
				case t_startR:
					GameObject gTileFR = Instantiate (tile_neutral, new Vector3 (x, 0, -y), Quaternion.identity) as GameObject;
					gTileFR.transform.SetParent(this.gameObject.transform, false);
					gTileFR.transform.localPosition = new Vector3 (x, 0, -y);

					GameObject gPlayer2 = Instantiate (player, new Vector3 (x, 0.15f, -y), Quaternion.identity) as GameObject;
					gPlayer2.transform.SetParent(this.gameObject.transform, false);
					gPlayer2.transform.localPosition =  new Vector3 (x, 0.15f, -y);
					rot = -90;
					break;
				case t_startL:
					GameObject gTileFL = Instantiate (tile_neutral, new Vector3 (x, 0, -y), Quaternion.identity) as GameObject;
					gTileFL.transform.SetParent(this.gameObject.transform, false);
					gTileFL.transform.localPosition = new Vector3 (x, 0, -y);

					GameObject gPlayer3 = Instantiate (player, new Vector3 (x, 0.15f, -y), Quaternion.identity) as GameObject;
					gPlayer3.transform.SetParent(this.gameObject.transform, false);
					gPlayer3.transform.localPosition =  new Vector3 (x, 0.15f, -y);
					rot = 90;
					break;
					/* END INSTATIATE PLAYER WITH ROT */
				case t_n:
					GameObject gTileN = Instantiate (tile_neutral, new Vector3 (x, 0, -y), Quaternion.identity) as GameObject;
					gTileN.transform.SetParent(this.gameObject.transform, false);
					gTileN.transform.localPosition = new Vector3 (x, 0, -y);
					break;
				case t_off:
					GameObject gTileO = Instantiate (tile_light, new Vector3 (x, 0, -y), Quaternion.identity) as GameObject;
					gTileO.transform.SetParent(this.gameObject.transform, false);
					gTileO.transform.localPosition =  new Vector3 (x, 0, -y);
					break;
				case t_null:
					//nothing
					break;
				}
			}
		}


	}

	public void deleteChildren() {
		/* FIRST DELETE LIGHT TILES */
		GameObject[] oldTiles = GameObject.FindGameObjectsWithTag ("tileOff");
		foreach (GameObject oldTile in oldTiles) {
			DestroyImmediate (oldTile);
		}
		foreach (Transform child in transform) {
			Destroy (child.gameObject);
		}
	}

	public string checkStep(Vector3 step){ // 2 0 1
		string aux = "";
		for (int y = 0; y < floor.Length; y++) {
			for (int x = 0; x < floor [0].Length; x++) {
				if (x == System.Math.Round(step.x) && y == System.Math.Round(-step.z)) {
					aux = floor [y] [x]; 
				}
			}
		}

		return aux;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	string[][] readFile(string file){
		//string text = System.IO.File.ReadAllText (file);

		/* 0 0 0
		 * 0 0 S
		 * */

		string[] lines = Regex.Split (file, "\r\n");
		int rows = lines.Length;

		string[][] levelMap = new string[rows][];

		for (int i = 0; i < lines.Length; i++) {
			string[] stringsOfLine = Regex.Split (lines[i], " ");
			levelMap[i] = stringsOfLine;
		}

		return levelMap;
	}
}
