using UnityEngine;
using Assets.Scripts.Board;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using Assets.Scripts.Postion;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public Maze mazePrefab;

    public Bomb bombPrefab;

    public GameObject monsterPrefab;

    public AISpawner aiSpawner;

    /*=================================*/

    /** Number of indestructible cubes in x axis */
    private int indestructibleCubesXNumber;

    /** Number of indestructible cubes in z axis */
    private int indestructibleCubesZNumber;

    /** Cell size -> cube length and width */
    private float cellSize;

    /** Cube height. */
    private float cubeHeight;

    /** Wall height.*/
    private float wallHeight;

    /** Player start position in x axis - MIN -> 1, MAX -> width - 1 */
    private StartPosition startPositionX;

    /** Player start position in z axis - MIN -> 1, MAX -> length - 1 */
    private StartPosition startPositionZ;

    /*=================================*/

    private Maze mazeInstance;

    private Board board;
    
    private Player player;

    private PositionConverter positionConverter;

    private ExplosionManager explosionManager;

    private UIController uiController;

    private PlayerPositionManager positionManager;

    private int levelTimeLeft;

    void Awake() {
        this.indestructibleCubesXNumber = GameManager.instance.GetCubesXCount();
        this.indestructibleCubesZNumber = GameManager.instance.GetCubesZCount();
        this.cellSize = GameManager.instance.GetCellSize();
        this.cubeHeight = GameManager.instance.GetCubeHeight();
        this.wallHeight = GameManager.instance.GetWallHeight();
        this.startPositionX = GameManager.instance.GetStartXPosition();
        this.startPositionZ = GameManager.instance.GetStartZPosition();
        this.player = GameManager.instance.GetPlayer();
    }
   
    // Use this for initialization
    void Start() {
        BeginGame();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("f")) {
            Vector3 sceneBombPosition = board.GetPlayers()[0].transform.position + board.GetPlayers()[0].transform.forward;
            Vector2 boardBombPosition = positionConverter.ConvertScenePositionToBoard(sceneBombPosition);
            explosionManager.PutBomb(player, bombPrefab, boardBombPosition);
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

        positionManager = new PlayerPositionManager(board, positionConverter);

        player.gameObject.transform.localPosition = new Vector3(startX, 0.5f, startZ);
        SetInitialCameraRotation();

        board.AddPlayer(player);
        
        InitAI();

        positionManager.AddPlayer(player);

        explosionManager = GameObject.Find(Constants.EXPLOSION_MANAGER_NAME).GetComponent<ExplosionManager>();
        uiController = GameObject.Find(Constants.UI_CONTROLLER_NAME).GetComponent<UIController>();
        uiController.InitializeHUD();

        StartCoroutine(StartLevelCountdown());
    }
    
    private void InitAI() 
    {
        aiSpawner.SetPostitionManager(positionManager);
        Vector2 playerPosition = positionConverter.ConvertScenePositionToBoard(player.gameObject.transform.position);
        aiSpawner.SpawnEnemies(board, positionConverter, playerPosition);
    }

    private void SetInitialCameraRotation()
    {
        if (startPositionX == StartPosition.MIN && startPositionZ == StartPosition.MIN)
        {
            GameManager.instance.SetCameraRotation(new Vector3(0, 0, 0));
        }
        else if (startPositionX == StartPosition.MIN && startPositionZ == StartPosition.MAX)
        {
            GameManager.instance.SetCameraRotation(new Vector3(0, 90, 0));
        }
        else if (startPositionX == StartPosition.MAX && startPositionZ == StartPosition.MIN)
        {
            GameManager.instance.SetCameraRotation(new Vector3(0, 0, 0));
        }
        else if (startPositionX == StartPosition.MAX && startPositionZ == StartPosition.MAX)
        {
            GameManager.instance.SetCameraRotation(new Vector3(0, 270, 0));
        }
    }

    private IEnumerator StartLevelCountdown()
    {
        levelTimeLeft = (int)Mathf.Ceil(GameManager.instance.GetLevelDuration());
        while (levelTimeLeft > 0)
        {
            uiController.SetTimerValue(levelTimeLeft--);
            yield return new WaitForSeconds(1);
        }
        Debug.Log(DateTime.Now + " Out of time! Level has not been cleared before countdown finished.");
        player.TriggerKill();
    }

    /* GETTER METHODS */

    public int GetCountdownValue()
    {
        return levelTimeLeft;
    }

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
