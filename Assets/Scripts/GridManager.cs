using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager> {

	[SerializeField] private GameObject BlockPrefab;
	[SerializeField] private float BlockScale;
	[SerializeField] private int GridSizeX;
	[SerializeField] private int GridSizeY;

	[Range(0.0f, 1.0f)]
	[SerializeField] private float StartFillPercentage; //Maybe change to StartBlockCount integer

	public List<BlockController> Blocks;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		GenerateGrid ();
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
				Vector3 blockPosition = new Vector3 (x*BlockScale, y*BlockScale, 0);
				GameObject block = Instantiate<GameObject> (BlockPrefab, blockPosition, Quaternion.identity, this.transform);
				block.transform.localPosition = blockPosition;
				Blocks.Add(block.GetComponent<BlockController>());
				if (Blocks.Count >= startBlockCount) {
					return;
				}
			}
		}
	}
}
