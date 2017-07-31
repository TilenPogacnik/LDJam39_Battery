using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : Singleton<UiController> {
	
	[SerializeField] private Animator animator;

	void OnEnable(){
		Events.gameEnded += EndGame;
	}

	void OnDisable(){
		Events.gameEnded -= EndGame;
	}

	public void StartGame(){
		animator.SetTrigger ("StartGame");
	}

	public void EndGame(){
		animator.SetTrigger ("EndGame");
	}

	public void RestartGame(){
		animator.SetTrigger ("RestartGame");
		GameManager.Instance.RestartGame ();
	}

	public void GameStarted(){
		StartCoroutine(GameManager.Instance.StartGame ());
	}

	public void GameRestarted(){
		GameManager.Instance.RestartGame ();
	}
}
