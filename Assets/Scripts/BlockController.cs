using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlockController : MonoBehaviour {

	[SerializeField] private float CurrentHealth;
	[SerializeField] private SpriteRenderer BlockSprite = null;
	[SerializeField] private Rigidbody2D Rigidbody = null;

	private bool isTouchingPlayer = false;
	private bool isFalling = false;
	private float CurrentTouchDuration = float.MaxValue;
	private float DeathYPosition;

	void Awake () {
		CurrentHealth = 1.0f;
		DeathYPosition = this.transform.localPosition.y + this.transform.localScale.y/2;
	}
	
	void FixedUpdate () {
		if (isTouchingPlayer || CurrentTouchDuration < GameManager.Instance.MinBlockTouchDuration) {
			DecreaseBlockHealth ();
		}
	}

	private void DecreaseBlockHealth(){
		CurrentHealth -= Time.fixedDeltaTime / GameManager.Instance.BlockTimeLimit;
		UpdateBlockColor ();
		UpdateBlockPosition ();
		if (CurrentHealth <= 0.0f) {
			Die ();
		}

		CurrentTouchDuration += Time.fixedDeltaTime;
	}

	private void UpdateBlockPosition(){
		this.transform.localPosition -= new Vector3(0, this.transform.localScale.y * CurrentHealth * Time.fixedDeltaTime, 0);
	}

	private void UpdateBlockColor(){
		Color newColor = BlockSprite.color;
		newColor.a = CurrentHealth;
		BlockSprite.color = newColor;
	}

	public void SetRigidbodyKinematic(bool kinematic){
		Rigidbody.isKinematic = kinematic;
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == Enums.Tags.Player) {
			//Debug.LogError ("Player died");
		}

		if (coll.gameObject.tag == Enums.Tags.Block) {
			Debug.Log ("Stopped falling");
			SetRigidbodyKinematic (true);
			DeathYPosition = this.transform.localPosition.y + this.transform.localScale.y/2;
			isFalling = false;
		}

	}

	void OnTriggerEnter2D(Collider2D coll){
		if (!isFalling && coll.gameObject.tag == Enums.Tags.Player){
			isTouchingPlayer = true;
			CurrentTouchDuration = 0.0f;
		} 
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == Enums.Tags.Player){
			isTouchingPlayer = false;
		}
	}

	private void Die(){
		GridManager.Instance.RemoveBlock (this);
	}
}
