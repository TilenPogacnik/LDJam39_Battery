using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour {

	void OnMouseOver(){
		if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("Restarting game");
			//TODO restart game
		}
	}
}
