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

	private int Score = 0;
	private int DestroyedBlocksCount = 0;

	[HideInInspector] public float BlockTimeLimit;

	// Use this for initialization
	void Start () {
		Debug.Log ("Starting gamemanager");
		BlockTimeLimit = StartBlockTimeLimit;
	}
	
	// Update is called once per frame
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
			Debug.Log ("New time limit: " + BlockTimeLimit + " destroyed blocks: " + DestroyedBlocksCount);
		}
	}

	public void OnGridGenerated(){
		SpawnPlayer ();
	}

	private void SpawnPlayer(){
		Debug.Log ("Spawning plyer");
		Vector3 spawnPosition = new  Vector3 (Random.Range(0, GridManager.Instance.GridWidth), GridManager.Instance.HighestBlockYPosition, 0f);
		spawnPosition.y += SpawnYOffset;
		Debug.LogError (spawnPosition);

		GameObject player = Instantiate<GameObject> (PlayerPrefab, Vector3.zero, Quaternion.identity, this.transform);
		player.transform.localPosition = spawnPosition;
		Debug.LogError (player.transform.localPosition	);

	}

	public float GetMinBlockTouchDuration(){
		return BlockTimeLimit * MinDamagePercentage;
	}
}
