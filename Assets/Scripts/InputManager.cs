using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager> {

	public bool JumpPressed { get; private set;}

	void Awake() {
		JumpPressed = false;
	}

	void Update () {
		JumpPressed = Input.GetKey (KeyCode.Space) ||
						Input.GetKey (KeyCode.W) ||
						Input.GetButton ("Jump");
	}
}
