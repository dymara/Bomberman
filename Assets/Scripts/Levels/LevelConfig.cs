using UnityEngine;
using System.Collections;

public class LevelConfig
{
    public float levelDuration;

    public int monstersCount;

    public int findingExtraBombsCount;

    public int findingExtraLivesCount;

    public int findingBombRangeCount;

    public int findingFasterMovingCount;

    public int findingRemoteDetonationCount;

    public Vector2 boardSize;

    public override string ToString()
    {
        string SEPARATOR = ", ";
        return "LevelConfig {" +
            "levelDuration: " + levelDuration + SEPARATOR +
            "monstersCount: " + monstersCount + SEPARATOR +
            "findingExtraLivesCount: " + findingExtraLivesCount + SEPARATOR +
            "findingExtraBombsCount: " + findingExtraBombsCount + SEPARATOR +
            "findingBombRangeCount: " + findingBombRangeCount + SEPARATOR +
            "findingFasterMovingCount: " + findingFasterMovingCount + SEPARATOR +
            "findingRemoteDetonationCount: " + findingRemoteDetonationCount + SEPARATOR +
            "boardSize: " + boardSize + "}";
    }
}
