﻿using UnityEngine;
using Assets.Scripts.Board;
using Assets.Scripts.Model;
using System.Collections.Generic;
using System;
using System.Collections;

public class ExplosionManager : MonoBehaviour {

    public GameObject[] explosions;

    public LevelManager levelManager;

    private Dictionary<Bomb, GameCell> bombMap = new Dictionary<Bomb, GameCell>();

    private readonly object bombMapLock = new object();

    private int placedBombCount = 0;

    public void PutBomb(GameObject player, Bomb bomb, Vector2 position)
    {
        GameCell cell = levelManager.GetBoard().GetGameCell(position);
        if (cell != null && cell.block == null && cell.bomb == null)
        {
            DoPutBomb(player, bomb, cell);
        } else
        {
            Debug.Log(DateTime.Now + " You can't place a bomb here!");
            levelManager.GetUIController().ShowTimedMessage("You can't place a bomb here!");
        }
    }

    private void DoPutBomb(GameObject player, Bomb bombPrefab, GameCell gameCell)
    {
        Vector3 bombPosition = levelManager.GetPositionConverter().ConvertBoardPositionToScene(gameCell.GetCoordinates(), true);
        bombPosition.y = bombPrefab.transform.localScale.y / 1.25f;

        Bomb bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity) as Bomb;
        bomb.player = player;
        bomb.detonateDelay = GameManager.instance.GetBombDetonateDelay();
        bomb.explosionRange = GameManager.instance.GetPlayer().bombRange;
        gameCell.bomb = bomb;

        AddToBombMap(bomb, gameCell);
        StartCoroutine(HandleBombPlaced(bomb, gameCell));
    }

    private IEnumerator HandleBombPlaced(Bomb bomb, GameCell gameCell)
    {
        // decrement counter value
        int timeLeft = bomb.detonateDelay;
        while (timeLeft > 0)
        {
            bomb.SetCountValue(timeLeft);
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        // handle explosion
        if (!bomb.HasBeenDetonated())
        {
            HashSet<GameCell> cellsToExplode = GetCellsToExplode(gameCell, bomb.explosionRange);
            List<Bomb> explodedBombs = new List<Bomb>();
            foreach (GameCell cell in cellsToExplode)
            {
                PlayExplosionEffect(cell.GetCoordinates(), bomb.transform.localScale.y);
                if (cell.bomb != null)
                {
                    explodedBombs.Add(cell.bomb);
                }
                cell.Explode();
            }
            RemoveFromBombMap(explodedBombs);
        }
    }

    private void AddToBombMap(Bomb bomb, GameCell gameCell)
    {
        lock (bombMapLock) {        
            bombMap.Add(bomb, gameCell);
            HighlightCellsToExplode(bomb, true);
            placedBombCount++;
        }
    }

    private void RemoveFromBombMap(List<Bomb> bombs)
    {
        lock (bombMapLock)
        {
            foreach (Bomb bomb in bombs)
            {
                HighlightCellsToExplode(bomb, false);
                bombMap.Remove(bomb);
                placedBombCount--;
            }
            foreach (Bomb remainingBomb in bombMap.Keys) {
                HighlightCellsToExplode(remainingBomb, true);
            }
        }
    }

    private void HighlightCellsToExplode(Bomb bomb, bool shouldBeHighlighted)
    {
        GameCell gameCell = bombMap[bomb];
        HashSet<GameCell> cellsToExplode = GetCellsToExplode(gameCell, bomb.explosionRange);
       
        foreach (GameCell cell in cellsToExplode)
        {
            cell.highlight.SetActive(shouldBeHighlighted);
        }
    }

    public void PlayExplosionEffect(Vector2 gameCellCoordinates, float height)
    {
        int explosionIndex = UnityEngine.Random.Range(0, explosions.Length);
        Vector3 explosionPosition = levelManager.GetPositionConverter().ConvertBoardPositionToScene(gameCellCoordinates, true);
        explosionPosition.y = height;

        GameObject explosionObject = Instantiate(explosions[explosionIndex], explosionPosition, Quaternion.identity) as GameObject;

        ParticleSystem explosion = explosionObject.GetComponent<ParticleSystem>();
        explosion.Play();
        Destroy(explosionObject, explosion.duration);
    }


    /* RECURSIVE GETTERS FOR CELLS IN BOMB RANGE */

    private HashSet<GameCell> GetCellsToExplode(GameCell bombGameCell, int bombRange)
    {
        int initialX = (int)bombGameCell.GetCoordinates().x;
        int initialY = (int)bombGameCell.GetCoordinates().y;

        HashSet<GameCell> cells = new HashSet<GameCell>();
        cells.Add(bombGameCell);

        HashSet<Bomb> bombs = new HashSet<Bomb>();
        bombs.Add(bombGameCell.bomb);

        InspectCell(cells, bombs, initialX - 1, initialY, RayDirection.LEFT, bombRange);
        InspectCell(cells, bombs, initialX, initialY - 1, RayDirection.UP, bombRange);
        InspectCell(cells, bombs, initialX + 1, initialY, RayDirection.RIGHT, bombRange);
        InspectCell(cells, bombs, initialX, initialY + 1, RayDirection.BOTTOM, bombRange);

        return cells;
    }

    private void InspectCell(HashSet<GameCell> resultCells, HashSet<Bomb> bombs, int cellX, int cellY, RayDirection direction, int rangeLeft)
    {
        if (rangeLeft > 0)
        {
            GameCell cell = levelManager.GetBoard().GetGameCell(cellX, cellY);
            if (IsGameCellDestructible(cell))
            {
                resultCells.Add(cell);

                // explode other bombs in range
                if (cell.bomb != null && !bombs.Contains(cell.bomb))
                {                    
                    bombs.Add(cell.bomb);
                    int bombRange = cell.bomb.explosionRange;
                    InspectCell(resultCells, bombs, cellX - 1, cellY, RayDirection.LEFT, bombRange);
                    InspectCell(resultCells, bombs, cellX, cellY - 1, RayDirection.UP, bombRange);
                    InspectCell(resultCells, bombs, cellX + 1, cellY, RayDirection.RIGHT, bombRange);
                    InspectCell(resultCells, bombs, cellX, cellY + 1, RayDirection.BOTTOM, bombRange);
                }

                switch (direction)
                {
                    case RayDirection.LEFT:
                        InspectCell(resultCells, bombs, cellX - 1, cellY, direction, rangeLeft - 1);
                        break;
                    case RayDirection.UP:
                        InspectCell(resultCells, bombs, cellX, cellY - 1, direction, rangeLeft - 1);
                        break;
                    case RayDirection.RIGHT:
                        InspectCell(resultCells, bombs, cellX + 1, cellY, direction, rangeLeft - 1);
                        break;
                    case RayDirection.BOTTOM:
                        InspectCell(resultCells, bombs, cellX, cellY + 1, direction, rangeLeft - 1);
                        break;
                }
            }
        }
    }

    private bool IsGameCellDestructible(GameCell gameCell)
    {
        return gameCell != null && (gameCell.block == null || gameCell.block.GetType() != typeof(IndesctructibleCubeObject));
    }

    private enum RayDirection { LEFT, UP, RIGHT, BOTTOM }

    public int GetPlacedBombCount()
    {
        return placedBombCount;
    }

}
