using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float MaxMoveVelocity = 0;
	[SerializeField] private float MinJumpVelocity = 0;
	[SerializeField] private float MaxJumpVelocity = 0;
	[SerializeField] private Transform GroundPosition = null;
	[SerializeField] private float MaxGroundDistance = 0;

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
			}
			isGrounded = true;
		} else {
			isGrounded = false;
		}
	}

	private void Move(){
		Rigidbody.velocity = new Vector2 (MaxMoveVelocity * Input.GetAxis ("Horizontal"), Rigidbody.velocity.y);
	}

	private void Jump(){
		isJumping = true;
		Rigidbody.velocity = new Vector2 (Rigidbody.velocity.x, MaxJumpVelocity);
	}

	private void StopVariableHeightJump(){
		if (Rigidbody.velocity.y > MinJumpVelocity) {
			Rigidbody.velocity = new Vector2 (Rigidbody.velocity.x, MinJumpVelocity);
		}
	}
}