using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Awake() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
		float cameraH = Camera.main.orthographicSize*2;
		Vector2 cameraSize = new Vector2 (Camera.main.aspect * cameraH, cameraH);
		Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

		Vector2 scale = transform.localScale;
		if (cameraSize.x >= cameraSize.y) { //if landscape
			scale *= cameraSize.x / spriteSize.x;
		} else { // if portrait
			scale *= cameraSize.y / spriteSize.y;
		}

		transform.position = Vector2.zero; 

		transform.localScale = scale;
	}
}
