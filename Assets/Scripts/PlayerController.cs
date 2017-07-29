using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float MaxMoveVelocity;
	[SerializeField] private float MinJumpVelocity;
	[SerializeField] private float MaxJumpVelocity;

	private bool isGrounded = true;
	private bool isJumping = false;

	private Rigidbody2D Rigidbody;

	void Awake() {
		Rigidbody = this.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		Move ();

		if(InputManager.Instance.JumpPressed && !isJumping){
			Jump();
		}
		if (!InputManager.Instance.JumpPressed && isJumping) {
			StopVariableHeightJump ();
		}
	}

	private void Move(){
		Rigidbody.velocity = new Vector2 (MaxMoveVelocity * Input.GetAxis ("Horizontal"), Rigidbody.velocity.y);
	}

	private void Jump(){
		Debug.Log ("Jumping");
		isJumping = true;
		Rigidbody.velocity = new Vector2 (Rigidbody.velocity.x, MaxJumpVelocity);
	}

	private void StopVariableHeightJump(){
		Debug.Log ("Stopping variable height jump");
		if (Rigidbody.velocity.y > MinJumpVelocity) {
			Rigidbody.velocity = new Vector2 (Rigidbody.velocity.x, MinJumpVelocity);
		}

		isJumping = false;
	}
}
