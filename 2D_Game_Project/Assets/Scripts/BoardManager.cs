using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour 
{	
	public int level = 1;
	public static BoardManager instance = null;
	public MazeGenerator mazeGenerator;
	public MazeScript generatedMaze;
	public List<Vector2> deadEnds;
	public List<GameObject> exits;
	public Vector2 sizeOfTile;

	private void InitialiseBoardManager()
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
		mazeGenerator = GetComponent<MazeGenerator>();
		generatedMaze = GetComponent<MazeScript>();
	}

	public void SetupBoard()
	{
		InitialiseBoardManager();
		generatedMaze.SetupMaze(mazeGenerator, level);
		deadEnds = mazeGenerator.GetDeadEnds();
		sizeOfTile = generatedMaze.GetSizeOfTile();
		ScaleDeadEnds();
		SpawnExit();
	}

	public static int GetRandomIndex <T> (List<T> list)
	{
		int index = Random.Range(0, list.Count);
		return index;
	}
	
	void SpawnExit()
	{	
		int exitsIndex = GetRandomIndex <GameObject> (exits);
		int deadEndsIndex = GetRandomIndex <Vector2> (deadEnds);
		Instantiate(exits[exitsIndex], (Vector3)deadEnds[deadEndsIndex], Quaternion.identity);
		deadEnds.RemoveAt(deadEndsIndex);
	}

	void ScaleDeadEnds()
	{
		for (int i = 0; i < deadEnds.Count; i++)
		{
			deadEnds[i] = Vector2.Scale(deadEnds[i], (Vector2)sizeOfTile);
		}
	}
	// Update is called once per frame
	// void Update () {
	
	// }

	public void SetLevel(int level)
	{
		this.level = level;
	}
}
