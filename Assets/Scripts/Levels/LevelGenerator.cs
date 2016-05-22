using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{
    public Configurator configurator;

    void Awake()
    {
        configurator = GameManager.instance.configurator;
    }

    public LevelConfig GenerateLevelConfig(int level)
    {
        LevelConfig levelConfig = new LevelConfig();
        levelConfig.boardSize = GetBoardSize(level);
        levelConfig.levelDuration = GetLevelDuration(levelConfig.boardSize, level);
        levelConfig.monstersCount = GetMonstersCount(level);
        return levelConfig;
    }

    private Vector2 GetBoardSize(int level)
    {
        int sizeX = configurator.level1CubesXCount + level / 2;
        int sizeZ = configurator.level1CubesZCount + level / 2;

        return new Vector2(sizeX, sizeZ);
    }

    private int GetMonstersCount(int level)
    {
        int monstersCount = configurator.level1EnemiesCount;
        while (level-- > 0)
        {
            if (Random.value < Mathf.Pow(0.7f, level))
            {
                monstersCount++;
            }
            else
            {
                break;
            }
        }
        Debug.Log("Enemies count:" + monstersCount);
        return monstersCount;
    }

    private float GetLevelDuration(Vector2 levelSize, int level)
    {
        return levelSize.x * levelSize.y * Mathf.Max(Mathf.Pow(0.97f, level), 0.6f) * configurator.levelDurationPerBlock;
    }
}
