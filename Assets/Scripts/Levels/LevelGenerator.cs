using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{
    private const int BASE_BOARD_SIZE = 5;

    public Configurator configurator;

    void Awake()
    {
		configurator = GameManager.instance.configurator;
    }

    public LevelConfig GenerateLevelConfig(int level)
    {
        LevelConfig levelConfig = new LevelConfig();
        levelConfig.boardSize = GetBoardSize(level);
        levelConfig.levelDuration = GetLevelDuration(levelConfig.boardSize);
        return levelConfig;
    }

    private Vector2 GetBoardSize(int level)
    {
        int sizeX = BASE_BOARD_SIZE + level / 2;
        int sizeY = BASE_BOARD_SIZE + level / 2;

        return new Vector2(sizeX, sizeY);
    }

    private float GetLevelDuration(Vector2 levelSize)
    {
        return levelSize.x * levelSize.y * configurator.levelDurationPerBlock;
    }
}
