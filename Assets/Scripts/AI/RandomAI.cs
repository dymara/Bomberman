using UnityEngine;
using Assets.Scripts.Board;
using System.Collections.Generic;

public class RandomAI : AIBehavior
{
    private GameCell prevCell;

    private GameCell currentCell;

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
            prevCell = currentCell;
            currentCell = adjacentCells[Random.Range(0, adjacentCells.Count)];
            return currentCell;
        }
        else
        {
            return null;
        }
    }

    protected override GameCell GetBackMove()
    {
        if(prevCell == null)
        {
            return currentCell;
        }
        return prevCell;
    }

}
