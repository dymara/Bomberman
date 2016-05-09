using Assets.Scripts.Model;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

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
        player.tag = Constants.HUMAN_PLAYER_TAG;
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

    public void SetCameraRotation(Vector3 rotation)
    {
        FirstPersonController fpsController = player.GetComponent<FirstPersonController>();
        fpsController.SetCameraRotation(rotation);
    }

    public void AdvanceToNextLevel()
    {
        levelNumber++;
        SwitchGameState(GameState.GAMEPLAY);
    }

    public float GetSplashDuration()
    {
        return configurator.splashDuration;
    }

    public float GetExitSpinSpeed()
    {
        return configurator.exitSpinSpeed;
    }

    public int GetBombDetonateDelay()
    {
        return configurator.bombDetonateDelay;
    }

    public float GetLevelDuration()
    {
        return GetCubesXCount() * GetCubesZCount() * configurator.levelDurationPerBlock;
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

    /* HUD AUTO-UPDATE METHODS */

    private UIController GetUIController()
    {
        GameObject controllerObject = GameObject.Find(Constants.UI_CONTROLLER_NAME);
        if (controllerObject != null)
        {
            return controllerObject.GetComponent<UIController>();
        }
        return null;
    }

    public void OnPlayerBombsChanged(int bombs)
    {
        UIController uiController = GetUIController();
        if (uiController != null) {
            uiController.SetBombsCount(bombs);
        }
    }

    public void OnPlayerBombRangeChanged(int bombRange)
    {
        UIController uiController = GetUIController();
        if (uiController != null)
        {
            uiController.SetRangeBonusValue(bombRange);
        }
    }

    public void OnPlayerSpeedChanged(float speed)
    {
        UIController uiController = GetUIController();
        if (uiController != null)
        {
            uiController.SetSpeedBonusValue(speed);
        }
    }

    public void OnPlayerRemoteDetonationBonusChanged(bool available)
    {
        UIController uiController = GetUIController();
        if (uiController != null)
        {
            uiController.SetRemoteDetonationBonusAvailable(available);
        }
    }

    public void OnPlayerScoreChanged(long score)
    {
        UIController uiController = GetUIController();
        if (uiController != null)
        {
            uiController.SetScoreValue(score);
        }
    }

    public void OnPlayerLivesChanged(int lives)
    {
        UIController uiController = GetUIController();
        if (uiController != null)
        {
            uiController.SetLivesCount(lives);
        }
    }


}
