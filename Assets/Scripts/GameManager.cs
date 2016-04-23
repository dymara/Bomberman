using UnityEngine;
using Assets.Scripts.Board;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using System.Collections.Generic;

public enum StartPosition { MIN, MAX }

public class GameManager : MonoBehaviour {

    public Player playerPrefab;

    public Maze mazePrefab;

    public Bomb bombPrefab;

    /** Number of indestructible cubes in x axis */
    [Range(4, 128)]
    public int indestructibleCubesXNumber;

    /** Number of indestructible cubes in z axis */
    [Range(4, 128)]
    public int indestructibleCubesZNumber;

    /** Cell size -> cube length and width */
    public float cellSize;

    /** Cube height. */
    public float cubeHeight;

    /** Wall height.*/
    public float wallHeight;

    /** Player start position in x axis - MIN -> 1, MAX -> width - 1 */
    public StartPosition startPositionX;

    /** Player start position in z axis - MIN -> 1, MAX -> length - 1 */
    public StartPosition startPositionZ;

    /*=================================*/

    private Maze mazeInstance;

    private Board board;

    private PositionConverter positionConverter;

    private ExplosionManager explosionManager;

    private UIController uiController;
    
    private Player player;
    
    public GameObject monsterPrefab;
   
    // Use this for initialization
    void Start() {
        BeginGame();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("f")) {
            Vector3 sceneBombPosition = board.GetPlayers()[0].transform.position + board.GetPlayers()[0].transform.forward;
            Vector2 boardBombPosition = positionConverter.ConvertScenePositionToBoard(sceneBombPosition);
            explosionManager.PutBomb(bombPrefab, boardBombPosition);
        }
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

        player = Instantiate(playerPrefab);
        player.transform.localPosition = new Vector3(startX, 0.5f, startZ);
        if (startPositionZ.Equals(StartPosition.MAX))
        {
            player.transform.Rotate(new Vector3(0, 180, 0));
        }

        board.AddPlayer(player);
        
        InitAI();

        explosionManager = GameObject.Find("ExplosionManager").GetComponent<ExplosionManager>();
        uiController = GameObject.Find("UIController").GetComponent<UIController>();
    }
    
    private void InitAI() 
    {
        Vector2 playerPosition = positionConverter.ConvertScenePositionToBoard(player.transform.position);
        Debug.Log(playerPosition);
        List<GameCell> freeCells = board.GetFreeCellsAtMinDistance(playerPosition, 8);
        foreach (GameCell gameCell in freeCells) 
        {
            Debug.Log(gameCell.GetCoordinates());
            Instantiate(monsterPrefab, positionConverter.ConvertBoardPositionToScene(gameCell.GetCoordinates(), true), Quaternion.identity);
        }
    }

    private void RestartGame() { }

    /* GETTER METHODS */

    public PositionConverter GetPositionConverter()
    {
        return positionConverter;
    }

    public Board GetBoard()
    {
        return board;
    }

    public UIController GetUIController()
    {
        return uiController;
    }
}
