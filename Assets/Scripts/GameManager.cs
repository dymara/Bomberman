using UnityEngine;
using System.Collections;
using Assets.Scripts.Board;
using Assets.Scripts.Model;
using Assets.Scripts.Util;

public class GameManager : MonoBehaviour {

    public Maze maze;

    private Maze mazeInstance;

    public int indestructibleCubesXNumber;

    public int indestructibleCubesZNumber;

    public float cellSize;

    public float cubeHeight;

    public float wallHeight;

    public float startPositionX;

    public float startPositionZ;

    private Board board;

    private PositionConverter positionConverter;

    // Use this for initialization
    void Start () {
        BeginGame();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void BeginGame()
    {
        positionConverter = new PositionConverter(cellSize);
        mazeInstance = Instantiate(maze);
        float mazeWidth = indestructibleCubesXNumber * cellSize * 2 + cellSize;
        float mazeLength = indestructibleCubesZNumber * cellSize * 2 + cellSize;
        board = mazeInstance.Generate(mazeWidth, mazeLength, cellSize, cubeHeight, wallHeight, startPositionX, startPositionZ, positionConverter);

 /*       for(int i = 0; i < board.GetSize().x; i++)
        {
            for(int j = 0; j < board.GetSize().y; j++)
            {
                GameCell cell = board.GetGameCell(i, j);
                Debug.Log(i + " " + j + " " + cell.block + " " + cell.finding);
            }
        }*/
    }

    private void RestartGame() { }
}