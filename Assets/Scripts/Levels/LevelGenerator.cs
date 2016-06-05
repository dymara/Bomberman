using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public class LevelGenerator : MonoBehaviour
{
    public LevelConfig GenerateLevelConfig(int level, Player player)
    {
        LevelConfig levelConfig = new LevelConfig();
        levelConfig.boardSize = GetBoardSize(level);
        levelConfig.levelDuration = GetLevelDuration(levelConfig.boardSize, level);
        levelConfig.monstersCount = GetMonstersCount(level);
        levelConfig.findingExtraBombsCount = GetExtraBombsFindingsCount(level, player);
        levelConfig.findingExtraLivesCount = GetExtraLivesFindingsCount(level, player);
        levelConfig.findingBombRangeCount = GetBombRangeFindingsCount(level, player);
        levelConfig.findingFasterMovingCount = GetFasterMovingFindingsCount(level, player);
        levelConfig.findingRemoteDetonationCount = GetRemoteDetonationFindingsCount(level, player);

        Debug.Log("Level Config: " + levelConfig.ToString());

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

    private int GetExtraBombsFindingsCount(int level, Player player)
    {
        if (player.maximumBombsCount == LevelGeneratorConfig.MAX_PLAYER_BOMBS_COUNT)
        {
            return 0;
        }
        else
        {
            return LevelGeneratorConfig.LEVEL_1_EXTRA_BOMBS_COUNT;
        }
    }

    private int GetExtraLivesFindingsCount(int level, Player player)
    {
        if (player.remainingLives == LevelGeneratorConfig.MAX_PLAYER_LIVES_COUNT)
        {
            return 0;
        }
        else
        {
            return LevelGeneratorConfig.LEVEL_1_EXTRA_LIVES_COUNT;
        }
    }

    private int GetBombRangeFindingsCount(int level, Player player)
    {
        if (player.bombRange >= LevelGeneratorConfig.MAX_PLAYER_BOMB_RANGE)
        {
            return 0;
        }
        else
        {
            return 1;
        }

    }

    private int GetFasterMovingFindingsCount(int level, Player player)
    {
        if (level >= LevelGeneratorConfig.MIN_LEVEL_FOR_FASTER_MOVING &&
            player.speed < LevelGeneratorConfig.MAX_PLAYER_SPEED &&
            Random.value > LevelGeneratorConfig.FASTER_MOVING_FINDING_PROBABILITY)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private int GetRemoteDetonationFindingsCount(int level, Player player)
    {
        if (player.remoteDetonationBonus)
        {
            return 0;
        }
        else
        {
            return level >= LevelGeneratorConfig.MIN_LEVEL_FOR_REMOTE_DETONATION ? 1 : 0;
        }
    }
}
