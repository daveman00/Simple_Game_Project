using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;

// ArrayExtensions - class with function Fill implementing filling of an array
public static class ArrayExtensions {
    public static void Fill<T>(this T[] originalArray, T with) {
        for(int i = 0; i < originalArray.Length; i++){
            originalArray[i] = with;
        }
    }  
}

// Direction - structure used for orientation in array, with index, colum, row indicators + constructor
public struct Direction
{
	public char direction;
	public int index;
	public int column;
	public int row;

	public Direction(char dir, int Index, int x, int y)
	{
		direction = dir;
		index = Index;
		column = x;
		row = y;
	}
}

// MazeGenerator - class handling the generation of the maze with the Hunt and Kill algorithm
public class MazeGenerator : MonoBehaviour 
{
	private const int DEFAULT_SIZE = 8;
	private const int STEP = 2;		// step size comes from the design of the grid
	private int nrOfColumns; 		// width of row
	private int nrOfRows;			// height of column

	private bool[] gridFrame; 		// array holding the frame of the maze
	private int verticalStep; 			// step used for vertical movement 
	private int horizontalStep; 	    // step used for horizontal movement

	private char[] allDirections = new char[] {'N','S','W','E'};  // array holding available directions, possible extension

	private List<Vector2> deadEnds = new List<Vector2>();	// list of dead ends in the maze with their coordinates
	public Vector2 startingPoint = new Vector2();

	// InitialiseGenerator - initialising function, argument influences the size of the maze in logarithmic way
	private void InitialiseGenerator(int level)
	{
		int size = CalculateSize(level);
		nrOfColumns = size;
		nrOfRows = size;
		gridFrame = InitialiseGrid(nrOfColumns, nrOfRows, true);
		horizontalStep = STEP;
		verticalStep = STEP * nrOfColumns;	
	}

	private int CalculateSize(int level)
	{
		int size = (int)(DEFAULT_SIZE * Mathf.Log(level));
		if (size < DEFAULT_SIZE) size = DEFAULT_SIZE;
		if (size % 2 == 0) size -= 1;
		return size;
	}

	// InitialiseGrid - constructs bool array initialised with 'state's sourounded
	// by their negation to make a bool grid 
	// Visualisation example: InitialiseGrid(5, 5, 1);
	// 		10101
	// 		00000
	// 		10101
	//		00000
	// 		10101

	private bool[] InitialiseGrid(int columns, int rows, bool state)
	{
		bool[] gridToInitialise = new bool[columns * rows];
		gridToInitialise.Fill(!state);
		for (int i = 0; i < columns; i += 2)
		{
			for (int j = 0; j < rows; j += 2)
			{
				gridToInitialise[i * rows + j] = state;
			}
		}
		return gridToInitialise;
	}

	// ChooseStartingPoint - function returns random position(x,y) in which the generation of the maze will be started
	private Vector2 ChooseStartingPoint()
	{
		int randomColumn = Random.Range(-1, nrOfColumns - 1);
		int randomRow = Random.Range(-1, nrOfRows - 1);
		if (randomColumn % 2 != 0) randomColumn++;
		if (randomRow % 2 != 0) randomRow++;
		return new Vector2(randomColumn, randomRow);
	}

	// CheckStartingPointDeadEnd - function checks whether the chosen starting point of maze generation is an actual dead end
	private void CheckStartingPointDeadEnd()
	{
		List<Direction> availableDirections = AvailableDirections((int)startingPoint.x, (int)startingPoint.y, gridFrame, nrOfColumns, nrOfRows, true);
		int row = (int)startingPoint.y;
		int column = (int)startingPoint.x;
		int originalIndex =  row * nrOfColumns + column;
		int nrOfRoads = 0;
		foreach (Direction direction in availableDirections)
		{
			int index = originalIndex;
			switch (direction.direction)
			{
				case 'N':
				{
					index += nrOfColumns;
					break;
				}
				case 'S':
				{
					index -= nrOfColumns; 
					break;
				}
				case 'W':
				{
					index -= 1;
					break;
				}
				case 'E':
				{
					index += 1;
					break;
				}
				default: break;
			}
			if (gridFrame[index])
						nrOfRoads++;
		}
		if (nrOfRoads == 1) deadEnds.Add(startingPoint);
	}

	// AvailableDirections - function returns list of available directions from current cell to neighbours based on the stateOfMapOfCells argument; 
	// bool stateOfMapOfCells: false if looking for unvisited cells, true if looking for visited cells
	private List<Direction> AvailableDirections(int column, int row, bool[] mapOfVisitedCells, int rowSize, int columnSize, bool stateOfMapOfCells)
	{
		int originalIndex = row * rowSize + column;
		List<Direction> availableDirections = new List<Direction>();
		foreach (char direction in allDirections)
		{
			int index = originalIndex;
			switch (direction)
			{
				case 'N':
				{
					index += verticalStep;
					if ( row < columnSize - 1 && mapOfVisitedCells[index] == stateOfMapOfCells)
						availableDirections.Add(new Direction('N', index, column, row + STEP));
					break;
				}
				case 'S':
				{
					index -= verticalStep; 
					if ( row > 0 && mapOfVisitedCells[index] == stateOfMapOfCells)
						availableDirections.Add(new Direction('S', index, column, row - STEP));
					break;
				}
				case 'W':
				{
					index -= horizontalStep;
					if ( column > 0 && mapOfVisitedCells[index] == stateOfMapOfCells)
						availableDirections.Add(new Direction('W', index, column - STEP, row));
					break;
				}
				case 'E':
				{
					index += horizontalStep;
					if ( column < rowSize - 1 && mapOfVisitedCells[index] == stateOfMapOfCells)
						availableDirections.Add(new Direction('E', index, column + STEP, row));
					break;
				}
				default: break;
			}
		}
		return availableDirections;
	}

	// ChooseRandomDirection - function returns randomly chosen Direction from List<Direction>
	private Direction ChooseRandomDirection(List<Direction> availableDirections)
	{
		int randomIndex = Random.Range(0, availableDirections.Count);
		return availableDirections[randomIndex];
	}
	
	// CarvePassage - function handles carving of new passages from current cell in the given direction, 
	// returns the cell that the passage was created to as new current cell, updates both mapOfVisitedCells and gridFrame through reference
	private Vector2 CarvePassage(Vector2 current, Direction direction, ref bool[] mapOfVisitedCells, ref bool[] gridFrame, int rowSize)
	{ 
		int index = (int)current.y * rowSize + (int)current.x;
		switch (direction.direction)
		{
			case 'N':
			{
				index += rowSize; 
				break;
			}
			case 'S':
			{
				index -= rowSize;
				break;
			}
			case 'W':
			{
				index -= 1;
				break;

			}
			case 'E':
			{
				index += 1;
				break;
			}
			default: break;
		}
		gridFrame[index] = true;
		mapOfVisitedCells[direction.index] = true;
		current.x = direction.column;
		current.y = direction.row;

		return current;
	}

	// HuntVisitedCellWithUnvisitedNeighbour - function is called when reaching dead end in carving, searches for unvisited cell with visited neighbour 
	// to continue the carving; 
	// returns the found unvisited cell as Vector2
	private Vector2 HuntVisitedCellWithUnvisitedNeighbour(Vector2 current, bool[] mapOfVisitedCells, int rowSize, int columnSize)
	{
		List<Direction> availableDirections = new List<Direction>();
		int index = 0;
		for (int x = 0; x < rowSize; x += 2)
		{
			for (int y = 0; y < columnSize; y += 2)
			{
				index = y * rowSize + x;
				if (!mapOfVisitedCells[index])
				{
					availableDirections = AvailableDirections(x, y, mapOfVisitedCells, rowSize, columnSize, true); 
					if (availableDirections.Count > 0) 
					{ 
						Direction randomDirection = ChooseRandomDirection(availableDirections);
						return new Vector2(randomDirection.column, randomDirection.row);
					}
					 
				}

			}
		}
		return current;
	}

	// HuntAndKillAlgorithm - final implementation of the algorithm; updates the gridFrame and deadEnds through reference; 
	// ends when all cells in mapOfVisitedCells has been visited
	private void HuntAndKillAlgorithm(ref bool[] gridFrame, ref List<Vector2> deadEnds)
	{
		deadEnds.Clear();
		bool mazeGenerated = false;
		bool[] mapOfVisitedCells = InitialiseGrid(nrOfColumns, nrOfRows, false);
		Vector2 current = ChooseStartingPoint();
		startingPoint = current;
		mapOfVisitedCells[(int)current.y * nrOfColumns + (int)current.x] = true;
		List<Direction> availableDirections = new List<Direction>();
		do
		{
			availableDirections = AvailableDirections((int)current.x, (int)current.y, mapOfVisitedCells, nrOfColumns, nrOfRows, false);
			if (availableDirections.Count > 0)
			{
				Direction direction = ChooseRandomDirection(availableDirections);
				current = CarvePassage(current, direction, ref mapOfVisitedCells, ref gridFrame, nrOfColumns);
			}
			else
			{
				deadEnds.Add(current);
				current = HuntVisitedCellWithUnvisitedNeighbour(current, mapOfVisitedCells, nrOfColumns, nrOfRows);
			}
			availableDirections.Clear();
			mazeGenerated = mapOfVisitedCells.All(x => x);
		} while (!mazeGenerated);

		deadEnds.Add(current);
		CheckStartingPointDeadEnd();
	}

	// GenerateMaze - function called from outside calling internal functions and returning the maze as bool array;
	// int level argument influences the size of maze in logarithmic way
	public bool[] GenerateMaze(int level)
	{
		InitialiseGenerator(level);
		HuntAndKillAlgorithm(ref gridFrame, ref deadEnds);
		return gridFrame;
	}

	//GetDeadEnds - getter for dead ends of the labyrinth
	public List<Vector2> GetDeadEnds()
	{
		return deadEnds;
	}

	// GetColumns - getter for nr of columns
	public int GetColumns()
	{
		return nrOfColumns;
	}

	// GetRows - getter for nr of rows
	public int GetRows()
	{
		return nrOfRows;
	}

}
