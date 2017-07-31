using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

	void OnMouseOver(){
		if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("Starting game");
			UiController.Instance.StartGame ();
		}
	}
}
