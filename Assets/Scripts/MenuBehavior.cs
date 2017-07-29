using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour {


	private UnityEngine.UI.Button musicOn;
	private UnityEngine.UI.Button musicOff;

	private UnityEngine.UI.Button playBtn;
	private UnityEngine.UI.Button lvl1;
	private UnityEngine.UI.Button lvl2;
	private UnityEngine.UI.Button lvl3;
	private UnityEngine.UI.Button lvl4;
	private UnityEngine.UI.Button lvl5;
	private UnityEngine.UI.Button lvl6;

	private UnityEngine.UI.Button back;
	private UnityEngine.UI.Button exit;

	public UnityEngine.UI.Text rN;
	public UnityEngine.UI.Text rL;
	public UnityEngine.UI.Text rM;
	public UnityEngine.UI.Text rLoading;

	private UnityEngine.UI.Button ranking;

	public UnityEngine.UI.InputField nameInput;

	private bool musicPlaying;
	private AudioSource audioSrc;

	private GameObject levelGo;
	private GameObject cat;
	private GameObject statsGo;

	private GameObject rankingPanel;

	// Use this for initialization
	void Start () {
		musicOn = GameObject.Find ("MusicOn-btn").GetComponent<UnityEngine.UI.Button> ();
		musicOff = GameObject.Find ("MusicOff-btn").GetComponent<UnityEngine.UI.Button> ();
		playBtn = GameObject.Find ("Play-btn").GetComponent<UnityEngine.UI.Button> ();
		lvl1 = GameObject.Find ("Level1").GetComponent<UnityEngine.UI.Button> ();
		lvl2 = GameObject.Find ("Level2").GetComponent<UnityEngine.UI.Button> ();
		lvl3 = GameObject.Find ("Level3").GetComponent<UnityEngine.UI.Button> ();
		lvl4 = GameObject.Find ("Level4").GetComponent<UnityEngine.UI.Button> ();
		lvl5 = GameObject.Find ("Level5").GetComponent<UnityEngine.UI.Button> ();
		lvl6 = GameObject.Find ("Level6").GetComponent<UnityEngine.UI.Button> ();

		back = GameObject.Find ("Back").GetComponent<UnityEngine.UI.Button> ();
		exit = GameObject.Find ("Exit").GetComponent<UnityEngine.UI.Button> ();

		ranking = GameObject.Find ("Ranking-btn").GetComponent<UnityEngine.UI.Button> ();
		rankingPanel = GameObject.Find ("Ranking");
		cat = GameObject.Find ("cat-menu");

		rN = GameObject.Find ("RankName").GetComponent<UnityEngine.UI.Text> ();
		rL = GameObject.Find ("RankLevel").GetComponent<UnityEngine.UI.Text> ();
		rM = GameObject.Find ("RankMovements").GetComponent<UnityEngine.UI.Text> ();
		rLoading = GameObject.Find ("RankText").GetComponent<UnityEngine.UI.Text> ();
		levelGo = GameObject.Find ("Level");

		statsGo = GameObject.Find ("Stats");

		nameInput = GameObject.Find ("SetName").GetComponent<UnityEngine.UI.InputField> ();

		musicOff.gameObject.SetActive (false);
		lvl1.gameObject.SetActive (false);
		lvl2.gameObject.SetActive (false);
		lvl3.gameObject.SetActive (false);
		lvl4.gameObject.SetActive (false);
		lvl5.gameObject.SetActive (false);
		lvl6.gameObject.SetActive (false);
		back.gameObject.SetActive (false);
		rankingPanel.gameObject.SetActive (false);
		rLoading.gameObject.SetActive (false);
		rN.gameObject.SetActive (false);
		rL.gameObject.SetActive (false);
		rM.gameObject.SetActive (false);

		musicPlaying = true;

		audioSrc = Camera.main.transform.GetComponent<AudioSource> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		musicOn.onClick.RemoveAllListeners();
		playBtn.onClick.RemoveAllListeners();
		lvl1.onClick.RemoveAllListeners();
		lvl2.onClick.RemoveAllListeners();
		lvl3.onClick.RemoveAllListeners();
		lvl4.onClick.RemoveAllListeners();
		lvl5.onClick.RemoveAllListeners();
		lvl6.onClick.RemoveAllListeners();

		back.onClick.RemoveAllListeners();
		exit.onClick.RemoveAllListeners();

		ranking.onClick.RemoveAllListeners();

		musicOn.onClick.AddListener (() => setMusic());
		musicOff.onClick.AddListener (() => setMusic());

		//only if name introduced
		if (nameInput.text != "") {			
			playBtn.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
			playBtn.onClick.AddListener (() => showLevels ());
		}
		else
			playBtn.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (200, 200, 200, 255);



		lvl1.onClick.AddListener (() => setLevel(1));
		lvl2.onClick.AddListener (() => setLevel(2));
		lvl3.onClick.AddListener (() => setLevel(3));
		lvl4.onClick.AddListener (() => setLevel(4));
		lvl5.onClick.AddListener (() => setLevel(5));
		lvl6.onClick.AddListener (() => setLevel(6));

		back.onClick.AddListener (() => showPlay());
		exit.onClick.AddListener (() => exitGame());

		ranking.onClick.AddListener (() => showRanking());
	}

	void exitGame () {
		Application.Quit ();
	}

	void showRanking () {
		rankingPanel.gameObject.SetActive (true);
		playBtn.gameObject.SetActive (false);
		lvl1.gameObject.SetActive (false);
		lvl2.gameObject.SetActive (false);
		lvl3.gameObject.SetActive (false);
		lvl4.gameObject.SetActive (false);
		lvl5.gameObject.SetActive (false);
		lvl6.gameObject.SetActive (false);

		back.gameObject.SetActive (true);	
		cat.gameObject.SetActive (false);

		rN.gameObject.SetActive (true);
		rL.gameObject.SetActive (true);
		rM.gameObject.SetActive (true);
		playBtn.gameObject.SetActive (false);
		rLoading.gameObject.SetActive (true);

		nameInput.gameObject.SetActive (false);
	
	}

	void showLevels () {
		levelGo.GetComponent<SetName>().nameOfPlayer = nameInput.text;
		playBtn.gameObject.SetActive (false);
		lvl1.gameObject.SetActive (true);
		lvl2.gameObject.SetActive (true);
		lvl3.gameObject.SetActive (true);
		lvl4.gameObject.SetActive (true);
		lvl5.gameObject.SetActive (true);
		lvl6.gameObject.SetActive (true);
		back.gameObject.SetActive (true);
		nameInput.gameObject.SetActive (false);
	}

	void showPlay () { 
		statsGo.GetComponent<SaveStats> ().totalMovements = 0;
		statsGo.GetComponent<SaveStats> ().totalScore = 0;
		playBtn.gameObject.SetActive (true);
		lvl1.gameObject.SetActive (false);
		lvl2.gameObject.SetActive (false);
		lvl3.gameObject.SetActive (false);
		lvl4.gameObject.SetActive (false);
		lvl5.gameObject.SetActive (false);
		lvl6.gameObject.SetActive (false);
		back.gameObject.SetActive (false);	
		cat.gameObject.SetActive (true);
		rN.gameObject.SetActive (false);
		rL.gameObject.SetActive (false);
		rM.gameObject.SetActive (false);
		rankingPanel.gameObject.SetActive (false);
		rLoading.gameObject.SetActive (false);
		nameInput.gameObject.SetActive (true);
	}

	void setLevel (int lvl) { 
		/* set level on click and reset stats*/
		levelGo.GetComponent<LevelSelection> ().levelSelected = lvl;
		SceneManager.LoadScene ("main");
	}


	void setMusic() {
		if (musicPlaying) {
			musicPlaying = false;
			audioSrc.mute = true;
			musicOn.gameObject.SetActive (false);
			musicOff.gameObject.SetActive (true);
		} else {
			musicPlaying = true;
			audioSrc.mute = false;
			musicOn.gameObject.SetActive (true);
			musicOff.gameObject.SetActive (false);
		}
	}
}
