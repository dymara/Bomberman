using UnityEngine;
using Assets.Scripts.Board;
using System.Collections.Generic;
using System;

public class LineAI : AIBehavior
{
    private GameCell prevCell;

    private GameCell currentCell;

    enum Direction
    {
        N,
        E,
        S,
        W
    }

    private Direction direction;

    public override void Start()
    {
        base.Start();
        this.direction = RandomEnumValue<Direction>();
    }

    static T RandomEnumValue<T> ()
    {
        Array values = Enum.GetValues(typeof(T));
        return (T) values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    private Vector2 GetDirectionOffset(Direction? direction)
    {
        switch(direction)
        {
            case Direction.N:
                return new Vector2(0, 1);
            case Direction.E:
                return new Vector2(1, 0);
            case Direction.S:
                return new Vector2(0, -1);
            case Direction.W:
                return new Vector2(-1, 0);
        }
        return new Vector2(0, 0);
    }

    private Direction GetOpositeDirection(Direction direction)
    {
        switch(direction)
        {
            case Direction.N:
                return Direction.S;
            case Direction.E:
                return Direction.W;
            case Direction.S:
                return Direction.N;
            case Direction.W:
                return Direction.E;
        }
        return direction;
    }

    private Direction SwitchDirection(Direction direction)
    {
        switch(direction)
        {
            case Direction.N:
                return UnityEngine.Random.value < 0.5 ? Direction.E : Direction.W;
            case Direction.E:
                return UnityEngine.Random.value < 0.5 ? Direction.N : Direction.S;
            case Direction.S:
                return UnityEngine.Random.value < 0.5 ? Direction.W : Direction.E;
            case Direction.W:
                return UnityEngine.Random.value < 0.5 ? Direction.S : Direction.N;
        }
        return direction;
    }

    override protected GameCell GetNextMove()
    {
        Vector2 currentPosition = positionConverter.ConvertScenePositionToBoard(this.transform.position);
        //Randomly change direction between horizontal <-> vertical
        if (UnityEngine.Random.value < 0.2)
        {
            this.direction = SwitchDirection(this.direction);
        }

        GameCell move = GetMoveInDirection(currentPosition);
        if (move != null)
        {
            prevCell = currentCell;
            currentCell = move;
            return move;
        }
        this.direction = SwitchDirection(this.direction);

        move = GetMoveInDirection(currentPosition);
        if (move != null)
        {
            prevCell = currentCell;
            currentCell = move;
            return move;
        }
        
        //We are stuck
        return null;
    }

    private GameCell GetMoveInDirection(Vector2 currentPosition)
    {
        GameCell move = GetMove(currentPosition, direction);
        if (move != null && move.IsEmpty())
        {
            return move;
        }
        // switch to oposite direction
        this.direction = GetOpositeDirection(direction);
        move = GetMove(currentPosition, direction);
        if (move != null && move.IsEmpty())
        {
            return move;
        }
        return null;
    }

    private GameCell GetMove(Vector2 currentPosition, Direction direction)
    {
        Vector2 offset = GetDirectionOffset(direction);
        GameCell nextMoveCell = board.GetGameCell(new Vector2(currentPosition.x + offset.x, currentPosition.y + offset.y));
        return nextMoveCell;
    }

    protected override GameCell GetBackMove()
    {   
        if (prevCell == null)
        {
            return currentCell;
        }
        return prevCell;
    }
}
