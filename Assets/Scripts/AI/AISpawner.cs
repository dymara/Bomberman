using UnityEngine;
using System.Collections;
using Assets.Scripts.Board;
using System.Collections.Generic;
using Assets.Scripts.Util;

public class AISpawner : MonoBehaviour
{
    public int enemiesCount;

    public int minDistance;

    public GameObject monsterPrefab;

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
            GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity) as GameObject;
            monster.AddComponent<RandomAI>();
        }
    }
}
