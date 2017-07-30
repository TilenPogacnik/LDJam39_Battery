using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlockController : MonoBehaviour {

	[SerializeField] private float CurrentHealth;
	[SerializeField] private SpriteRenderer BlockSprite = null;
	[SerializeField] private Rigidbody2D Rigidbody = null;

	public int Column;
	public bool isDamaged {
		get {
			return CurrentHealth < 1.0f;
		}
	}
	public bool CanBeDamaged = false;
	private bool isTouchingPlayer = false;
	public bool IsFalling = false;
	private float CurrentTouchDuration = float.MaxValue;
	private float DeathYPosition;

	void Awake () {
		CurrentHealth = 1.0f;
		DeathYPosition = this.transform.localPosition.y + this.transform.localScale.y/2;
	}
	
	void FixedUpdate () {
		if (CanBeDamaged && (isTouchingPlayer || CurrentTouchDuration < GameManager.Instance.MinBlockTouchDuration)) {
			DecreaseBlockHealth ();
		}
		if (Rigidbody.isKinematic) {
			Rigidbody.velocity = Vector2.zero;
		}

		//BlockSprite.color = CanBeDamaged ? Color.red : Color.white;
		//BlockSprite.color = IsFalling ? Color.green: BlockSprite.color;
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
		this.transform.localPosition -= new Vector3(0, this.transform.localScale.y * CurrentHealth * Time.fixedDeltaTime / GameManager.Instance.BlockTimeLimit, 0);
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

		if (IsFalling && coll.gameObject.tag == Enums.Tags.Block) {
			Debug.Log ("Stopped falling");
			BlockController otherBlock = coll.gameObject.GetComponent<BlockController> ();
			if (otherBlock != null){
				if (otherBlock.Column == this.Column && otherBlock.isDamaged) {
					otherBlock.Die ();
					return;
				}
			}
			SetRigidbodyKinematic (true);
			transform.localPosition = GridManager.Instance.GetNearestPointOnGrid (transform.localPosition);
			DeathYPosition = this.transform.localPosition.y + this.transform.localScale.y/2;
			IsFalling = false;
			GridManager.Instance.RefreshColumnDamages (Column);
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (!IsFalling && coll.gameObject.tag == Enums.Tags.Player){
			isTouchingPlayer = true;
			CurrentTouchDuration = 0.0f;
		} 
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == Enums.Tags.Player){
			isTouchingPlayer = false;
		}
	}

	public void Die(){
		GridManager.Instance.RemoveBlock (this);
	}
}
