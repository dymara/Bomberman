using Assets.Scripts.Model;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Configurator configurator;

    public Player playerPrefab;

    public static GameManager instance = null;

    private GameState state;

    private int levelNumber;

    private Player player;

    // Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);  // Sets this to not be destroyed when reloading scene

        Init();

        SwitchGameState(configurator.initialApplicationState);
    }

    private void Init()
    {
        levelNumber = configurator.initialLevelNumber;
        player = Instantiate(playerPrefab) as Player;
        player.name = "Player";
        player.remainingLives = configurator.initialPlayerLives;
        player.bombs = configurator.initialPlayerBombs;
        player.bombRange = configurator.initialPlayerBombRange;
        player.speed = configurator.initialPlayerSpeed;
        player.remoteDetonationBonus = configurator.initialPlayerRemoteDetonationBonus;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /****************************************/
    /*          Public API Methods          */
    /****************************************/

    public void SwitchGameState(GameState state)
    {
        this.state = state;
        switch (state)
        {
            case GameState.SPLASH:
                SceneFader.LoadScene("Splash", 0.5f, 1);
                // Hack - fadeOut time must be greater than zero. Otherwise animation framerate decreases significantly.
                break;
            case GameState.MAIN_MENU:
                SceneFader.LoadScene("Menu", 1, 0);
                break;
            case GameState.SCORE_BOARD:
                SceneFader.LoadScene("ScoreBoard", 1, 0);
                break;
            case GameState.GAMEPLAY:
                SceneFader.LoadScene("Gameplay", 0, 1);
                break;
            default:
                break;
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void AdvanceToNextLevel()
    {
        levelNumber++;
        if (levelNumber <= Configurator.MAXIMUM_LEVEL_NUMBER)
        {
            SwitchGameState(GameState.GAMEPLAY);
        }
        else
        {
            // WORLD EXPLODES HERE!
            // TODO implement this extra ordinary case
        }
    }

    public float GetSplashDuration()
    {
        return configurator.splashDuration;
    }

    public float GetExitSpinSpeed()
    {
        return configurator.exitSpinSpeed;
    }

    public float GetFindingtSpinSpeed()
    {
        return configurator.findingSpinSpeed;
    }

    public float GetFindingtFloatSpeed()
    {
        return configurator.findingFloatSpeed;
    }

    public float GetFindingtFloatDistance()
    {
        return configurator.findingFloatDist;
    }

    public float GetFindingtCount()
    {
        return configurator.findingCount;
    }

    public int GetBombDetonateDelay()
    {
        return configurator.bombDetonateDelay;
    }

    public int GetEnemiesCount()
    {
        // TODO This method's result should be dependent on current level number value!
        return configurator.level1EnemiesCount;
    }

    public int GetExitExplosionEnemiesCount()
    {
        return configurator.exitExplosionEnemiesCount;
    }

    public int GetEnemiesMinimumDistance()
    {
        return configurator.enemiesMinDistance;
    }

    public int GetCubesXCount()
    {
        // TODO This method's result should be dependent on current level number value!
        return configurator.level1CubesXCount;
    }

    public int GetCubesZCount()
    {
        // TODO This method's result should be dependent on current level number value!
        return configurator.level1CubesZCount;
    }

    public float GetCellSize()
    {
        return configurator.cellSize;
    }

    public float GetCubeHeight()
    {
        return configurator.cubeHeight;
    }

    public float GetWallHeight()
    {
        return configurator.wallHeight;
    }

    public StartPosition GetStartXPosition()
    {
        return configurator.startPositionX;
    }

    public StartPosition GetStartZPosition()
    {
        return configurator.startPositionZ;
    }

}
