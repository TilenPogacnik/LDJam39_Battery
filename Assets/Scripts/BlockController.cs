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
	private bool CanDie = true;
	private bool isTouchingPlayer = false;
	public bool IsFalling = false;
	private float CurrentTouchDuration = float.MaxValue;
	private float DeathYPosition;

	void OnEnable(){
		Events.playerDied += MakeImmortal;
	}

	void OnDisable(){
		Events.playerDied -= MakeImmortal;
	}

	void Awake () {
		CurrentHealth = 1.0f;
		DeathYPosition = this.transform.localPosition.y + this.transform.localScale.y/2;
	}
	
	void FixedUpdate () {
		if (CanBeDamaged && (isTouchingPlayer || CurrentTouchDuration < GameManager.Instance.GetMinBlockTouchDuration())) {
			DecreaseBlockHealth ();
		}
		if (Rigidbody.isKinematic) {
			Rigidbody.velocity = Vector2.zero;
		}

		//BlockSprite.color = CanBeDamaged ? Color.red : Color.white;
		//BlockSprite.color = IsFalling ? Color.green: BlockSprite.color;
		//BlockSprite.color = isTouchingPlayer ? Color.green: Color.white;
	}

	private void MakeImmortal(){
		CanDie = false;
	}

	private void DecreaseBlockHealth(){
		CurrentHealth -= Time.fixedDeltaTime / GameManager.Instance.BlockTimeLimit;
		UpdateBlockColor ();
		UpdateBlockPosition ();
		if (CurrentHealth <= 0.0f) {
			if (CanDie) {
				Die ();
			}
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
		if (IsFalling && coll.gameObject.tag == Enums.Tags.Block) {
			BlockController otherBlock = coll.gameObject.GetComponent<BlockController> ();
			if (otherBlock != null){
				if (otherBlock.Column == this.Column && otherBlock.isDamaged) {
					if (CanDie) {
						otherBlock.Die ();
					}
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

		if (coll.gameObject.tag == Enums.Tags.PlayerDeath) {
			coll.gameObject.GetComponentInParent<PlayerController> ().Die ();

		}
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.tag == Enums.Tags.Player){
			isTouchingPlayer = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == Enums.Tags.Player){
			isTouchingPlayer = false;
		}
	}

	public void Die(){
		GameManager.Instance.UpdateBlockTimeLimit ();
		GameManager.Instance.IncreaseScore ();
		GridManager.Instance.RemoveBlock (this);
	}
}
