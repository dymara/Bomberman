using UnityEngine;
using Assets.Scripts.Board;
using Assets.Scripts.Model;
using System.Collections.Generic;
using System;
using System.Collections;

public class ExplosionManager : MonoBehaviour {

    public GameObject[] explosions;

    private GameManager gameManager;

    public void setGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void PutBomb(Bomb bomb, Vector2 position)
    {
        GameCell cell = gameManager.getBoard().GetGameCell((int)position.x, (int)position.y);
        if (cell != null && cell.block == null && cell.bomb == null)
        {
            doPutBomb(bomb, cell);
        } else
        {
            Debug.Log(DateTime.Now + " You can't place a bomb here!");
            // display "Cannot place bomb here message"
        }
    }

    private void doPutBomb(Bomb bombPrefab, GameCell gameCell)
    {
        Vector3 bombPosition = gameManager.getPositionConverter().ConvertBoardPositionToScene(gameCell.GetCoordinates(), true);
        bombPosition.y = bombPrefab.transform.localScale.y / 1.5f;

        Bomb bomb = GameObject.Instantiate(bombPrefab, bombPosition, Quaternion.identity) as Bomb;
        gameCell.bomb = bomb;

        StartCoroutine(handleBombPlaced(bomb, gameCell));
    }

    private IEnumerator handleBombPlaced(Bomb bomb, GameCell gameCell)
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
        if (!bomb.hasBeenDetonated())
        {
            HashSet<GameCell> cellsToExplode = GetCellsToExplode(gameCell, bomb.explosionRange);
            foreach (GameCell cell in cellsToExplode)
            {
                PlayExplosionEffect(cell.GetCoordinates(), bomb.transform.localScale.y);
                cell.Explode();
            }
        }
    }

    public void PlayExplosionEffect(Vector2 gameCellCoordinates, float height)
    {
        int explosionIndex = UnityEngine.Random.Range(0, explosions.Length);
        Vector3 explosionPosition = gameManager.getPositionConverter().ConvertBoardPositionToScene(gameCellCoordinates, true);
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

        inspectCell(cells, bombs, initialX - 1, initialY, RayDirection.LEFT, bombRange);
        inspectCell(cells, bombs, initialX, initialY - 1, RayDirection.UP, bombRange);
        inspectCell(cells, bombs, initialX + 1, initialY, RayDirection.RIGHT, bombRange);
        inspectCell(cells, bombs, initialX, initialY + 1, RayDirection.BOTTOM, bombRange);

        return cells;
    }

    private void inspectCell(HashSet<GameCell> resultCells, HashSet<Bomb> bombs, int cellX, int cellY, RayDirection direction, int rangeLeft)
    {
        if (rangeLeft > 0)
        {
            GameCell cell = gameManager.getBoard().GetGameCell(cellX, cellY);
            if (isGameCellDestructible(cell))
            {
                resultCells.Add(cell);

                // explode other bombs in range
                if (cell.bomb != null && !bombs.Contains(cell.bomb))
                {                    
                    bombs.Add(cell.bomb);
                    int bombRange = cell.bomb.explosionRange;
                    inspectCell(resultCells, bombs, cellX - 1, cellY, RayDirection.LEFT, bombRange);
                    inspectCell(resultCells, bombs, cellX, cellY - 1, RayDirection.UP, bombRange);
                    inspectCell(resultCells, bombs, cellX + 1, cellY, RayDirection.RIGHT, bombRange);
                    inspectCell(resultCells, bombs, cellX, cellY + 1, RayDirection.BOTTOM, bombRange);
                }

                switch (direction)
                {
                    case RayDirection.LEFT:
                        inspectCell(resultCells, bombs, cellX - 1, cellY, direction, rangeLeft - 1);
                        break;
                    case RayDirection.UP:
                        inspectCell(resultCells, bombs, cellX, cellY - 1, direction, rangeLeft - 1);
                        break;
                    case RayDirection.RIGHT:
                        inspectCell(resultCells, bombs, cellX + 1, cellY, direction, rangeLeft - 1);
                        break;
                    case RayDirection.BOTTOM:
                        inspectCell(resultCells, bombs, cellX, cellY + 1, direction, rangeLeft - 1);
                        break;
                }
            }
        }
    }

    private bool isGameCellDestructible(GameCell gameCell)
    {
        return gameCell != null && (gameCell.block == null || gameCell.block.GetType() != typeof(IndesctructibleCubeObject));
    }

    private enum RayDirection { LEFT, UP, RIGHT, BOTTOM }

}
