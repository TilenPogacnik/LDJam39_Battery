using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlockController : MonoBehaviour {

	[SerializeField] private float CurrentHealth;
	[SerializeField] private SpriteRenderer BlockSprite;
	private bool isTouchingPlayer = false;
	private bool isFalling = false;

	private float DeathYPosition;

	void Awake () {
		CurrentHealth = 1.0f;
		DeathYPosition = this.transform.localPosition.y + this.transform.localScale.y/2;
	}
	
	void Update () {
		if (isTouchingPlayer) {
			DecreaseBlockHealth ();
		}
	}

	private void DecreaseBlockHealth(){
		CurrentHealth -= Time.deltaTime / GameManager.Instance.BlockTimeLimit;
		UpdateBlockColor ();
		//UpdateBlockPosition ();
		if (CurrentHealth <= 0.0f) {
			Die ();
		}
	}

	private void UpdateBlockPosition(){
		Vector3 position = this.transform.localPosition;
		position.y = DeathYPosition + this.transform.localScale.y * CurrentHealth;
		this.transform.localPosition = position;
	}

	private void UpdateBlockColor(){
		Color newColor = BlockSprite.color;
		newColor.a = CurrentHealth;
		BlockSprite.color = newColor;
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
		Destroy (this.gameObject);
	}
}
