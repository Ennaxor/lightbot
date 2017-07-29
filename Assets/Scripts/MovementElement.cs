using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;

public class MovementElement : MonoBehaviour {

	private UnityEngine.UI.Button  mybtn;
	public int myMovement;
	private GameObject playerGo;

	// Use this for initialization
	void Start () {
		mybtn = GetComponent<UnityEngine.UI.Button> ();
		playerGo = GameObject.Find("cat(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		//check mouse click	
		mybtn.onClick.RemoveAllListeners();
		if(GameObject.Find("cat(Clone)") != null)
			if(!playerGo.GetComponent<PlayerBehavior>().playing)
				mybtn.onClick.AddListener(() => DeleteEl ());
	}

	public void setType (int t){
		myMovement = t;
	}

	public int getTypeMove(){
		return myMovement;
	}
		
	void DeleteEl(){	
		playerGo.GetComponent<PlayerBehavior> ().DeleteFromArr (this.gameObject);	
		Destroy (gameObject);	
	}
}
