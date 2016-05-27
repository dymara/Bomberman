using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{
    public LevelConfig GenerateLevelConfig(int level)
    {
        LevelConfig levelConfig = new LevelConfig();
        levelConfig.boardSize = GetBoardSize(level);
        levelConfig.levelDuration = GetLevelDuration(levelConfig.boardSize, level);
        levelConfig.monstersCount = GetMonstersCount(level);
        return levelConfig;
    }

    /**
    * Returns Vector2 with board dimensions x, y in number od indestructible cubes
    * Every LEVELS_TO_INCREASE_BOARD_SIZE levels we increase the level size by 1
    */
    private Vector2 GetBoardSize(int level)
    {
        int sizeX = LevelGeneratorConfig.LEVEL_1_BOARD_SIZE + level / LevelGeneratorConfig.LEVELS_TO_INCREASE_BOARD_SIZE;
        int sizeZ = LevelGeneratorConfig.LEVEL_1_BOARD_SIZE + level / LevelGeneratorConfig.LEVELS_TO_INCREASE_BOARD_SIZE;

        return new Vector2(sizeX, sizeZ);
    }

    private int GetMonstersCount(int level)
    {
        int monstersCount = level;
        float additionalMonsterChance = 0.7f;
        int i = level;
        while (i-- > 0)
        {
            if (Random.value < additionalMonsterChance)
            {
                monstersCount++;
                additionalMonsterChance = additionalMonsterChance * 0.7f;
            }
        }
        monstersCount = Mathf.Max(monstersCount, level);
        Debug.Log("Level: " + level + " Enemies count:" + monstersCount);
        return monstersCount;
    }

    private float GetLevelDuration(Vector2 levelSize, int level)
    {
        float levelDimen = levelSize.x * levelSize.y;
        return levelDimen * Mathf.Max(Mathf.Pow(0.98f, level) * LevelGeneratorConfig.BASE_LEVEL_DURATION_PER_BLOCK, LevelGeneratorConfig.MIN_LEVEL_DURATION_PER_BLOCK);
    }
}
