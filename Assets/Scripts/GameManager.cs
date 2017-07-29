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
		Vector3 spawnPosition = GridManager.Instance.Blocks [GridManager.Instance.Blocks.Count - 1].transform.position;
		spawnPosition.y += SpawnYOffset;
		Instantiate<GameObject> (PlayerPrefab, spawnPosition, Quaternion.identity, this.transform);
	}
}
