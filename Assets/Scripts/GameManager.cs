using UnityEngine;
using System.Collections;
using Assets.Scripts.Board;
using Assets.Scripts.Model;
using Assets.Scripts.Util;

public enum StartPosition { MIN, MAX}

public class GameManager : MonoBehaviour {

    public Player playerPrefab;

    public Maze mazePrefab;

    public int indestructibleCubesXNumber;

    public int indestructibleCubesZNumber;

    public float cellSize;

    public float cubeHeight;

    public float wallHeight;

    public StartPosition startPositionX;

    public StartPosition startPositionZ;

    /*=================================*/

    private Maze mazeInstance;

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
        mazeInstance = Instantiate(mazePrefab);
        float mazeWidth = indestructibleCubesXNumber * cellSize * 2 + cellSize;
        float mazeLength = indestructibleCubesZNumber * cellSize * 2 + cellSize;
        float startX = startPositionX.Equals(StartPosition.MIN) ? 1 : mazeWidth - 1;
        float startZ = startPositionZ.Equals(StartPosition.MIN) ? 1 : mazeLength - 1;
        board = mazeInstance.Generate(mazeWidth, mazeLength, cellSize, cubeHeight, wallHeight, startX, startZ, positionConverter);

        Player player = Instantiate(playerPrefab);
        player.transform.localPosition = new Vector3(startX, 0.5f, startZ);
        if (startPositionZ.Equals(StartPosition.MAX))
        {
            player.transform.Rotate(new Vector3(0, 180, 0));
        }

        board.AddPlayer(player);
    }

    private void RestartGame() { }
}