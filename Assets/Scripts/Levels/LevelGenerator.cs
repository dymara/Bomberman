using UnityEngine;
using Assets.Scripts.Model;
using System;

public class LevelGenerator : MonoBehaviour
{
    private System.Random random = new System.Random();

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
        levelConfig.enemiesCountAfterExitExploded = GetEnemiesCountAfterExitExploded(levelConfig.monstersCount);

        Debug.Log(DateTime.Now + " " + levelConfig.ToString());

        return levelConfig;
    }

    /**
    * Returns Vector2 with board dimensions x, y in number od indestructible cubes
    * Every LEVELS_TO_INCREASE_BOARD_SIZE levels we increase the level size by 1
    */
    private Vector2 GetBoardSize(int level)
    {
        int divider = LevelGeneratorConfig.LEVELS_TO_INCREASE_BOARD_SIZE;

        int baseSize = LevelGeneratorConfig.LEVEL_1_BOARD_SIZE + (level - 1) / divider;
        int deltaX = (level % divider == divider - 1) ? 1 : 0;
        int deltaY = (level % divider == 0) ? 1 : 0;

        return new Vector2(baseSize + deltaX, baseSize + deltaY);
    }

    private int GetMonstersCount(int level)
    {
        int monstersCount = Mathf.Max(1, level / 2);
        float additionalMonsterChance = 0.7f;
        int i = level;
        while (i-- > 0)
        {
            if (random.NextDouble() < additionalMonsterChance)
            {
                monstersCount++;
                additionalMonsterChance *= 0.7f;
            }
        }
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
            if (random.NextDouble() <= GetExtraBombFindingProbability(player))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    private float GetExtraBombFindingProbability(Player player)
    {
        return (1 - player.maximumBombsCount * LevelGeneratorConfig.EXTRA_BOMB_PROBABILITY_PENALTY);
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
            UnityEngine.Random.value > LevelGeneratorConfig.FASTER_MOVING_FINDING_PROBABILITY)
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

    private int GetEnemiesCountAfterExitExploded(int currentLevelMonstersCount)
    {
        return Mathf.Max(GameManager.instance.GetExitExplosionEnemiesCount(), currentLevelMonstersCount / 3);
    }
}
