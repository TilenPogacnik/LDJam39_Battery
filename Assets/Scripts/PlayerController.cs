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
	private PlayerAudio audio;

	void Awake() {
		Rigidbody = this.GetComponent<Rigidbody2D> ();
		audio = this.GetComponent<PlayerAudio> ();
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
		//PlayerAnimator.ResetTrigger ("jump");
		PlayerAnimator.SetBool ("jumping", isJumping);

		audio.PlayJumpSound ();

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
		//foreach (Collider2D col in this.gameObject.GetComponentsInChildren<Collider2D> ()) {
		//	col.enabled = false;
		//}
		this.gameObject.layer = 14;
		foreach (Transform ts in this.gameObject.GetComponentsInChildren<Transform>()){
			ts.gameObject.layer = 14;
		}


		//Events.OnPlayerDied ();
		StartCoroutine (DeathAnimation());
		//TODO: actually die
		//Application.LoadLevel(Application.loadedLevel);
		//this.transform.position = new Vector2 (this.transform.position.x, GridManager.Instance.SpawnHeight);
	}

	private IEnumerator DeathAnimation(){
		float timeScale = Time.timeScale;
		Time.timeScale = 0;
		yield return StartCoroutine(WaitForRealSeconds (0.1f));
		Time.timeScale = timeScale;
		audio.PlayDeathSound ();

		Events.OnPlayerDied ();

		yield return WaitForRealSeconds (2.0f);
		Destroy (this.gameObject);
	}

	public static IEnumerator WaitForRealSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return null;
		}
	}
}