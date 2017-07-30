using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	
	[SerializeField] private GameObject PlayerPrefab;
	[SerializeField] private float SpawnYOffset;

	public float BlockTimeLimit;
	public float MinBlockTouchDuration;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
