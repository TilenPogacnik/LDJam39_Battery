using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float MaxMoveVelocity = 0;
	[SerializeField] private float MinJumpVelocity = 0;
	[SerializeField] private float MaxJumpVelocity = 0;
	[SerializeField] private Transform GroundPosition = null;
	[SerializeField] private float MaxGroundDistance = 0;
	[SerializeField] private Animator PlayerAnimator;

	private bool isGrounded = true;
	private bool isJumping = false;

	private Rigidbody2D Rigidbody;

	void Awake() {
		Rigidbody = this.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		CheckIfGrounded ();
		Move ();

		if(isGrounded && !isJumping && InputManager.Instance.JumpPressed){
			Jump();
		}
		if (isJumping && !InputManager.Instance.JumpPressed) {
			StopVariableHeightJump ();
		}
	}

	private void CheckIfGrounded(){
		Debug.DrawRay (GroundPosition.position, Vector2.down * MaxGroundDistance, isGrounded ? Color.green : Color.red, 1.0f);
		RaycastHit2D hit = Physics2D.Raycast(GroundPosition.position, Vector2.down, MaxGroundDistance);
		if (hit.transform != null) {
			if (!InputManager.Instance.JumpPressed) {
				isJumping = false;
				PlayerAnimator.SetBool ("jumping", isJumping);

			}
			isGrounded = true;
		} else {
			isGrounded = false;
		}
		PlayerAnimator.SetBool ("grounded", isGrounded);

	}

	private void Move(){
		Rigidbody.velocity = new Vector2 (MaxMoveVelocity * Input.GetAxis ("Horizontal"), Rigidbody.velocity.y);

		UpdatePlayerDirection ();
		if (!isJumping && Mathf.Abs(Rigidbody.velocity.x) > 0) {
			PlayerAnimator.SetBool ("walking", true);
		} else {
			PlayerAnimator.SetBool ("walking", false);
		}
	}

	private void UpdatePlayerDirection (){
		if (Mathf.Abs (Rigidbody.velocity.x) > 0) {
			PlayerAnimator.transform.localScale = new Vector3 (Rigidbody.velocity.x < 0 ? -1 : 1, PlayerAnimator.transform.localScale.y, PlayerAnimator.transform.localScale.z);
		}
	}

	private void Jump(){
		isJumping = true;
		PlayerAnimator.SetTrigger ("jump");
		PlayerAnimator.SetBool ("jumping", isJumping);

		Rigidbody.velocity = new Vector2 (Rigidbody.velocity.x, MaxJumpVelocity);
	}

	private void StopVariableHeightJump(){
		if (Rigidbody.velocity.y > MinJumpVelocity) {
			Rigidbody.velocity = new Vector2 (Rigidbody.velocity.x, MinJumpVelocity);
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == Enums.Tags.KillZone) {
			Die ();
		}
	}
	public void Die(){
		Debug.Log ("Player died");
		GameManager.Instance.EndGame ();
		//TODO: actually die
		//Application.LoadLevel(Application.loadedLevel);
		//this.transform.position = new Vector2 (this.transform.position.x, GridManager.Instance.SpawnHeight);
	}
}