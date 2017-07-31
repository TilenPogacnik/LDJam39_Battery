﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : Singleton<UiController> {
	
	[SerializeField] private Animator animator;

	public void StartGame(){
		animator.SetTrigger ("StartGame");
	}

	public void EndGame(){
		animator.SetTrigger ("EndGame");
	}

	public void RestartGame(){
		animator.SetTrigger ("RestartGame");
	}

	public void GameStarted(){
		GameManager.Instance.StartGame ();
	}

	public void GameRestarted(){
		GameManager.Instance.RestartGame ();
	}
}
