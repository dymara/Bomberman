using UnityEngine;
using Assets.Scripts.Board;
using System.Collections.Generic;

public class RandomAI : AIBehavior
{
    public override void Start()
    {
        base.Start();
    }

    override protected GameCell GetNextMove()
    {
        Vector2 currentPosition = positionConverter.ConvertScenePositionToBoard(this.transform.position);
        List<GameCell> adjacentCells = board.GetAdjacentCells(currentPosition);
        if (adjacentCells.Count != 0)
        {
            GameCell nextCell = adjacentCells[Random.Range(0, adjacentCells.Count)];
            return nextCell;
        }
        else
        {
            return null;
        }
    }
}
