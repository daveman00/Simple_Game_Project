using UnityEngine;
//using System.Collections;

public class GameManager : MonoBehaviour 
{
	public int level = 1;
	public float levelStartDelay = 2f;
	public static GameManager instance = null;
	public GameObject player;
	public GameObject boardManager;
	public GameObject enemyManager;
	public GameObject lootManager;
	private BoardManager boardScript;
	private EnemyManager enemyScript;
	private LootManager lootScript;
	private bool doingSetup;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if ( instance != this)
		{
			Destroy(gameObject);
		}
		
		DontDestroyOnLoad(gameObject);
		InitialiseGameManager();
	}

	void InitialiseGameManager()
	{
		Vector3 initVector = new Vector3(0f, 0f, 0f);
		boardScript = boardManager.GetComponent<BoardManager>();
		boardManager = Instantiate(boardManager, initVector, Quaternion.identity) as GameObject;
		boardManager.transform.SetParent(gameObject.transform);
		enemyScript = enemyManager.GetComponent<EnemyManager>();
		enemyManager = Instantiate(enemyManager, initVector, Quaternion.identity) as GameObject;
		enemyManager.transform.SetParent(gameObject.transform);
		lootScript = lootManager.GetComponent<LootManager>();
		lootManager = Instantiate(lootManager, initVector, Quaternion.identity) as GameObject;
		lootManager.transform.SetParent(gameObject.transform);
		player = GameObject.Find("Player");
	}

	private void OnLevelWasLoaded(int index)
	{
		level++;
		InitGame();
	}

	// should be differentiation between the very first initialisation and the next ones?
	void InitGame()
	{
		doingSetup = true;
		SetupBoard();
		SpawnPlayer();

		//Spawn enemies, spawn loot, etc; GUI;
		doingSetup = false;
	}

	void SetupBoard()
	{
		boardScript.SetLevel(level);
		boardScript.SetupBoard();
	}

	void SpawnPlayer()
	{
		int randomIndex = BoardManager.GetRandomIndex <Vector2>(boardScript.deadEnds);
		player.transform.position = (Vector3)boardScript.deadEnds[randomIndex];
		boardScript.deadEnds.RemoveAt(randomIndex);
	}

	void Start()
	{
		InitGame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
