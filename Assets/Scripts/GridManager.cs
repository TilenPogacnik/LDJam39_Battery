using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : Singleton<GridManager> {

	[SerializeField] private GameObject BlockPrefab;
	[SerializeField] private float BlockScale;
	[SerializeField] private int GridSizeX;
	[SerializeField] private int GridSizeY;
	public int SpawnHeight;

	[Range(0.0f, 1.0f)]
	[SerializeField] private float StartFillPercentage;
	[SerializeField] private float SpawnInterval;

	private int CurrentBlockCount = 0;
	private int RespawnQueueBlockCount = 0;

	public List<List<BlockController>> Blocks = new List<List<BlockController>>();
	public List<Vector2> SpawnPositions;
	private bool SpawnBlocks = false;

	public bool GridGenerated = false;

	public float GridWidth {
		get{
			return (GridSizeX - 1) * BlockScale;
		}
	}

	public int MaxGridBlockCount {
		get{
			return GridSizeX * GridSizeY;
		}
	}

	public float HighestBlockYPosition;

	void OnEnable(){
		Events.playerDied += KillGrid;
		Events.gameStarted += EnableSpawnBlocks;
	}

	void OnDisable(){
		Events.playerDied -= KillGrid;
		Events.gameStarted += EnableSpawnBlocks;
	}

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(GenerateGrid ());
		StartCoroutine (RespawnBlocks ());
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (CurrentBlockCount + " " + RespawnQueueBlockCount);
	}

	private void EnableSpawnBlocks(){
		SpawnBlocks = true;
	}
	private void KillGrid(){
		SpawnBlocks = false;
		StartCoroutine (AnimateGridKilling ());
	}

	private IEnumerator AnimateGridKilling(){
		List<BlockController> allBlocks = Blocks.SelectMany (x => x).ToList ();
		while (allBlocks.Count > 0) {
			BlockController block = allBlocks [Random.Range (0, allBlocks.Count - 1)];
			allBlocks.Remove (block);
			RemoveBlock(block);
			yield return new WaitForSeconds (0.01f);
		}
		RespawnQueueBlockCount = 0;
		GridGenerated = false;
		GameManager.Instance.EndGame ();
	}


	public IEnumerator GenerateGrid(bool animate = false){
		Debug.Log ("Generating grid");
		Blocks = new List<List<BlockController>>();
		int startBlockCount = Mathf.RoundToInt(MaxGridBlockCount * StartFillPercentage);

		for (int y = 0; y < GridSizeY; y++){
			Debug.Log ("Generating grid in " + y + " column.");
			for (int x = 0; x < GridSizeX; x++) {
				if (Blocks.Count <= x) {
					Blocks.Add(new List<BlockController> ());
				}
				InstantiateBlock (x, y, true);

				if (animate){
					yield return new WaitForSeconds (0.01f);
				}

				if (CurrentBlockCount >= startBlockCount) {
					HighestBlockYPosition = y * BlockScale;
					for (int i = 0; i < GridSizeX; i++) {
						RefreshColumnDamages (i);
					}
					GameManager.Instance.OnGridGenerated ();
					GridGenerated = true;
					yield break;
				}
			}
		}
	}

	private BlockController InstantiateBlock (int gridXPosition, int gridYPosition, bool initialGeneration = false){
		CurrentBlockCount++;

		BlockController block = Instantiate<GameObject> (BlockPrefab, Vector2.zero, Quaternion.identity, this.transform).GetComponent<BlockController>();
		block.transform.localPosition = new Vector2(gridXPosition, gridYPosition) * BlockScale;
		block.SetRigidbodyKinematic (initialGeneration);
		block.Column = gridXPosition;
		block.CanBeDamaged = true;
		block.IsFalling = !initialGeneration;
		Blocks[gridXPosition].Add(block);
		return block;
	}

	public void RemoveBlock(BlockController block){
		int column = block.Column;
		Blocks[column].Remove (block);
		Destroy (block.gameObject);
		CurrentBlockCount--;
		RespawnQueueBlockCount++;

		RefreshColumnDamages (column);
	}

	public void RefreshColumnDamages(int column){
		List<BlockController> columnBlocks = Blocks [column];
		if (columnBlocks.Count > 0) {
			foreach (BlockController block in columnBlocks) {
				block.CanBeDamaged = false;
			}
			for (int i = columnBlocks.Count - 1; i >= 0; i--) {
				columnBlocks [i].CanBeDamaged = true;
				if (!columnBlocks [i].IsFalling) {
					break;
				}
			}
		}
	}
		
	private IEnumerator RespawnBlocks(){
		while (true) {
			if (SpawnBlocks && RespawnQueueBlockCount > 0) {
				List<int> availableColumns = GetAvailableSpawnColumns ();
				if (availableColumns.Count > 0) {
					int column = SelectRandomColumn (availableColumns);
					if (column >= 0) {
						InstantiateBlock (column, SpawnHeight);
						RespawnQueueBlockCount--;
					}
				}
			}
			yield return null;
		}
	}

	private List<int> GetAvailableSpawnColumns(){
		List<int> availableColumns = new List<int>();
		//Make a list of available spawn positions (those that are not full)
		for (int i = 0; i < Blocks.Count; i++) {
			if (!IsColumnFull(i) && IsSpawnPointEmpty(i)) {
				availableColumns.Add (i);
			} else {
				Debug.Log ("Unavailable column: " + i);
			}
		}
		return availableColumns;
	}

	private int SelectRandomColumn(List<int> availableColumns){
		int randomNumber = Random.Range (0, (availableColumns.Count*GridSizeY) - CountBlocksInColumns (availableColumns));
		for (int i = 0; i < availableColumns.Count; i++){
			randomNumber -= GridSizeY - CountBlocksInColumn(availableColumns[i]);
			if (randomNumber <= 0){
				return availableColumns[i];
			}
		}
		return -1;
	}

	private int CountBlocksInColumns(List<int> columns){
		int blockCount = 0;
		foreach (int i in columns) {
			blockCount += Blocks [i].Count;
		}
		return blockCount;
	}

	private int CountBlocksInColumn(int column){
		return Blocks [column].Count;
	}

	private bool IsColumnEmpty(int column){
		return CountBlocksInColumn (column) <= 0;
	}

	private bool IsColumnFull(int column){
		return CountBlocksInColumn(column) >= GridSizeY;
	}

	private bool IsSpawnPointEmpty(int column){
		if (CountBlocksInColumn (column) > 0) {
			return Blocks [column] [CountBlocksInColumn (column) - 1].gameObject.transform.position.y < (SpawnHeight - 1) * BlockScale;
		}
		return true;
	}

	public Vector2 GetNearestPointOnGrid(Vector2 startLocation){
		return new Vector2(RoundToMultiplier(startLocation.x, BlockScale), RoundToMultiplier(startLocation.y, BlockScale));
	}

	private float RoundToMultiplier(float number, float multiplier){
		return Mathf.RoundToInt (number / multiplier) * multiplier;
	}
}
