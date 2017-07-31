using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
	
	[SerializeField] private GameObject PlayerPrefab;
	[SerializeField] private float SpawnYOffset;

	[SerializeField] private float MinDamagePercentage;

	[SerializeField] private float StartBlockTimeLimit;
	[SerializeField] private float EndBlockTimeLimit;
	[SerializeField] private int BlocksBeforeEndTimeLimit;
	[SerializeField] private Text ScoreText;
	[SerializeField] private Animator FakeUI;

	public Enums.GameState GameState;
	private int Score = 0;
	private int DestroyedBlocksCount = 0;

	[HideInInspector] public float BlockTimeLimit;

	void Start () {
		GameState = Enums.GameState.MainMenu;

		Debug.Log ("Starting gamemanager");
		BlockTimeLimit = StartBlockTimeLimit;
	}
	
	void Update () {
	}

	public void IncreaseScore(){
		Score++;
		ScoreText.text = Score.ToString (); 
	}

	public void UpdateBlockTimeLimit(){
		if (DestroyedBlocksCount < BlocksBeforeEndTimeLimit) {
			DestroyedBlocksCount++;
			BlockTimeLimit = StartBlockTimeLimit - (StartBlockTimeLimit - EndBlockTimeLimit) * ((float)DestroyedBlocksCount / (float)BlocksBeforeEndTimeLimit);
		}
	}

	public void OnGridGenerated(){
		SpawnPlayer ();
	}

	private void SpawnPlayer(){
		Debug.Log ("Spawning plyer");
		Vector3 spawnPosition = new  Vector3 (Random.Range(0, GridManager.Instance.GridWidth), GridManager.Instance.HighestBlockYPosition, 0f);
		spawnPosition.y += SpawnYOffset;

		GameObject player = Instantiate<GameObject> (PlayerPrefab, Vector3.zero, Quaternion.identity, this.transform);
		player.transform.localPosition = spawnPosition;
	}

	public float GetMinBlockTouchDuration(){
		return BlockTimeLimit * MinDamagePercentage;
	}

	public void StartGame(){
		Events.OnGameStarted ();
		GameState = Enums.GameState.Playing;
	}

	public void EndGame(){
		Events.OnGameEnded ();
		GameState = Enums.GameState.GameOver;
	}

	public void RestartGame(){
		Events.OnGameRestarted ();
		GameState = Enums.GameState.Playing;
		//TODO
	}
}
