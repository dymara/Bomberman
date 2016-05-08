using UnityEngine;
using System.Collections;
using Assets.Scripts.Board;
using System.Collections.Generic;
using Assets.Scripts.Util;

public class AISpawner : MonoBehaviour
{
    public void SpawnEnemies(Board board, PositionConverter positionConverter, int enemiesCount, int minDistance, Vector2 originPoint, GameObject monsterPrefab)
    {
        List<GameCell> freeCells = board.GetFreeCellsAtMinDistance(originPoint, minDistance);
        freeCells.Shuffle();
        Debug.Log("Free cells: " + freeCells.Count);
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
