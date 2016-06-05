using Assets.Scripts.Model;
using System;
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

        this.player = Instantiate(playerPrefab) as Player;
        Init();

        SwitchGameState(configurator.initialApplicationState);
    }

    private void Init()
    {
        levelNumber = configurator.initialLevelNumber;
        player.name = "Player";
        player.tag = Constants.HUMAN_PLAYER_TAG;
        player.remainingLives = configurator.initialPlayerLives;
        player.score = 0;
        player.bombs = configurator.initialPlayerBombs;
        player.maximumBombsCount = configurator.initialPlayerBombs;
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
                SceneFader.LoadScene("Menu", 1, 1);
                break;
            case GameState.SCORE_BOARD:
                SceneFader.LoadScene("ScoreBoard", 1, 0);
                break;
            case GameState.GAMEPLAY:
                Debug.Log(DateTime.Now + " Loading level " + levelNumber + "...");
                SceneFader.LoadScene("Gameplay", 1, 1);
                player.bombs = player.maximumBombsCount;
                Debug.Log(DateTime.Now + " Level " + levelNumber + " loaded successfully!");
                break;
            default:
                break;
        }
    }

    public void SetCameraMenuMode(bool setMenuMode)
    {
        // disable FPS Camera, MouseLook and enable cursor
        GetPlayer().gameObject.transform.FindChild("FirstPersonCharacter").gameObject.SetActive(!setMenuMode);
        GetPlayer().gameObject.SetActive(!setMenuMode);
        Cursor.lockState = setMenuMode ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = setMenuMode;
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

    public void RestartLevel()
    {
        SwitchGameState(GameState.GAMEPLAY);
    }

    public void ResetPlayerState()
    {
        Init();
    }

    public int GetCurrentLevelNumber()
    {
        return levelNumber;
    }

    public float GetSplashDuration()
    {
        return configurator.splashDuration;
    }

    public float GetExitSpinSpeed()
    {
        return configurator.exitSpinSpeed;
    }

    public float GetFindingSpinSpeed()
    {
        return configurator.findingSpinSpeed;
    }

    public float GetFindingFloatSpeed()
    {
        return configurator.findingFloatSpeed;
    }

    public float GetFindingFloatDistance()
    {
        return configurator.findingFloatDist;
    }

    public float GetExtraBombFindingsCount()
    {
        //TODO uzależnić od poziomu i ilości bomb u gracza
        return configurator.initialExtraBombFindings; ;
    }

    public float GetExtraLiveFindingsCount()
    {
        //TODO uzależnić od poziomu i ilości żyć u gracza
        return configurator.initialExtraLiveFindings;
    }

    public float GetRangeBombFindingsCount()
    {
        //TODO uzależnić od poziomu
        return configurator.initialRangeBombFindings;
    }

    public float GetFasterMovingFindingsCount()
    {
        //TODO uzależnić od poziomu
        return configurator.initialFasterMovingFindings;
    }

    public float GetRemoteDetonationFindingsCount()
    {
        //TODO uzależnić od poziomu
        return configurator.initialRemoteBombFindings;
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

    public bool IsWatermarkEnabled()
    {
        return configurator.displayWatermark;
    }

    public float GetSpeedMultiplier()
    {
        return configurator.speedMultiplier;
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

    /* SCORE UPDATE METHODS */

    public void OnBlockDestroyed()
    {
        player.score += configurator.blockDestructionPoints;
    }

    public void OnFindingPickedUp()
    {
        player.score += configurator.findingPickupPoints;
    }

    public void OnEnemyKilled()
    {
        player.score += configurator.enemyKillingPoints;
    }

    public void OnLevelCleared(int timeLeft)
    {
        int levelBonus = levelNumber * configurator.levelClearedPoints;
        int timeBonus = timeLeft * configurator.timeMultiplierPoints;
        player.score += levelBonus + timeBonus;
    }

}
