using Assets.Scripts.Model;
using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {

    public Configurator configurator;

    public Player playerPrefab;

    public static GameManager instance = null;

    private GameState state;

    private int levelNumber;

    private Player player;

    private LevelScore levelScore;

    private bool levelCleared;

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
        levelScore = new LevelScore();
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
                ScreenFader.LoadScene("Splash", 0.5f, 1);
                // Hack - fadeOut time must be greater than zero. Otherwise animation framerate decreases significantly.
                break;
            case GameState.MAIN_MENU:
                ScreenFader.LoadScene("Menu", 1, 1);
                break;
            case GameState.SCORE_BOARD:
                ScreenFader.LoadScene("ScoreBoard", 1, 1);
                break;
            case GameState.GAMEPLAY:
                Debug.Log(DateTime.Now + " Loading level " + levelNumber + "...");
                ScreenFader.LoadScene("Gameplay", 1, 1);
                player.bombs = player.maximumBombsCount;
                StartCoroutine(PrepareForNextLevel());
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

    public FirstPersonController GetFPSController()
    {
        return player.GetComponent<FirstPersonController>();
    }

    public void SetCameraRotation(Vector3 rotation)
    {
        GetFPSController().SetCameraRotation(rotation);
    }

    public void EndCurrentLevel(bool cleared)
    {
        if (levelScore != null)
        {
            this.levelCleared = cleared;
            GetUIController().DisplayLevelSummary(cleared, player.remainingLives, levelScore.clearBonus, levelScore.blocksBonus, levelScore.monsterBonus, levelScore.pickupBonus, levelScore.timeBonus);
            levelScore = null;
        }
    }

    public void OnSummaryDisplayingFinished()
    {
        if (levelCleared)
        {
            AdvanceToNextLevel();
        } else if (!levelCleared && player.remainingLives > 0)
        {
            RestartLevel();
        } else
        {
            SwitchGameState(GameState.MAIN_MENU);
        }
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

    public bool CanPlayerBeKilled()
    {
        return levelScore != null;  // level score is null only when summary screen is being displayed
    }

    private IEnumerator PrepareForNextLevel()
    {
        yield return new WaitForSeconds(3); // [dymara] Hack for disabling exit & death events until scene fade out animation finishes
        player.ResetPlayerFlags();
        levelScore = new LevelScore();
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

    public int GetBombDetonateDelay()
    {
        return configurator.bombDetonateDelay;
    }

    public int GetExitExplosionEnemiesCount()
    {
        return configurator.exitExplosionEnemiesCount;
    }

    public int GetEnemiesMinimumDistance()
    {
        return configurator.enemiesMinDistance;
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
        if (levelScore != null)
        {
            levelScore.blocksBonus += configurator.blockDestructionPoints;
            player.score += configurator.blockDestructionPoints;
        }
    }

    public void OnFindingPickedUp()
    {
        if (levelScore != null)
        {
            levelScore.pickupBonus += configurator.findingPickupPoints;
            player.score += configurator.findingPickupPoints;
        }
    }

    public void OnEnemyKilled()
    {
        if (levelScore != null)
        {
            levelScore.monsterBonus += configurator.enemyKillingPoints;
            player.score += configurator.enemyKillingPoints;
        }
    }

    public void OnLevelCleared(int timeLeft)
    {
        if (levelScore != null)
        {
            int levelBonus = levelNumber * configurator.levelClearedPoints;
            levelScore.clearBonus = levelBonus;
            int timeBonus = timeLeft * configurator.timeMultiplierPoints;
            levelScore.timeBonus = timeBonus;
            player.score += levelBonus + timeBonus;
        }
    }

    private class LevelScore
    {
        public int clearBonus { set; get; }

        public int blocksBonus { set; get; }

        public int monsterBonus { set; get; }
        
        public int pickupBonus { set; get; }

        public int timeBonus { set; get; }
    }

}
