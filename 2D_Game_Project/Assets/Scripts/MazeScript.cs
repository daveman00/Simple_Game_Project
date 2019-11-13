using UnityEngine;
using Random = UnityEngine.Random;

public class MazeScript: MonoBehaviour
{
	public bool[] mazeGrid;
	public int nrOfColumns;
	public int nrOfRows;
	public GameObject[] wallTiles;
	public GameObject[] pathTiles;
	private Vector3 sizeOfTile;
	private Transform mazeHolder = null;

	private void InitialiseMaze(MazeGenerator mazeGenerator, int level)
	{
		mazeGrid = mazeGenerator.GenerateMaze(level);
		nrOfColumns = mazeGenerator.GetColumns();
		nrOfRows = mazeGenerator.GetRows();
		sizeOfTile = pathTiles[0].GetComponent<Renderer>().bounds.size;
	}
	
	public void SetupMaze(MazeGenerator mazeGenerator, int level)
	{
		InitialiseMaze(mazeGenerator, level);
		if ( mazeHolder != null) DestroyMaze();
		mazeHolder = new GameObject("Maze").transform;

		for (int x = -1; x < nrOfColumns + 1; x++)
			for (int y = -1; y < nrOfRows + 1; y++)
			{
				GameObject toInstantiate;
				if ( x > -1 && x < nrOfColumns && y > -1 && y < nrOfRows && mazeGrid[y * nrOfColumns + x])
				{
					toInstantiate = pathTiles[Random.Range(0, pathTiles.Length)];
				}
				else
				{
					toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
				}
				GameObject instance = Instantiate(toInstantiate, Vector3.Scale(new Vector3(x, y, 0f), sizeOfTile), Quaternion.identity) as GameObject;
				instance.transform.SetParent(mazeHolder);
			}
	}

	private void DestroyMaze()
	{
		GameObject maze = GameObject.Find("Maze");
		foreach (Transform child in maze.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
		GameObject.Destroy(maze.gameObject);
		maze = GameObject.FindWithTag("Exit");
		GameObject.Destroy(maze.gameObject);
		mazeHolder = null;
	}

	public Vector2 GetSizeOfTile()
	{
		return sizeOfTile;
	}

}

