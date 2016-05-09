using UnityEngine;
using Assets.Scripts.Board;
using System.Collections.Generic;
using Assets.Scripts.Util;
using Assets.Scripts.Postion;
using Assets.Scripts.Model;
using System.Collections;

public class AISpawner : MonoBehaviour
{
    public int enemiesCount;

    public int enemiesCountAfterExitExploded;

    public int minDistance;

    public GameObject monsterPrefab;

    private PlayerPositionManager positionManager;

    public void SetPostitionManager(PlayerPositionManager positionManager)
    {
        this.positionManager = positionManager;
    }

    public void SpawnEnemies(Board board, PositionConverter positionConverter, Vector2 originPoint)
    {
        List<GameCell> freeCells = board.GetFreeCellsAtMinDistance(originPoint, minDistance);
        freeCells.Shuffle();
        for (int i = 0; i < enemiesCount; i++)
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
            monster.AddComponent<RandomAI>();
            Monster monsterComponent = monster.GetComponent<Monster>();
            monsterComponent.positionManager = positionManager;
            positionManager.AddPlayer(monsterComponent);
    }

    public void SpawnEnemiesAfterExitExploded(Vector3 exitPosition)
    {
        StartCoroutine(SpawnEnemiesCoroutine(enemiesCountAfterExitExploded, 1.3f, exitPosition));
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
