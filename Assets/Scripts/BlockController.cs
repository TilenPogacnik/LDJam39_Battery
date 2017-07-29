using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private float CurrentHealth;
	private bool isTouchingPlayer = false;
	private bool isFalling = false;

	void Awake () {
		CurrentHealth = 1.0f;	
	}
	
	void FixedUpdate () {
		if (isTouchingPlayer) {
			Debug.Log ("TouchingPlayer");

			DecreaseBlockHealth ();
		}
	}

	private void DecreaseBlockHealth(){
		CurrentHealth -= Time.fixedDeltaTime / GameManager.Instance.BlockTimeLimit;
		if (CurrentHealth <= 0.0f) {
			Die ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player"){
			Debug.Log ("Player started touching block");
			isTouchingPlayer = true;
		} 

		if (coll.gameObject.tag == "Block") {
			isFalling = false;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag == "Player"){
			Debug.Log ("Player stopped touching block");
			isTouchingPlayer = false;
		}
	}

	private void Die(){
		Debug.Log ("Killing block");
	}
}
