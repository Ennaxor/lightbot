using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamara : MonoBehaviour {

	private GameObject floor;

	public UnityEngine.UI.Button camL;
	public UnityEngine.UI.Button camR;

	private float speedCam;

	private bool camLenabled, camRenabled;

	// Use this for initialization
	void Start () {
		floor = GameObject.Find ("Floor");
		camL = GameObject.Find ("Cam-L").GetComponent<UnityEngine.UI.Button> ();
		camR = GameObject.Find ("Cam-R").GetComponent<UnityEngine.UI.Button> ();
		camLenabled = camRenabled = true;
		speedCam = 20.0f;
	}
	
	// Update is called once per frame
	void Update () {

		camL.onClick.RemoveAllListeners();
		camR.onClick.RemoveAllListeners();

		camL.onClick.AddListener (() => turnRight());
		camR.onClick.AddListener (() => turnLeft());

		if(!camLenabled)
			camL.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (200, 200, 200, 255);
		else
			camL.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
		
		if(!camRenabled)
			camR.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (200, 200, 200, 255);
		else
			camR.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
	}

	void turnLeft () {
		if (floor.transform.localEulerAngles.y < 200) {
			camRenabled = true;	
			camLenabled = true;
			floor.transform.RotateAround (Vector3.zero, Camera.main.transform.up, speedCam);
		} else
			camRenabled = false;
		
	}

	void turnRight () {
		print (floor.transform.localEulerAngles.y);
		if (floor.transform.localEulerAngles.y >= 90) {
			camLenabled = true;	
			camRenabled = true;
			floor.transform.RotateAround (Vector3.zero, Camera.main.transform.up, -1 * speedCam);
		} else
			camLenabled = false;

	}
}
