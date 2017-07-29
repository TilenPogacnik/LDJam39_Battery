using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager> {

	[SerializeField] private GameObject BlockPrefab;
	[SerializeField] private float BlockScale;
	[SerializeField] private int GridSizeX;
	[SerializeField] private int GridSizeY;
	[SerializeField] private Transform SpawnHeight;

	[Range(0.0f, 1.0f)]
	[SerializeField] private float StartFillPercentage;

	public List<BlockController> Blocks;
	public List<Vector2> SpawnPositions;
	void Awake() {
	}

	// Use this for initialization
	void Start () {
		GenerateGrid ();
		CalculateSpawnPositions ();
		GameManager.Instance.OnGridGenerated ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void GenerateGrid(){
		int gridSpaces = GridSizeX * GridSizeY;
		int startBlockCount = Mathf.RoundToInt(gridSpaces * StartFillPercentage);

		for (int y = 0; y < GridSizeY; y++){
			for (int x = 0; x < GridSizeX; x++) {
				InstantiateBlock (new Vector2 (x*BlockScale, y*BlockScale), true);
				if (Blocks.Count >= startBlockCount) {
					return;
				}
			}
		}
	}

	private BlockController InstantiateBlock (Vector2 blockPosition, bool rigidbodyKinematic = false){
		BlockController block = Instantiate<GameObject> (BlockPrefab, blockPosition, Quaternion.identity, this.transform).GetComponent<BlockController>();
		block.transform.localPosition = blockPosition;
		block.SetRigidbodyKinematic (rigidbodyKinematic);
		Blocks.Add(block);
		return block;
	}

	public void RemoveBlock(BlockController block){
		Blocks.Remove (block);
		Destroy (block.gameObject);
	}

	private void CalculateSpawnPositions(){
		for (int i = 0; i < GridSizeX; i++) {
			SpawnPositions.Add (new Vector2 (i * BlockScale, SpawnHeight.localPosition.y));
		}

		foreach (Vector2 position in SpawnPositions) {
			InstantiateBlock (position);
		}
	}
}
