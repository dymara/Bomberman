using UnityEngine;
using Assets.Scripts.Board;
using System.Collections.Generic;
using Assets.Scripts.Util;
using Assets.Scripts.Model;
using System.Collections;
using Assets.Scripts.Position;

public class AISpawner : MonoBehaviour
{
    public GameObject monsterPrefab;

    private LevelConfig levelConfig;

    private int minDistance;    

    private PlayerPositionManager positionManager;

    void Awake()
    {
        this.minDistance = GameManager.instance.GetEnemiesMinimumDistance();
    }

    public void SetPostitionManager(PlayerPositionManager positionManager)
    {
        this.positionManager = positionManager;
    }

    public void SetLevelConfig(LevelConfig levelConfig)
    {
        this.levelConfig = levelConfig;
    }

    public void SpawnEnemies(Board board, PositionConverter positionConverter, Vector2 originPoint)
    {
        List<GameCell> freeCells = board.GetFreeCellsAtMinDistance(originPoint, minDistance);
        freeCells.Shuffle();
        for (int i = 0; i < levelConfig.monstersCount; i++)
        {
            if (i >= freeCells.Count)
            {
                break;
            }
            GameCell monsterCell = freeCells[i];
            Vector3 monsterPosition = positionConverter.ConvertBoardPositionToScene(monsterCell.GetCoordinates(), true);
            SpawnEnemiesInPosition(monsterPosition);
        }
    }

    public void SpawnEnemiesInPosition(Vector3 scenePosition)
    {
            GameObject monster = Instantiate(monsterPrefab, scenePosition, Quaternion.identity) as GameObject;
            monster.AddComponent<LineAI>();
            Monster monsterComponent = monster.GetComponent<Monster>();
            monsterComponent.positionManager = positionManager;
            positionManager.AddPlayer(monsterComponent);
    }

    public void SpawnEnemiesAfterExitExploded(Vector3 exitPosition)
    {
        StartCoroutine(SpawnEnemiesCoroutine(levelConfig.enemiesCountAfterExitExploded, 2.0f, exitPosition));
    }

    private IEnumerator SpawnEnemiesCoroutine(int count, float interval, Vector3 scenePosition)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemiesInPosition(scenePosition);
            yield return new WaitForSeconds(interval);
         }
    }

}
