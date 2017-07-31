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
	[SerializeField] private Text GameOverScoreText;
	[SerializeField] private Animator FakeUI;
	[SerializeField] private Transform SpawnPosition;

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
		GameOverScoreText.text = Score.ToString (); 
	}

	private void ResetScore(){
		Score = 0;
		ScoreText.text = Score.ToString (); 
	}

	public void UpdateBlockTimeLimit(){
		if (DestroyedBlocksCount < BlocksBeforeEndTimeLimit) {
			DestroyedBlocksCount++;
			BlockTimeLimit = StartBlockTimeLimit - (StartBlockTimeLimit - EndBlockTimeLimit) * ((float)DestroyedBlocksCount / (float)BlocksBeforeEndTimeLimit);
		}
	}

	public void OnGridGenerated(){
		Debug.Log ("Grid generated");
	}

	private void SpawnPlayer(){
		Debug.Log ("Spawning player");
		GameObject player = Instantiate<GameObject> (PlayerPrefab, SpawnPosition.position, Quaternion.identity, this.transform);
		//player.transform.localPosition = SpawnPosition;
	}

	public float GetMinBlockTouchDuration(){
		return BlockTimeLimit * MinDamagePercentage;
	}

	public IEnumerator StartGame(){
		ResetScore ();
		if (!GridManager.Instance.GridGenerated) {
			yield return StartCoroutine(GridManager.Instance.GenerateGrid (true));
		}
		SpawnPlayer ();
		Events.OnGameStarted ();
		GameState = Enums.GameState.Playing;
	}

	public void EndGame(){
		Events.OnGameEnded ();
		GameState = Enums.GameState.GameOver;
	}

	public void RestartGame(){
		Debug.Log ("Restaring game");
		Events.OnGameRestarted ();
		StartCoroutine(StartGame ());
	}
}
